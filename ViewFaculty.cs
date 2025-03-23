using MySql.Data.MySqlClient;
using Scheduling_and_Teacher_Loading_Assignment_System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Help_Scheduling_and_Teacher_Assignment_Loading_System
{
    public partial class ViewFaculty : Form
    {
        private int facultyId;

        public ViewFaculty(int id)
        {
            InitializeComponent();
            this.facultyId = id;
            LoadFacultyData();
        }

        private void LoadFacultyData()
        {
            string query = "SELECT * FROM faculty WHERE id = @id";

            using (MySqlDataReader reader = DatabaseHelper.ExecuteReader(query, new MySqlParameter[]
            {
                new MySqlParameter("@id", facultyId)
            }))
            {
                if (reader.Read())
                {
                    FacultyName.Text = $"{reader["last_name"]}, {reader["first_name"]} {reader["middle_name"]}";
                    Gender.Text = reader["gender"].ToString();
                    Email.Text = reader["email"].ToString();
                    Contact.Text = reader["contact_number"].ToString();
                    Address.Text = reader["address"].ToString();
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}