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
using System.Windows.Forms.DataVisualization.Charting;

namespace Help_Scheduling_and_Teacher_Assignment_Loading_System
{
    public partial class AreaHome : UserControl
    {
        public AreaHome()
        {
            InitializeComponent();
            lblWelcome.Text = "Welcome!";
            LoadGraphReports();
        }

        private void AreaHome_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
        }

        public void SetWelcomeMessage(string message)
        {
            // Assuming you have a label called lblWelcome in your AreaHome control
            lblWelcome.Text = message;
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

                ApplyFlatDesign(chartRoom, "Room Utilization");
                ApplyColorPalette(chartRoom);
                Toggle3DEffect(chartRoom, true); // Disable 3D
                ToggleDataLabels(chartRoom, true);

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

                ApplyFlatDesign(chartSection, "Course Distribution");
                ApplyColorPalette(chartSection);
                Toggle3DEffect(chartSection, true); // Disable 3D
                ToggleDataLabels(chartSection, true);

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

                ApplyFlatDesign(chartTeacher, "Teacher Workload");
                ApplyColorPalette(chartTeacher);
                Toggle3DEffect(chartTeacher, true); // Disable 3D
                ToggleDataLabels(chartTeacher, true);

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

                chartRoom.MouseMove += (sender, e) => HandleChartHover(chartRoom, e);
                chartSection.MouseMove += (sender, e) => HandleChartHover(chartSection, e);
                chartTeacher.MouseMove += (sender, e) => HandleChartHover(chartTeacher, e);
                chartRoom.MouseLeave += (sender, e) => ResetChartHover(chartRoom);
                chartSection.MouseLeave += (sender, e) => ResetChartHover(chartSection);
                chartTeacher.MouseLeave += (sender, e) => ResetChartHover(chartTeacher);
            }
        }

        private void ApplyFlatDesign(Chart chart, string title)
        {
            // Chart Title
            chart.Titles.Clear();
            chart.Titles.Add(title);
            chart.Titles[0].Font = new Font("Segoe UI", 12, FontStyle.Bold);
            chart.Titles[0].ForeColor = Color.White;

            // Chart Area
            chart.ChartAreas[0].BackColor = Color.Transparent;
            chart.ChartAreas[0].AxisX.LabelStyle.Enabled = false; // Hide X-axis labels
            chart.ChartAreas[0].AxisY.LabelStyle.Enabled = false; // Hide Y-axis labels

            // Series Styling
            foreach (var series in chart.Series)
            {
                series.Font = new Font("Segoe UI", 10);
                series.LabelForeColor = Color.White;
                series["PieLabelStyle"] = "Outside"; // or "Disabled" for no labels
                series.BorderColor = Color.Transparent;
                series.BorderWidth = 2;
                series.ChartType = SeriesChartType.Doughnut; // Change to Doughnut for a flat design
                series["DoughnutRadius"] = "75"; // Adjust radius for doughnut effect
            }

            // Legend
            chart.Legends.Clear();
            chart.Legends.Add(new Legend());
            chart.Legends[0].Font = new Font("Segoe UI", 10);
            chart.Legends[0].ForeColor = Color.White;
            chart.Legends[0].BackColor = Color.Transparent;
            chart.Legends[0].Docking = Docking.Bottom;
            chart.Legends[0].Alignment = StringAlignment.Center;
            chart.Legends[0].TableStyle = LegendTableStyle.Wide;
        }

        private void HandleChartHover(Chart chart, MouseEventArgs e)
        {
            var hit = chart.HitTest(e.X, e.Y);
            if (hit.ChartElementType == ChartElementType.DataPoint)
            {
                // Reset all points
                foreach (DataPoint point in chart.Series[0].Points)
                {
                    point.BorderWidth = 1;
                    point.BorderColor = Color.FromArgb(40, 40, 40);
                }

                // Highlight hovered point
                chart.Series[0].Points[hit.PointIndex].BorderWidth = 3;
                chart.Series[0].Points[hit.PointIndex].BorderColor = Color.White;
                chart.Refresh();
            }
        }

        private void ResetChartHover(Chart chart)
        {
            if (chart.Series.Count > 0 && chart.Series[0].Points.Count > 0)
            {
                foreach (DataPoint point in chart.Series[0].Points)
                {
                    point.BorderWidth = 1;
                    point.BorderColor = Color.FromArgb(40, 40, 40);
                }
                chart.Refresh();
            }
        }

        // Toggle 3D Effects
        private void Toggle3DEffect(Chart chart, bool enable3D)
        {
            chart.ChartAreas[0].Area3DStyle.Enable3D = enable3D;
            chart.ChartAreas[0].Area3DStyle.Inclination = 30; // Adjust angle
        }

        // Toggle Data Labels
        private void ToggleDataLabels(Chart chart, bool showLabels)
        {
            foreach (var series in chart.Series)
            {
                series.IsValueShownAsLabel = showLabels;
            }
        }

        // Apply Color Palette
        private void ApplyColorPalette(Chart chart)
        {
            // Use flat design colors
            Color[] palette = {
        Color.FromArgb(1, 122, 201),   // True Blue
        Color.FromArgb(212, 58, 58),   // Jasper
        Color.FromArgb(253, 123, 10),  // Heat Wave
        Color.FromArgb(237, 238, 85),    // Corn
        Color.FromArgb(99, 169, 45),   // RYB Green
        Color.FromArgb(108, 75, 164),    // Royal Purple
        Color.FromArgb(106, 137, 204),   // Soft Periwinkle
        Color.FromArgb(72, 201, 176)     // Mint Green
    };

            chart.Palette = ChartColorPalette.None;
            chart.PaletteCustomColors = palette;
        }
    }
}