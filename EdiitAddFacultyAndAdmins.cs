using MySql.Data.MySqlClient;
using Mysqlx;
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
    public partial class EdiitAddFacultyAndAdmins : Form
    {
        private bool isFaculty, isAdmin;
        private int id;

        public EdiitAddFacultyAndAdmins(int State, int id = -1)
        {
            InitializeComponent();
            this.id = id;

            switch (State)
            {
                case 0:
                    this.isFaculty = true;
                    FormHeader.Text = "Faculty Update";
                    label1.Text = "Last Name";
                    label2.Text = "First Name";
                    label3.Text = "Middle Name";

                    break;

                case 1:
                    this.isAdmin = true;
                    FormHeader.Text = "User Update";
                    label1.Text = "Username";
                    label2.Text = "Password";
                    FacultyInputs.Hide();
                    label3.Hide();
                    Error3.Hide();
                    input3.Hide();
                    isActive.Hide();

                    foreach (Control control in FacultyInputs.Controls)
                    {
                        control.Hide();
                    }

                    this.Height = 422;
                    break;
            }

            if (id != -1)
            {
                LoadRecordData();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validate inputs
            bool isValid = ValidateInputs();

            if (!isValid)
            {
                MessageBox.Show("Please fill up all fields correctly.");
                return;
            }

            if (isFaculty)
            {
                SaveFacultyRecord();
            }
            else if (isAdmin)
            {
                SaveAdminRecord();
            }

            MessageBox.Show("Record saved successfully!");
            this.Close();
        }

        private bool ValidateInputs()
        {
            bool isValid = true;

            // Hide all error labels initially
            Error1.Hide();
            Error2.Hide();
            Error3.Hide();
            Error4.Hide();
            Error5.Hide();
            Error6.Hide();
            Error7.Hide();
            Error8.Hide();

            // Validate Faculty fields
            if (isFaculty)
            {
                if (string.IsNullOrEmpty(txtIDNO.Text))
                {
                    Error1.Show();
                    isValid = false;
                }
                if (string.IsNullOrEmpty(input1.Text))
                {
                    Error2.Show();
                    isValid = false;
                }
                if (string.IsNullOrEmpty(input2.Text))
                {
                    Error3.Show();
                    isValid = false;
                }
                if (string.IsNullOrEmpty(input3.Text))
                {
                    Error4.Show();
                    isValid = false;
                }
                if (string.IsNullOrEmpty(input4.Text))
                {
                    Error5.Show();
                    isValid = false;
                }
                if (string.IsNullOrEmpty(input5.Text))
                {
                    Error6.Show();
                    isValid = false;
                }
                if (string.IsNullOrEmpty(cmbGender.Text))
                {
                    Error7.Show();
                    isValid = false;
                }
                if (string.IsNullOrEmpty(txtAddress.Text))
                {
                    Error8.Show();
                    isValid = false;
                }
            }
            // Validate Admin fields
            else if (isAdmin)
            {
                if (string.IsNullOrEmpty(input1.Text))
                {
                    Error2.Show();
                    isValid = false;
                }
                if (string.IsNullOrEmpty(input2.Text))
                {
                    Error3.Show();
                    isValid = false;
                }
                if (string.IsNullOrEmpty(input4.Text))
                {
                    Error5.Show();
                    isValid = false;
                }
                if (string.IsNullOrEmpty(input5.Text))
                {
                    Error6.Show();
                    isValid = false;
                }
            }

            return isValid;
        }

        private void LoadRecordData()
        {
            string tableName;
            if (isFaculty)
            {
                tableName = "faculty";
            }
            else
            {
                tableName = "admins";
            }
            string query = $"SELECT * FROM {tableName} WHERE id = @id";

            using (MySqlDataReader reader = DatabaseHelper.ExecuteReader(query, new MySqlParameter[]
            {
            new MySqlParameter("@id", id)
            }))
            {
                if (reader.Read())
                {
                    if (isFaculty)
                    {
                        txtIDNO.Text = reader["id_no"].ToString();
                        input1.Text = reader["last_name"].ToString();
                        input2.Text = reader["first_name"].ToString();
                        input3.Text = reader["middle_name"].ToString();
                        input4.Text = reader["email"].ToString();
                        input5.Text = reader["contact_number"].ToString();
                        cmbGender.SelectedItem = reader["gender"].ToString();
                        isActive.Checked = Convert.ToBoolean(reader["is_active"]);
                        txtAddress.Text = reader["address"].ToString();
                    }
                    else
                    {
                        input1.Text = reader["username"].ToString();
                        input4.Text = reader["email"].ToString();
                        input5.Text = reader["contact_number"].ToString();
                        input2.Text = reader["password"].ToString();
                    }
                }
            }
        }

        private void SaveFacultyRecord()
        {
            string query;
            if (id == -1)
            {
                query = @"
                    INSERT INTO faculty (id_no, last_name, first_name, middle_name, email, contact_number, gender, address, is_active)
                    VALUES (@id_no, @last_name, @first_name, @middle_name, @email, @contact_number, @gender, @address, @is_active)";
            }
            else
            {
                query = @"
                    UPDATE faculty
                    SET id_no = @id_no, last_name = @last_name, first_name = @first_name,
                        middle_name = @middle_name, email = @email, contact_number = @contact_number,
                        gender = @gender, address = @address, is_active = @is_active
                    WHERE id = @id";
            }

            DatabaseHelper.ExecuteNonQuery(query, new MySqlParameter[]
            {
                new MySqlParameter("@id_no", txtIDNO.Text),
                new MySqlParameter("@last_name", input1.Text),
                new MySqlParameter("@first_name", input2.Text),
                new MySqlParameter("@middle_name", input3.Text),
                new MySqlParameter("@email", input4.Text),
                new MySqlParameter("@contact_number", input5.Text),
                new MySqlParameter("@gender", cmbGender.SelectedItem.ToString()),
                new MySqlParameter("@is_active", isActive.Checked ? 1 : 0),
                new MySqlParameter("@address", txtAddress.Text),
                new MySqlParameter("@id", id)
            });
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SaveAdminRecord()
        {
            string query;
            if (id == -1)
            {
                query = @"
                    INSERT INTO admins (username, email, contact_number, password)
                    VALUES (@username, @email, @contact_number, @password)";
            }
            else
            {
                query = @"
                    UPDATE admins
                    SET username = @username, email = @email, contact_number = @contact_number, password = @password
                    WHERE id = @id";
            }

            DatabaseHelper.ExecuteNonQuery(query, new MySqlParameter[]
            {
                new MySqlParameter("@username", input1.Text),
                new MySqlParameter("@email", input4.Text),
                new MySqlParameter("@contact_number", input5.Text),
                new MySqlParameter("@password", input2.Text),
                new MySqlParameter("@id", id)
            });
        }
    }
}