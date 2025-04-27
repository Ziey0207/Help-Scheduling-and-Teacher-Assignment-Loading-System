using MySql.Data.MySqlClient;
using ReaLTaiizor.Controls;
using Scheduling_and_Teacher_Loading_Assignment_System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        private DateTime currentDate;
        private int currentEventId = -1;
        private ErrorProvider errorProvider = new ErrorProvider();

        public DayPopup()
        {
            InitializeComponent();
            this.AutoValidate = AutoValidate.Disable;
            SetupValidation();
            Load += DayPopup_Load;
        }

        private void DayPopup_Load(object sender, EventArgs e)
        {
            LoadComboBoxData();
            LoadEvents();
        }

        private void SetupValidation()
        {
            // Time input validation
            txtTimeIn.Validating += TimeInput_Validating;
            txtTimeOut.Validating += TimeInput_Validating;

            // ComboBox validation
            cmbSubjects.Validating += ComboBox_Validating;
            cmbTeachers.Validating += ComboBox_Validating;
            cmbRooms.Validating += ComboBox_Validating;
        }

        public void UpdatePopupTitle(DateTime date)
        {
            currentDate = date;
            airForm1.Text = $"Day Events of {date:dd MMMM yyyy}";
            LoadEvents();
        }

        private void LoadComboBoxData()
        {
            try
            {
                cmbSubjects.Items.Clear();
                // Load subjects
                using (var reader = DatabaseHelper.ExecuteReader(
                    "SELECT subject_name FROM subjects", null))
                {
                    cmbSubjects.Items.Clear();
                    while (reader.Read())
                        cmbSubjects.Items.Add(reader["subject_name"]);
                }

                cmbTeachers.Items.Clear();
                // Load teachers
                using (var reader = DatabaseHelper.ExecuteReader(
                    "SELECT CONCAT(first_name, ' ', last_name) FROM faculty", null))
                {
                    cmbTeachers.Items.Clear();
                    while (reader.Read())
                        cmbTeachers.Items.Add(reader[0]);
                }

                cmbRooms.Items.Clear();
                // Set room options
                cmbRooms.Items.AddRange(new[] { "Room 303", "Room 404", "Room 301", "Comlab" });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
        }

        private void LoadEvents()
        {
            try
            {
                var parameters = new[]
                {
                new MySqlParameter("@selectedDate", currentDate.Date)
            };

                using (var reader = DatabaseHelper.ExecuteReader(
                    @"SELECT id, subject, teacher, room, time_in, time_out
                FROM schedules
                WHERE date = @selectedDate", parameters))
                {
                    var dt = new DataTable();
                    dt.Load(reader);
                    dgvEvents.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading events: {ex.Message}");
            }
        }

        private bool TryParseTimes(out DateTime startTime, out DateTime endTime)
        {
            startTime = endTime = DateTime.MinValue;

            bool validStart = DateTime.TryParseExact(txtTimeIn.Text.Trim(),
                "h:mm tt", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out startTime);

            bool validEnd = DateTime.TryParseExact(txtTimeOut.Text.Trim(),
                "h:mm tt", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out endTime);

            if (!validStart || !validEnd || startTime >= endTime)
            {
                MessageBox.Show("Invalid time range");
                return false;
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateAll() || !TryParseTimes(out DateTime startTime, out DateTime endTime))
                return;

            if (!CheckForConflicts(startTime, endTime))
                return;

            try
            {
                var parameters = new[]
                {
            new MySqlParameter("@subject", cmbSubjects.Text),
            new MySqlParameter("@teacher", cmbTeachers.Text),
            new MySqlParameter("@room", cmbRooms.Text),
            new MySqlParameter("@date", currentDate.Date),
            new MySqlParameter("@timeIn", startTime.ToString("hh:mm tt")),
            new MySqlParameter("@timeOut", endTime.ToString("hh:mm tt")),
            new MySqlParameter("@id", currentEventId)
        };

                string query = currentEventId == -1 ?
                    @"INSERT INTO schedules
              (subject, teacher, room, date, time_in, time_out)
              VALUES (@subject, @teacher, @room, @date, @timeIn, @timeOut)"
                    :
                    @"UPDATE schedules SET
              subject = @subject,
              teacher = @teacher,
              room = @room,
              time_in = @timeIn,
              time_out = @timeOut
              WHERE id = @id";

                DatabaseHelper.ExecuteNonQuery(query, parameters);
                LoadEvents();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving event: {ex.Message}");
            }
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
                var parameters = new[]
                {
            new MySqlParameter("@date", currentDate.Date),
            new MySqlParameter("@room", cmbRooms.Text),
            new MySqlParameter("@teacher", cmbTeachers.Text),
            new MySqlParameter("@newStart", newStart.ToString("HH:mm")),
            new MySqlParameter("@newEnd", newEnd.ToString("HH:mm")),
            new MySqlParameter("@id", currentEventId)
        };

                // Room conflict check
                var roomConflictCount = Convert.ToInt32(DatabaseHelper.ExecuteScalar(
                    @"SELECT COUNT(*) FROM schedules
            WHERE date = @date
            AND room = @room
            AND id != @id
            AND (
                STR_TO_DATE(time_in, '%h:%i %p') < STR_TO_DATE(@newEnd, '%H:%i')
                AND STR_TO_DATE(time_out, '%h:%i %p') > STR_TO_DATE(@newStart, '%H:%i')
            )", parameters));

                // Teacher conflict check
                var teacherConflictCount = Convert.ToInt32(DatabaseHelper.ExecuteScalar(
                    @"SELECT COUNT(*) FROM schedules
            WHERE date = @date
            AND teacher = @teacher
            AND id != @id
            AND (
                STR_TO_DATE(time_in, '%h:%i %p') < STR_TO_DATE(@newEnd, '%H:%i')
                AND STR_TO_DATE(time_out, '%h:%i %p') > STR_TO_DATE(@newStart, '%H:%i')
            )", parameters));

                if (roomConflictCount > 0)
                {
                    MessageBox.Show("Room conflict: Already booked during this time");
                    return false;
                }

                if (teacherConflictCount > 0)
                {
                    MessageBox.Show("Teacher conflict: Already assigned during this time");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking conflicts: {ex.Message}");
                return false;
            }
        }

        private void dgvEvents_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvEvents.SelectedRows.Count > 0 && !dgvEvents.SelectedRows[0].IsNewRow)
            {
                DataGridViewRow row = dgvEvents.SelectedRows[0];
                if (row.Cells["id"].Value != null)
                {
                    currentEventId = Convert.ToInt32(row.Cells["id"].Value);
                    cmbSubjects.Text = row.Cells["subject"]?.Value?.ToString() ?? "";
                    cmbTeachers.Text = row.Cells["teacher"]?.Value?.ToString() ?? "";
                    cmbRooms.Text = row.Cells["room"]?.Value?.ToString() ?? "";
                    txtTimeIn.Text = row.Cells["time_in"]?.Value?.ToString() ?? "";
                    txtTimeOut.Text = row.Cells["time_out"]?.Value?.ToString() ?? "";
                }
            }
        }

        private void ClearForm()
        {
            currentEventId = -1;
            cmbSubjects.SelectedIndex = -1;
            cmbTeachers.SelectedIndex = -1;
            cmbRooms.SelectedIndex = -1;
            txtTimeIn.Clear();
            txtTimeOut.Clear();
            errorProvider.Clear();
        }

        private void TimeInput_Validating(object sender, CancelEventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;

            if (!IsValidTimeFormat(control.Text))
            {
                errorProvider.SetError(control, "Invalid time format (HH:MM AM/PM)");
            }
            else
            {
                errorProvider.SetError(control, "");
            }
        }

        private void ComboBox_Validating(object sender, CancelEventArgs e)
        {
            var combo = (ComboBox)sender;
            if (combo.SelectedIndex == -1)
            {
                errorProvider.SetError(combo, "Please select an option");
            }
            else
            {
                errorProvider.SetError(combo, "");
            }
        }

        private bool IsValidTimeFormat(string input)
        {
            return Regex.IsMatch(input.Trim(),
                @"^(0?[1-9]|1[0-2]):[0-5][0-9] [AP]M$",
                RegexOptions.IgnoreCase);
        }

        private bool ValidateAll()
        {
            return ValidateChildren() &&
                   string.IsNullOrEmpty(errorProvider.GetError(txtTimeIn)) &&
                   string.IsNullOrEmpty(errorProvider.GetError(txtTimeOut));
        }
    }
}