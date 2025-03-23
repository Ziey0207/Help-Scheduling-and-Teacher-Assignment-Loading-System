using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using MySql.Data.MySqlClient;

namespace Help_Scheduling_and_Teacher_Assignment_Loading_System
{
    public partial class Forgot : Form
    {
        private string generatedOTP; // Store OTP globally
        private string userEmail; // Store email globally

        public Forgot()
        {   
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connString = "server=localhost;database=school_management;uid=root;pwd=;";
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    // Check if email exists
                    string query = "SELECT COUNT(*) FROM admins WHERE email = @email";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count == 1)
                        {
                            // Generate a random 6-digit OTP
                            Random rand = new Random();
                            generatedOTP = rand.Next(100000, 999999).ToString();
                            userEmail = txtEmail.Text; // Store email for next step

                            // Send OTP via email
                            string senderEmail = "cakeslodi24@gmail.com";
                            string senderPassword = "lndmxsqfhzzbvlwy"; // Use App Password

                            MailMessage mail = new MailMessage();
                            mail.From = new MailAddress(senderEmail);
                            mail.To.Add(txtEmail.Text);
                            mail.Subject = "Your Password Reset OTP";
                            mail.Body = "Your OTP for password reset is: " + generatedOTP;

                            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                            smtp.Credentials = new NetworkCredential(senderEmail, senderPassword);
                            smtp.EnableSsl = true;
                            smtp.Send(mail);

                            MessageBox.Show("OTP sent to your email!");

                            // Open OTP Verification Form
                            OTP otpForm = new OTP(generatedOTP, userEmail);
                            otpForm.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Email not found!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
    }
}