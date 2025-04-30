using MySql.Data.MySqlClient;
using Scheduling_and_Teacher_Loading_Assignment_System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;

namespace Help_Scheduling_and_Teacher_Assignment_Loading_System.AddEdit_userControls
{
    public partial class AE_CourseSubj : UserControl
    {
        private const string DEBUG_PREFIX = "[AE_CourseSubj]";

        public bool IsEditMode { get; private set; }
        private int currentId = -1;
        private bool _isCourse;
        private bool _isSubject;
        private bool _isRoom;
        private bool _isSection;
        private bool isCodeManuallyEdited = false;
        private bool isCodeFocused = false;
        private string originalName = string.Empty;
        private string originalCode = string.Empty;

        private Point _originalDescriptionLocation;
        private bool _positionsInitialized = false;

        public event Action DataSaved;

        public AE_CourseSubj()
        {
            InitializeComponent();
            isCodeManuallyEdited = false;
            SetAddMode();
            _originalDescriptionLocation = lblDescription.Location;
            _positionsInitialized = true;
        }

        public void SetMode(bool isCourse, bool isSubject, bool isRoom, bool isSection)
        {
            Debug.WriteLine($"[AE] SetMode - C:{isCourse} S:{isSubject} R:{isRoom} Sec:{isSection}");

            // Reset all flags
            _isCourse = isCourse;
            _isSubject = isSubject;
            _isRoom = isRoom;
            _isSection = isSection;

            UpdateLabels();
            UpdateCodeVisibility();
        }

        private void UpdateLabels()
        {
            if (_isCourse)
            {
                lblCourseSubj.Text = "Add Course";
                lblName.Text = "Course Name";
            }
            else if (_isSubject)
            {
                lblCourseSubj.Text = "Add Subject";
                lblName.Text = "Subject Name";
            }
            else if (_isRoom)
            {
                lblCourseSubj.Text = "Add Room";
                lblName.Text = "Room Name";
            }
            else if (_isSection)
            {
                lblCourseSubj.Text = "Add Section";
                lblName.Text = "Section Name";
            }

            lblErrorCode.Text = lblErrorCourse.Text = lblformError.Text = "";
        }

        private void UpdateCodeVisibility()
        {
            // Store original position first time
            if (!_positionsInitialized)
            {
                _originalDescriptionLocation = lblDescription.Location;
                _positionsInitialized = true;
            }

            bool showCode = (_isCourse || _isSubject);
            lblCode.Visible = txtCode.Visible = lblErrorCode.Visible = showCode;

            // Adjust description position based on code visibility
            if (showCode)
            {
                // Reset to original position
                lblDescription.Location = _originalDescriptionLocation;
                txtDescription.Location = new Point(_originalDescriptionLocation.X,
                    _originalDescriptionLocation.Y + 25);
            }
            else
            {
                // Move up to where code fields would be
                int newY = txtName.Bottom + 15;
                lblDescription.Location = new Point(lblDescription.Left, newY);
                txtDescription.Location = new Point(txtDescription.Left, newY + 25);
            }

            Debug.WriteLine($"[Layout] Description position: {txtDescription.Location}");
        }

        public void SetAddMode()
        {
            Debug.WriteLine($"[AE] SetAddMode - Current mode: {lblCourseSubj.Text}");

            IsEditMode = false;
            currentId = -1;
            btnSave.Text = "Add";

            // Update labels based on current mode
            lblCourseSubj.Text = _isCourse ? "Add Course" :
                                _isSubject ? "Add Subject" :
                                _isRoom ? "Add Room" :
                                "Add Section";

            // Clear fields
            txtName.Text = txtDescription.Text = "";
            txtCode.Text = "Auto-generate if empty";
            txtCode.ForeColor = SystemColors.GrayText;

            if (_isRoom || _isSection) txtCode.Text = ""; // Clear code for rooms/sections
        }

