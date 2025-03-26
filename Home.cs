using MySql.Data.MySqlClient;
using Scheduling_and_Teacher_Loading_Assignment_System;
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
    public partial class Home : Form
    {
        private bool mouseDown;
        private Point lastLocation;

        public Home()
        {
            InitializeComponent();
            ShowPass.Image = Properties.Resources.eye_slash_solid;
            ShowPass.Cursor = Cursors.Hand;
            SignUp.Cursor = Cursors.Hand;
            ForgotPassword.Cursor = Cursors.Hand;
            txtLoginPass.UseSystemPasswordChar = true;
            DatabaseHelper dhelper = new DatabaseHelper();
            if (!dhelper.TestConnection())
            {
                MessageBox.Show("Failed to connect to the database. Please check your settings.");
                Environment.Exit(1);
            }
        }

        public void LoginFORM()
        {
            LoginPanel.Show();
            foreach (Control control in LoginPanel.Controls)
            {
                control.Show();
            }

            if (txtLoginError.Text == "")
            {
                txtLoginError.Hide();
            }
        }

        private void SignUp_Click(object sender, EventArgs e)
        {
            foreach (Control control in LoginPanel.Controls)
            {
                control.Hide();
            }
            LoginPanel.Hide();
            SignUpPanel SignUp = new SignUpPanel();
            panel1.Controls.Add(SignUp);
            SignUp.Location = new Point(0, 0);
        }

        private void ShowPass_Click(object sender, EventArgs e)
        {
            // Toggle password visibility
            if (txtLoginPass.UseSystemPasswordChar)
            {
                txtLoginPass.UseSystemPasswordChar = false;
                ShowPass.Image = Properties.Resources.eye_solid; // Open eye icon
            }
            else
            {
                txtLoginPass.UseSystemPasswordChar = true;
                ShowPass.Image = Properties.Resources.eye_slash_solid; // Closed eye icon
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtLoginUser.Text;
            string password = txtLoginPass.Text;

            txtLoginError.Text = "";
            txtLoginError.Show();

            if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
            {
                txtLoginError.Text = "Username and Password Required";
                return;
            }
            else if (string.IsNullOrEmpty(username))
            {
                txtLoginError.Text = "Username Required";
                return;
            }
            else if (string.IsNullOrEmpty(password))
            {
                txtLoginError.Text = "Password Required";
                return;
            }

            try
            {
                // First check if user exists
                string userExistsQuery = "SELECT COUNT(*) FROM admins WHERE username = @username";
                MySqlParameter[] userExistsParams =
                {
                    new MySqlParameter("username", username)
                };

                int userCount = Convert.ToInt32(DatabaseHelper.ExecuteScalar(userExistsQuery, userExistsParams));

                if (userCount == 0)
                {
                    txtLoginError.Text = "Username does not exist";
                    return;
                }

                // If user exists, check password
                string passwordQuery = "SELECT password FROM admins WHERE username = @username";
                MySqlParameter[] passwordParams =
                {
                  new MySqlParameter("username", username)
                };

                string dbPassword = DatabaseHelper.ExecuteScalar(passwordQuery, passwordParams)?.ToString();

                if (dbPassword == password) // Consider using secure password comparison
                {
                    txtLoginError.Text = "Login Successful";

                    AdminDashboard adminDashboard = new AdminDashboard();
                    adminDashboard.FormClosed += (s, args) => this.Close();
                    adminDashboard.Show();
                    this.Hide();
                }
                else
                {
                    txtLoginError.Text = "Incorrect password";
                }
            }
            catch (Exception ex)
            {
                txtLoginError.Text = "Error: " + ex.Message;
            }
        }

        private void ForgotPassword_Click(object sender, EventArgs e)
        {
            // is ricky na dito

            Forgot fr = new Forgot();
            fr.Show();
        }

        private void txtLoginPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Trigger the search
                btnLogin_Click(sender, e);
                e.SuppressKeyPress = true; // Prevent the "ding" sound
            }
        }

        private void SignUp_MouseEnter(object sender, EventArgs e)
        {
            SignUp.BackColor = Color.FromArgb(19, 15, 64);
        }

        private void SignUp_MouseLeave(object sender, EventArgs e)
        {
            SignUp.BackColor = Color.FromArgb(48, 51, 107);
        }

        private void ForgotPassword_MouseEnter(object sender, EventArgs e)
        {
            ForgotPassword.BackColor = Color.FromArgb(19, 15, 64);
        }

        private void ForgotPassword_MouseLeave(object sender, EventArgs e)
        {
            ForgotPassword.BackColor = Color.FromArgb(48, 51, 107);
        }

        private void ShowPass_MouseEnter(object sender, EventArgs e)
        {
            ShowPass.BackColor = Color.FromArgb(19, 15, 64);
        }

        private void ShowPass_MouseLeave(object sender, EventArgs e)
        {
            ShowPass.BackColor = Color.FromArgb(48, 51, 107);
        }
    }
}