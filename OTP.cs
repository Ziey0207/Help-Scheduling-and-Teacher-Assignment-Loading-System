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
    public partial class OTP : Form
    {
        private string correctOTP;
        private string userEmail;

        public OTP(string otp, string email)
        {
            InitializeComponent();
            correctOTP = otp;
            userEmail = email;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtOTP.Text == correctOTP)
            {
                MessageBox.Show("OTP Verified! Please set a new password.");

                // Open Reset Password Form
                Resetpass resetForm = new Resetpass(userEmail);
                resetForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid OTP. Please try again.");
            }
        }
    }
}
