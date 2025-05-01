using MySql.Data.MySqlClient;
using ReaLTaiizor.Controls;
using Scheduling_and_Teacher_Loading_Assignment_System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Help_Scheduling_and_Teacher_Assignment_Loading_System
{
    public partial class DayPopup : Form
    {
        private const int TIME_INTERVAL = 30; // Minutes
        private DateTime currentDate;
        private int currentEventId = -1;
        private ErrorProvider errorProvider = new ErrorProvider();
        private DateTime? selectedTimeIn = null;
        private bool isTimeInChangeHandled = false; // Add this flag

        private class TimeSlot
        {
            public DateTime Start { get; }
            public DateTime End { get; }

            public TimeSlot(DateTime start, DateTime end)
            {
                Start = start;
                End = end;
            }
        }

        public DayPopup()
        {
            InitializeComponent();
            SetupValidation();
            WireUpEvents();
        }

        private void WireUpEvents()
        {
            cmbRooms.SelectedIndexChanged += async (s, e) => await LoadTimeOptionsAsync();
            cmbTeachers.SelectedIndexChanged += async (s, e) => await LoadTimeOptionsAsync();
            cmbCourse.SelectedIndexChanged += async (s, e) => await LoadTimeOptionsAsync();
            cmbSections.SelectedIndexChanged += async (s, e) => await LoadTimeOptionsAsync();
            cmbTimeIn.SelectedIndexChanged += cmbTimeIn_SelectedIndexChanged; // Add this line
        }

        private void DayPopup_Load(object sender, EventArgs e)
        {
            LoadComboBoxData();
            LoadEvents();
        }

        private void SetupValidation()
        {
            var validateCombo = new CancelEventHandler((s, e) =>
            {
                var combo = (ComboBox)s;
                errorProvider.SetError(combo, combo.SelectedIndex == -1 ? "Required field" : "");
            });

            cmbSubjects.Validating += validateCombo;
            cmbTeachers.Validating += validateCombo;
            cmbRooms.Validating += validateCombo;
            cmbCourse.Validating += validateCombo;
            cmbSections.Validating += validateCombo;
        }

        public void UpdatePopupTitle(DateTime date)
        {
            currentDate = date;
            airForm1.Text = $"Day Events of {date:dd MMMM yyyy}";
            LoadComboBoxData();
            LoadEvents();
        }

        private void LoadComboBoxData()
        {
            try
            {
                Debug.WriteLine("[Initialization] Loading combo box data...");

                LoadComboItems("SELECT subject_name FROM subjects", cmbSubjects, "subjects");
                LoadComboItems("SELECT CONCAT(first_name, ' ', last_name) FROM faculty", cmbTeachers, "teachers");
                LoadComboItems("SELECT room_name FROM rooms", cmbRooms, "rooms");
                LoadComboItems("SELECT course_code FROM courses", cmbCourse, "courses");
                LoadComboItems("SELECT section_name FROM sections", cmbSections, "sections");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Initialization Error] {ex.Message}");
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
        }

        private void LoadComboItems(string query, ComboBox combo, string name)
        {
            combo.Items.Clear();
            using (var reader = DatabaseHelper.ExecuteReader(query, null))
            {
                while (reader.Read()) combo.Items.Add(reader[0].ToString());
                Debug.WriteLine($"[Initialization] Loaded {combo.Items.Count} {name}");
            }
        }

        private async System.Threading.Tasks.Task LoadTimeOptionsAsync()
        {
            try
            {
                Debug.WriteLine($"[Time Load] === STARTING TIME IN GENERATION ===");
                Debug.WriteLine($"[Time Load] Selected values - Room: {cmbRooms.Text}, Teacher: {cmbTeachers.Text}, Course: {cmbCourse.Text}, Section: {cmbSections.Text}");

                cmbTimeIn.Items.Clear();
                cmbTimeOut.Items.Clear();

                if (!ValidatePrerequisites())
                {
                    Debug.WriteLine($"[Time Load] Aborted - missing prerequisites");
                    return;
                }

                Debug.WriteLine($"[Time Load] Fetching existing slots...");
                var existingSlots = await GetExistingTimeSlotsAsync();

                Debug.WriteLine($"[Time Load] Generated all possible slots:");
                var allSlots = GenerateDaySlots(currentDate);
                foreach (var slot in allSlots)
                {
                    Debug.WriteLine($"- {slot.Start:t} to {slot.End:t}");
                }

                var availableSlots = FilterAvailableSlots(allSlots, existingSlots);
                Debug.WriteLine($"[Time Load] Available slots after filtering:");
                foreach (var slot in availableSlots)
                {
                    Debug.WriteLine($"- {slot.Start:t} to {slot.End:t}");
                }

                PopulateTimeCombos(availableSlots);
                Debug.WriteLine($"[Time Load] Total available slots: {availableSlots.Count}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Time Load Error] {ex.ToString()}");
                MessageBox.Show("Error loading time options");
            }
        }

        private bool ValidatePrerequisites()
        {
            var isValid = cmbRooms.SelectedIndex != -1 &&
                         cmbTeachers.SelectedIndex != -1 &&
                         cmbCourse.SelectedIndex != -1 &&
                         cmbSections.SelectedIndex != -1 &&
                         currentDate != default;

            if (!isValid) Debug.WriteLine("[Time Load] Aborted - missing selections");
            return isValid;
        }

        private List<TimeSlot> GenerateDaySlots(DateTime date, DateTime? startFrom = null)
        {
            var slots = new List<TimeSlot>();
            var current = startFrom ?? date.Date;
            var dayEnd = date.Date.AddDays(1).AddMinutes(-1);

            Debug.WriteLine($"[Slot Gen] Generating slots from {current:t} to {dayEnd:t}");

            while (current < dayEnd)
            {
                var end = current.AddMinutes(TIME_INTERVAL);
                end = end > dayEnd ? dayEnd : end;
                slots.Add(new TimeSlot(current, end));
                current = end;
            }

            Debug.WriteLine($"[Slot Gen] Generated {slots.Count} slots");
            return slots;
        }

        private async Task<List<TimeSlot>> GetExistingTimeSlotsAsync()
        {
            var parameters = new[]
            {
        new MySqlParameter("@date", currentDate.Date),
        new MySqlParameter("@room", cmbRooms.Text),
        new MySqlParameter("@teacher", cmbTeachers.Text),
        new MySqlParameter("@course", cmbCourse.Text),
        new MySqlParameter("@section", cmbSections.Text)
    };

            var existingSlots = new List<TimeSlot>();
            using (var reader = await DatabaseHelper.ExecuteReaderAsync(
                @"SELECT DATE_FORMAT(time_in, '%h:%i %p') as time_in,
                 DATE_FORMAT(time_out, '%h:%i %p') as time_out
          FROM schedules
          WHERE date = @date
          AND (room = @room
               OR teacher = @teacher
               OR (course_code = @course AND section = @section))", parameters))
            {
                while (await reader.ReadAsync())
                {
                    string timeInStr = reader["time_in"].ToString();
                    string timeOutStr = reader["time_out"].ToString();

                    if (DateTime.TryParseExact(timeInStr, "h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime timeIn) &&
                        DateTime.TryParseExact(timeOutStr, "h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime timeOut))
                    {
                        DateTime start = currentDate.Date.Add(timeIn.TimeOfDay);
                        DateTime end = currentDate.Date.Add(timeOut.TimeOfDay);
                        existingSlots.Add(new TimeSlot(start, end));
                        Debug.WriteLine($"[DB Existing Slot] {start:t} - {end:t}");
                    }
                    else
                    {
                        Debug.WriteLine($"[DB Error] Failed to parse time: {timeInStr} or {timeOutStr}");
                    }
                }
            }
            return existingSlots;
        }

        private List<TimeSlot> FilterAvailableSlots(List<TimeSlot> allSlots, List<TimeSlot> existingSlots)
        {
            return allSlots.Where(slot =>
                !existingSlots.Any(existing =>
                {
                    bool isOverlap = slot.Start < existing.End && existing.Start < slot.End;
                    if (isOverlap) Debug.WriteLine($"[Overlap] Slot {slot.Start:t}-{slot.End:t} overlaps with existing {existing.Start:t}-{existing.End:t}");
                    return isOverlap;
                }))
                .ToList();
        }

        private void PopulateTimeCombos(List<TimeSlot> slots)
        {
            cmbTimeIn.Items.Clear();
            foreach (var slot in slots)
            {
                cmbTimeIn.Items.Add(slot.Start.ToString("h:mm tt"));
            }
        }

        private void LoadEvents()
        {
            try
            {
                dgvEvents.SelectionChanged -= dgvEvents_SelectionChanged;

                // Load data from database
                var parameters = new[] { new MySqlParameter("@selectedDate", currentDate.Date) };
                DataTable dt = new DataTable();

                using (var reader = DatabaseHelper.ExecuteReader(
                    @"SELECT id, course_code, section, subject, teacher, room, time_in, time_out
            FROM schedules WHERE date = @selectedDate", parameters))
                {
                    dt.Load(reader);
                }

                // Configure DataGridView before binding data
                ConfigureDataGridView();

                // Bind data to grid
                dgvEvents.DataSource = dt;
                dgvEvents.ClearSelection();
                dgvEvents.CurrentCell = null; // Remove focus from any cell

                Debug.WriteLine($"[Load] Loaded {dt.Rows.Count} events");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Load Error] {ex.Message}");
                MessageBox.Show($"Error loading events: {ex.Message}");
            }
            finally
            {
                // Reattach the SelectionChanged event
                dgvEvents.SelectionChanged += dgvEvents_SelectionChanged;
            }
        }

        private void ConfigureDataGridView()
        {
            // Disable auto-generation to maintain control
            dgvEvents.AutoGenerateColumns = false;
            dgvEvents.Columns.Clear();

            // Prevent row height changes
            dgvEvents.AllowUserToResizeRows = false;
            dgvEvents.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            // Prevent column width changes
            dgvEvents.AllowUserToResizeColumns = false;
            dgvEvents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Create and configure columns manually
            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colID",
                DataPropertyName = "id",
                HeaderText = "ID",
                Visible = false  // Hide ID as it's for internal use
            });

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCourse",
                DataPropertyName = "course_code",
                HeaderText = "Course",
                Width = 100
            });

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSection",
                DataPropertyName = "section",
                HeaderText = "Section",
                Width = 80
            });

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSubject",
                DataPropertyName = "subject",
                HeaderText = "Subject",
                Width = 150,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTeacher",
                DataPropertyName = "teacher",
                HeaderText = "Teacher",
                Width = 150,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colRoom",
                DataPropertyName = "room",
                HeaderText = "Room",
                Width = 80
            });

            // Time columns with formatting
            AddTimeColumn("colTimeIn", "time_in", "Start Time");
            AddTimeColumn("colTimeOut", "time_out", "End Time");

            // Styling
            dgvEvents.AllowUserToAddRows = false;
            dgvEvents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEvents.EnableHeadersVisualStyles = false;
            dgvEvents.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dgvEvents.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            // Remove row highlight color when not focused
            dgvEvents.DefaultCellStyle.SelectionBackColor = Color.White;
            dgvEvents.DefaultCellStyle.SelectionForeColor = Color.Black;
        }

        private void AddTimeColumn(string name, string dataProperty, string header)
        {
            var col = new DataGridViewTextBoxColumn
            {
                Name = name,
                DataPropertyName = dataProperty,
                HeaderText = header,
                Width = 80
            };

            // Format time display
            col.DefaultCellStyle.Format = "t";  // Short time format
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvEvents.Columns.Add(col);
        }

        private bool TryParseTimes(out DateTime start, out DateTime end)
        {
            start = end = DateTime.MinValue;

            // Check if time-in is selected
            if (string.IsNullOrEmpty(cmbTimeIn.Text))
            {
                MessageBox.Show("Please select a start time");
                return false;
            }

            // Check if time-out is selected
            if (string.IsNullOrEmpty(cmbTimeOut.Text))
            {
                MessageBox.Show("Please select an end time");
                return false;
            }

            // Parse the time strings
            bool validTimeIn = DateTime.TryParseExact(cmbTimeIn.Text, "h:mm tt",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out start);

            bool validTimeOut = DateTime.TryParseExact(cmbTimeOut.Text, "h:mm tt",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out end);

            if (!validTimeIn || !validTimeOut)
            {
                Debug.WriteLine($"[Time Parse] Invalid time format - Start: {cmbTimeIn.Text}, End: {cmbTimeOut.Text}");
                MessageBox.Show("Invalid time format");
                return false;
            }

            // Combine with current date
            start = currentDate.Date.Add(start.TimeOfDay);
            end = currentDate.Date.Add(end.TimeOfDay);

            Debug.WriteLine($"[Time Parse] Parsed times - Start: {start:yyyy-MM-dd HH:mm:ss}, End: {end:yyyy-MM-dd HH:mm:ss}");

            // Ensure end is after start
            if (start >= end)
            {
                MessageBox.Show("End time must be after start time");
                return false;
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Debug.WriteLine("[Save] Save button clicked");

                // First check prerequisites
                if (!ValidatePrerequisites())
                {
                    MessageBox.Show("Please complete all required fields");
                    return;
                }

                // Then validate inputs
                if (!ValidateInputs())
                {
                    Debug.WriteLine("[Save] Input validation failed");
                    return;
                }

                // Then parse times
                if (!TryParseTimes(out DateTime start, out DateTime end))
                {
                    Debug.WriteLine("[Save] Time parsing failed");
                    return;
                }

                // Check for conflicts
                if (!CheckForConflicts(start, end))
                {
                    Debug.WriteLine("[Save] Conflict check failed");
                    return;
                }

                // Save the schedule
                SaveSchedule(start, end);
                Debug.WriteLine("[Save] Schedule saved successfully");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Save Error] {ex.Message}");
                MessageBox.Show($"Error saving schedule: {ex.Message}");
            }
        }

        private bool ValidateInputs()
        {
            var isValid = ValidateChildren() &&
                        cmbTimeIn.SelectedIndex != -1 &&
                        cmbTimeOut.SelectedIndex != -1;

            if (!isValid) MessageBox.Show("Please fill all required fields");
            return isValid;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvEvents.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an event to delete");
                return;
            }

            // Get the ID directly from the selected row
            var selectedId = Convert.ToInt32(dgvEvents.SelectedRows[0].Cells["colID"].Value);

            if (MessageBox.Show("Delete this event?", "Confirm",
                MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            try
            {
                var parameters = new[] {
            new MySqlParameter("@id", selectedId)
        };

                DatabaseHelper.ExecuteNonQuery(
                    "DELETE FROM schedules WHERE id = @id",
                    parameters);

                LoadEvents();  // Refresh the grid
                ClearForm();    // Clear the form
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting event: {ex.Message}");
            }
        }

        private bool CheckForConflicts(DateTime newStart, DateTime newEnd)
        {
            try
            {
                Debug.WriteLine($"[Conflict Check] Checking conflicts for {newStart:t} - {newEnd:t}");

                var parameters = new[]
                {
            new MySqlParameter("@date", currentDate.Date),
            new MySqlParameter("@room", cmbRooms.Text),
            new MySqlParameter("@teacher", cmbTeachers.Text),
            new MySqlParameter("@course", cmbCourse.Text),
            new MySqlParameter("@section", cmbSections.Text),
            new MySqlParameter("@newStart", newStart.TimeOfDay),
            new MySqlParameter("@newEnd", newEnd.TimeOfDay),
            new MySqlParameter("@id", currentEventId)
        };

                // Unified conflict check query with IFNULL to handle NULL values
                string conflictQuery = @"
            SELECT
                IFNULL(SUM(IF(room = @room, 1, 0)), 0) AS room_conflict,
                IFNULL(SUM(IF(teacher = @teacher, 1, 0)), 0) AS teacher_conflict,
                IFNULL(SUM(IF(course_code = @course AND section = @section, 1, 0)), 0) AS section_conflict
            FROM schedules
            WHERE date = @date
            AND id != @id
            AND (
                (time_in < @newEnd AND time_out > @newStart)
            )";

                using (var reader = DatabaseHelper.ExecuteReader(conflictQuery, parameters))
                {
                    if (reader.Read())
                    {
                        // Safely convert values with null handling
                        int roomConflict = reader["room_conflict"] == DBNull.Value ? 0 : Convert.ToInt32(reader["room_conflict"]);
                        int teacherConflict = reader["teacher_conflict"] == DBNull.Value ? 0 : Convert.ToInt32(reader["teacher_conflict"]);
                        int sectionConflict = reader["section_conflict"] == DBNull.Value ? 0 : Convert.ToInt32(reader["section_conflict"]);

                        Debug.WriteLine($"[Conflict Check] Found conflicts - Room: {roomConflict}, Teacher: {teacherConflict}, Section: {sectionConflict}");

                        List<string> conflicts = new List<string>();
                        if (roomConflict > 0) conflicts.Add("Room");
                        if (teacherConflict > 0) conflicts.Add("Teacher");
                        if (sectionConflict > 0) conflicts.Add("Section");

                        if (conflicts.Count > 0)
                        {
                            MessageBox.Show($"Conflict detected with: {string.Join(", ", conflicts)}");
                            return false;
                        }
                    }
                }
                Debug.WriteLine("[Conflict Check] No conflicts found");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Conflict Check Error] {ex.Message}");
                MessageBox.Show($"Error checking for conflicts: {ex.Message}");
                return false;
            }
        }

        private void SaveSchedule(DateTime start, DateTime end)
        {
            try
            {
                var parameters = new[]
                {
                    new MySqlParameter("@subject", cmbSubjects.Text),
                    new MySqlParameter("@teacher", cmbTeachers.Text),
                    new MySqlParameter("@course", cmbCourse.Text),
                    new MySqlParameter("@section", cmbSections.Text),
                    new MySqlParameter("@room", cmbRooms.Text),
                    new MySqlParameter("@date", currentDate.Date),
                    new MySqlParameter("@timeIn", start.TimeOfDay),
                    new MySqlParameter("@timeOut", end.TimeOfDay),
                    new MySqlParameter("@id", currentEventId)
                };

                var query = currentEventId == -1 ?
                    @"INSERT INTO schedules
                    (course_code, section, subject, teacher, room, date, time_in, time_out)
                    VALUES (@course, @section, @subject, @teacher, @room, @date, @timeIn, @timeOut)"
                    :
                    @"UPDATE schedules SET
                    course_code = @course,
                    section = @section,
                    subject = @subject,
                    teacher = @teacher,
                    room = @room,
                    time_in = @timeIn,
                    time_out = @timeOut
                    WHERE id = @id";

                DatabaseHelper.ExecuteNonQuery(query, parameters);
                LoadEvents();
                ClearForm();
                Debug.WriteLine($"[Save] Schedule {(currentEventId == -1 ? "created" : "updated")}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Save Error] {ex.Message}");
                MessageBox.Show($"Error saving schedule: {ex.Message}");
            }
        }

        private void dgvEvents_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvEvents.SelectedRows.Count == 0) return;

            var row = dgvEvents.SelectedRows[0];
            currentEventId = Convert.ToInt32(row.Cells["colID"].Value);

            // Map values using DataGridView column names
            cmbSubjects.Text = row.Cells["colSubject"].Value?.ToString();
            cmbTeachers.Text = row.Cells["colTeacher"].Value?.ToString();
            cmbCourse.Text = row.Cells["colCourse"].Value?.ToString();
            cmbRooms.Text = row.Cells["colRoom"].Value?.ToString();
            cmbSections.Text = row.Cells["colSection"].Value?.ToString();

            // Handle Time In/Out
            string storedTimeIn = row.Cells["colTimeIn"].Value?.ToString();
            string storedTimeOut = row.Cells["colTimeOut"].Value?.ToString();
        }

        private void ClearForm()
        {
            currentEventId = -1;
            cmbSubjects.SelectedIndex = -1;
            cmbTeachers.SelectedIndex = -1;
            cmbCourse.SelectedIndex = -1;
            cmbRooms.SelectedIndex = -1;
            cmbSections.SelectedIndex = -1;
            cmbTimeIn.SelectedIndex = -1;
            cmbTimeOut.SelectedIndex = -1;
            errorProvider.Clear();
        }

        private bool IsValidTimeFormat(string input)
        {
            return Regex.IsMatch(input.Trim(),
                @"^(0?[1-9]|1[0-2]):[0-5][0-9] [AP]M$",
                RegexOptions.IgnoreCase);
        }

        private void airForm1_Click(object sender, EventArgs e)
        {
        }

        private async void cmbTimeIn_SelectedIndexChanged(object sender, EventArgs e)
        {
            Debug.WriteLine($"[Time In] Selection changed: {cmbTimeIn.Text}");

            if (isTimeInChangeHandled || string.IsNullOrEmpty(cmbTimeIn.Text))
            {
                Debug.WriteLine("[Time In] Change already handled or empty text, skipping");
                return;
            }

            try
            {
                isTimeInChangeHandled = true;
                Debug.WriteLine("[Time In] Set handling flag");

                if (DateTime.TryParseExact(cmbTimeIn.Text, "h:mm tt",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime timeIn))
                {
                    Debug.WriteLine($"[Time In] Valid time parsed: {timeIn:t}");
                    selectedTimeIn = timeIn;

                    // Clear time-out dropdown first
                    cmbTimeOut.Items.Clear();
                    cmbTimeOut.Text = "";

                    // Now load time-out options
                    await LoadTimeOutOptionsAsync();
                }
                else
                {
                    Debug.WriteLine($"[Time In] WARNING: Failed to parse time: '{cmbTimeIn.Text}'");
                    selectedTimeIn = null;
                    cmbTimeOut.Items.Clear();
                    cmbTimeOut.Text = "";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Time In] ERROR: {ex.Message}");
                MessageBox.Show($"Error processing time selection: {ex.Message}");
            }
            finally
            {
                isTimeInChangeHandled = false;
                Debug.WriteLine("[Time In] Cleared handling flag");
            }
        }

        private async Task LoadTimeOutOptionsAsync()
        {
            try
            {
                Debug.WriteLine($"[Time Out] === STARTING TIME OUT GENERATION ===");
                cmbTimeOut.Items.Clear();

                if (!selectedTimeIn.HasValue || !ValidatePrerequisites())
                {
                    Debug.WriteLine($"[Time Out] Prerequisites not met or no selected time in");
                    return;
                }

                // Get the full datetime for the selected time (combine current date with selected time)
                DateTime fullSelectedTimeIn = currentDate.Date.Add(selectedTimeIn.Value.TimeOfDay);
                Debug.WriteLine($"[Time Out] Selected time in: {fullSelectedTimeIn:yyyy-MM-dd HH:mm:ss}");

                // 1. Get existing slots
                var existingSlots = await GetExistingTimeSlotsAsync();
                Debug.WriteLine($"[Time Out] Retrieved {existingSlots.Count} existing slots");

                // 2. Generate possible end times
                var possibleEndTimes = new List<DateTime>();
                DateTime currentEnd = fullSelectedTimeIn.AddMinutes(TIME_INTERVAL);
                DateTime dayEnd = currentDate.Date.AddDays(1).AddMinutes(-1);

                Debug.WriteLine($"[Time Out] Generating possible end times from {currentEnd:HH:mm} to {dayEnd:HH:mm}");

                while (currentEnd <= dayEnd)
                {
                    possibleEndTimes.Add(currentEnd);
                    currentEnd = currentEnd.AddMinutes(TIME_INTERVAL);
                }
                Debug.WriteLine($"[Time Out] Generated {possibleEndTimes.Count} possible end times");

                // 3. Check each potential slot
                int availableCount = 0;
                foreach (var endTime in possibleEndTimes)
                {
                    var potentialSlot = new TimeSlot(fullSelectedTimeIn, endTime);

                    bool hasConflict = existingSlots.Any(existing =>
                    {
                        bool conflict = potentialSlot.Start < existing.End && existing.Start < potentialSlot.End;
                        if (conflict)
                        {
                            Debug.WriteLine($"[Time Out] Conflict: {potentialSlot.Start:HH:mm}-{potentialSlot.End:HH:mm} conflicts with {existing.Start:HH:mm}-{existing.End:HH:mm}");
                        }
                        return conflict;
                    });

                    if (!hasConflict)
                    {
                        cmbTimeOut.Items.Add(endTime.ToString("h:mm tt"));
                        availableCount++;
                        Debug.WriteLine($"[Time Out] Added valid end time: {endTime:h:mm tt}");
                    }
                }

                Debug.WriteLine($"[Time Out] Added {availableCount} valid end times to dropdown");

                // Set the first item as selected if available
                if (cmbTimeOut.Items.Count > 0)
                {
                    cmbTimeOut.SelectedIndex = 0;
                    Debug.WriteLine($"[Time Out] Auto-selected first available end time");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Time Out Error] {ex.ToString()}");
                MessageBox.Show($"Error loading time options: {ex.Message}");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // Reset event ID
            currentEventId = -1;

            // Clear combobox selections
            cmbSubjects.SelectedIndex = -1;
            cmbTeachers.SelectedIndex = -1;
            cmbCourse.SelectedIndex = -1;
            cmbRooms.SelectedIndex = -1;
            cmbSections.SelectedIndex = -1;
            cmbTimeIn.SelectedIndex = -1;
            cmbTimeOut.SelectedIndex = -1;

            // Clear text in comboboxes (if any)
            cmbTimeIn.Text = "";
            cmbTimeOut.Text = "";

            // Deselect DataGridView row
            dgvEvents.ClearSelection();

            // Remove error highlights
            errorProvider.Clear();
        }
    }
}