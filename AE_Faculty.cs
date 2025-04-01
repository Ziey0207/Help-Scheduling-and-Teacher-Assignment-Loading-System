using MySql.Data.MySqlClient;
using ReaLTaiizor.Controls;
using Scheduling_and_Teacher_Loading_Assignment_System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace Help_Scheduling_and_Teacher_Assignment_Loading_System.AddEdit_userControls
{
    public partial class AE_Faculty : UserControl
    {
        [DllImport("user32.dll")]
        private static extern int GetCaretPos(out Point lpPoint);

        private bool _editMode = false;
        private int _currentId = -1;

        private bool _isIdManuallyEdited = false;
        private bool _isIdFocused = false;
        private string _originalFullName = string.Empty;
        private string _originalEmail = string.Empty;
        private string _originalContact = string.Empty;
        private Random _random = new Random();
        private ContextMenuStrip _emptyContextMenu = new ContextMenuStrip();

        public event Action Datasaved;

        public AE_Faculty()
        {
            InitializeComponent();
            SetAddMode();
            LoadCourseCodes();

            txtID.ContextMenuStrip = _emptyContextMenu;
        }

        private void AE_Faculty_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
        }

        private int GetCursorPosition(SmallTextBox textBox)
        {
            if (!textBox.Focused) return 0;

            Point caretPos;
            GetCaretPos(out caretPos);
            Point relativePos = textBox.PointToClient(caretPos);

            using (Graphics g = textBox.CreateGraphics())
            {
                for (int i = 0; i <= textBox.Text.Length; i++)
                {
                    if (g.MeasureString(textBox.Text.Substring(0, i), textBox.Font).Width > relativePos.X)
                        return i - 1;
                }
                return textBox.Text.Length;
            }
        }

        private bool IsValidIdFormat(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;

            // Must contain exactly one dash
            var parts = id.Split('-');
            if (parts.Length != 2) return false;

            // First part letters, second part numbers
            return parts[0].All(char.IsLetter) &&
                   parts[1].All(char.IsDigit);
        }

        private async void LoadCourseCodes()
        {
            try
            {
                string query = "SELECT course_code FROM courses ORDER BY course_code";
                using (var reader = await DatabaseHelper.ExecuteReaderAsync(query, null))
                {
                    cmbCourse.Items.Clear();
                    cmbCourse.Items.Add(""); // Add empty option
                    while (await reader.ReadAsync())
                    {
                        cmbCourse.Items.Add(reader["course_code"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading course codes: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SetAddMode()
        {
            _editMode = false;
            _currentId = -1;

            btnSave.Text = "Add";
            btnCancel.Visible = false;
            lblFaculty.Text = "Add Faculty";

            txtID.Text = "Pick course to generate";
            txtID.ForeColor = SystemColors.WindowText;
            txtFullName.Text = txtEmail.Text = txtContact.Text = txtAddress.Text = "";
            isActive.Checked = true;
            cmbGender.SelectedIndex = 0;
            _isIdManuallyEdited = false;
            _isIdFocused = false;

            ClearAllErrors();
        }

        public void SetEditMode(int id, DataRow data)
        {
            _editMode = true;
            _currentId = id;

            btnSave.Text = "Update";
            btnCancel.Visible = true;
            lblFaculty.Text = "Edit Faculty";

            // Populate all fields from the data row
            txtID.Text = data["id_no"].ToString();
            txtID.ForeColor = SystemColors.WindowText;

            // Handle name fields
            string lastName = data["last_name"].ToString();
            string firstName = data["first_name"].ToString();
            string middleName = data["middle_name"]?.ToString();
            txtFullName.Text = $"{lastName}, {firstName}" + (string.IsNullOrEmpty(middleName) ? "" : $" {middleName}");
            _originalFullName = txtFullName.Text;

            // Populate other fields
            txtEmail.Text = data["email"].ToString();
            _originalEmail = txtEmail.Text;
            txtContact.Text = data["contact_number"].ToString();
            _originalContact = txtContact.Text;
            txtAddress.Text = data["address"].ToString();
            isActive.Checked = Convert.ToBoolean(data["is_active"]);
            cmbGender.SelectedItem = data["gender"].ToString();

            // Try to set course code if it exists in the ID
            string idNo = data["id_no"].ToString();
            if (idNo.Contains("-"))
            {
                string possibleCourseCode = idNo.Split('-')[0];
                if (cmbCourse.Items.Contains(possibleCourseCode))
                {
                    cmbCourse.SelectedItem = possibleCourseCode;
                }
            }

            ClearAllErrors();
        }

        private void ClearAllErrors()
        {
            lblErrorFullname.Text = "";
            lblErrorEmail.Text = "";
            lblErrorContact.Text = "";
            lblErrorAddress.Text = "";
            lblErrorID.Text = "";
            lblformError.Text = "";
        }

        private string FormatFullName(string fullName)
        {
            // Split into parts and capitalize each part
            var names = fullName.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < names.Length; i++)
            {
                names[i] = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(names[i].ToLower());
            }

            // Reconstruct in "Last, First Middle" format
            if (names.Length == 0) return "";
            if (names.Length == 1) return names[0];

            string lastName = names[0];
            string firstName = names[1];
            string middleName = names.Length > 2 ? string.Join(" ", names.Skip(2)) : null;

            return $"{lastName}, {firstName}" + (string.IsNullOrEmpty(middleName) ? "" : $" {middleName}");
        }

        private string GetCurrentPrefix()
        {
            return cmbCourse.SelectedItem?.ToString() ?? "FAC";
        }

        private async Task<string> GenerateFacultyId()
        {
            string prefix = GetCurrentPrefix();
            string id;
            int attempts = 0;

            do
            {
                id = $"{prefix}-{_random.Next(100000, 999999)}";
                attempts++;
            } while (await CheckForDuplicateId(id) && attempts < 5);

            return attempts < 5 ? id : $"{prefix}-{DateTime.Now:HHmmss}";
        }

        private async Task<bool> CheckForDuplicateFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return false;

            var nameParts = FormatFullName(fullName).Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            string lastName = nameParts.Length > 0 ? nameParts[0] : "";
            string firstName = nameParts.Length > 1 ? nameParts[1] : "";
            string middleName = nameParts.Length > 2 ? string.Join(" ", nameParts.Skip(2)) : null;

            string query = @"SELECT COUNT(*) FROM faculty
                   WHERE last_name = @lastName
                   AND first_name = @firstName
                   AND (middle_name = @middleName OR (middle_name IS NULL AND @middleName IS NULL))
                   AND id != @id";

            MySqlParameter[] parameters = {
                new MySqlParameter("@lastName", lastName),
                new MySqlParameter("@firstName", firstName),
                new MySqlParameter("@middleName", string.IsNullOrEmpty(middleName) ? null : middleName),
                new MySqlParameter("@id", _editMode ? _currentId : -1)
            };

            int count = Convert.ToInt32(await DatabaseHelper.ExecuteScalarAsync(query, parameters));
            return count > 0;
        }

        private async Task<bool> CheckForDuplicateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            string query = @"SELECT COUNT(*) FROM faculty
                   WHERE email = @email
                   AND id != @id";

            MySqlParameter[] parameters = {
                new MySqlParameter("@email", email),
                new MySqlParameter("@id", _editMode ? _currentId : -1)
            };

            int count = Convert.ToInt32(await DatabaseHelper.ExecuteScalarAsync(query, parameters));
            return count > 0;
        }

        private async Task<bool> CheckForDuplicateContact(string contact)
        {
            if (string.IsNullOrWhiteSpace(contact)) return false;

            string query = @"SELECT COUNT(*) FROM faculty
                   WHERE contact_number = @contact
                   AND id != @id";

            MySqlParameter[] parameters = {
                new MySqlParameter("@contact", contact),
                new MySqlParameter("@id", _editMode ? _currentId : -1)
            };

            int count = Convert.ToInt32(await DatabaseHelper.ExecuteScalarAsync(query, parameters));
            return count > 0;
        }

        private async Task<bool> CheckForDuplicateId(string idNo)
        {
            if (string.IsNullOrWhiteSpace(idNo)) return false;

            string query = @"SELECT COUNT(*) FROM faculty
                   WHERE id_no = @idNo
                   AND id != @id";

            MySqlParameter[] parameters = {
                new MySqlParameter("@idNo", idNo),
                new MySqlParameter("@id", _editMode ? _currentId : -1)
            };

            int count = Convert.ToInt32(await DatabaseHelper.ExecuteScalarAsync(query, parameters));
            return count > 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetAddMode();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            ClearAllErrors();

            // Validate required fields
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                lblErrorFullname.Text = "Full name is required";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                lblErrorEmail.Text = "Email is required";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtContact.Text))
            {
                lblErrorContact.Text = "Contact number is required";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                lblErrorAddress.Text = "Address is required";
                isValid = false;
            }

            // Check for duplicates
            if (!string.IsNullOrWhiteSpace(txtFullName.Text) &&
                await CheckForDuplicateFullName(txtFullName.Text) &&
                (!_editMode || txtFullName.Text != _originalFullName))
            {
                lblErrorFullname.Text = "Name already exists";
                isValid = false;
            }

            if (!string.IsNullOrWhiteSpace(txtEmail.Text) &&
                await CheckForDuplicateEmail(txtEmail.Text) &&
                (!_editMode || txtEmail.Text != _originalEmail))
            {
                lblErrorEmail.Text = "Email already exists";
                isValid = false;
            }

            if (!string.IsNullOrWhiteSpace(txtContact.Text) &&
                await CheckForDuplicateContact(txtContact.Text) &&
                (!_editMode || txtContact.Text != _originalContact))
            {
                lblErrorContact.Text = "Contact # already exists";
                isValid = false;
            }

            // Handle ID generation/validation
            string idNo;
            if (string.IsNullOrWhiteSpace(txtID.Text) || txtID.Text == "Pick course to generate")
            {
                idNo = await GenerateFacultyId();
            }
            else
            {
                idNo = txtID.Text;
                if (await CheckForDuplicateId(idNo) && (!_editMode || idNo != txtID.Text))
                {
                    lblErrorID.Text = "ID already exists";
                    isValid = false;
                }
                if (!IsValidManualIdFormat(txtID.Text))
                {
                    lblErrorID.Text = "Format: COURSE-123456 (numbers only after dash)";
                    isValid = false;
                }
            }

            if (!isValid)
            {
                lblformError.Text = "Please fix all errors before saving";
                return;
            }

            try
            {
                string formattedName = FormatFullName(txtFullName.Text);
                var nameParts = formattedName.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

                string lastName = nameParts.Length > 0 ? nameParts[0] : "";
                string firstName = nameParts.Length > 1 ? nameParts[1] : "";
                string middleName = nameParts.Length > 2 ? string.Join(" ", nameParts.Skip(2)) : null;

                idNo = txtID.Text.Replace("Generated:", "").Trim();

                string query = _editMode ?
                    @"UPDATE faculty SET
                     id_no = @id_no,
                     last_name = @last_name,
                     first_name = @first_name,
                     middle_name = @middle_name,
                     email = @email,
                     contact_number = @contact,
                     address = @address,
                     is_active = @active,
                     gender = @gender
                     WHERE id = @id" :
                    @"INSERT INTO faculty
                     (id_no, last_name, first_name, middle_name, email, contact_number, address, is_active, gender)
                     VALUES
                     (@id_no, @last_name, @first_name, @middle_name, @email, @contact, @address, @active, @gender)";

                MySqlParameter[] parameters = {
                    new MySqlParameter("@id_no", idNo),
                    new MySqlParameter("@last_name", lastName),
                    new MySqlParameter("@first_name", firstName),
                    new MySqlParameter("@middle_name", string.IsNullOrEmpty(middleName) ? null : middleName),
                    new MySqlParameter("@email", txtEmail.Text),
                    new MySqlParameter("@contact", txtContact.Text),
                    new MySqlParameter("@address", txtAddress.Text),
                    new MySqlParameter("@active", isActive.Checked),
                    new MySqlParameter("@gender", cmbGender.SelectedItem.ToString()),
                    new MySqlParameter("@id", _currentId)
                };

                DatabaseHelper.ExecuteNonQuery(query, parameters);

                Datasaved?.Invoke();

                SetAddMode();
            }
            catch (Exception ex)
            {
                lblformError.Text = $"Error: {ex.Message}";
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidManualIdFormat(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;

            // Format: COURSE-123456 (numbers only after dash)
            string[] parts = id.Split('-');
            if (parts.Length != 2) return false;

            // Check second part is all numbers
            return parts[1].All(char.IsDigit);
        }

        private void txtID_Enter(object sender, EventArgs e)
        {
            Console.WriteLine("ID Enter");

            _isIdFocused = true;
            if (txtID.Text == "Pick course to generate")
            {
                txtID.Text = $"{GetCurrentPrefix()}-";
            }
            else if (txtID.Text.StartsWith("Generated:"))
            {
                string cleanText = txtID.Text.Replace("Generated:", "").Trim();
                txtID.Text = cleanText;
            }

            txtID.Tag = txtID.Text;
        }

        private async void txtID_Leave(object sender, EventArgs e)
        {
            Console.WriteLine("ID Leave");
            _isIdFocused = false;

            bool isDuplicate = await CheckForDuplicateId(txtID.Text);
            if (isDuplicate)
            {
                lblErrorID.Text = "ID already exists";
                return;
            }
            else
            {
                lblErrorID.Text = "";
            }

            bool idChanged = txtID.Tag?.ToString() == txtID.Text;
            string cleanText = txtID.Text?.Replace("Generated:", "").Trim();
            int dashIndex = cleanText.IndexOf('-');
            cleanText = dashIndex > 0 ? cleanText.Substring(0, dashIndex) : cleanText;
            Console.WriteLine(cleanText);
            var foundItem = cmbCourse.Items.Cast<string>().FirstOrDefault(item => item.Equals(cleanText, StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                txtID.Text = "Generated:" + await GenerateFacultyId();
            }
            else if (idChanged || (foundItem != null || txtID.Text == "FAC"))
            {
                cmbCourse.SelectedItem = foundItem;
                txtID.Text = $"{foundItem}";
                Console.WriteLine("NYahello");
                if (!IsValidManualIdFormat(txtID.Text))
                {
                    txtID.Text = "Generated:" + await GenerateFacultyId();
                }
                else
                {
                    txtID.Text = "Generated:" + txtID.Text;
                }
            }
        }

        private void txtID_TextChanged(object sender, EventArgs e)
        {
            // Update text color
            if (!txtID.Text.StartsWith("Generated:"))
            {
                txtID.ForeColor = SystemColors.WindowText;
            }
            else
            {
                txtID.ForeColor = Color.Gray;
            }
        }

        private void txtID_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only digits and control characters
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }

            // Get current text without formatting
            string current = Regex.Replace(txtContact.Text, @"[^\d]", "");

            // Prevent more than 11 digits
            if (current.Length >= 11 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                return;
            }
        }

        private void cmbCourseCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prefix = GetCurrentPrefix();

            // Only auto-update if text is empty, placeholder, or starts with old prefix
            if (string.IsNullOrWhiteSpace(txtID.Text) ||
                txtID.Text == "Pick course to generate" ||
                txtID.Text.StartsWith("Generated:"))
            {
                string numbers = txtID.Text.Contains('-') && !txtID.Text.StartsWith("Generated:")
                    ? txtID.Text.Split('-')[1]
                    : _random.Next(100000, 999999).ToString();

                txtID.Text = $"Generated:{prefix}-{numbers}";
                txtID.ForeColor = Color.Gray;
            }
            else if (txtID.Text.Contains('-'))
            {
                // For manually edited IDs that contain a dash, keep the numbers but update the prefix
                string[] parts = txtID.Text.Split('-');
                if (parts.Length == 2)
                {
                    txtID.Text = $"{prefix}-{parts[1]}";
                    txtID.ForeColor = SystemColors.WindowText;
                }
            }
        }

        private void txtFullName_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                txtFullName.Text = FormatFullName(txtFullName.Text);
            }
        }

        private async void txtFullName_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                lblErrorFullname.Text = "Full name is required";
                return;
            }

            if (await CheckForDuplicateFullName(txtFullName.Text) &&
                (!_editMode || txtFullName.Text != _originalFullName))
            {
                lblErrorFullname.Text = "Faculty with this name already exists";
            }
            else
            {
                lblErrorFullname.Text = "";
            }
        }

        private async void txtEmail_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                lblErrorEmail.Text = "Email is required";
                return;
            }

            if (!IsValidEmail(txtEmail.Text))
            {
                lblErrorEmail.Text = "Invalid email format";
                return;
            }

            if (await CheckForDuplicateEmail(txtEmail.Text) &&
                (!_editMode || txtEmail.Text != _originalEmail))
            {
                lblErrorEmail.Text = "Email already exists";
            }
            else
            {
                lblErrorEmail.Text = "";
            }
        }

        private async void txtContact_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtContact.Text))
            {
                lblErrorContact.Text = "Mobile number is required (09XXX-XXX-XXX)";
                return;
            }

            // Get current cursor position using your existing method
            int cursorPos = GetCursorPosition(txtID);

            // Remove all non-digit characters
            string cleanNumber = Regex.Replace(txtContact.Text, @"[^\d]", "");

            // Validate strictly 09 + 9 digits (11 total)
            bool isValid = cleanNumber.Length == 11 && cleanNumber.StartsWith("09");

            if (isValid)
            {
                // Format as 09XX-XXX-XXXX
                string formatted = $"{cleanNumber.Substring(0, 4)}-{cleanNumber.Substring(4, 3)}-{cleanNumber.Substring(7)}";

                // Only update if different to prevent infinite loops
                if (txtContact.Text != formatted)
                {
                    txtContact.Text = formatted;
                    // Try to maintain reasonable cursor position
                    TrySetCursorPosition(txtContact, cursorPos);
                }

                lblErrorContact.Text = "";
            }
            else
            {
                lblErrorContact.Text = "Must be exactly 11 digits starting with 09\nExample: 0917-123-4567";
                return;
            }

            // Check for duplicates
            if (await CheckForDuplicateContact(txtContact.Text) &&
                (!_editMode || txtContact.Text != _originalContact))
            {
                lblErrorContact.Text = "This mobile number is already registered";
            }
        }

        private void TrySetCursorPosition(HopeTextBox textBox, int originalPos)
        {
            // Simple heuristic to maintain cursor position after formatting
            int newPos = originalPos;

            // If we added a dash before the cursor, move it forward
            if (originalPos >= 4 && textBox.Text.Length > originalPos && textBox.Text[originalPos] == '-')
            {
                newPos++;
            }
            if (originalPos >= 8 && textBox.Text.Length > originalPos && textBox.Text[originalPos] == '-')
            {
                newPos++;
            }

            // Ensure we don't set position beyond text length
            newPos = Math.Min(newPos, textBox.Text.Length);

            // Your existing cursor position logic here
            // (May need to adapt based on your exact GetCaretPos implementation)
        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                lblErrorAddress.Text = "Address is required";
            }
            else
            {
                lblErrorAddress.Text = "";
            }
        }

        private async void btnGen_Click(object sender, EventArgs e)
        {
            string newID = await GenerateFacultyId();
            txtID.Text = $"Generated:{newID}";
        }

        private void txtContact_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Only allow: digits, backspace, delete
            if (!char.IsDigit(e.KeyChar) &&
                e.KeyChar != (char)Keys.Back &&
                e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
                SystemSounds.Beep.Play(); // Optional audio feedback
            }

            // Get current text without formatting
            string current = Regex.Replace(txtContact.Text, @"[^\d]", "");

            // Prevent more than 11 digits
            if (current.Length >= 11 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtContact_KeyDown(object sender, KeyEventArgs e)
        {
            // Block Ctrl+V (paste)
            if (e.Control && e.KeyCode == Keys.V)
            {
                e.SuppressKeyPress = true;
            }

            // Block Shift+Insert (alternative paste)
            if (e.Shift && e.KeyCode == Keys.Insert)
            {
                e.SuppressKeyPress = true;
            }
        }
    }
}