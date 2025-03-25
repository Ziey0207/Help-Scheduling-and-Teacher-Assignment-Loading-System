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

namespace Help_Scheduling_and_Teacher_Assignment_Loading_System
{
    public partial class FacultyListandUsersList : UserControl
    {
        private bool isFaculty, isAdmin;

        // Add with your other class variables
        private Color _headerColor = Color.FromArgb(19, 15, 64); // Material Blue

        private Color _rowColor = Color.White;
        private Color _alternatingRowColor = Color.FromArgb(240, 240, 240);

        public FacultyListandUsersList(int FacultyorUsers)
        {
            InitializeComponent();

            ApplyModernDataGridViewStyle();
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
            string query;
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

        private void AdjustColumns()
        {
            dataGridView1.Columns.Clear();

            if (isFaculty)
            {
                AddColumn("id", "ID", 60, false);
                AddColumn("id_no", "ID No", 100);
                AddColumn("name", "Name", 200);
                AddColumn("email", "Email", 200);
                AddColumn("contact_number", "Contact", 120);

                // Custom checkbox column for active status
                var activeCol = new DataGridViewCheckBoxColumn
                {
                    Name = "is_active",
                    HeaderText = "Active",
                    DataPropertyName = "is_active",
                    ReadOnly = true,
                    Width = 60
                };
                dataGridView1.Columns.Add(activeCol);
            }
            else if (isAdmin)
            {
                AddColumn("id", "ID", 60, false);
                AddColumn("username", "Username", 150);
                AddColumn("email", "Email", 200);
                AddColumn("contact_number", "Contact", 120);

                // Password column with masked text
                var passwordCol = new DataGridViewTextBoxColumn
                {
                    Name = "password",
                    HeaderText = "Password",
                    DataPropertyName = "password",
                    Width = 150,
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        ForeColor = Color.Gray,
                        Font = new Font("Segoe UI", 8)
                    }
                };
                dataGridView1.Columns.Add(passwordCol);
            }

            // Make the last column fill remaining space
            if (dataGridView1.Columns.Count > 0)
            {
                dataGridView1.Columns[dataGridView1.Columns.Count - 1].AutoSizeMode =
                    DataGridViewAutoSizeColumnMode.Fill;
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

        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor =
                    Color.FromArgb(245, 245, 245);
            }
        }

        private void dataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor =
                    e.RowIndex % 2 == 0 ? _rowColor : _alternatingRowColor;
            }
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

        // Modern DataGridView style

        private void ApplyModernDataGridViewStyle()
        {
            // Basic Grid Setup
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            // Header Styling
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = _headerColor;
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 15, FontStyle.Bold);
            dataGridView1.ColumnHeadersHeight = 40;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Row Styling
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.DefaultCellStyle.BackColor = _rowColor;
            dataGridView1.DefaultCellStyle.ForeColor = Color.FromArgb(66, 66, 66);
            dataGridView1.DefaultCellStyle.Font = new Font("Arial", 12);
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = _alternatingRowColor;
            dataGridView1.RowTemplate.Height = 60;

            // Selection Styling
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(229, 243, 255);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.FromArgb(66, 66, 66);

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
            dataGridView1.AllowUserToResizeColumns = true;
        }

        private void AddColumn(string name, string headerText, int width, bool visible = true)
        {
            var col = new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = headerText,
                DataPropertyName = name,
                Width = width,
                Visible = visible,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Padding = new Padding(5, 0, 5, 0) // Add cell padding
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
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(229, 243, 255);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;
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
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (dataGridView1.Columns.Count > 0)
            {
                // Keep the last column filling remaining space
                dataGridView1.Columns[dataGridView1.Columns.Count - 1].AutoSizeMode =
                    DataGridViewAutoSizeColumnMode.Fill;
            }
        }
    }
}