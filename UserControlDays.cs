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

        public UserControlDays()
        {
            InitializeComponent();
        }

        private void UserControlDays_Load(object sender, EventArgs e)
        {

        }

        public void days(int numday)
        {
            lbdays.Text = numday + "";
        }

        private void lbdays_Click(object sender, EventArgs e)
        {

        }

        private void UserControlDays_Click(object sender, EventArgs e)
        {
            timer1.Start();
            string selectedDate = ScheduleCalendar.static_year + "-" + ScheduleCalendar.static_month.ToString("D2") + "-" + lbdays.Text.PadLeft(2, '0');

            addschedule sched = new addschedule(selectedDate); // Ipadala ang buong date
            sched.Show();
        }

        private void lblSchedule_Click(object sender, EventArgs e)
        {
      
        }

        public void displayEvent()
        {
            lbevent.Text = ""; // Clear previous event before loading new one

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();

                // Construct the full date using the selected year, month, and day
                string fullDate = $"{ScheduleCalendar.static_year}-{ScheduleCalendar.static_month:00}-{lbdays.Text.PadLeft(2, '0')}";

                // Query to fetch events for the specific date
                string sql = "SELECT subject, teacher, time, room FROM tbl_calendar WHERE date = @date";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@date", fullDate); // Use the full date as a parameter
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Display the event details
                            lbevent.Text = $"Subject: {reader["subject"]}\n" +
                                           $"Teacher: {reader["teacher"]}\n" +
                                           $"Time: {reader["time"]}\n" +
                                           $"Room: {reader["room"]}";
                        }
                        else
                        {
                            lbevent.Text = "No events scheduled."; // Display if no events are found
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

        }
    }
}
