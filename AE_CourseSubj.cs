using MySql.Data.MySqlClient;
using Scheduling_and_Teacher_Loading_Assignment_System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Help_Scheduling_and_Teacher_Assignment_Loading_System.AddEdit_userControls
{
    public partial class AE_CourseSubj : UserControl
    {
        public bool IsEditMode { get; private set; }
        private int currentId = -1;
        private bool isCourse = true;
        private bool isCodeManuallyEdited = false;
        private bool isCodeFocused = false;
        private string originalName = string.Empty;
        private string originalCode = string.Empty;

        public event Action DataSaved;

        public AE_CourseSubj()
        {
            InitializeComponent();
            isCodeManuallyEdited = false;
            SetAddMode();
        }

        public void SetMode(bool ISCourse)
        {
            isCourse = ISCourse;
            // Update UI labels based on mode
            SetAddMode(); // Reset to add mode when changing types
        }

        public void SetAddMode()
        {
            IsEditMode = false;
            currentId = -1;
            btnSave.Text = "Add";
            lblCourseSubj.Text = isCourse ? "Add Course" : "Add Subject";
            lblName.Text = isCourse ? "Course Name" : "Subject Name";
            btnCancel.Visible = false;

            // Clear fields
            txtName.Text = txtDescription.Text = "";
            txtCode.Text = isCourse ? "Suggested: CRS001" : "Suggested: SUB001";
            txtCode.ForeColor = Color.Gray;
            txtCode.Tag = null;
            isCodeManuallyEdited = false;
            isCodeFocused = false;
            lblformError.Text = lblErrorCourse.Text = lblErrorCode.Text = string.Empty;
        }

        public void SetEditMode(int id, DataRow data)
        {
            IsEditMode = true;
            currentId = id;
            btnSave.Text = "Update";
            lblCourseSubj.Text = isCourse ? "Edit Course" : "Edit Subject";
            lblName.Text = isCourse ? "Course Name" : "Subject Name";
            btnCancel.Visible = true;

            // Populate fields from the DataRow
            // Use different column names based on whether we're editing Course or Subject
            txtName.Text = data[isCourse ? "course_name" : "subject_name"].ToString();
            txtCode.Text = data[isCourse ? "course_code" : "subject_code"].ToString();
            txtCode.ForeColor = SystemColors.WindowText;
            originalName = txtName.Text;
            originalCode = txtCode.Text;
            txtDescription.Text = data["description"].ToString();
            lblErrorCode.Text = lblErrorCourse.Text = string.Empty; // Clear any previous errors
        }

        private void AE_CourseSubj_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;

            // Add placeholder text
            txtCode.Enter += (s, ev) =>
            {
                isCodeFocused = true;
                // Only remove "Suggested:" if present
                if (txtCode.Text.StartsWith("Suggested:"))
                {
                    string cleanText = txtCode.Text.Replace("Suggested:", "").Trim();
                    txtCode.Text = cleanText;
                    txtCode.ForeColor = SystemColors.WindowText;
                    txtCode.Tag = cleanText; // Store for comparison
                }
            };

            txtCode.Leave += (s, ev) =>
            {
                isCodeFocused = false;

                // Check if text was changed from original
                bool textChanged = txtCode.Tag?.ToString() != txtCode.Text;
                isCodeManuallyEdited = textChanged && !string.IsNullOrWhiteSpace(txtCode.Text);

                // Cases where we revert to suggestion:
                // 1. Textbox is completely blank
                // 2. Text was entered but not changed (same as when entered)
                // 3. Duplicate was found and then erased
                if (string.IsNullOrWhiteSpace(txtCode.Text) || !textChanged)
                {
                    UpdateCodeSuggestion();
                    SearchInParent(""); // Clear search when showing suggestion
                }
            };

            txtCode.TextChanged += (s, ev) =>
            {
                if (!string.IsNullOrWhiteSpace(txtCode.Text) || !txtCode.Text.StartsWith("Suggested:"))
                {
                    bool isDuplicate = CheckForDuplicateCode(txtCode.Text.Trim());
                    lblErrorCode.Text = isDuplicate ?
                        $"{txtCode.Text.ToUpper()} already exists" : "";

                    if (isDuplicate)
                    {
                        SearchInParent(txtCode.Text.Trim());
                    }
                    else
                    {
                        SearchInParent("");
                    }
                }
                else if (string.IsNullOrWhiteSpace(txtCode.Text))
                {
                    UpdateCodeSuggestion();
                    SearchInParent(""); // Clear search when showing suggestion
                    lblErrorCode.Text = "";
                }
            };

            // Initialize placeholder
            if (string.IsNullOrEmpty(txtCode.Text))
            {
                txtCode.Text = "Auto-generate if empty";
                txtCode.ForeColor = SystemColors.GrayText;
            }
        }

        private void UpdateCodeSuggestion()
        {
            if (!isCodeManuallyEdited && !isCodeFocused)
            {
                string finalCode;

                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    // Default suggestion when name is empty
                    finalCode = isCourse ? "Suggested: CRS001" : "Suggested: SUB001";
                }
                else
                {
                    // Generate suggestion based on database
                    finalCode = $"Suggested: {GenerateFinalCode(GenerateBaseCode(txtName.Text, isCourse), isCourse)}";
                }

                // Only update if different from current text
                if (txtCode.Text != finalCode)
                {
                    txtCode.Text = finalCode;
                    txtCode.ForeColor = Color.Gray;
                    txtCode.Tag = finalCode; // Store for comparison
                }
            }
        }

        private string GenerateBaseCode(string name, bool isCourse)
        {
            if (string.IsNullOrWhiteSpace(name))
                return isCourse ? "CRS" : "SUB";

            // Get first letters of words
            string[] words = name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string prefix = "";

            foreach (string word in words.Take(2)) // Only consider first 2 words
            {
                if (word.Length >= 2)
                {
                    // Take first 3 letters if available, but at least 2
                    int take = word.Length >= 3 ? 3 : 2;
                    prefix += word.Substring(0, take).ToUpperInvariant();
                }
            }

            // Ensure prefix is 3-5 characters
            if (prefix.Length < 3)
                return isCourse ? "CRS" : "SUB";
            if (prefix.Length > 5)
                return prefix.Substring(0, 5);

            return prefix;
        }

        private string GenerateFinalCode(string baseCode, bool isCourse)
        {
            string table = isCourse ? "courses" : "subjects";
            string codeField = isCourse ? "course_code" : "subject_code";

            string query = $@"
        SELECT {codeField}
        FROM {table}
        WHERE {codeField} REGEXP @pattern
        ORDER BY {codeField} DESC
        LIMIT 1";

            MySqlParameter[] parameters = { new MySqlParameter("@pattern", $"^{baseCode}[0-9]{{3}}$") };
            string lastCode = DatabaseHelper.ExecuteScalar(query, parameters)?.ToString();

            int nextNum = 1;
            if (!string.IsNullOrEmpty(lastCode))
            {
                string numPart = lastCode.Substring(baseCode.Length); // Get just the numeric part
                if (int.TryParse(numPart, out int lastNum))
                {
                    nextNum = lastNum + 1;
                }
            }

            return $"{baseCode}{nextNum:D3}";
        }

        private bool CheckForDuplicateName(string name)
        {
            string table = isCourse ? "courses" : "subjects";
            string nameField = isCourse ? "course_name" : "subject_name";

            string query = $@"SELECT COUNT(*) FROM {table}
                     WHERE {nameField} = @name AND id != @id";

            MySqlParameter[] parameters = {
        new MySqlParameter("@name", name),
        new MySqlParameter("@id", IsEditMode ? currentId : -1)
    };

            int count = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters));
            return count > 0;
        }

        private bool CheckForDuplicateCode(string code)
        {
            string table = isCourse ? "courses" : "subjects";
            string codeField = isCourse ? "course_code" : "subject_code";

            string query = $@"SELECT COUNT(*) FROM {table}
                     WHERE {codeField} = @code AND id != @id";

            MySqlParameter[] parameters = {
        new MySqlParameter("@code", code),
        new MySqlParameter("@id", IsEditMode ? currentId : -1)
    };

            int count = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters));
            return count > 0;
        }

        private void SearchInParent(string searchText)
        {
            // Get reference to parent form
            Control parent = this.Parent; // Replace with actual type

            while (parent != null && !(parent is ListCRUD))
            {
                parent = parent.Parent;
            }

            if (parent is ListCRUD listParent)
            {
                listParent.PerformSearch(searchText);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            lblErrorCode.Text = lblErrorCourse.Text = lblformError.Text = "";

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                lblErrorCourse.Text = "Name is required";
                return;
            }

            // Validate code (if manually entered)
            string code = txtCode.Text.StartsWith("Suggested:")
                ? GenerateFinalCode(GenerateBaseCode(txtName.Text, isCourse), isCourse)
                : txtCode.Text.Trim().ToUpper();

            if (CheckForDuplicateName(txtName.Text.Trim()) &&
                (!IsEditMode || txtName.Text.Trim() != originalName))
            {
                lblErrorCourse.Text = $"{txtName.Text} already exists";
                return;
            }

            if (!txtCode.Text.StartsWith("Suggested:") && CheckForDuplicateCode(code) &&
                (!IsEditMode || code != originalCode))
            {
                lblErrorCode.Text = $"{code} already exists";
                return;
            }
            try
            {
                string query;
                MySqlParameter[] parameters;

                if (IsEditMode)
                {
                    query = isCourse ?
                        @"UPDATE courses SET
                  course_name = @name,
                  course_code = @code,
                  description = @desc
                  WHERE id = @id" :
                        @"UPDATE subjects SET
                  subject_name = @name,
                  subject_code = @code,
                  description = @desc
                  WHERE id = @id";

                    parameters = new MySqlParameter[]
                    {
                new MySqlParameter("@name", txtName.Text),
                new MySqlParameter("@code", code),
                new MySqlParameter("@desc", txtDescription.Text),
                new MySqlParameter("@id", currentId)
                    };
                }
                else
                {
                    query = isCourse ?
                        @"INSERT INTO courses (course_name, course_code, description)
                  VALUES (@name, @code, @desc)" :
                        @"INSERT INTO subjects (subject_name, subject_code, description)
                  VALUES (@name, @code, @desc)";

                    parameters = new MySqlParameter[]
                    {
                new MySqlParameter("@name", txtName.Text),
                new MySqlParameter("@code", code),
                new MySqlParameter("@desc", txtDescription.Text)
                    };
                }

                DatabaseHelper.ExecuteNonQuery(query, parameters);
                DataSaved?.Invoke();
                SetAddMode();
            }
            catch (Exception ex)
            {
                lblformError.Text = $"Error: {ex.Message}";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetAddMode();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (IsEditMode)
            {
            }
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                // Reset to default when name is empty
                UpdateCodeSuggestion();
                SearchInParent("");
                lblErrorCourse.Text = "";
                return;
            }

            if (!isCodeManuallyEdited && !isCodeFocused)
            {
                UpdateCodeSuggestion();
            }

            if (!string.IsNullOrWhiteSpace(txtName.Text))
            {
                bool isDuplicate = CheckForDuplicateName(txtName.Text.Trim());
                lblErrorCourse.Text = isDuplicate ?
                    $"{txtName.Text} already exists" : "";

                if (isDuplicate)
                {
                    SearchInParent(txtName.Text.Trim());
                }
                else
                {
                    lblErrorCourse.Text = "";
                    SearchInParent("");
                }
            }
        }
    }
}