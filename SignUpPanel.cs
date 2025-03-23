using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using Scheduling_and_Teacher_Loading_Assignment_System;

namespace Help_Scheduling_and_Teacher_Assignment_Loading_System
{
    public partial class SignUpPanel : UserControl
    {
        public SignUpPanel()
        {
            InitializeComponent();
            Home parent = (Home)this.Parent;
            ReShowPass.Image = Properties.Resources.eye_slash_solid;
            ReShowPass.Cursor = Cursors.Hand;
            Login.Cursor = Cursors.Hand;
            txtPassword.UseSystemPasswordChar = true;
            txtConfirmPassword.UseSystemPasswordChar = true;
        }

        private void Login_Click(object sender, EventArgs e)
        {
            Home parent = (Home)this.FindForm();

            if (parent != null)
            {
                this.Parent.Controls.Remove(this);
                parent.LoginFORM();
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string email = txtEmail.Text;
            string contactNumber = txtContactNumber.Text;
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            txtError.Text = "";
            txtError.Show();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(contactNumber) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                txtError.Text = "All Fields Are Required.";
                return;
            }
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                txtError.Text = "Invalid email format.";
                return;
            }

            if (!Regex.IsMatch(contactNumber, @"^\d{11}$"))
            {
                txtError.Text = "Contact number must be 10 digits.";
                return;
            }

            if (password != confirmPassword)
            {
                txtError.Text = "Passwords do not match.";
                return;
            }

            try
            {
                string query = "INSERT INTO admins (username, email, contact_number, password) VALUES (@username, @email, @contactNumber, @password)";
                MySqlParameter[] parameters =
                {
                    new MySqlParameter("@username", username),
                    new MySqlParameter("@email", email),
                    new MySqlParameter("@contactNumber", contactNumber),
                    new MySqlParameter("@password", password) // Save plain text password
                };

                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);

                if (rowsAffected > 0)
                {
                    txtError.Text = "Registration successful!";
                    // Optionally, clear the form fields
                    txtUsername.Text = "";
                    txtEmail.Text = "";
                    txtContactNumber.Text = "";
                    txtPassword.Text = "";
                    txtConfirmPassword.Text = "";
                }
            }
            catch (Exception ex)
            {
                txtError.Text = "Error: " + ex.Message;
            }
        }

        private void ReShowPass_Click(object sender, EventArgs e)
        {
            // Toggle password visibility
            if (txtPassword.UseSystemPasswordChar)
            {
                txtPassword.UseSystemPasswordChar = false;
                txtConfirmPassword.UseSystemPasswordChar = false;
                ReShowPass.Image = Properties.Resources.eye_solid; // Open eye icon
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true;
                txtConfirmPassword.UseSystemPasswordChar = true;
                ReShowPass.Image = Properties.Resources.eye_slash_solid; // Closed eye icon
            }
        }
    }
}