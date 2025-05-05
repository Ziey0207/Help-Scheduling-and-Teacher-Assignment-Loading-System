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

        // Add these new class fields
        private bool isRecurring = false;

        private List<DateTime> recurringDates = new List<DateTime>();

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
            // New frequency change handler
            cmbFrequency.SelectedIndexChanged += FrequencyChanged;
        }

        private void DayPopup_Load(object sender, EventArgs e)
        {
            cmbFrequency.SelectedIndex = 0; // Default to "Single"
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
            airForm1.Text = $"Schedule/s of {date:dd MMMM yyyy}";
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
            Debug.WriteLine("[LoadEvents] Starting to load events");

            try
            {
                // Detach event handlers to prevent recursive calls
                Debug.WriteLine("[LoadEvents] Detaching selection changed event");
                dgvEvents.SelectionChanged -= dgvEvents_SelectionChanged;

                // Format the times directly in SQL
                var parameters = new[] { new MySqlParameter("@selectedDate", currentDate.Date) };
                Debug.WriteLine($"[LoadEvents] Loading events for date: {currentDate.Date:yyyy-MM-dd}");

                string query = @"
    SELECT
        id,
        course_code,
        section,
        subject,
        teacher,
        room,
        time_in,
        time_out,
        CONCAT(DATE_FORMAT(time_in, '%h:%i %p'), ' - ', DATE_FORMAT(time_out, '%h:%i %p')) as time_range,
        weekly_group_id
    FROM schedules
    WHERE date = @selectedDate
    ORDER BY time_in";

                Debug.WriteLine($"[LoadEvents] Executing query: {query}");
                DataTable dt = new DataTable();

                using (var reader = DatabaseHelper.ExecuteReader(query, parameters))
                {
                    Debug.WriteLine("[LoadEvents] Loading data into DataTable");
                    dt.Load(reader);
                    Debug.WriteLine($"[LoadEvents] DataTable loaded with {dt.Rows.Count} rows");

                    // Display column names for debugging
                    Debug.WriteLine("[LoadEvents] Columns in DataTable:");
                    foreach (DataColumn col in dt.Columns)
                    {
                        Debug.WriteLine($"[LoadEvents]   {col.ColumnName}");
                    }

                    // Display a sample row for debugging
                    if (dt.Rows.Count > 0)
                    {
                        Debug.WriteLine("[LoadEvents] Sample row data:");
                        foreach (DataColumn col in dt.Columns)
                        {
                            Debug.WriteLine($"[LoadEvents]   {col.ColumnName}: {dt.Rows[0][col]}");
                        }
                    }
                }

                // Configure DataGridView before binding data
                Debug.WriteLine("[LoadEvents] Configuring DataGridView");
                ConfigureDataGridView();

                // Bind data to grid
                Debug.WriteLine("[LoadEvents] Setting DataGridView DataSource");
                dgvEvents.DataSource = dt;

                // Clear selection
                dgvEvents.ClearSelection();
                dgvEvents.CurrentCell = null; // Remove focus from any cell
                Debug.WriteLine("[LoadEvents] Selection cleared");

                Debug.WriteLine($"[LoadEvents] Successfully loaded {dt.Rows.Count} events");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[LoadEvents] ERROR: {ex.Message}");
                Debug.WriteLine($"[LoadEvents] Stack trace: {ex.StackTrace}");
                MessageBox.Show($"Error loading events: {ex.Message}");
            }
            finally
            {
                // Reattach the SelectionChanged event
                Debug.WriteLine("[LoadEvents] Reattaching selection changed event");
                dgvEvents.SelectionChanged += dgvEvents_SelectionChanged;
            }
        }

        private void ConfigureDataGridView()
        {
            Debug.WriteLine("[ConfigureDataGridView] Starting grid configuration");

            // Disable auto-generation to maintain control
            dgvEvents.AutoGenerateColumns = false;
            dgvEvents.Columns.Clear();
            Debug.WriteLine("[ConfigureDataGridView] Cleared existing columns");

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
            });

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSection",
                DataPropertyName = "section",
                HeaderText = "Section",
            });

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSubject",
                DataPropertyName = "subject",
                HeaderText = "Subject",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTeacher",
                DataPropertyName = "teacher",
                HeaderText = "Teacher",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            });

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colRoom",
                DataPropertyName = "room",
                HeaderText = "Room",
            });

            // Add the time range column
            Debug.WriteLine("[ConfigureDataGridView] Adding time range column");
            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTimeRange",
                DataPropertyName = "time_range",  // This will bind directly to our SQL-generated column
                HeaderText = "Class Time",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
            });

            // Keep the original time columns but make them invisible - needed for data operations
            Debug.WriteLine("[ConfigureDataGridView] Adding hidden time columns");
            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTimeIn",
                DataPropertyName = "time_in",
                HeaderText = "Time In",
                Visible = false
            });

            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTimeOut",
                DataPropertyName = "time_out",
                HeaderText = "Time Out",
                Visible = false
            });
            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colWeeklyGroupId",
                DataPropertyName = "weekly_group_id",
                HeaderText = "Weekly Group ID",
                Visible = false
            });

            // Styling
            dgvEvents.AllowUserToAddRows = false;
            dgvEvents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEvents.EnableHeadersVisualStyles = false;
            dgvEvents.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dgvEvents.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            // Remove row highlight color when not focused
            dgvEvents.DefaultCellStyle.SelectionBackColor = Color.White;
            dgvEvents.DefaultCellStyle.SelectionForeColor = Color.Black;

            Debug.WriteLine("[ConfigureDataGridView] Grid configuration complete");
        }

        private void AddCombinedTimeColumn()
        {
            Debug.WriteLine("[AddCombinedTimeColumn] Adding time column");

            // Keep the original time columns but make them invisible for data binding
            Debug.WriteLine("[AddCombinedTimeColumn] Adding hidden time_in column for data binding");
            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTimeIn",
                DataPropertyName = "time_in",
                HeaderText = "Time In",
                Visible = false
            });

            Debug.WriteLine("[AddCombinedTimeColumn] Adding hidden time_out column for data binding");
            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTimeOut",
                DataPropertyName = "time_out",
                HeaderText = "Time Out",
                Visible = false
            });

            // Add the combined time column
            Debug.WriteLine("[AddCombinedTimeColumn] Adding visible time_range column");
            var timeColumn = new DataGridViewTextBoxColumn
            {
                Name = "colTime",
                DataPropertyName = "time_range",
                HeaderText = "Class Time",
                Width = 140,
                ReadOnly = true
            };

            // Set up cell formatting
            timeColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Add the column to the grid
            dgvEvents.Columns.Add(timeColumn);

            Debug.WriteLine("[AddCombinedTimeColumn] Setting up DataBindingComplete event");
            // Remove any existing event handlers to prevent duplicates
            dgvEvents.DataBindingComplete -= DgvEvents_DataBindingComplete;
            dgvEvents.DataBindingComplete += DgvEvents_DataBindingComplete;

            Debug.WriteLine("[AddCombinedTimeColumn] Time column added successfully");
        }

        private void DgvEvents_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            Debug.WriteLine("[DataBindingComplete] Data binding complete event triggered");
            Debug.WriteLine($"[DataBindingComplete] Row count: {dgvEvents.Rows.Count}");

            try
            {
                // Process each row in the grid
                foreach (DataGridViewRow row in dgvEvents.Rows)
                {
                    Debug.WriteLine($"[DataBindingComplete] Processing row {row.Index}");

                    // Log the value of each cell to debug
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        Debug.WriteLine($"[DataBindingComplete] Cell {cell.OwningColumn.Name}: {cell.Value}");
                    }

                    // Check if the time_range column already has data
                    if (row.Cells["colTime"].Value != null && !string.IsNullOrEmpty(row.Cells["colTime"].Value.ToString()))
                    {
                        Debug.WriteLine($"[DataBindingComplete] Row {row.Index} already has time_range: {row.Cells["colTime"].Value}");
                    }
                    else
                    {
                        Debug.WriteLine($"[DataBindingComplete] Row {row.Index} needs time formatting");

                        // Check if the time cells exist and have data
                        if (row.Cells["colTimeIn"] != null && row.Cells["colTimeOut"] != null &&
                            row.Cells["colTimeIn"].Value != null && row.Cells["colTimeOut"].Value != null)
                        {
                            Debug.WriteLine($"[DataBindingComplete] Row {row.Index} has time values: In={row.Cells["colTimeIn"].Value}, Out={row.Cells["colTimeOut"].Value}");

                            // Format the time values
                            try
                            {
                                DateTime timeIn = Convert.ToDateTime(row.Cells["colTimeIn"].Value);
                                DateTime timeOut = Convert.ToDateTime(row.Cells["colTimeOut"].Value);

                                string formattedTimeIn = timeIn.ToString("h:mm tt");
                                string formattedTimeOut = timeOut.ToString("h:mm tt");

                                string combinedTime = $"{formattedTimeIn} - {formattedTimeOut}";
                                Debug.WriteLine($"[DataBindingComplete] Row {row.Index} formatted time: {combinedTime}");

                                row.Cells["colTime"].Value = combinedTime;
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"[DataBindingComplete] Error formatting time for row {row.Index}: {ex.Message}");
                            }
                        }
                        else
                        {
                            Debug.WriteLine($"[DataBindingComplete] Row {row.Index} missing time data");
                        }
                    }
                }
                Debug.WriteLine("[DataBindingComplete] Completed processing all rows");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[DataBindingComplete] Error in data binding event: {ex.Message}");
                Debug.WriteLine($"[DataBindingComplete] Stack trace: {ex.StackTrace}");
            }
        }

        private void FormatCombinedTimeColumn()
        {
            try
            {
                // Process each row in the grid
                foreach (DataGridViewRow row in dgvEvents.Rows)
                {
                    if (row.Cells["time_in"].Value != null && row.Cells["time_out"].Value != null)
                    {
                        // Get time values from the hidden columns
                        DateTime timeIn = Convert.ToDateTime(row.Cells["time_in"].Value);
                        DateTime timeOut = Convert.ToDateTime(row.Cells["time_out"].Value);

                        // Format the time values
                        string formattedTimeIn = timeIn.ToString("h:mm tt");
                        string formattedTimeOut = timeOut.ToString("h:mm tt");

                        // Set the combined time string
                        row.Cells["colTime"].Value = $"{formattedTimeIn} - {formattedTimeOut}";
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Format Time Error] {ex.Message}");
                // Don't show error message to prevent user confusion
            }
        }

        private void AddTimeColumn(string name, string dataProperty, string header)
        {
            var col = new DataGridViewTextBoxColumn
            {
                Name = name,
                DataPropertyName = dataProperty,
                HeaderText = header,
                Width = 100
            };

            // Format time display with custom 12-hour format
            col.DefaultCellStyle.Format = "h:mm tt";  // 12-hour format with AM/PM
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

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
            if (dgvEvents.SelectedRows.Count == 0) return;

            var selectedRow = dgvEvents.SelectedRows[0];
            var selectedId = Convert.ToInt32(selectedRow.Cells["colID"].Value);
            var weeklyGroupId = selectedRow.Cells["colWeeklyGroupId"].Value?.ToString();

            try
            {
                MySqlParameter[] parameters;
                string query;

                if (!string.IsNullOrEmpty(weeklyGroupId))
                {
                    Debug.WriteLine($"[Delete] Recurring event detected (Weekly Group ID: {weeklyGroupId})");

                    var result = MessageBox.Show(
                        "Delete this recurring event:\n[Yes] - All instances\n[No] - Just this day\n[Cancel] - Don't delete",
                        "Confirm Delete",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question
                    );

                    switch (result)
                    {
                        case DialogResult.Yes:
                            // Delete all instances
                            Debug.WriteLine("[Delete] User chose to delete ALL recurring instances");
                            query = "DELETE FROM schedules WHERE weekly_group_id = @weeklyGroupId";
                            parameters = new[] { new MySqlParameter("@weeklyGroupId", weeklyGroupId) };
                            break;

                        case DialogResult.No:
                            // Delete single instance
                            Debug.WriteLine("[Delete] User chose to delete SINGLE instance");
                            query = "DELETE FROM schedules WHERE id = @id";
                            parameters = new[] { new MySqlParameter("@id", selectedId) };
                            break;

                        default:
                            Debug.WriteLine("[Delete] User canceled deletion");
                            return;
                    }
                }
                else
                {
                    Debug.WriteLine("[Delete] Single event deletion requested");
                    if (MessageBox.Show("Delete this event?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes)
                        return;

                    query = "DELETE FROM schedules WHERE id = @id";
                    parameters = new[] { new MySqlParameter("@id", selectedId) };
                }

                Debug.WriteLine($"[Delete] Executing: {query}");
                Debug.WriteLine($"[Delete] Parameters: {string.Join(", ", parameters.Select(p => $"{p.ParameterName}={p.Value}"))}");

                int affectedRows = DatabaseHelper.ExecuteNonQuery(query, parameters);
                Debug.WriteLine($"[Delete] Deleted {affectedRows} record(s)");

                LoadEvents();
                ClearForm();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Delete Error] {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show($"Error deleting event: {ex.Message}");
            }
        }

        private bool CheckForConflicts(DateTime newStart, DateTime newEnd)
        {
            try
            {
                Debug.WriteLine($"[Conflict Check] Starting check for {(isRecurring ? "recurring" : "single")} schedule");

                List<MySqlParameter> parameters = new List<MySqlParameter>
        {
            new MySqlParameter("@room", cmbRooms.Text),
            new MySqlParameter("@teacher", cmbTeachers.Text),
            new MySqlParameter("@course", cmbCourse.Text),
            new MySqlParameter("@section", cmbSections.Text),
            new MySqlParameter("@newStart", newStart.TimeOfDay),
            new MySqlParameter("@newEnd", newEnd.TimeOfDay),
            new MySqlParameter("@id", currentEventId)
        };

                string dateCondition = "";
                if (isRecurring)
                {
                    Debug.WriteLine($"[Conflict Check] Recurring range: {dtpStartDate.Value:yyyy-MM-dd} to {dtpEndDate.Value:yyyy-MM-dd}");

                    // Add recurring-specific parameters
                    parameters.Add(new MySqlParameter("@startDate", dtpStartDate.Value.Date));
                    parameters.Add(new MySqlParameter("@endDate", dtpEndDate.Value.Date));
                    parameters.Add(new MySqlParameter("@recurringDays", GetSelectedDays()));

                    dateCondition = @"(
    (s.is_recurring = 1
        AND s.start_date <= @endDate
        AND s.end_date >= @startDate
        AND FIND_IN_SET(DAYNAME(s.date), @recurringDays)
    )
    OR
    (s.is_recurring = 0
        AND s.date BETWEEN @startDate AND @endDate
        AND FIND_IN_SET(DAYNAME(s.date), @recurringDays)
    )
)";
                }
                else
                {
                    Debug.WriteLine($"[Conflict Check] Single date: {currentDate:yyyy-MM-dd}");
                    parameters.Add(new MySqlParameter("@date", currentDate.Date));
                    dateCondition = "s.date = @date";
                }

                string conflictQuery = $@"
    SELECT
        IFNULL(SUM(IF(s.room = @room, 1, 0)), 0) AS room_conflict,
        IFNULL(SUM(IF(s.teacher = @teacher, 1, 0)), 0) AS teacher_conflict,
        IFNULL(SUM(IF(s.course_code = @course AND s.section = @section, 1, 0)), 0) AS section_conflict
    FROM schedules s
    WHERE
        s.id != @id
        AND (
            ({dateCondition})
            AND (
                (s.time_in < @newEnd AND s.time_out > @newStart)
                OR (s.time_in >= @newStart AND s.time_out <= @newEnd)
                OR (s.time_in <= @newStart AND s.time_out >= @newEnd)
            )
        )";
                Debug.WriteLine($"[Conflict Check] Executing query:\n{conflictQuery}");
                Debug.WriteLine($"[Conflict Check] Parameters:\n{string.Join("\n", parameters.Select(p => $"{p.ParameterName}: {p.Value}"))}");

                using (var reader = DatabaseHelper.ExecuteReader(conflictQuery, parameters.ToArray()))
                {
                    if (reader.Read())
                    {
                        int roomConflict = reader["room_conflict"] != DBNull.Value ? Convert.ToInt32(reader["room_conflict"]) : 0;
                        int teacherConflict = reader["teacher_conflict"] != DBNull.Value ? Convert.ToInt32(reader["teacher_conflict"]) : 0;
                        int sectionConflict = reader["section_conflict"] != DBNull.Value ? Convert.ToInt32(reader["section_conflict"]) : 0;

                        Debug.WriteLine($"[Conflict Check] Conflicts found - Room: {roomConflict}, Teacher: {teacherConflict}, Section: {sectionConflict}");

                        List<string> conflicts = new List<string>();
                        if (roomConflict > 0) conflicts.Add("Room");
                        if (teacherConflict > 0) conflicts.Add("Teacher");
                        if (sectionConflict > 0) conflicts.Add("Section");

                        if (conflicts.Count > 0)
                        {
                            string conflictMessage = $"Conflict detected on {(isRecurring ? "one or more dates" : "selected date")} with: {string.Join(", ", conflicts)}";
                            Debug.WriteLine($"[Conflict Check] {conflictMessage}");
                            MessageBox.Show(conflictMessage);
                            return false;
                        }
                    }
                }

                Debug.WriteLine("[Conflict Check] No conflicts found");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Conflict Check Error] {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show($"Error checking conflicts: {ex.Message}");
                return false;
            }
        }

        private void SaveSchedule(DateTime start, DateTime end)
        {
            try
            {
                Debug.WriteLine("[Save] Starting save process...");

                // Handle date population
                if (!isRecurring)
                {
                    Debug.WriteLine("[Save] Single event - adding current date");
                    recurringDates.Clear();
                    recurringDates.Add(currentDate);
                }
                else
                {
                    Debug.WriteLine("[Save] Generating recurring dates");
                    GenerateRecurringDates();
                }

                Debug.WriteLine($"[Save] Saving {recurringDates.Count} date(s)");

                string weeklyGroupId = null;
                if (isRecurring)
                {
                    weeklyGroupId = (currentEventId == -1)
                        ? Guid.NewGuid().ToString()
                        : GetExistingWeeklyGroupId(currentEventId);
                    Debug.WriteLine($"[Save] Weekly Group ID: {weeklyGroupId}");
                }

                foreach (var date in recurringDates)
                {
                    Debug.WriteLine($"[Save] Processing date: {date:yyyy-MM-dd}");

                    var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@course", cmbCourse.Text),
                new MySqlParameter("@section", cmbSections.Text),
                new MySqlParameter("@subject", cmbSubjects.Text),
                new MySqlParameter("@teacher", cmbTeachers.Text),
                new MySqlParameter("@room", cmbRooms.Text),
                new MySqlParameter("@date", date.Date),
                new MySqlParameter("@timeIn", start.TimeOfDay),
                new MySqlParameter("@timeOut", end.TimeOfDay),
                new MySqlParameter("@isRecurring", isRecurring ? 1 : 0),
                new MySqlParameter("@recurringDays", GetSelectedDays()),
                new MySqlParameter("@startDate", dtpStartDate.Value.Date),
                new MySqlParameter("@endDate", dtpEndDate.Value.Date),
                new MySqlParameter("@weeklyGroupId", weeklyGroupId)
            };

                    if (currentEventId != -1)
                    {
                        Debug.WriteLine("[Save] Adding ID parameter for update");
                        parameters.Add(new MySqlParameter("@id", currentEventId));
                    }

                    string query = currentEventId == -1
                        ? @"INSERT INTO schedules
                    (course_code, section, subject, teacher, room, date,
                     time_in, time_out, is_recurring, recurring_days,
                     start_date, end_date, weekly_group_id)
                  VALUES
                    (@course, @section, @subject, @teacher, @room, @date,
                     @timeIn, @timeOut, @isRecurring, @recurringDays,
                     @startDate, @endDate, @weeklyGroupId)"
                        : @"UPDATE schedules SET
                    course_code = @course,
                    section = @section,
                    subject = @subject,
                    teacher = @teacher,
                    room = @room,
                    date = @date,
                    time_in = @timeIn,
                    time_out = @timeOut,
                    is_recurring = @isRecurring,
                    recurring_days = @recurringDays,
                    start_date = @startDate,
                    end_date = @endDate,
                    weekly_group_id = @weeklyGroupId
                  WHERE id = @id";

                    Debug.WriteLine($"[Save] Executing query: {query}");
                    Debug.WriteLine($"[Save] Parameters: {string.Join(", ", parameters.Select(p => $"{p.ParameterName}={p.Value}"))}");

                    int affectedRows = DatabaseHelper.ExecuteNonQuery(query, parameters.ToArray());
                    Debug.WriteLine($"[Save] Database operation affected {affectedRows} rows");
                }

                Debug.WriteLine("[Save] Save completed successfully");
                LoadEvents();
                ClearForm();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Save Error] {ex.Message}");
                Debug.WriteLine($"[Save Error] Stack Trace: {ex.StackTrace}");
                MessageBox.Show($"Error saving schedule: {ex.Message}");
            }
        }

        private string GetExistingWeeklyGroupId(int eventId)
        {
            using (var reader = DatabaseHelper.ExecuteReader(
                "SELECT weekly_group_id FROM schedules WHERE id = @id",
                // Wrap parameter in an array
                new MySqlParameter[] {
            new MySqlParameter("@id", eventId)
                }))
            {
                return reader.Read() ? reader["weekly_group_id"].ToString() : null;
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

        // New method to generate recurring dates
        private void GenerateRecurringDates()
        {
            recurringDates.Clear();
            var selectedDays = GetSelectedDaysList();

            if (selectedDays.Count == 0)
            {
                MessageBox.Show("Please select at least one day for weekly recurrence");
                return;
            }

            DateTime startDate = dtpStartDate.Value.Date;
            DateTime endDate = dtpEndDate.Value.Date;

            Debug.WriteLine($"[Recurrence] Generating dates from {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}");

            // Changed to include both start and end dates in the range
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (selectedDays.Contains(date.DayOfWeek))
                {
                    recurringDates.Add(date);
                    Debug.WriteLine($"[Recurrence] Added date: {date:yyyy-MM-dd}");
                }
                else
                {
                    Debug.WriteLine($"[Recurrence] Skipped date (not selected day): {date:yyyy-MM-dd}");
                }
            }

            // Add debug output to verify dates
            Debug.WriteLine($"[Recurrence] Total dates generated: {recurringDates.Count}");
            if (recurringDates.Count > 0)
            {
                Debug.WriteLine($"[Recurrence] First date: {recurringDates.First():yyyy-MM-dd}");
                Debug.WriteLine($"[Recurrence] Last date: {recurringDates.Last():yyyy-MM-dd}");
            }
        }

        // Helper method to get selected days
        private List<DayOfWeek> GetSelectedDaysList()
        {
            var days = new List<DayOfWeek>();
            if (chkMon.Checked) days.Add(DayOfWeek.Monday);
            if (chkTue.Checked) days.Add(DayOfWeek.Tuesday);
            if (chkWed.Checked) days.Add(DayOfWeek.Wednesday);
            if (chkThu.Checked) days.Add(DayOfWeek.Thursday);
            if (chkFri.Checked) days.Add(DayOfWeek.Friday);
            if (chkSat.Checked) days.Add(DayOfWeek.Saturday);
            if (chkSun.Checked) days.Add(DayOfWeek.Sunday);
            return days;
        }

        // Helper method to get days as string
        private string GetSelectedDays()
        {
            return string.Join(",", GetSelectedDaysList().Select(d => d.ToString()));
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

        // New method to handle frequency selection
        private void FrequencyChanged(object sender, EventArgs e)
        {
            isRecurring = cmbFrequency.Text == "Weekly";

            // Toggle visibility of recurring controls

            if (!isRecurring)
            {
                recurringDates.Clear();
                recurringDates.Add(currentDate);
            }

            dtpStartDate.Visible = isRecurring;
            dtpEndDate.Visible = isRecurring;
            label10.Visible = label9.Visible = isRecurring;
            chkMon.Visible = chkTue.Visible = chkWed.Visible =
            chkThu.Visible = chkFri.Visible = chkSat.Visible = chkSun.Visible = isRecurring;

            Debug.WriteLine($"[Frequency] Changed to: {(isRecurring ? "Weekly" : "Single")}");
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