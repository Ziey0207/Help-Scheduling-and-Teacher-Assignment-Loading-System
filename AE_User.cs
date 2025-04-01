using MySql.Data.MySqlClient;
using Scheduling_and_Teacher_Loading_Assignment_System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Help_Scheduling_and_Teacher_Assignment_Loading_System.AddEdit_userControls
{
    public partial class AE_User : UserControl
    {
        private bool _editMode = false;
        private int _currentId = -1;

        private DateTime _lastUsernameCheck = DateTime.MinValue;
        private DateTime _lastEmailCheck = DateTime.MinValue;
        private DateTime _lastContactCheck = DateTime.MinValue;
        private const int DEBOUNCE_DELAY_MS = 300; // milliseconds

        public event Action DataSaved;

        public AE_User()
        {
            InitializeComponent();
            SetAddMode();
            SetupValidation();
        }

        private void SetupValidation()
        {
            // Set max lengths
            txtUsername.MaxLength = 50;
            txtEmail.MaxLength = 100;
            txtContact.MaxLength = 11;

            // Attach event handlers for real-time validation
            txtUsername.KeyUp += async (sender, e) => await ValidateUsername();
            txtEmail.KeyUp += async (sender, e) => await ValidateEmail();
            txtContact.KeyUp += async (sender, e) => await ValidateContact();
            txtPassword.KeyUp += (sender, e) => ValidatePassword();

            // Only allow digits in contact field
            txtContact.KeyPress += (sender, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };
        }

        public void SetAddMode()
        {
            _editMode = false;
            _currentId = -1;

            // Update UI for add mode
            btnSave.Text = "Add";
            btnCancel.Visible = false;

            // Clear fields
            txtUsername.Text = "";
            txtEmail.Text = "";
            txtContact.Text = "";
            txtPassword.Text = "";

            lblErrorContact.Text = lblErrorEmail.Text = lblErrorPass.Text =
            lblErrorUsername.Text = lblformError.Text = lblPasswordRequirements.Text = "";

            // Reset fields
        }

        public void SetEditMode(int id, DataRow data)
        {
            _editMode = true;
            _currentId = id;

            // Update UI for edit mode
            btnSave.Text = "Update";
            btnCancel.Visible = true;

            // Populate fields from data
            txtUsername.Text = data["username"].ToString();
            txtEmail.Text = data["email"].ToString();
            txtContact.Text = data["contact_number"].ToString();
            txtPassword.Text = data["password"].ToString();

            // Trigger validation for populated fields
            ValidateUsername();
            ValidateEmail();
            ValidateContact();
            ValidatePassword();
        }

        private async Task ValidateUsername()
        {
            // Debounce check
            _lastUsernameCheck = DateTime.Now;
            await Task.Delay(DEBOUNCE_DELAY_MS);
            if ((DateTime.Now - _lastUsernameCheck).TotalMilliseconds < DEBOUNCE_DELAY_MS)
                return;

            string username = txtUsername.Text.Trim();

            // Clear status if empty
            if (string.IsNullOrEmpty(username))
            {
                lblErrorUsername.Text = "";
                return;
            }

            // Show checking status
            lblErrorUsername.Text = "Checking...";
            lblErrorUsername.ForeColor = Color.Blue;

            // Minimum length check
            if (username.Length < 4)
            {
                lblErrorUsername.Text = "Too short (min 4)";
                lblErrorUsername.ForeColor = Color.Orange;
                return;
            }

            // Database check (skip if in edit mode and username hasn't changed)
            try
            {
                string query = _editMode
                    ? "SELECT COUNT(*) FROM admins WHERE username = @username AND id != @id"
                    : "SELECT COUNT(*) FROM admins WHERE username = @username";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@username", username),
                    new MySqlParameter("@id", _currentId)
                };

                int count = Convert.ToInt32(await DatabaseHelper.ExecuteScalarAsync(query, parameters));

                lblErrorUsername.Text = count > 0 ? "❌ Username taken" : "✓ Available";
                lblErrorUsername.ForeColor = count > 0 ? Color.Red : Color.Green;
            }
            catch (Exception ex)
            {
                lblErrorUsername.Text = "⚠ Error checking username";
                lblErrorUsername.ForeColor = Color.Red;
                Console.WriteLine($"Username check error: {ex.Message}");
            }
        }

        private async Task ValidateEmail()
        {
            // Debounce check
            _lastEmailCheck = DateTime.Now;
            await Task.Delay(DEBOUNCE_DELAY_MS);
            if ((DateTime.Now - _lastEmailCheck).TotalMilliseconds < DEBOUNCE_DELAY_MS)
                return;

            string email = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(email))
            {
                lblErrorEmail.Text = "";
                return;
            }

            lblErrorEmail.Text = "Checking...";
            lblErrorEmail.ForeColor = Color.Blue;

            // Email format validation
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                lblErrorEmail.Text = "Invalid email format";
                lblErrorEmail.ForeColor = Color.Orange;
                return;
            }

            try
            {
                string query = _editMode
                    ? "SELECT COUNT(*) FROM admins WHERE email = @email AND id != @id"
                    : "SELECT COUNT(*) FROM admins WHERE email = @email";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@email", email),
                    new MySqlParameter("@id", _currentId)
                };

                int count = Convert.ToInt32(await DatabaseHelper.ExecuteScalarAsync(query, parameters));

                lblErrorEmail.Text = count > 0 ? "❌ Email registered" : "✓ Available";
                lblErrorEmail.ForeColor = count > 0 ? Color.Red : Color.Green;
            }
            catch (Exception ex)
            {
                lblErrorEmail.Text = "⚠ Error checking email";
                lblErrorEmail.ForeColor = Color.Red;
                Console.WriteLine($"Email check error: {ex.Message}");
            }
        }

        private async Task ValidateContact()
        {
            // Debounce check
            _lastContactCheck = DateTime.Now;
            await Task.Delay(DEBOUNCE_DELAY_MS);
            if ((DateTime.Now - _lastContactCheck).TotalMilliseconds < DEBOUNCE_DELAY_MS)
                return;

            string number = txtContact.Text.Trim();

            if (string.IsNullOrEmpty(number))
            {
                lblErrorContact.Text = "";
                return;
            }

            lblErrorContact.Text = "Checking...";
            lblErrorContact.ForeColor = Color.Blue;

            // Number validation
            if (!Regex.IsMatch(number, @"^\d+$"))
            {
                lblErrorContact.Text = "Digits only";
                lblErrorContact.ForeColor = Color.Orange;
                return;
            }

            if (number.Length > 11)
            {
                lblErrorContact.Text = "Max 11 digits";
                lblErrorContact.ForeColor = Color.Orange;
                return;
            }

            try
            {
                string query = _editMode
                    ? "SELECT COUNT(*) FROM admins WHERE contact_number = @contact AND id != @id"
                    : "SELECT COUNT(*) FROM admins WHERE contact_number = @contact";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@contact", number),
                    new MySqlParameter("@id", _currentId)
                };

                int count = Convert.ToInt32(await DatabaseHelper.ExecuteScalarAsync(query, parameters));

                lblErrorContact.Text = count > 0 ? "❌ Number registered" :
                                      number.Length == 11 ? "✓ Valid" : "✓ Typing...";
                lblErrorContact.ForeColor = count > 0 ? Color.Red :
                                          number.Length == 11 ? Color.Green : Color.Blue;
            }
            catch (Exception ex)
            {
                lblErrorContact.Text = "⚠ Error checking contact";
                lblErrorContact.ForeColor = Color.Red;
                Console.WriteLine($"Contact check error: {ex.Message}");
            }
        }

        private void ValidatePassword()
        {
            string password = txtPassword.Text;

            // Check password requirements
            bool hasMinLength = password.Length >= 8;
            bool hasNumber = password.Any(char.IsDigit);
            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasSpecial = password.Any(ch => !char.IsLetterOrDigit(ch));

            if (string.IsNullOrEmpty(password))
            {
                lblErrorPass.Text = "";
            }
            else
            {
                lblErrorPass.Text = (hasMinLength && hasNumber && hasUpper && hasLower && hasSpecial)
                    ? "✓ Strong password"
                    : "⚠ Weak password";

                lblErrorPass.ForeColor = (hasMinLength && hasNumber && hasUpper && hasLower && hasSpecial)
                    ? Color.Green
                    : Color.Red;

                // Show detailed requirements
                lblPasswordRequirements.Text =
                    (hasMinLength ? "✓ 8+ characters\n" : "✗ 8+ characters\n") +
                    (hasNumber ? "✓ Contains number\n" : "✗ Contains number\n") +
                    (hasUpper ? "✓ Uppercase letter\n" : "✗ Uppercase letter\n") +
                    (hasLower ? "✓ Lowercase letter\n" : "✗ Lowercase letter\n") +
                    (hasSpecial ? "✓ Special character" : "✗ Special character");
            }
        }

        private void AE_User_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetAddMode();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validate all fields before saving
            if (!AllFieldsValid())
            {
                lblformError.Text = "Please correct all fields before saving";
                lblformError.ForeColor = Color.Red;
                return;
            }

            try
            {
                string query = _editMode
                    ? @"UPDATE admins SET
                        username = @username,
                        email = @email,
                        contact_number = @contact,
                        password = @password
                        WHERE id = @id"
                    : @"INSERT INTO admins
                        (username, email, contact_number, password)
                        VALUES (@username, @email, @contact, @password)";

                MySqlParameter[] parameters =
                {
                    new MySqlParameter("@username", txtUsername.Text.Trim()),
                    new MySqlParameter("@email", txtEmail.Text.Trim()),
                    new MySqlParameter("@contact", txtContact.Text.Trim()),
                    new MySqlParameter("@password", txtPassword.Text),
                    new MySqlParameter("@id", _currentId)
                };

                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);

                if (rowsAffected > 0)
                {
                    lblformError.Text = _editMode ? "User updated successfully!" : "User added successfully!";
                    lblformError.ForeColor = Color.Green;

                    if (!_editMode)
                    {
                        SetAddMode(); // Reset form after successful add
                    }

                    DataSaved?.Invoke(); // Notify parent form
                }
            }
            catch (Exception ex)
            {
                lblformError.Text = "Error: " + ex.Message;
                lblformError.ForeColor = Color.Red;
            }
        }

        private bool AllFieldsValid()
        {
            // Check if all validation labels show success
            bool usernameValid = lblErrorUsername.Text.StartsWith("✓") ||
                               (_editMode && string.IsNullOrEmpty(lblErrorUsername.Text));
            bool emailValid = lblErrorEmail.Text.StartsWith("✓");
            bool contactValid = lblErrorContact.Text.StartsWith("✓") && txtContact.Text.Length == 11;
            bool passwordValid = lblErrorPass.Text.StartsWith("✓") ||
                                (_editMode && string.IsNullOrEmpty(txtPassword.Text));

            return usernameValid && emailValid && contactValid && passwordValid;
        }
    }
}