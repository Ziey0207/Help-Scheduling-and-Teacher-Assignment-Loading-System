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

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                txtLoginError.Text = "Username and Password Required";
                return;
            }

            try
            {
                string query = "SELECT password FROM admins WHERE username = @username";
                MySqlParameter[] parameters =
                    {
                    new MySqlParameter("username", username)
                };

                string dbPassword = DatabaseHelper.ExecuteScalar(query, parameters)?.ToString();

                if (dbPassword != null && dbPassword == password)
                {
                    txtLoginError.Text = "Login Succesful";

                    AdminDashboard adminDashboard = new AdminDashboard();
                    adminDashboard.FormClosed += (s, args) => this.Close();
                    adminDashboard.Show();
                    this.Hide();
                }
                else
                {
                    txtLoginError.Text = "Invalid Username Or Password";
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
    }
}