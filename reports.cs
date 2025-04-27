using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Help_Scheduling_and_Teacher_Assignment_Loading_System
{
    public partial class reports : Form
    {
        public reports()
        {
            InitializeComponent();
            LoadGraphReports();
        }


        private async void LoadGraphReports()
        {
            string connectionString = "server=localhost;user=root;password=;database=school_management;";
            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                // ------------------- ROOM CHART -------------------
                chartRoom.Series.Clear();
                chartRoom.ChartAreas.Clear();
                chartRoom.ChartAreas.Add(new ChartArea("RoomArea"));

                var roomSeries = new Series("Room Usage")
                {
                    ChartType = SeriesChartType.Pie
                };
                chartRoom.Series.Add(roomSeries);

                using (var cmdRoom = new MySqlCommand("SELECT room, COUNT(*) AS usage_count FROM schedules GROUP BY room ORDER BY usage_count DESC", connection))
                using (var readerRoom = await cmdRoom.ExecuteReaderAsync())
                {
                    while (await readerRoom.ReadAsync())
                    {
                        string room = readerRoom["room"].ToString();
                        int count = Convert.ToInt32(readerRoom["usage_count"]);

                        roomSeries.Points.AddXY(room, count);
                    }
                }

                await connection.CloseAsync(); // Close then open again for next command
                await connection.OpenAsync();

                // ------------------- SECTION (COURSE) CHART -------------------
                chartSection.Series.Clear();
                chartSection.ChartAreas.Clear();
                chartSection.ChartAreas.Add(new ChartArea("SectionArea"));

                var sectionSeries = new Series("Section Usage")
                {
                    ChartType = SeriesChartType.Pie
                };
                chartSection.Series.Add(sectionSeries);

                using (var cmdSection = new MySqlCommand("SELECT course_code, COUNT(*) AS usage_count FROM schedules GROUP BY course_code ORDER BY usage_count DESC", connection))
                using (var readerSection = await cmdSection.ExecuteReaderAsync())
                {
                    while (await readerSection.ReadAsync())
                    {
                        string section = readerSection["course_code"].ToString();
                        int count = Convert.ToInt32(readerSection["usage_count"]);

                        sectionSeries.Points.AddXY(section, count);
                    }
                }

                await connection.CloseAsync();
                await connection.OpenAsync();

                // ------------------- TEACHER CHART -------------------
                chartTeacher.Series.Clear();
                chartTeacher.ChartAreas.Clear();
                chartTeacher.ChartAreas.Add(new ChartArea("TeacherArea"));

                var teacherSeries = new Series("Teacher Usage")
                {
                    ChartType = SeriesChartType.Pie
                };
                chartTeacher.Series.Add(teacherSeries);

                using (var cmdTeacher = new MySqlCommand("SELECT teacher, COUNT(*) AS usage_count FROM schedules GROUP BY teacher ORDER BY usage_count DESC", connection))
                using (var readerTeacher = await cmdTeacher.ExecuteReaderAsync())
                {
                    while (await readerTeacher.ReadAsync())
                    {
                        string teacher = readerTeacher["teacher"].ToString();
                        int count = Convert.ToInt32(readerTeacher["usage_count"]);

                        teacherSeries.Points.AddXY(teacher, count);
                    }
                }

                chartRoom.Invalidate();
                chartSection.Invalidate();
                chartTeacher.Invalidate();
            }
        }

        private void chartRoom_Click(object sender, EventArgs e)
        {

        }

        private void reports_Load(object sender, EventArgs e)
        {

        }

        private void chartTeacher_Click(object sender, EventArgs e)
        {

        }

        private void chartSection_Click(object sender, EventArgs e)
        {

        }
    }
}
