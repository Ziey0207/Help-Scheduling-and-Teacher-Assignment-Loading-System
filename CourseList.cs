using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Scheduling_and_Teacher_Loading_Assignment_System;
using System.Data.SqlClient;

namespace Help_Scheduling_and_Teacher_Assignment_Loading_System
{
    public partial class CourseList : UserControl
    {
        private bool isEditMode = false; // Flag to track if the form is in Edit mode
        private int currentCourseId = -1; // Track the course being edited
        private Timer searchTimer;

        public bool isCourses, isSubject;

        public CourseList(int IfAnythingOtherThanCourses)
        {
            InitializeComponent();
            switch (IfAnythingOtherThanCourses)
            {
                case 0:
                    isCourses = true;
                    break;

                case 1:
                    isSubject = true;
                    txtHeaderMain.Text = "Subject List";
                    txtHeaderinfo.Text = "Subject";
                    label2.Text = "Subject";
                    txtAddEditForm.Text = "Add Subject";
                    break;
            }

            // Initialize the Timer
            searchTimer = new Timer();
            searchTimer.Interval = 500; // 500 milliseconds delay
            searchTimer.Tick += SearchTimer_Tick;
        }

        private void CourseList_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
            LoadCourses();
            // Attach KeyDown event to the search TextBox
            txtCourseSearch.TextChanged += TxtSearch_TextChanged;
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            // Reset the Timer
            searchTimer.Stop();
            searchTimer.Start();
        }

        private void SearchTimer_Tick(object sender, EventArgs e)
        {
            // Stop the Timer
            searchTimer.Stop();

            // Trigger the search
            string searchQuery = txtCourseSearch.Text;
            LoadCourses(searchQuery);
        }

