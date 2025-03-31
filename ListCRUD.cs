using Help_Scheduling_and_Teacher_Assignment_Loading_System.AddEdit_userControls;
using MySql.Data.MySqlClient;
using Scheduling_and_Teacher_Loading_Assignment_System;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Help_Scheduling_and_Teacher_Assignment_Loading_System
{
    public partial class ListCRUD : UserControl
    {
        private enum ListType
        { Faculty = 0, Admin = 1, Course = 2, Subject = 3 }

        private ListType _currentListType;

        // Updated color scheme
        private Color _headerColor = Color.FromArgb(19, 15, 64);  // Dark blue header

        private Color _rowColor = Color.FromArgb(60, 63, 120);     // Row color
        private Color _textColor = Color.FromArgb(220, 220, 220);  // Light text
        private Color _buttonHoverColor = Color.FromArgb(19, 15, 64); // Hover color as requested

        private System.Threading.Timer _searchTimer;

        public ListCRUD(int listType)
        {
            InitializeComponent();
            ApplyDarkModeTableStyle(); // Updated to use dark theme
            ConfigureSelectionBehavior();

            _currentListType = (ListType)listType;

            switch (_currentListType)
            {
                case ListType.Faculty:
                    txtHeaderMain.Text = "Faculty List";

                    AE_Faculty facultyAE = new AE_Faculty();
                    facultyAE.Datasaved += () => LoadData();
                    AddEditArea.Controls.Add(facultyAE);
                    break;

                case ListType.Admin:
                    txtHeaderMain.Text = "Users List";

                    AE_User UsersAE = new AE_User();
                    AddEditArea.Controls.Add(UsersAE);
                    break;

                case ListType.Course:
                    txtHeaderMain.Text = "Courses List";

                    AE_CourseSubj CoursesAE = new AE_CourseSubj();
                    CoursesAE.SetMode(true); // Set to course mode
                    CoursesAE.DataSaved += () => LoadData();

                    AddEditArea.Controls.Add(CoursesAE);
                    break;

                case ListType.Subject:
                    txtHeaderMain.Text = "Subjects List";

                    AE_CourseSubj SubjectsAE = new AE_CourseSubj();
                    SubjectsAE.SetMode(false); // Set to subject mode
                    SubjectsAE.DataSaved += () => LoadData();

                    AddEditArea.Controls.Add(SubjectsAE);
                    break;
            }
            LoadData();
        }

        public void ResetToAddMode()
        {
            foreach (Control control in AddEditArea.Controls)
            {
                if (control is AE_Faculty facultyControl)
                {
                    facultyControl.SetAddMode();
                }
                else if (control is AE_User userControl)
                {
                    userControl.SetAddMode();
                }
                else if (control is AE_CourseSubj courseSubjControl)
                {
                    courseSubjControl.SetAddMode();
                }
            }
        }

        public void RefreshData()
        {
            LoadData();
        }

        private void LoadData(string searchText = "")
        {
            dataGridView1.Visible = false;
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                DataTable data = GetDataFromDatabase(searchText);
                DisplayDataInGridView(data);

                // Show "No results" message if empty
                if (data.Rows.Count == 0 && !string.IsNullOrEmpty(searchText))
                {
                    lblErrorSearch.Visible = true;
                    lblErrorSearch.Text = "No matching records found";
                }
                else
                {
                    lblErrorSearch.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                dataGridView1.Visible = true;
            }
        }

        private DataTable GetDataFromDatabase(string searchText)
        {
            string query = BuildQuery(searchText);
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@searchText", $"%{searchText}%")
            };

            using (MySqlDataReader reader = DatabaseHelper.ExecuteReader(query, parameters))
            {
                DataTable dt = new DataTable();
                dt.Load(reader);
                return dt;
            }
        }

        private string BuildQuery(string searchText)
        {
            switch (_currentListType)
            {
                case ListType.Faculty:
                    return string.IsNullOrEmpty(searchText)
                        ? @"SELECT id,
                          id_no,
                          last_name,
                          first_name,
                          middle_name,
                          CONCAT(last_name, ', ', first_name, ' ', IFNULL(middle_name, '')) AS Name,
                          contact_number,
                          email,
                          gender,
                          address,
                          is_active,
                          CASE WHEN is_active THEN '✔️' ELSE '✖️' END AS Active
                   FROM faculty"
                        : @"SELECT id,
                          id_no,
                          last_name,
                          first_name,
                          middle_name,
                          CONCAT(last_name, ', ', first_name, ' ', IFNULL(middle_name, '')) AS Name,
                          contact_number,
                          email,
                          gender,
                          address,
                          is_active,
                          CASE WHEN is_active THEN '✔️' ELSE '✖️' END AS Active
                   FROM faculty
                   WHERE last_name LIKE @searchText
                      OR first_name LIKE @searchText
                      OR middle_name LIKE @searchText
                      OR id_no LIKE @searchText
                      OR email LIKE @searchText
                      OR contact_number LIKE @searchText
                      OR address LIKE @searchText
                      OR gender LIKE @searchText";

                case ListType.Admin:
                    return string.IsNullOrEmpty(searchText)
                        ? "SELECT id, username, email, contact_number, password FROM admins"
                        : @"SELECT id, username, email, contact_number, password FROM admins
                           WHERE username LIKE @searchText OR email LIKE @searchText";

                case ListType.Course:
                    return string.IsNullOrEmpty(searchText)
                        ? "SELECT id, course_code, course_name, description FROM courses"
                        : @"SELECT id, course_code, course_name, description FROM courses
                           WHERE course_name LIKE @searchText OR description LIKE @searchText OR course_code LIKE @searchtext";

                case ListType.Subject:
                    return string.IsNullOrEmpty(searchText)
                        ? "SELECT id, subject_code, subject_name, description FROM subjects"
                        : @"SELECT id, subject_code, subject_name, description FROM subjects
                           WHERE subject_name LIKE @searchText OR description LIKE @searchText OR subject_code LIKE @searchtext";

                default:
                    return string.Empty;
            }
        }

        private void DisplayDataInGridView(DataTable data)
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = data;
            AdjustColumns();
        }

        private void AdjustColumns()
        {
            dataGridView1.Columns.Clear();

            switch (_currentListType)
            {
                case ListType.Faculty:
                    AddColumn("id", "ID", 50, false);
                    AddColumn("id_no", "ID No", 100);
                    AddColumn("Name", "Name", 200, true, DataGridViewAutoSizeColumnMode.Fill);
                    AddColumn("email", "Email", 200, true, DataGridViewAutoSizeColumnMode.Fill);
                    AddColumn("contact_number", "Contact", 120, true, DataGridViewAutoSizeColumnMode.None);

                    var activeCol = new DataGridViewTextBoxColumn
                    {
                        Name = "Active",
                        HeaderText = "Active",
                        DataPropertyName = "Active",
                        Width = 20,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
                        DefaultCellStyle = new DataGridViewCellStyle
                        {
                            Alignment = DataGridViewContentAlignment.MiddleCenter,
                            Font = new Font("Segoe UI Emoji", 10), // Ensures emoji display
                            BackColor = _headerColor,
                            ForeColor = Color.White
                        },
                        ReadOnly = true
                    };
                    dataGridView1.Columns.Add(activeCol);

                    dataGridView1.Columns.Add(CreateActionButtonColumn("View", "👁", "View details"));
                    dataGridView1.Columns.Add(CreateActionButtonColumn("Edit", "✏", "Edit record"));
                    dataGridView1.Columns.Add(CreateActionButtonColumn("Delete", "🗑", "Delete record"));
                    break;

                case ListType.Admin:
                    AddColumn("id", "ID", 50, false);
                    AddColumn("username", "Username", 150);
                    AddColumn("email", "Email", 200, true, DataGridViewAutoSizeColumnMode.Fill);
                    AddColumn("contact_number", "Contact", 120);

                    var passwordCol = new DataGridViewTextBoxColumn
                    {
                        Name = "password",
                        HeaderText = "Password",
                        DataPropertyName = "password",
                        Width = 150,
                        DefaultCellStyle = new DataGridViewCellStyle
                        {
                            ForeColor = Color.Silver,
                            Font = new Font("Arial", 10)
                        }
                    };
                    dataGridView1.Columns.Add(passwordCol);

                    dataGridView1.Columns.Add(CreateActionButtonColumn("Edit", "✏", "Edit record"));
                    dataGridView1.Columns.Add(CreateActionButtonColumn("Delete", "🗑", "Delete record"));
                    break;

                case ListType.Course:
                    AddColumn("id", "ID", 50, false);
                    AddColumn("course_code", "Code", 100);
                    AddColumn("course_name", "Course Name", 200, true, DataGridViewAutoSizeColumnMode.Fill);
                    AddColumn("description", "Description", 300, true, DataGridViewAutoSizeColumnMode.Fill);

                    dataGridView1.Columns.Add(CreateActionButtonColumn("Edit", "✏", "Edit record"));
                    dataGridView1.Columns.Add(CreateActionButtonColumn("Delete", "🗑", "Delete record"));
                    break;

                case ListType.Subject:
                    AddColumn("id", "ID", 50, false);
                    AddColumn("subject_code", "Code", 100);
                    AddColumn("subject_name", "Subject Name", 200, true, DataGridViewAutoSizeColumnMode.Fill);
                    AddColumn("description", "Description", 300, true, DataGridViewAutoSizeColumnMode.Fill);

                    dataGridView1.Columns.Add(CreateActionButtonColumn("Edit", "✏", "Edit record"));
                    dataGridView1.Columns.Add(CreateActionButtonColumn("Delete", "🗑", "Delete record"));
                    break;
            }
        }

        public void PerformSearch(string SearchText)
        {
            txtSearch.Text = SearchText;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Debounce the search to avoid querying on every keystroke
            _searchTimer?.Dispose(); // Cancel previous timer if exists

            _searchTimer = new System.Threading.Timer(_ =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    LoadData(txtSearch.Text.Trim());
                });
            }, null, 500, Timeout.Infinite); // 500ms delay
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            bool isRowSelected = dataGridView1.SelectedRows.Count > 0;
        }

        private void FacultyListandUsersList_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
            ConfigureDataGridViewResizing();
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn newColumn = dataGridView1.Columns[e.ColumnIndex];
            DataGridViewColumn oldColumn = dataGridView1.SortedColumn;
            ListSortDirection direction;

            if (oldColumn != null)
            {
                direction = oldColumn.HeaderCell.SortGlyphDirection == SortOrder.Ascending
                    ? ListSortDirection.Descending
                    : ListSortDirection.Ascending;
            }
            else
            {
                direction = ListSortDirection.Ascending;
            }

            // Clear the previous sort glyph
            dataGridView1.Sort(newColumn, direction);
            newColumn.HeaderCell.SortGlyphDirection =
                direction == ListSortDirection.Ascending ? SortOrder.Ascending : SortOrder.Descending;
        }

        // Modern DataGridView style - UPDATED TO DARK THEME
        private void ApplyDarkModeTableStyle()
        {
            // Basic Grid Setup
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridView1.BackgroundColor = _headerColor;
            dataGridView1.GridColor = Color.FromArgb(70, 73, 130);

            // Header Styling
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = _headerColor;
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = _textColor;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 16, FontStyle.Bold);
            dataGridView1.ColumnHeadersHeight = 40;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Row Styling (no alternating colors)
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.DefaultCellStyle.BackColor = _rowColor;
            dataGridView1.DefaultCellStyle.ForeColor = _textColor;
            dataGridView1.DefaultCellStyle.Font = new Font("Arial", 12);
            dataGridView1.RowTemplate.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;

            // Auto-size rows based on content
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True; // Allow text wrapping

            // Selection Styling
            dataGridView1.DefaultCellStyle.SelectionBackColor = _buttonHoverColor;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;

            // Enable Double Buffering for smooth scrolling
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, dataGridView1, new object[] { true });
        }

        private void ConfigureDataGridViewResizing()
        {
            // Anchor to all sides so it resizes with the container
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                                 | AnchorStyles.Left | AnchorStyles.Right;

            // Set margins/padding if needed
            dataGridView1.Margin = new Padding(10);

            // Configure auto-sizing
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AllowUserToResizeColumns = false;

            // Prevent row height adjustment
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.RowTemplate.Resizable = DataGridViewTriState.False;
        }

        // Updated AddColumn method with auto-size mode parameter
        private void AddColumn(string name, string headerText, int width, bool visible = true,
                              DataGridViewAutoSizeColumnMode autoSizeMode = DataGridViewAutoSizeColumnMode.None)
        {
            var col = new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = headerText,
                DataPropertyName = name,
                Width = width,
                Visible = visible,
                AutoSizeMode = autoSizeMode,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Padding = new Padding(5, 10, 5, 10)
                }
            };
            dataGridView1.Columns.Add(col);
        }

        private void ConfigureSelectionBehavior()
        {
            // Enable full row selection
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Multi-select if needed (set to false for single selection)
            dataGridView1.MultiSelect = false;

            // Visual improvements for selection
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(19, 15, 64);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;
            dataGridView1.RowTemplate.Height = 40; // Taller rows for better touch
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ignore header clicks
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // Clear any default selection when data loads
            dataGridView1.ClearSelection();

            // Auto-size all rows after data is loaded
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Height = row.GetPreferredHeight(row.Index, DataGridViewAutoSizeRowMode.AllCells, true);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var column = dataGridView1.Columns[e.ColumnIndex];
            DataRow row = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row;
            int id = Convert.ToInt32(row["id"]);

            if (column.Name == "View" && e.RowIndex >= 0 && _currentListType == ListType.Faculty)
            {
                ViewFaculty viewFaculty = new ViewFaculty(id);
                viewFaculty.ShowDialog();
            }
            else if (column.Name == "Edit" && e.RowIndex >= 0)
            {
                // Find the appropriate user control in AddEditArea
                var userControl = AddEditArea.Controls.OfType<UserControl>().FirstOrDefault();

                if (userControl is AE_Faculty facultyControl)
                {
                    facultyControl.SetEditMode(id, row);
                }
                else if (userControl is AE_User UserlistControl)
                {
                    UserlistControl.SetEditMode(id, row);
                }
                else if (userControl is AE_CourseSubj courseSubjControl)
                {
                    courseSubjControl.SetEditMode(id, row);
                }
            }
            else if (column.Name == "Delete" && e.RowIndex >= 0)
            {
                var result = MessageBox.Show("Are you sure you want to delete this record?", "Delete Record",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        string query = "";
                        MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@id", id) };

                        switch (_currentListType)
                        {
                            case ListType.Faculty:
                                query = "DELETE FROM faculty WHERE id = @id";
                                break;

                            case ListType.Admin:
                                query = "DELETE FROM admins WHERE id = @id";
                                break;

                            case ListType.Course:
                                query = "DELETE FROM courses WHERE id = @id";
                                break;

                            case ListType.Subject:
                                query = "DELETE FROM subjects WHERE id = @id";
                                break;
                        }

                        DatabaseHelper.ExecuteNonQuery(query, parameters);
                        LoadData(); // Refresh the data
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting record: {ex.Message}", "Error",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private DataGridViewButtonColumn CreateActionButtonColumn(string name, string icon, string tooltip)
        {
            var btnColumn = new DataGridViewButtonColumn
            {
                Name = name,
                HeaderText = "",
                Text = icon,
                UseColumnTextForButtonValue = true,
                FlatStyle = FlatStyle.Flat,
                Width = 40,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,

                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    BackColor = _headerColor,
                    ForeColor = _textColor,
                    Font = new Font("Arial", 12),
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Padding = new Padding(5),
                },
                HeaderCell = new DataGridViewColumnHeaderCell()
                {
                    Style = new DataGridViewCellStyle()
                    {
                        BackColor = _headerColor,
                        ForeColor = _textColor,
                        Alignment = DataGridViewContentAlignment.MiddleCenter
                    }
                }
            };

            dataGridView1.CellToolTipTextNeeded += (sender, e) =>
            {
                if (e.ColumnIndex == dataGridView1.Columns[name].Index && e.RowIndex >= 0)
                    e.ToolTipText = tooltip;
            };

            return btnColumn;
        }
    }
}