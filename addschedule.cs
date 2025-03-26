using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Help_Scheduling_and_Teacher_Assignment_Loading_System
{
    public partial class addschedule : Form
    {
        string connString = "server=localhost;database=school_management;uid=root;pwd=;";
        private string selectedDate; // New variable to store the selected date


        public addschedule(string date)
        {
            InitializeComponent();
            selectedDate = date;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Check for empty fields
            if (string.IsNullOrWhiteSpace(txtDate.Text) ||
                string.IsNullOrWhiteSpace(txtSubject.Text) ||
                string.IsNullOrWhiteSpace(txtTeacher.Text) ||
                string.IsNullOrWhiteSpace(txtTime.Text) ||
                string.IsNullOrWhiteSpace(txtRoom.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate date format
            DateTime parsedDate;
            if (!DateTime.TryParse(txtDate.Text, out parsedDate))
            {
                MessageBox.Show("Invalid date format. Please enter a valid date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate time format
            string[] timeParts = txtTime.Text.Split('-');
            if (timeParts.Length != 2)
            {
                MessageBox.Show("Invalid time format. Use 'HH:mm AM/PM - HH:mm AM/PM' (e.g., 12:00 PM - 1:00 PM).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Restrict time range
            if (!DateTime.TryParse(timeParts[0].Trim(), out DateTime startTime) ||
                !DateTime.TryParse(timeParts[1].Trim(), out DateTime endTime))
            {
                MessageBox.Show("Invalid time format. Please enter a valid time range.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (startTime >= endTime)
            {
                MessageBox.Show("Start time must be earlier than end time.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Restrict allowed time range (7:00 AM - 7:00 PM)
            TimeSpan earliestTime = TimeSpan.FromHours(7);  // 07:00 AM
            TimeSpan latestTime = TimeSpan.FromHours(19);   // 07:00 PM

            if (startTime.TimeOfDay < earliestTime || endTime.TimeOfDay > latestTime)
            {
                MessageBox.Show("Time must be between 07:00 AM and 07:00 PM.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Prevent duplicate schedules
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();
                string checkSql = "SELECT COUNT(*) FROM schedules WHERE date = @date AND subject = @subject AND teacher = @teacher AND time = @time AND room = @room";
                using (MySqlCommand checkCmd = new MySqlCommand(checkSql, conn))
                {
                    checkCmd.Parameters.AddWithValue("@date", txtDate.Text);
                    checkCmd.Parameters.AddWithValue("@subject", txtSubject.Text);
                    checkCmd.Parameters.AddWithValue("@teacher", txtTeacher.Text);
                    checkCmd.Parameters.AddWithValue("@time", txtTime.Text);
                    checkCmd.Parameters.AddWithValue("@room", txtRoom.Text);

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("A schedule already exists for this time slot. Please choose a different time.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Insert into database
                string sql = "INSERT INTO schedules (date, subject, teacher, time, room) VALUES (@date, @subject, @teacher, @time, @room)";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@date", txtDate.Text);
                    cmd.Parameters.AddWithValue("@subject", txtSubject.Text);
                    cmd.Parameters.AddWithValue("@teacher", txtTeacher.Text);
                    cmd.Parameters.AddWithValue("@time", txtTime.Text);
                    cmd.Parameters.AddWithValue("@room", txtRoom.Text);

                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Schedule Saved!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void addschedule_Load(object sender, EventArgs e)
        {
            txtDate.Text = selectedDate;
        }
    }
    }

