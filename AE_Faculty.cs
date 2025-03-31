using MySql.Data.MySqlClient;
using Scheduling_and_Teacher_Loading_Assignment_System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Help_Scheduling_and_Teacher_Assignment_Loading_System.AddEdit_userControls
{
    public partial class AE_Faculty : UserControl
    {
        private bool _editMode = false;
        private int _currentId = -1;

        private bool _isIdManuallyEdited = false;
        private bool _isIdFocused = false;
        private string _originalFullName = string.Empty;
        private string _originalEmail = string.Empty;
        private string _originalContact = string.Empty;

        public event Action Datasaved;

        public AE_Faculty()
        {
            InitializeComponent();
            SetAddMode();
        }

        public void SetAddMode()
        {
            _editMode = false;
            _currentId = -1;

            btnSave.Text = "Add";
            btnCancel.Visible = false;
            lblFaculty.Text = "Add Faculty";

            txtID.Text = "Leave this empty";
            txtID.ForeColor = Color.Gray;
            txtFullName.Text = txtEmail.Text = txtContact.Text = txtAddress.Text = "";
            isActive.Checked = true;
            cmbGender.SelectedIndex = 0;
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

            // Handle name fields
            string lastName = data["last_name"].ToString();
            string firstName = data["first_name"].ToString();
            string middleName = data["middle_name"]?.ToString();
            txtFullName.Text = $"{lastName}, {firstName}" + (string.IsNullOrEmpty(middleName) ? "" : $" {middleName}");

            // Populate other fields
            txtEmail.Text = data["email"].ToString();
            txtContact.Text = data["contact_number"].ToString();
            txtAddress.Text = data["address"].ToString();
            isActive.Checked = Convert.ToBoolean(data["is_active"]);
            cmbGender.SelectedItem = data["gender"].ToString();
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

        private string GenerateFacultyId()
        {
            // Get the current year (last 2 digits)
            string year = DateTime.Now.ToString("yy");

            // Get the next available sequence number
            string sequence = GetNextFacultySequence().ToString("D3"); // 3-digit format

            // Combine to create ID (format: FY23-001)
            return $"FY{year}-{sequence}";
        }

        private int GetNextFacultySequence()
        {
            string query = "SELECT MAX(CAST(SUBSTRING(id_no, 6) AS UNSIGNED)) FROM faculty WHERE id_no LIKE @pattern";

            MySqlParameter[] parameters = {
        new MySqlParameter("@pattern", "FY%-%")
    };

            object result = DatabaseHelper.ExecuteScalar(query, parameters);

            if (result == null || result == DBNull.Value)
            {
                return 1;
            }

            return Convert.ToInt32(result) + 1;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text) || string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                lblformError.Text = "ID and Name are required";
                return;
            }

            try
            {
                if (string.IsNullOrEmpty(txtID.Text))
                {
                    txtID.Text = GenerateFacultyId();
                }

                string formattedName = FormatFullName(txtFullName.Text);
                var nameParts = formattedName.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

                string lastName = nameParts.Length > 0 ? nameParts[0] : "";
                string firstName = nameParts.Length > 1 ? nameParts[1] : "";
                string middleName = nameParts.Length > 2 ? string.Join(" ", nameParts.Skip(2)) : null;

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
                    new MySqlParameter("@id_no", txtID.Text),
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

        private void txtFullName_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                txtFullName.Text = FormatFullName(txtFullName.Text);
            }
        }

        private void AE_Faculty_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetAddMode();
        }

        private void txtID_Enter(object sender, EventArgs e)
        {
            if (txtID.Text == "Leave this empty")
            {
                txtID.Text = "";
                txtID.ForeColor = Color.Black;
            }
        }

        private void txtID_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                txtID.Text = "Leave this empty";
                txtID.ForeColor = Color.Gray;
            }
        }
    }
}