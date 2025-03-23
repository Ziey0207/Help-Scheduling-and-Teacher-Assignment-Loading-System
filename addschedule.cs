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
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();
                string sql = "INSERT INTO tbl_calendar (date, subject, teacher, time, room) VALUES (@date, @subject, @teacher, @time, @room)";
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
            MessageBox.Show("Schedule Saved!");
        }

        private void addschedule_Load(object sender, EventArgs e)
        {
            txtDate.Text = selectedDate;
        }
    }
    }

