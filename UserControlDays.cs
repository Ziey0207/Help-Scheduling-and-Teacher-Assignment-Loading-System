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

                    if (count > 0)
                    {
                        MessageBox.Show("A schedule already exists for this date. You cannot add another one.",
                                        "Schedule Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                if (lines.Length >= 4) // Ensure valid format
                {
                    string subjectDetails = lines[0].Trim(); // Extract subject
                    string teacher = lines[1].Replace("Teacher: ", "").Trim();
                    string time = lines[2].Replace("Time: ", "").Trim();
                    string room = lines[3].Replace("Room: ", "").Trim();

                    DateTime selectedFullDate = new DateTime(currentDate.Year, currentDate.Month, int.Parse(lbdays.Text));
                    string selectedDate = selectedFullDate.ToString("yyyy-MM-dd");

                    using (MySqlConnection conn = new MySqlConnection(connString))
                    {
                        conn.Open();
                        string sql = "SELECT id FROM schedules WHERE date = @date AND subject = @subject LIMIT 1";
                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@date", selectedDate);
                            cmd.Parameters.AddWithValue("@subject", subjectDetails);

                            object result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                int scheduleId = Convert.ToInt32(result);

                                // Show options to View, Edit, or Delete
                                DialogResult dialogResult = MessageBox.Show(
                                    $"Schedule Details:\n\nSubject: {subjectDetails}\nTeacher: {teacher}\nTime: {time}\nRoom: {room}\nDate: {selectedDate}\n\nDo you want to Edit this schedule?",
                                    "Schedule Options",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question
                                );

                                if (dialogResult == DialogResult.Yes) // Edit
                                {
                                    addschedule editForm = new addschedule(selectedDate, scheduleId);

                                    // Attach event handler para mag-refresh pag nagsara ang edit form
                                    editForm.FormClosed += (s, args) => displayEvent();

                                    editForm.ShowDialog();
                                }
                            }
                            else
                            {
                                MessageBox.Show($"Schedule not found for '{subjectDetails}'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Invalid schedule format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
