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
            string tableName, query;
            if (isFaculty)
            {
                query = $"SELECT * FROM faculty WHERE last_name LIKE @searchText";
            }
            else if (isAdmin)
            {
                query = $"SELECT * FROM admins WHERE username LIKE @searchText";
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
                poisonDataGridView1.DataSource = dt;
                AdjustColumns();
            }
        }

        private void AdjustColumns()
        {
            poisonDataGridView1.Columns.Clear();

            if (isFaculty)
            {
                poisonDataGridView1.Columns.Add("id", "ID");
                poisonDataGridView1.Columns.Add("id_no", "ID No");
                poisonDataGridView1.Columns.Add("name", "Name");
                poisonDataGridView1.Columns.Add("email", "Email");
                poisonDataGridView1.Columns.Add("contact", "Contact");
                poisonDataGridView1.Columns.Add("action", "Action");
            }
            else if (isAdmin)
            {
                poisonDataGridView1.Columns.Add("id", "ID");
                poisonDataGridView1.Columns.Add("username", "Username");
                poisonDataGridView1.Columns.Add("email", "Email");
                poisonDataGridView1.Columns.Add("contact_number", "Contact Number");
                poisonDataGridView1.Columns.Add("password", "Password");
            }

            poisonDataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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