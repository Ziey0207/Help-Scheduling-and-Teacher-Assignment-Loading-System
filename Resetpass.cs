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
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Help_Scheduling_and_Teacher_Assignment_Loading_System
{
    public partial class Resetpass : Form
    {
        private string correctOTP;
        private string userEmail;


        public Resetpass(string email)
        {
            InitializeComponent();
            userEmail = email;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtNewPassword.Text == txtConfirmPassword.Text)
            {
                string connString = "server=localhost;database=school_management;uid=root;pwd=;";
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    try
                    {
                        conn.Open();
                        string query = "UPDATE admins SET password = @password WHERE email = @userEmail";
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@password", txtNewPassword.Text);
                            cmd.Parameters.AddWithValue("@userEmail", userEmail);
                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Password successfully updated! You can now log in.");
                        this.Close(); // Close the reset form
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Passwords do not match!");
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            OTP tp = new OTP(correctOTP, userEmail);
            tp.Show();
        }
    }
}
