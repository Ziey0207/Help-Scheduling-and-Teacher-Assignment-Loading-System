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

            addschedule sched = new addschedule(selectedDate);
            sched.ShowDialog();  // Use ShowDialog instead of Show to prevent duplicate instances
            displayEvent();
        }

        private void lblSchedule_Click(object sender, EventArgs e)
        {
      
        }

        public void displayEvent()
        {
            lbevent.Text = ""; // Clear previous events before loading new ones.

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();

                string fullDate = currentDate.ToString("yyyy-MM-dd"); // Gamitin ang tamang petsa

                string sql = "SELECT subject, teacher, time, room FROM schedules WHERE date = @date";
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
                                eventDetails.AppendLine($"{reader["subject"]}");
                                eventDetails.AppendLine($"{reader["teacher"]}");
                                eventDetails.AppendLine($"{reader["time"]}");
                                eventDetails.AppendLine($"{reader["room"]}");
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

        }
    }
}
