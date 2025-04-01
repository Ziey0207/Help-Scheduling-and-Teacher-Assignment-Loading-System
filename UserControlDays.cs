using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Help_Scheduling_and_Teacher_Assignment_Loading_System
{
    public partial class UserControlDays : UserControl
    {
        string connString = "server=localhost;database=school_management;uid=root;pwd=;";

        public static string static_day;
        private DateTime currentDate;


        public UserControlDays(DateTime date)
        {
            InitializeComponent();
            currentDate = date;
        }

        private void UserControlDays_Load(object sender, EventArgs e)
        {

        }

        public void days(int numday)
        {
            lbdays.Text = numday.ToString();
            displayEvent();
        }

        private void lbdays_Click(object sender, EventArgs e)
        {

        }

        private void UserControlDays_Click(object sender, EventArgs e)
        {
            string selectedDate = currentDate.ToString("yyyy-MM-dd");

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM schedules WHERE date = @date";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@date", selectedDate);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count >= 2)
                    {
                        MessageBox.Show("You can only add up to 2 schedules per day.",
                                        "Schedule Limit Reached", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            // Open the add schedule form
            addschedule sched = new addschedule(selectedDate);

            // Attach event handler to refresh the UI after closing
            sched.FormClosed += (s, args) => displayEvent();

            sched.ShowDialog();

        }

        private void lblSchedule_Click(object sender, EventArgs e)
        {

        }

        public void displayEvent()
        {
            lbevent.Text = ""; // Clear previous events

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();
                string fullDate = currentDate.ToString("yyyy-MM-dd");
                string sql = "SELECT subject, teacher, time_in, time_out, room FROM schedules WHERE date = @date";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@date", fullDate);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            StringBuilder eventDetails = new StringBuilder();
                            while (reader.Read())
                            {
                                string subject = reader["subject"].ToString().Trim();
                                string teacher = reader["teacher"].ToString().Trim();
                                string timeIn = reader["time_in"].ToString().Trim();
                                string timeOut = reader["time_out"].ToString().Trim();
                                string room = reader["room"].ToString().Trim();

                                eventDetails.AppendLine($"{subject}");
                                eventDetails.AppendLine($"{teacher}");
                                eventDetails.AppendLine($"{timeIn} - {timeOut}");
                                eventDetails.AppendLine($"{room}");
                                eventDetails.AppendLine("---------------------------");
                            }
                            lbevent.Text = eventDetails.ToString();
                        }
                    }
                }
            }
        }

        private void OpenEditForm(string date, int scheduleId)
        {
            addschedule editForm = new addschedule(date, scheduleId);
            editForm.FormClosed += (s, args) => displayEvent();
            editForm.ShowDialog();
        }

        private string PromptScheduleSelection(List<(int id, string details)> schedules)
        {
            using (Form selectForm = new Form())
            {
                selectForm.Text = "Select Schedule to Edit";
                selectForm.Size = new Size(400, 200);
                selectForm.StartPosition = FormStartPosition.CenterScreen;

                ListBox listBox = new ListBox() { Dock = DockStyle.Fill };
                foreach (var schedule in schedules)
                {
                    listBox.Items.Add(schedule.details);
                }

                Button btnOK = new Button() { Text = "OK", Dock = DockStyle.Bottom };
                btnOK.Click += (s, e) => selectForm.DialogResult = DialogResult.OK;

                selectForm.Controls.Add(listBox);
                selectForm.Controls.Add(btnOK);

                if (selectForm.ShowDialog() == DialogResult.OK && listBox.SelectedItem != null)
                {
                    return listBox.SelectedItem.ToString();
                }
                return null;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            displayEvent();
        }

        private void lbevent_Click(object sender, EventArgs e)
        {
            Label lbl = sender as Label;
            if (lbl != null)
            {
                string[] lines = lbl.Text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                if (lines.Length < 4)
                {
                    MessageBox.Show("Invalid schedule format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DateTime selectedFullDate = new DateTime(currentDate.Year, currentDate.Month, int.Parse(lbdays.Text));
                string selectedDate = selectedFullDate.ToString("yyyy-MM-dd");

                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    string sql = "SELECT id, subject, teacher, time_in, time_out, room FROM schedules WHERE date = @date";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@date", selectedDate);
                        List<(int id, string details)> schedules = new List<(int, string)>();

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = Convert.ToInt32(reader["id"]);
                                string subject = reader["subject"].ToString().Trim();
                                string teacher = reader["teacher"].ToString().Trim();
                                string timeIn = reader["time_in"].ToString().Trim();
                                string timeOut = reader["time_out"].ToString().Trim();
                                string room = reader["room"].ToString().Trim();

                                string details = $"{subject} \n\n({teacher})\n\n{timeIn} - {timeOut}\n\nRoom: {room}";
                                schedules.Add((id, details));
                            }
                        }

                        // If only one schedule exists, open it directly
                        if (schedules.Count == 1)
                        {
                            OpenEditForm(selectedDate, schedules[0].id);
                        }
                        else if (schedules.Count == 2)
                        {
                            // Let user choose between two schedules
                            string selectedSchedule = PromptScheduleSelection(schedules);
                            if (!string.IsNullOrEmpty(selectedSchedule))
                            {
                                int selectedId = schedules.First(s => s.details == selectedSchedule).id;
                                OpenEditForm(selectedDate, selectedId);
                            }
                        }
                    }
                }
            }
        }
    }
}
