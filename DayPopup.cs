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

        private class TimeSlot
        {
            public DateTime Start { get; }
            public DateTime End { get; }

            public TimeSlot(DateTime start, DateTime end)
            {
                Start = start.Date == end.Date ? start : start.Date;
                End = end.Date == start.Date ? end : start.Date.AddDays(1).AddMinutes(-1);
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
                Debug.WriteLine("[Time Load] Starting time slot generation...");

                cmbTimeIn.Items.Clear();
                cmbTimeOut.Items.Clear();

                if (!ValidatePrerequisites()) return;

                var existingSlots = await GetExistingTimeSlotsAsync();
                var allSlots = GenerateDaySlots(currentDate);
                var availableSlots = FilterAvailableSlots(allSlots, existingSlots);

                PopulateTimeCombos(availableSlots);
                Debug.WriteLine($"[Time Load] Found {availableSlots.Count} available slots");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Time Load Error] {ex.Message}");
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

        private List<TimeSlot> GenerateDaySlots(DateTime date)
        {
            var slots = new List<TimeSlot>();
            var current = date.Date;
            var dayEnd = date.Date.AddDays(1).AddMinutes(-1);

            while (current < dayEnd)
            {
                var end = current.AddMinutes(TIME_INTERVAL);
                end = end > dayEnd ? dayEnd : end;
                slots.Add(new TimeSlot(current, end));
                current = end;
            }
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
                @"SELECT time_in, time_out FROM schedules
                WHERE date = @date
                AND (room = @room
                     OR teacher = @teacher
                     OR (course_code = @course AND section = @section))", parameters))
            {
                while (await reader.ReadAsync())
                {
                    var start = DateTime.ParseExact(reader["time_in"].ToString(), "h:mm tt", CultureInfo.InvariantCulture);
                    var end = DateTime.ParseExact(reader["time_out"].ToString(), "h:mm tt", CultureInfo.InvariantCulture);
                    existingSlots.Add(new TimeSlot(start, end));
                }
            }
            return existingSlots;
        }

        private List<TimeSlot> FilterAvailableSlots(List<TimeSlot> allSlots, List<TimeSlot> existingSlots)
        {
            return allSlots.Where(slot =>
                !existingSlots.Any(existing =>
                    slot.Start < existing.End &&
                    existing.Start < slot.End))
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
                var parameters = new[] { new MySqlParameter("@selectedDate", currentDate.Date) };
                using (var reader = DatabaseHelper.ExecuteReader(
                    @"SELECT id, course_code, section, subject, teacher, room, time_in, time_out
                    FROM schedules WHERE date = @selectedDate", parameters))
                {
                    var dt = new DataTable();
                    dt.Load(reader);
                    dgvEvents.DataSource = dt;
                }
                Debug.WriteLine($"[Load] Loaded {dgvEvents.Rows.Count} events");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Load Error] {ex.Message}");
                MessageBox.Show($"Error loading events: {ex.Message}");
            }
        }

        private bool TryParseTimes(out DateTime start, out DateTime end)
        {
            start = end = DateTime.MinValue;
            var formatValid = DateTime.TryParseExact(cmbTimeIn.Text, "h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out start) &&
                            DateTime.TryParseExact(cmbTimeOut.Text, "h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out end);

            if (!formatValid || start >= end)
            {
                MessageBox.Show("Invalid time range");
                return false;
            }
            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs() || !TryParseTimes(out DateTime start, out DateTime end)) return;
            if (!CheckForConflicts(start, end)) return;
            SaveSchedule(start, end);
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
            var selectedId = Convert.ToInt32(dgvEvents.SelectedRows[0].Cells["id"].Value);

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
                Debug.WriteLine($"[Conflict Check] Checking for {cmbCourse.Text}-{cmbSections.Text}");

                var parameters = new[]
                {
                    new MySqlParameter("@date", currentDate.Date),
                    new MySqlParameter("@room", cmbRooms.Text),
                    new MySqlParameter("@teacher", cmbTeachers.Text),
                    new MySqlParameter("@course", cmbCourse.Text),
                    new MySqlParameter("@section", cmbSections.Text),
                    new MySqlParameter("@newStart", newStart.ToString("HH:mm")),
                    new MySqlParameter("@newEnd", newEnd.ToString("HH:mm")),
                    new MySqlParameter("@id", currentEventId)
                };

                var conflictChecks = new[]
                {
                    ("room", @"SELECT COUNT(*) FROM schedules
                             WHERE date = @date AND room = @room AND id != @id
                             AND (time_in < @newEnd AND time_out > @newStart)"),
                    ("teacher", @"SELECT COUNT(*) FROM schedules
                                 WHERE date = @date AND teacher = @teacher AND id != @id
                                 AND (time_in < @newEnd AND time_out > @newStart)"),
                    ("course-section", @"SELECT COUNT(*) FROM schedules
                                       WHERE date = @date AND course_code = @course
                                       AND section = @section AND id != @id
                                       AND (time_in < @newEnd AND time_out > @newStart)")
                };

                var conflicts = conflictChecks
                    .Where(check => Convert.ToInt32(DatabaseHelper.ExecuteScalar(check.Item2, parameters)) > 0)
                    .Select(check => check.Item1)
                    .ToList();

                if (conflicts.Count > 0)
                {
                    MessageBox.Show($"Conflict detected: {string.Join(", ", conflicts)}");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Conflict Check Error] {ex.Message}");
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
                    new MySqlParameter("@timeIn", start.ToString("h:mm tt")),
                    new MySqlParameter("@timeOut", end.ToString("h:mm tt")),
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
            currentEventId = Convert.ToInt32(row.Cells["id"].Value);
            cmbSubjects.Text = row.Cells["subject"].Value?.ToString();
            cmbTeachers.Text = row.Cells["teacher"].Value?.ToString();
            cmbCourse.Text = row.Cells["course_code"].Value?.ToString();
            cmbRooms.Text = row.Cells["room"].Value?.ToString();
            cmbSections.Text = row.Cells["section"].Value?.ToString();
            cmbTimeIn.Text = row.Cells["time_in"].Value?.ToString();
            cmbTimeOut.Text = row.Cells["time_out"].Value?.ToString();
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

        private void cmbTimeIn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DateTime.TryParseExact(cmbTimeIn.Text, "h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime timeIn))
            {
                selectedTimeIn = timeIn;
                _ = LoadTimeOutOptionsAsync(); // Load Time Out based on selected Time In
            }
            else
            {
                selectedTimeIn = null;
            }
        }

        private async Task LoadTimeOutOptionsAsync()
        {
            try
            {
                cmbTimeOut.Items.Clear();
                if (!selectedTimeIn.HasValue || !ValidatePrerequisites()) return;

                var existingSlots = await GetExistingTimeSlotsAsync();
                var allSlots = GenerateDaySlots(currentDate);
                var availableSlots = FilterAvailableSlots(allSlots, existingSlots);

                // Find next conflicting slot after selected Time In
                var nextConflict = existingSlots
                    .Where(s => s.Start > selectedTimeIn.Value)
                    .OrderBy(s => s.Start)
                    .FirstOrDefault();

                DateTime maxEnd = nextConflict != null
                    ? nextConflict.Start
                    : currentDate.Date.AddDays(1).AddMinutes(-1);

                // Generate valid Time Out slots
                var validEndSlots = availableSlots
                    .Where(slot => slot.Start >= selectedTimeIn && slot.End <= maxEnd)
                    .ToList();

                foreach (var slot in validEndSlots)
                {
                    cmbTimeOut.Items.Add(slot.End.ToString("h:mm tt"));
                }

                Debug.WriteLine($"[Time Out Load] Found {validEndSlots.Count} valid end slots");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Time Out Load Error] {ex.Message}");
            }
        }
    }
}