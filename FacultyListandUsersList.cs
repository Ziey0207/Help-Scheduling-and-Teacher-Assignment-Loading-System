using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReaLTaiizor.Controls;
using MySql.Data.MySqlClient;
using Scheduling_and_Teacher_Loading_Assignment_System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Help_Scheduling_and_Teacher_Assignment_Loading_System
{
    public partial class FacultyListandUsersList : UserControl
    {
        private bool isFaculty, isAdmin;

        // Updated color scheme
        private Color _headerColor = Color.FromArgb(19, 15, 64);  // Dark blue header

        private Color _rowColor = Color.FromArgb(60, 63, 120);     // Row color
        private Color _textColor = Color.FromArgb(220, 220, 220);  // Light text
        private Color _buttonHoverColor = Color.FromArgb(19, 15, 64); // Hover color as requested

        public FacultyListandUsersList(int FacultyorUsers)
        {
            InitializeComponent();

            ApplyDarkModeTableStyle(); // Updated to use dark theme
            ConfigureSelectionBehavior();

            switch (FacultyorUsers)
            {
                case 0:
                    this.isFaculty = true;
                    txtHeaderMain.Text = "Faculty List";
                    break;

                case 1:
                    this.isAdmin = true;
                    txtHeaderMain.Text = "Users List";
                    break;
            }

            btnDelete.Visible = false;
            btnEdit.Visible = false;
            btnView.Visible = false;

            LoadData();
        }

        private void LoadData(string searchText = "")
        {
            // Add this at the start
            dataGridView1.Visible = false;
            Cursor.Current = Cursors.WaitCursor;

            string query;
            try
            {
                if (isFaculty)
                {
                    if (string.IsNullOrEmpty(searchText))
                    {
                        query = $@"
                        SELECT
                            id,
                            id_no,
                            CONCAT(last_name, ', ', first_name, ' ', middle_name) AS name,
                            email,
                            contact_number,
                            is_active
                        FROM faculty";
                    }
                    else
                    {
                        query = $@"
                        SELECT
                            id,
                            id_no,
                            CONCAT(last_name, ', ', first_name, ' ', middle_name) AS name,
                            email,
                            contact_number,
                            is_active
                        FROM faculty
                        WHERE last_name LIKE @searchText OR first_name LIKE @searchText OR middle_name LIKE @searchText";
                    }
                }
                else if (isAdmin)
                {
                    if (string.IsNullOrEmpty(searchText))
                    {
                        query = $"SELECT * FROM admins";
                    }
                    else
                    {
                        query = $"SELECT * FROM admins WHERE username LIKE @searchText OR email LIKE @searchText";
                    }
                }
                else
                {
                    return;
                }

                using (MySqlDataReader reader = DatabaseHelper.ExecuteReader(query, new MySqlParameter[]
                {
                new MySqlParameter("@searchText", $"%{searchText}%")
                }))
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    // Disable auto-generated columns
                    dataGridView1.AutoGenerateColumns = false;

                    // Clear existing columns
                    dataGridView1.Columns.Clear();

                    // Bind the DataTable to the DataGridView
                    dataGridView1.DataSource = dt;

                    // Adjust columns based on the view
                    AdjustColumns();
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                dataGridView1.Visible = true;
            }
        }

        private void AdjustColumns()
        {
            dataGridView1.Columns.Clear();

            if (isFaculty)
            {
                // ID column - auto-sized to content
                AddColumn("id", "ID", 50, false, DataGridViewAutoSizeColumnMode.DisplayedCells);

                // ID No column
                AddColumn("id_no", "ID No", 100);

                // Name column - takes remaining space
                AddColumn("name", "Name", 200, true, DataGridViewAutoSizeColumnMode.Fill);

                // Email column
                AddColumn("email", "Email", 200, true, DataGridViewAutoSizeColumnMode.Fill);

                // Contact column
                AddColumn("contact_number", "Contact", 120, true, DataGridViewAutoSizeColumnMode.DisplayedCells);

                // Active column - auto-sized to content
                var activeCol = new DataGridViewCheckBoxColumn
                {
                    Name = "is_active",
                    HeaderText = "Active",
                    DataPropertyName = "is_active",
                    ReadOnly = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
                    FlatStyle = FlatStyle.Flat,
                    MinimumWidth = 60
                };
                dataGridView1.Columns.Add(activeCol);

                // Action buttons - auto-sized to content
                dataGridView1.Columns.Add(CreateActionButtonColumn("View", "👁", "View details"));
                dataGridView1.Columns.Add(CreateActionButtonColumn("Edit", "✏", "Edit record"));
                dataGridView1.Columns.Add(CreateActionButtonColumn("Delete", "🗑", "Delete record"));
            }
            else if (isAdmin)
            {
                // ID column - auto-sized to content
                AddColumn("id", "ID", 50, false, DataGridViewAutoSizeColumnMode.DisplayedCells);

                // Username column
                AddColumn("username", "Username", 150, true, DataGridViewAutoSizeColumnMode.Fill);

                // Email column - takes remaining space
                AddColumn("email", "Email", 200, true, DataGridViewAutoSizeColumnMode.Fill);

                // Contact column
                AddColumn("contact_number", "Contact", 120, true, DataGridViewAutoSizeColumnMode.DisplayedCells);

                // Password column
                var passwordCol = new DataGridViewTextBoxColumn
                {
                    Name = "password",
                    HeaderText = "Password",
                    DataPropertyName = "password",
                    Width = 150,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        ForeColor = Color.Silver,
                        Font = new Font("Arial", 10)
                    }
                };
                dataGridView1.Columns.Add(passwordCol);

                // Action buttons - auto-sized to content
                dataGridView1.Columns.Add(CreateActionButtonColumn("Edit", "✏", "Edit record"));
                dataGridView1.Columns.Add(CreateActionButtonColumn("Delete", "🗑", "Delete record"));
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadData(txtSearch.Text);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            bool isRowSelected = dataGridView1.SelectedRows.Count > 0;

            // Show Edit and Delete buttons if a row is selected
            btnEdit.Visible = isRowSelected;
            btnDelete.Visible = isRowSelected;

            // Show View button only for faculty and if a row is selected
            btnView.Visible = isFaculty && isRowSelected;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (isFaculty)
            {
                OpenEditAddForm(0, -1);
            }
            else if (isAdmin)
            {
                OpenEditAddForm(1, -1);
            }

            LoadData();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["id"].Value);
                if (isFaculty)
                {
                    OpenEditAddForm(0, recordId: id);
                }
                else if (isAdmin)
                {
                    OpenEditAddForm(1, recordId: id);
                }
                LoadData();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["id"].Value);
                DeleteRecord(id);
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["id"].Value);
                OpenViewForm(id);
            }
        }

        private void OpenEditAddForm(int state, int recordId = -1)
        {
            EdiitAddFacultyAndAdmins editAddForm = new EdiitAddFacultyAndAdmins(state, recordId);
            editAddForm.Show();
            // Refresh the DataGridView after the form is closed
            LoadData();
        }

        private void OpenViewForm(int id)
        {
            ViewFaculty viewForm = new ViewFaculty(id)
            {
                StartPosition = FormStartPosition.CenterParent
            };
            viewForm.ShowDialog();
        }

        private void DeleteRecord(int id)
        {
            string tableName = "";
            if (isFaculty)
            {
                tableName = "faculty";
            }
            else if (isAdmin)
            {
                tableName = "admins";
            }
            string query = $"DELETE FROM {tableName} WHERE id = @id";

            DialogResult result = MessageBox.Show("Are you sure you want to delete this record?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, new MySqlParameter[]
                {
                    new MySqlParameter("@id", id)
                });

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Record deleted successfully!");
                    LoadData(); // Refresh the DataGridView
                }
                else
                {
                    MessageBox.Show("Failed to delete record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
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
            btnEdit.Visible = false;
            btnDelete.Visible = false;
            btnView.Visible = false;

            // Auto-size all rows after data is loaded
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Height = row.GetPreferredHeight(row.Index, DataGridViewAutoSizeRowMode.AllCells, true);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            {
                int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);
                string buttonName = senderGrid.Columns[e.ColumnIndex].Name;

                switch (buttonName)
                {
                    case "View":
                        OpenViewForm(id);
                        break;

                    case "Edit":
                        if (isFaculty)
                        {
                            OpenEditAddForm(0, recordId: id);
                        }
                        else if (isAdmin)
                        {
                            OpenEditAddForm(1, recordId: id);
                        }
                        break;

                    case "Delete":
                        DeleteRecord(id);
                        break;
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
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
                MinimumWidth = 40,
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    BackColor = _rowColor,
                    ForeColor = _textColor,
                    Font = new Font("Arial", 15),
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Padding = new Padding(5)
                }
            };

            // Add tooltip
            dataGridView1.CellToolTipTextNeeded += (sender, e) =>
            {
                if (e.ColumnIndex == dataGridView1.Columns[name].Index && e.RowIndex >= 0)
                    e.ToolTipText = tooltip;
            };

            return btnColumn;
        }

        // NEW METHOD: Handle button hover effects
        private void dataGridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var column = dataGridView1.Columns[e.ColumnIndex];
            if (column is DataGridViewButtonColumn)
            {
                dataGridView1.Cursor = Cursors.Hand;
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = _buttonHoverColor;
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.White;
            }
        }

        // NEW METHOD: Reset button appearance when mouse leaves
        private void dataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var column = dataGridView1.Columns[e.ColumnIndex];
            if (column is DataGridViewButtonColumn)
            {
                dataGridView1.Cursor = Cursors.Default;
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = _rowColor;
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = _textColor;
            }
        }
    }
}