using MySql.Data.MySqlClient;
using Scheduling_and_Teacher_Loading_Assignment_System;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Help_Scheduling_and_Teacher_Assignment_Loading_System
{
    public partial class SignUpPanel : UserControl
    {
        private DateTime _lastUsernameCheck = DateTime.MinValue;
        private DateTime _lastEmailCheck = DateTime.MinValue;
        private DateTime _lastContactCheck = DateTime.MinValue;
        private bool _passwordValid = false;
        private bool _confirmPasswordValid = false;
        private const int DEBOUNCE_DELAY_MS = 300; // milliseconds

        public SignUpPanel()
        {
            InitializeComponent();
            Home parent = (Home)this.Parent;
            ReShowPass.Image = Properties.Resources.eye_slash_solid;

            ReShowPass.Cursor = Cursors.Hand;
            Login.Cursor = Cursors.Hand;

            txtPassword.UseSystemPasswordChar = true;
            txtConfirmPassword.UseSystemPasswordChar = true;

            txtUsername.MaxLength = 50;
            txtEmail.MaxLength = 100;
            txtContactNumber.MaxLength = 11;
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
            // Check if all validations passed
            if (lblUsernameStatus.ForeColor != Color.Green ||
                lblEmailStatus.ForeColor != Color.Green ||
                lblContactStatus.ForeColor != Color.Green)
            {
                txtError.Text = "Please complete all fields correctly";
                txtError.ForeColor = Color.Red;
                return;
            }
            if (!_passwordValid)
            {
                txtError.Text = "Password doesn't meet all requirements";
                txtError.ForeColor = Color.Red;
                return;
            }

            if (!_confirmPasswordValid)
            {
                txtError.Text = "Passwords don't match";
                txtError.ForeColor = Color.Red;
                return;
            }

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
                txtError.Text = "Contact number must be 11 digits.";
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
                    txtUsername.Text = txtEmail.Text = txtContactNumber.Text = txtPassword.Text = txtConfirmPassword.Text = "";
                    lblUsernameStatus.Text = lblEmailStatus.Text = lblContactStatus.Text = lblPasswordStatus.Text = lblConfirmPassStatus.Text = lblPasswordHelp.Text = "";
                    lblUsernameStatus.ForeColor = lblEmailStatus.ForeColor = lblContactStatus.ForeColor = lblPasswordStatus.ForeColor = lblConfirmPassStatus.ForeColor = Color.FromArgb(48, 51, 107);
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

        private void Login_MouseEnter(object sender, EventArgs e)
        {
            Login.BackColor = Color.FromArgb(19, 15, 64);
        }

        private void Login_MouseLeave(object sender, EventArgs e)
        {
            Login.BackColor = Color.FromArgb(48, 51, 107);
        }

        private void txtContactNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            // Block all other non-digit characters
            if (txtContactNumber.Text.Length >= 11 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void ReShowPass_MouseEnter(object sender, EventArgs e)
        {
            ReShowPass.BackColor = Color.FromArgb(19, 15, 64);
        }

        private void ReShowPass_MouseLeave(object sender, EventArgs e)
        {
            ReShowPass.BackColor = Color.FromArgb(48, 51, 107);
        }

        private async void txtUsername_KeyUp(object sender, KeyEventArgs e)
        {
            // Debounce check - only proceed if no new keypress in last 300ms
            _lastUsernameCheck = DateTime.Now;
            await Task.Delay(DEBOUNCE_DELAY_MS);
            if ((DateTime.Now - _lastUsernameCheck).TotalMilliseconds < DEBOUNCE_DELAY_MS)
                return;

            string username = txtUsername.Text.Trim();

            // Clear status if empty
            if (string.IsNullOrEmpty(username))
            {
                lblUsernameStatus.Text = "";
                return;
            }

            // Show checking status

            lblUsernameStatus.Text = "Checking...";
            lblUsernameStatus.ForeColor = Color.Blue;

            //Minimum legnth check
            if (username.Length < 4)
            {
                lblUsernameStatus.Text = "Too short (min 4)";
                lblUsernameStatus.ForeColor = Color.Orange;
                return;
            }

            //Database check
            try
            {
                string query = "SELECT COUNT(*) FROM admins WHERE username = @username";
                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@username", username)
                };

                int count = Convert.ToInt32(await DatabaseHelper.ExecuteScalarAsync(query, parameters));

                lblUsernameStatus.Text = count > 0 ? "❌ Taken" : "✓ Available";
                lblUsernameStatus.ForeColor = count > 0 ? Color.Red : Color.Green;
            }
            catch (Exception ex)
            {
                lblUsernameStatus.Text = "⚠ Error checking";
                lblUsernameStatus.ForeColor = Color.Red;
                Console.WriteLine($"Username check error: {ex.Message}");
            }
        }

        private async void txtEmail_KeyUp(object sender, KeyEventArgs e)
        {
            // Debounce check
            _lastEmailCheck = DateTime.Now;
            await Task.Delay(DEBOUNCE_DELAY_MS);
            if ((DateTime.Now - _lastEmailCheck).TotalMilliseconds < DEBOUNCE_DELAY_MS)
                return;

            string email = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(email))
            {
                lblEmailStatus.Text = "";
                return;
            }

            lblEmailStatus.Text = "Checking...";
            lblEmailStatus.ForeColor = Color.Blue;

            // Email format validation
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                lblEmailStatus.Text = "Invalid format";
                lblEmailStatus.ForeColor = Color.Orange;
                return;
            }

            try
            {
                string query = "SELECT COUNT(*) FROM admins WHERE email = @email";
                var parameters = new[] { new MySqlParameter("@email", email) };

                int count = Convert.ToInt32(await DatabaseHelper.ExecuteScalarAsync(query, parameters));

                lblEmailStatus.Text = count > 0 ? "❌ Registered" : "✓ Available";
                lblEmailStatus.ForeColor = count > 0 ? Color.Red : Color.Green;
            }
            catch (Exception ex)
            {
                lblEmailStatus.Text = "Check failed";
                lblEmailStatus.ForeColor = Color.Red;
                Debug.WriteLine($"Email check error: {ex.Message}");
            }
        }

        private async void txtContactNumber_KeyUp(object sender, KeyEventArgs e)
        {
            // Debounce check
            _lastContactCheck = DateTime.Now;
            await Task.Delay(DEBOUNCE_DELAY_MS);
            if ((DateTime.Now - _lastContactCheck).TotalMilliseconds < DEBOUNCE_DELAY_MS)
                return;

            string number = txtContactNumber.Text.Trim();

            if (string.IsNullOrEmpty(number))
            {
                lblContactStatus.Text = "";
                return;
            }

            lblContactStatus.Text = "Checking...";
            lblContactStatus.ForeColor = Color.Blue;

            // Number validation
            if (!Regex.IsMatch(number, @"^\d+$"))
            {
                lblContactStatus.Text = "Digits only";
                lblContactStatus.ForeColor = Color.Orange;
                return;
            }

            if (number.Length > 11)
            {
                lblContactStatus.Text = "Max 11 digits";
                lblContactStatus.ForeColor = Color.Orange;
                return;
            }

            try
            {
                string query = "SELECT COUNT(*) FROM admins WHERE contact_number = @contact";
                var parameters = new[] { new MySqlParameter("@contact", number) };

                int count = Convert.ToInt32(await DatabaseHelper.ExecuteScalarAsync(query, parameters));

                lblContactStatus.Text = count > 0 ? "❌ Registered" : number.Length == 11 ? "✓ Valid" : "✓ Typing...";
                lblContactStatus.ForeColor = count > 0 ? Color.Red :
                                          number.Length == 11 ? Color.Green : Color.Blue;
            }
            catch (Exception ex)
            {
                lblContactStatus.Text = "Check failed";
                lblContactStatus.ForeColor = Color.Red;
                Debug.WriteLine($"Contact check error: {ex.Message}");
            }
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            string password = txtPassword.Text;

            // Check password requirements
            bool hasMinLength = password.Length >= 8;
            bool hasNumber = password.Any(char.IsDigit);
            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasSpecial = password.Any(ch => !char.IsLetterOrDigit(ch));

            // Update validation status
            _passwordValid = hasMinLength && hasNumber && hasUpper && hasLower && hasSpecial;

            if (string.IsNullOrEmpty(password))
            {
                lblPasswordStatus.Text = "";
            }
            else
            {
                lblPasswordStatus.Text = _passwordValid ? "✓ Strong" : "⚠ Weak";
                lblPasswordStatus.ForeColor = _passwordValid ? Color.Green : Color.Red;

                // Show detailed requirements
                lblPasswordHelp.Text =
                    (hasMinLength ? "✓ 8+ characters " : "✗ 8+ characters ") +
                    (hasNumber ? "✓ Contains number " : "✗ Contains number ") +
                    (hasUpper ? "✓ Uppercase letter " : "✗ Uppercase letter ") +
                    (hasLower ? "✓ Lowercase letter " : "✗ Lowercase letter ") +
                    (hasSpecial ? "✓ Special character " : "✗ Special character ");
            }

            // Trigger confirm password revalidation
            txtConfirmPassword_KeyUp(null, null);
        }

        private void txtConfirmPassword_KeyUp(object sender, KeyEventArgs e)
        {
            string password = txtPassword.Text;
            string confirm = txtConfirmPassword.Text;

            // Update validation status
            _confirmPasswordValid = password == confirm && !string.IsNullOrEmpty(confirm);

            // Update UI feedback
            lblConfirmPassStatus.Text = string.IsNullOrEmpty(confirm) ? "" :
                                  _confirmPasswordValid ? "✓ Match" : "⚠ No match";
            lblConfirmPassStatus.ForeColor = _confirmPasswordValid ? Color.Green : Color.Red;
        }

        private void SignUpPanel_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Left;
        }
    }
}