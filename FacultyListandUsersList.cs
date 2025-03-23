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

        public FacultyListandUsersList(int FacultyorUsers)
        {
            InitializeComponent();

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
                    contact_number
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
                    contact_number
                FROM faculty
                WHERE last_name LIKE @searchText or first_name LIKE @searchText OR middle_name LIKE @searchText";
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
                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "id",
                    HeaderText = "ID",
                    DataPropertyName = "id" // Bind to the "id" column in the DataTable
                });

                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "id_no",
                    HeaderText = "ID No",
                    DataPropertyName = "id_no" // Bind to the "id_no" column in the DataTable
                });

                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "name",
                    HeaderText = "Name",
                    DataPropertyName = "name" // Bind to the "name" column in the DataTable
                });

                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "email",
                    HeaderText = "Email",
                    DataPropertyName = "email" // Bind to the "email" column in the DataTable
                });

                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "contact",
                    HeaderText = "Contact",
                    DataPropertyName = "contact_number" // Bind to the "contact_number" column in the DataTable
                });

                dataGridView1.Columns.Add(new DataGridViewButtonColumn
                {
                    Name = "action",
                    HeaderText = "Action",
                    Text = "Edit",
                    UseColumnTextForButtonValue = true
                });
            }
            else if (isAdmin)
            {
                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "id",
                    HeaderText = "ID",
                    DataPropertyName = "id" // Bind to the "id" column in the DataTable
                });

                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "username",
                    HeaderText = "Username",
                    DataPropertyName = "username" // Bind to the "username" column in the DataTable
                });

                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "email",
                    HeaderText = "Email",
                    DataPropertyName = "email" // Bind to the "email" column in the DataTable
                });

                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "contact_number",
                    HeaderText = "Contact Number",
                    DataPropertyName = "contact_number" // Bind to the "contact_number" column in the DataTable
                });

                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "password",
                    HeaderText = "Password",
                    DataPropertyName = "password" // Bind to the "password" column in the DataTable
                });
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadData(txtSearch.Text);
        }

        private void FacultyListandUsersList_Load(object sender, EventArgs e)
        {
        }
    }
}