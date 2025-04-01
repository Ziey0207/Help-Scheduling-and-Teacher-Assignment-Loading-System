using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Help_Scheduling_and_Teacher_Assignment_Loading_System
{
    public partial class teacherdash : Form
    {
        private string connString = "server=localhost;database=school_management;uid=root;pwd=;Convert Zero Datetime = True";
        private DateTime currentMonth = DateTime.Now;

        public teacherdash()
        {
            InitializeComponent();
            LoadSchedules();
        }

        private void LoadSchedules(string teacherFilter = "")
        {
            // Set DataGridView properties
            dgvTeacherSchedules.EnableHeadersVisualStyles = false;
            dgvTeacherSchedules.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dgvTeacherSchedules.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvTeacherSchedules.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            dgvTeacherSchedules.DefaultCellStyle.BackColor = Color.LightGray;
            dgvTeacherSchedules.DefaultCellStyle.ForeColor = Color.Black;
            dgvTeacherSchedules.DefaultCellStyle.Font = new Font("Arial", 9);
            dgvTeacherSchedules.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
            dgvTeacherSchedules.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvTeacherSchedules.GridColor = Color.DarkGray;

            // Set row height
            dgvTeacherSchedules.RowTemplate.Height = 30;

            // Alternating row colors
            dgvTeacherSchedules.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            dgvTeacherSchedules.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

            // Border and selection mode
            dgvTeacherSchedules.BorderStyle = BorderStyle.FixedSingle;
            dgvTeacherSchedules.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvTeacherSchedules.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTeacherSchedules.MultiSelect = false;

            dgvTeacherSchedules.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvTeacherSchedules.DefaultCellStyle.Padding = new Padding(5); // Adds spacing in cells

            dgvTeacherSchedules.RowHeadersVisible = false;

            dgvTeacherSchedules.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    string query = "SELECT teacher, subject, date, time_in, time_out, room FROM schedules";

                    if (!string.IsNullOrEmpty(teacherFilter))
                    {
                        query += " WHERE teacher LIKE @teacher";
                    }

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    if (!string.IsNullOrEmpty(teacherFilter))
                    {
                        cmd.Parameters.AddWithValue("@teacher", "%" + teacherFilter + "%");
                    }

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvTeacherSchedules.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadSchedules(txtSearch.Text);
        }

        private void dgvScheduleResults_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}