        public void SetEditMode(int id, DataRow data)
        {
            IsEditMode = true;
            currentId = id;
            btnSave.Text = "Update";

            // Update labels based on mode
            lblCourseSubj.Text = _isCourse ? "Edit Course" :
                                _isSubject ? "Edit Subject" :
                                _isRoom ? "Edit Room" :
                                "Edit Section";

            // Load data based on mode
            if (_isCourse)
            {
                txtName.Text = data["course_name"].ToString();
                txtCode.Text = data["course_code"].ToString();
            }
            else if (_isSubject)
            {
                txtName.Text = data["subject_name"].ToString();
                txtCode.Text = data["subject_code"].ToString();
            }
            else if (_isRoom)
            {
                txtName.Text = data["room_name"].ToString();
            }
            else if (_isSection)
            {
                txtName.Text = data["section_name"].ToString();
            }

            txtDescription.Text = data["description"].ToString();
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
            try
            {
                Debug.WriteLine("[AE_CourseSubj] Updating code suggestion");

                if (!isCodeManuallyEdited && !isCodeFocused && (_isCourse || _isSubject))
                {
                    string finalCode;

                    if (string.IsNullOrWhiteSpace(txtName.Text))
                    {
                        finalCode = _isCourse ? "Suggested: CRS001" : "Suggested: SUB001";
                        Debug.WriteLine("[AE_CourseSubj] Using default code suggestion");
                    }
                    else
                    {
                        string baseCode = GenerateBaseCode(txtName.Text, _isCourse);
                        finalCode = $"Suggested: {GenerateFinalCode(baseCode, _isCourse)}";
                        Debug.WriteLine($"[AE_CourseSubj] Generated code: {finalCode}");
                    }

                    if (txtCode.Text != finalCode)
                    {
                        txtCode.Text = finalCode;
                        txtCode.ForeColor = Color.Gray;
                        txtCode.Tag = finalCode;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AE_CourseSubj] Code suggestion error: {ex.Message}");
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
            try
            {
                Debug.WriteLine($"[Duplicate Check] Starting check for: {name}");

                // 1. Determine table and column names
                string table = "";
                string nameField = "";

                if (_isCourse)
                {
                    table = "courses";
                    nameField = "course_name";
                }
                else if (_isSubject)
                {
                    table = "subjects";
                    nameField = "subject_name";
                }
                else if (_isRoom)
                {
                    table = "rooms";
                    nameField = "room_name";
                }
                else if (_isSection)
                {
                    table = "sections";
                    nameField = "section_name";
                }

                Debug.WriteLine($"[Duplicate Check] Using table: {table}, column: {nameField}");

                // 2. Build parameterized query
                string query = $@"SELECT COUNT(*) FROM {table}
                        WHERE {nameField} = @name
                        {(IsEditMode ? "AND id != @id" : "")}";

                MySqlParameter[] parameters = {
            new MySqlParameter("@name", name),
            new MySqlParameter("@id", IsEditMode ? currentId : -1)
        };

                // 3. Execute query
                int count = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters));
                Debug.WriteLine($"[Duplicate Check] Found {count} matching records");

                return count > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Duplicate Check] ERROR: {ex.Message}");
                return false;
            }
        }