        private void LoadCourses(string searchQuery = "")
        {
            // Clear existing rows (except the first control, which is the header)
            while (flowLayoutPanel1.Controls.Count > 1)
            {
                Control control = flowLayoutPanel1.Controls[flowLayoutPanel1.Controls.Count - 1];
                flowLayoutPanel1.Controls.Remove(control);
                control.Dispose();
            }

            // Fetch data from the database
            DataTable dt = GetCourses(searchQuery);

            // Debug: Check if data is being fetched
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("No data found in the database.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Dynamically create rows
            foreach (DataRow row in dt.Rows)
            {
                // Create a new CourseRowControl
                ItemRow rowControl = new ItemRow();

                if (isSubject)
                {
                    rowControl.CourseId = Convert.ToInt32(row["id"]);
                    rowControl.CourseName = row["subject_name"].ToString();
                    rowControl.CourseDescription = row["description"].ToString();
                }
                else if (isCourses)
                {
                    rowControl.CourseId = Convert.ToInt32(row["id"]);
                    rowControl.CourseName = row["course_name"].ToString();
                    rowControl.CourseDescription = row["description"].ToString();
                }

                // Update the UI with course data
                rowControl.UpdateCourseData();

                // Attach event handlers
                rowControl.EditClicked += (sender, courseId) =>
                {
                    // Handle Edit button click
                    EditCourse(courseId);
                };

                rowControl.DeleteClicked += (sender, courseId) =>
                {
                    // Handle Delete button click
                    DeleteCourse(courseId);
                };

                // Add the row control to the FlowLayoutPanel
                flowLayoutPanel1.Controls.Add(rowControl);
            }
        }

        private void EditCourse(int courseId)
        {
            string query;
            try
            {
                // Switch to Edit mode
                isEditMode = true;
                currentCourseId = courseId; // Set the currentCourseId
                if (isSubject)
                {
                    txtAddEditForm.Text = "Edit Subject";

                    query = "SELECT subject_name, description FROM subjects WHERE id = @id";
                    using (MySqlConnection conn = DatabaseHelper.GetConnection())
                    {
                        conn.Open();
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", courseId);
                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    // Load course name and description into separate fields
                                    txtCourseName.Text = reader["subject_name"].ToString();
                                    txtCourseDescription.Text = reader["description"].ToString();
                                    //currentCourseId = Convert.ToInt32(reader["id"]);
                                }
                            }
                        }
                    }
                }
                else if (isCourses)
                {
                    txtAddEditForm.Text = "Edit Course";

                    query = "SELECT course_name, description FROM courses WHERE id = @id";
                    using (MySqlConnection conn = DatabaseHelper.GetConnection())
                    {
                        conn.Open();
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", courseId);
                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    // Load course name and description into separate fields
                                    txtCourseName.Text = reader["course_name"].ToString();
                                    txtCourseDescription.Text = reader["description"].ToString();
                                    //currentCourseId = Convert.ToInt32(reader["id"]);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading course data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteCourse(int courseId)
        {
            if (isCourses)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this course?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    string query = "DELETE FROM courses WHERE id = @id";
                    using (MySqlConnection conn = DatabaseHelper.GetConnection())
                    {
                        conn.Open();
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", courseId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    LoadCourses(); // Refresh the course list
                }
            }
            else if (isSubject)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this subject?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    string query = "DELETE FROM subjects WHERE id = @id";
                    using (MySqlConnection conn = DatabaseHelper.GetConnection())
                    {
                        conn.Open();
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", courseId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    LoadCourses(); // Refresh the course list
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string courseName = txtCourseName.Text.Trim();
            string description = txtCourseDescription.Text.Trim();

            // Clear previous error messages
            txtCourseAddandEditError.Text = "";

            // Validation
            if (string.IsNullOrEmpty(courseName))
            {
                txtCourseAddandEditError.Show();
                if (isCourses)
                {
                    txtCourseAddandEditError.Text = "Course name is required.";
                }
                else if (isSubject)
                {
                    txtCourseAddandEditError.Text = "Subject name is required.";
                }
                return;
            }

            if (string.IsNullOrEmpty(description))
            {
                txtCourseAddandEditError.Show();
                txtCourseAddandEditError.Text = "Description is required.";
                return;
            }

            // Save or update the course
            try
            {
                string query = "";

                if (isEditMode)
                {
                    if (isCourses)
                    {
                        // Update existing course
                        query = "UPDATE courses SET course_name = @courseName, description = @description WHERE id = @id";
                    }
                    else if (isSubject)
                    {
                        query = "UPDATE subjects SET subject_name = @subjectName, description = @description WHERE id = @id";
                    }
                }
                else
                {
                    if (isCourses)
                    {
                        // Add new course
                        query = "INSERT INTO courses (course_name, description) VALUES (@courseName, @description)";
                    }
                    else if (isSubject)
                    {
                        query = "INSERT INTO subjects (subject_name, description) VALUES (@subjectName, @description)";
                    }
                }

                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        if (isCourses)
                        {
                            cmd.Parameters.AddWithValue("@courseName", courseName);
                            cmd.Parameters.AddWithValue("@description", description);
                            if (isEditMode)
                            {
                                cmd.Parameters.AddWithValue("@id", currentCourseId);
                            }
                        }
                        else if (isSubject)
                        {
                            cmd.Parameters.AddWithValue("@subjectName", courseName);
                            cmd.Parameters.AddWithValue("@description", description);
                            if (isEditMode)
                            {
                                cmd.Parameters.AddWithValue("@id", currentCourseId);
                            }
                        }

                        cmd.ExecuteNonQuery();
                    }
                }

                if (isCourses)
                {
                    txtCourseAddandEditError.Text = "Course Saved Successfully";
                    txtCourseAddandEditError.Show();
                }
                else if (isSubject)
                {
                    txtCourseAddandEditError.Text = "Subject Saved Successfully";
                    txtCourseAddandEditError.Show();
                }
                // Hide the Add/Edit panel after saving

                // Refresh the course list
                LoadCourses();

                // Switch back to Add mode
                btnCancel_Click(sender, e);
            }
            catch (Exception ex)
            {
                txtCourseAddandEditError.Text = "Error: " + ex.Message;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Reset the form for "Add" mode
            isEditMode = false;
            currentCourseId = -1;
            txtCourseName.Text = "";
            txtCourseDescription.Text = "";

            if (isCourses)
            {
                // Show the Add/Edit panel in "Add" mode
                txtAddEditForm.Text = "Add Course";
            }
            else if (isSubject)
            {
                // Show the Add/Edit panel in "Add" mode
                txtAddEditForm.Text = "Add Subject";
            }
        }

        /*
        private void ShowAddEditPanel(string title)
        {
            lblFormTitle.Text = title; // Set the form title
            panelAddEdit.Visible = true; // Show the Add/Edit panel
        }

        private void HideAddEditPanel()
        {
            panelAddEdit.Visible = false; // Hide the Add/Edit panel
        }
        */

        private DataTable GetCourses(string searchQuery = "")
        {
            DataTable dt = new DataTable();
            if (isSubject)
            {
                try
                {
                    string query = "SELECT id, subject_name, description FROM subjects WHERE subject_name LIKE @searchQuery OR description LIKE @searchQuery";
                    using (MySqlConnection conn = DatabaseHelper.GetConnection())
                    {
                        conn.Open();
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@searchQuery", $"%{searchQuery}%");
                            using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                            {
                                adapter.Fill(dt);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error connecting to the database: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (isCourses)
            {
                try
                {
                    string query = "SELECT id, course_name, description FROM courses WHERE course_name LIKE @searchQuery OR description LIKE @searchQuery";
                    using (MySqlConnection conn = DatabaseHelper.GetConnection())
                    {
                        conn.Open();
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@searchQuery", $"%{searchQuery}%");
                            using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                            {
                                adapter.Fill(dt);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error connecting to the database: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return dt;
        }
    }
}