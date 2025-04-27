using MySql.Data.MySqlClient;
using Scheduling_and_Teacher_Loading_Assignment_System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Help_Scheduling_and_Teacher_Assignment_Loading_System
{
    public partial class Home : Form
    {
        private const string SESSION_SETTING = "CurrentSessionToken";
        private const string REMEMBER_ME_SETTING = "RememberMe";

        public Home()
        {
            InitializeComponent();

            Debug.WriteLine("Home form loaded");

            ShowPass.Image = Properties.Resources.eye_slash_solid;
            ShowPass.Cursor = SignUp.Cursor = ForgotPassword.Cursor = Cursors.Hand;
            txtLoginPass.UseSystemPasswordChar = true;

            DatabaseHelper dhelper = new DatabaseHelper();

            if (!dhelper.TestConnection())
            {
                Debug.WriteLine("DB connection failed");
                MessageBox.Show("Failed to connect to the database. Please check your settings.");
                Environment.Exit(1);
                return;
            }

            Debug.WriteLine("DB connection successful");
            CheckAutoLogin();
        }

        private void CheckAutoLogin()
        {
            if (Properties.Settings.Default.RememberMe &&
                !string.IsNullOrEmpty(Properties.Settings.Default.CurrentSessionToken))
            {
                string oldToken = Properties.Settings.Default.CurrentSessionToken;

                Task.Run(() =>
                {
                    // Get user ID from old token
                    int? userId = DatabaseHelper.GetAdminIdBySession(oldToken);

                    if (userId.HasValue && DatabaseHelper.IsValidSession(oldToken))
                    {
                        // Create brand new session
                        string newToken = DatabaseHelper.CreateNewSession(userId.Value, true);

                        // Update settings with new token
                        Properties.Settings.Default.CurrentSessionToken = newToken;
                        Properties.Settings.Default.Save();

                        this.Invoke((MethodInvoker)delegate
                        {
                            OpenDashboard(newToken);
                        });
                    }
                    else
                    {
                        // Clear invalid session
                        this.Invoke((MethodInvoker)delegate
                        {
                            Properties.Settings.Default.CurrentSessionToken = "";
                            Properties.Settings.Default.Save();
                        });
                    }
                });
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
                ShowPass.Image = Properties.Resources.hide_solid_24; // Open eye icon
            }
            else
            {
                txtLoginPass.UseSystemPasswordChar = true;
                ShowPass.Image = Properties.Resources.show_solid_24; // Closed eye icon
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Login button clicked");
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
                Debug.WriteLine($"Attempting login for user: {username}");

                // Verify credentials
                int userId = DatabaseHelper.GetUserId(username);
                if (userId == 0)
                {
                    Debug.WriteLine("User not found");
                    txtLoginError.Text = "Username does not exist";
                    return;
                }

                // If user exists, check password
                string dbPassword = DatabaseHelper.ExecuteScalar(
                    "SELECT password FROM admins WHERE id = @id",
                    new MySqlParameter[] { new MySqlParameter("@id", userId) })?.ToString();

                if (dbPassword == password) // Consider using secure password comparison
                {
                    Debug.WriteLine("Password verified, creating session");
                    // Create and record session
                    string newToken = DatabaseHelper.CreateNewSession(userId, chkRememberMe.Checked);

                    // Store new token if Remember Me is checked
                    Properties.Settings.Default.CurrentSessionToken = chkRememberMe.Checked ? newToken : "";
                    Properties.Settings.Default.RememberMe = chkRememberMe.Checked;
                    Properties.Settings.Default.Save();

                    Debug.WriteLine($"Session created - RememberMe: {chkRememberMe.Checked}, Token: {newToken}");
                    OpenDashboard(newToken);
                }
                else
                {
                    Debug.WriteLine("Incorrect password");
                    txtLoginError.Text = "Incorrect password";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Login error: {ex.Message}");
                txtLoginError.Text = "Error: " + ex.Message;
            }
        }

        private void OpenDashboard(string sessionToken)
        {
            this.Hide();
            var dashboard = new AdminDashboard(sessionToken);
            dashboard.Show();
        }

        private void ForgotPassword_Click(object sender, EventArgs e)
        {
            // is ricky na dito

            Forgot fr = new Forgot();
            fr.FormClosed += (s, args) =>
            {
                // Bring focus back to login form after password reset
                this.Show();
            };
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
        }

        private void ShowPass_MouseLeave(object sender, EventArgs e)
        {
        }

        private void Home_FormClosing(object sender, FormClosingEventArgs e)
        {
            Debug.WriteLine("Home form closing");

            string sessionToken = Properties.Settings.Default[SESSION_SETTING]?.ToString();
            if (!string.IsNullOrEmpty(sessionToken))
            {
                Debug.WriteLine($"Recording logout for token: {sessionToken}");
                DatabaseHelper.RecordLogout(sessionToken);
            }

            // Only exit completely if this is the main close action
            if (Application.OpenForms.Count <= 1)
            {
                Debug.WriteLine("Closing application completely");
                Application.Exit();
            }
        }

        private void ShowPass_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {

        }

        private void Home_Load(object sender, EventArgs e)
        {
            // Toggle password visibility
            if (txtLoginPass.UseSystemPasswordChar)
            {
                txtLoginPass.UseSystemPasswordChar = false;
                ShowPass.Image = Properties.Resources.hide_solid_24; // Open eye icon
            }
            else
            {
                txtLoginPass.UseSystemPasswordChar = true;
                ShowPass.Image = Properties.Resources.show_solid_24; // Closed eye icon
            }
        }
    }
}