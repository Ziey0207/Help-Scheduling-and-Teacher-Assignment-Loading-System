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
        private int? scheduleId;
        public addschedule(string date, int? id = null)
        {
            InitializeComponent();
            selectedDate = date;
            scheduleId = id;

            txtDate.Text = selectedDate; // Set the date field

            if (scheduleId.HasValue) // If editing, load the details
            {
                LoadScheduleDetails(scheduleId.Value);
            }
        }

        private void LoadScheduleDetails(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();
                string sql = "SELECT * FROM schedules WHERE id = @id";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtDate.Text = reader["date"].ToString();
                            txtSubject.Text = reader["subject"].ToString();
                            txtTeacher.Text = reader["teacher"].ToString();
                            txtTimeIn.Text = reader["time_in"].ToString();
                            txtTimeOut.Text = reader["time_out"].ToString();
                            txtRoom.Text = reader["room"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("No schedule found for editing.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

            private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDate.Text) ||
            string.IsNullOrWhiteSpace(txtSubject.Text) ||
            string.IsNullOrWhiteSpace(txtTeacher.Text) ||
            string.IsNullOrWhiteSpace(txtTimeIn.Text) ||
            string.IsNullOrWhiteSpace(txtTimeOut.Text) ||
            string.IsNullOrWhiteSpace(txtRoom.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();

                string sql;
                if (scheduleId.HasValue) // Updating existing record
                {
                    sql = "UPDATE schedules SET subject = @subject, teacher = @teacher, time_in = @time_in, time_out = @time_out, room = @room WHERE id = @id";
                }
                else // Adding new record
                {
                    sql = "INSERT INTO schedules (date, subject, teacher, time_in, time_out, room) VALUES (@date, @subject, @teacher, @time_in, @time_out, @room)";
                }

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@date", txtDate.Text);
                    cmd.Parameters.AddWithValue("@subject", txtSubject.Text);
                    cmd.Parameters.AddWithValue("@teacher", txtTeacher.Text);
                    cmd.Parameters.AddWithValue("@time_in", txtTimeIn.Text);
                    cmd.Parameters.AddWithValue("@time_out", txtTimeOut.Text);
                    cmd.Parameters.AddWithValue("@room", txtRoom.Text);

                    if (scheduleId.HasValue) // Update mode
                    {
                        cmd.Parameters.AddWithValue("@id", scheduleId.Value);
                    }

                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show(scheduleId.HasValue ? "Schedule Updated!" : "Schedule Saved!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close(); // Close the form after saving
        }

        private void addschedule_Load(object sender, EventArgs e)
        {
            txtDate.Text = selectedDate;
        }

        private void button2_Click(object sender, EventArgs e)
        {
          
    }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete this schedule?",
                                          "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    string sql = "DELETE FROM schedules WHERE date = @date AND subject = @subject AND teacher = @teacher AND time_in = @time_in AND time_out = @time_out AND room = @room";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@date", txtDate.Text);
                        cmd.Parameters.AddWithValue("@subject", txtSubject.Text);
                        cmd.Parameters.AddWithValue("@teacher", txtTeacher.Text);
                        cmd.Parameters.AddWithValue("@time_in", txtTimeIn.Text);
                        cmd.Parameters.AddWithValue("@time_out", txtTimeOut.Text);
                        cmd.Parameters.AddWithValue("@room", txtRoom.Text);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Schedule Deleted!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("No schedule found to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
        }
    }

   
    
    