        private bool CheckForDuplicateCode(string code)
        {
            string table = _isCourse ? "courses" : "subjects";
            string codeField = _isCourse ? "course_code" : "subject_code";

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
            string searchField = _isCourse ? "course_name" :
                                _isSubject ? "subject_name" :
                                _isRoom ? "room_name" :
                                "section_name";

            string entityType = _isCourse ? "Course" :
                               _isSubject ? "Subject" :
                               _isRoom ? "Room" : "Section";

            Debug.WriteLine($"[Search] Looking for {entityType} with: {searchText}");

            Control parent = this.Parent;
            while (parent != null && !(parent is ListCRUD))
            {
                parent = parent.Parent;
            }

            if (parent is ListCRUD listParent)
            {
                // Pass both search text and field to parent
                listParent.PerformSearch(searchText);
                Debug.WriteLine($"[Search] Sent search to ListCRUD: {searchText} in {searchField}");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Debug.WriteLine("[AE_CourseSubj] Save started");

                string name = txtName.Text.Trim();
                string desc = txtDescription.Text.Trim();
                string code = "";

                // Code handling for Courses/Subjects
                if (_isCourse || _isSubject)
                {
                    Debug.WriteLine("[AE_CourseSubj] Handling code for course/subject");
                    code = txtCode.Text.StartsWith("Suggested:")
                        ? GenerateFinalCode(GenerateBaseCode(name, _isCourse), _isCourse)
                        : txtCode.Text.Trim().ToUpper();

                    Debug.WriteLine($"[AE_CourseSubj] Final code: {code}");

                    if (string.IsNullOrEmpty(code))
                    {
                        Debug.WriteLine("[AE_CourseSubj] Code validation failed");
                        lblErrorCode.Text = "Code is required";
                        return;
                    }
                }

                // Common validation
                if (string.IsNullOrEmpty(name))
                {
                    Debug.WriteLine("[AE_CourseSubj] Name validation failed");
                    lblErrorCourse.Text = "Name is required";
                    return;
                }

                // Build query based on type
                string query = "";
                MySqlParameter[] parameters;

                if (IsEditMode)
                {
                    Debug.WriteLine("[AE_CourseSubj] Building UPDATE query");
                    if (_isCourse)
                    {
                        query = @"UPDATE courses SET
                        course_name = @name,
                        course_code = @code,
                        description = @desc
                        WHERE id = @id";
                    }
                    else if (_isSubject)
                    {
                        query = @"UPDATE subjects SET
                         subject_name = @name,
                         subject_code = @code,
                         description = @desc
                         WHERE id = @id";
                    }
                    else if (_isRoom)
                    {
                        query = @"UPDATE rooms SET
                         room_name = @name,
                         description = @desc
                         WHERE id = @id";
                    }
                    else if (_isSection)
                    {
                        query = @"UPDATE sections SET
                         section_name = @name,
                         description = @desc
                         WHERE id = @id";
                    }

                    parameters = new MySqlParameter[] {
                new MySqlParameter("@name", name),
                new MySqlParameter("@desc", desc),
                new MySqlParameter("@id", currentId)
            };

                    // Add code parameter for courses/subjects
                    if (_isCourse || _isSubject)
                    {
                        parameters = parameters.Append(new MySqlParameter("@code", code)).ToArray();
                    }
                }
                else
                {
                    Debug.WriteLine("[AE_CourseSubj] Building INSERT query");
                    if (_isCourse)
                    {
                        query = @"INSERT INTO courses
                        (course_name, course_code, description)
                        VALUES (@name, @code, @desc)";
                    }
                    else if (_isSubject)
                    {
                        query = @"INSERT INTO subjects
                         (subject_name, subject_code, description)
                         VALUES (@name, @code, @desc)";
                    }
                    else if (_isRoom)
                    {
                        query = @"INSERT INTO rooms
                         (room_name, description)
                         VALUES (@name, @desc)";
                    }
                    else if (_isSection)
                    {
                        query = @"INSERT INTO sections
                         (section_name, description)
                         VALUES (@name, @desc)";
                    }

                    parameters = new MySqlParameter[] {
                new MySqlParameter("@name", name),
                new MySqlParameter("@desc", desc)
            };

                    // Add code parameter for courses/subjects
                    if (_isCourse || _isSubject)
                    {
                        parameters = parameters.Append(new MySqlParameter("@code", code)).ToArray();
                    }
                }

                Debug.WriteLine($"[AE_CourseSubj] Executing query: {query}");
                Debug.WriteLine($"[AE_CourseSubj] Parameters: {string.Join(", ", parameters.Select(p => $"{p.ParameterName}={p.Value}"))}");

                int result = DatabaseHelper.ExecuteNonQuery(query, parameters);
                Debug.WriteLine($"[AE_CourseSubj] Query executed. Rows affected: {result}");

                DataSaved?.Invoke();
                SetAddMode();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AE_CourseSubj] ERROR: {ex.Message}");
                lblformError.Text = $"Error: {ex.Message}";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetAddMode();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            Debug.WriteLine($"[Validation] Name text changed: {txtName.Text}");

            // Clear previous errors
            lblErrorCourse.Text = "";
            SearchInParent("");

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                UpdateCodeSuggestion();
                return;
            }

            // Real-time duplicate check for all types
            bool isDuplicate = CheckForDuplicateName(txtName.Text.Trim());
            string entityType = _isCourse ? "Course" :
                               _isSubject ? "Subject" :
                               _isRoom ? "Room" : "Section";

            if (isDuplicate)
            {
                lblErrorCourse.Text = $"{entityType} already exists!";
                SearchInParent(txtName.Text.Trim());
                Debug.WriteLine($"[Validation] Duplicate {entityType} detected in real-time");
            }
            else
            {
                SearchInParent("");
                Debug.WriteLine($"[Validation] No duplicate {entityType} found");
            }

            // Auto-suggest code only for Courses/Subjects
            if (!isCodeManuallyEdited && !isCodeFocused && (_isCourse || _isSubject))
            {
                UpdateCodeSuggestion();
            }
        }
    }
}