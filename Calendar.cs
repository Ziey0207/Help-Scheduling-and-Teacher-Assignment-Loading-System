using MySql.Data.MySqlClient;
using ReaLTaiizor.Controls;
using Scheduling_and_Teacher_Loading_Assignment_System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Help_Scheduling_and_Teacher_Assignment_Loading_System
{
    public partial class Calendar : UserControl
    {
        private System.Windows.Forms.Panel loadingOverlay;
        private DateTime currentDate;
        private Timer searchDelayTimer;
        private bool showAllMode = false;
        private Dictionary<DateTime, List<Schedule>> monthlySchedules;

        public Calendar()
        {
            InitializeComponent();
            InitializeLoadingOverlay();
            InitializeCalendar();
            SetupSearchComponents();

            calendarGrid.ColumnCount = 0;
            calendarGrid.RowCount = 0;
            currentDate = DateTime.Now;
        }

        private void InitializeCalendar()
        {
            calendarGrid.ColumnCount = 0;
            calendarGrid.RowCount = 0;
            currentDate = DateTime.Now;
        }

        private void InitializeLoadingOverlay()
        {
            loadingOverlay = new System.Windows.Forms.Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Visible = false
            };
            Controls.Add(loadingOverlay);
        }

        private void SetupSearchComponents()
        {
            // Configure SplitContainer
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Panel1Collapsed = true;
            splitContainer1.SplitterDistance = 150;
            splitContainer1.IsSplitterFixed = true;

            splitContainer1.SplitterWidth = 1; // Make splitter thinner
            splitContainer1.Panel1MinSize = 0;
            splitContainer1.Panel2MinSize = 0;
            splitContainer1.FixedPanel = FixedPanel.None;

            // Configure DataGridView
            dgvSearchResults.Dock = DockStyle.Fill;
            dgvSearchResults.AutoGenerateColumns = false;
            dgvSearchResults.ReadOnly = true;

            // Configure search delay timer
            searchDelayTimer = new Timer { Interval = 500 };
            searchDelayTimer.Tick += SearchDelayTimer_Tick;
            txtSearch.TextChanged += TxtSearch_TextChanged;

            ConfigureSearchGrid();
        }

        private void ConfigureSearchGrid()
        {
            // Clear existing columns and settings
            dgvSearchResults.Columns.Clear();
            dgvSearchResults.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            // Grid behavior properties
            dgvSearchResults.AutoGenerateColumns = false;
            dgvSearchResults.ReadOnly = true;
            dgvSearchResults.AllowUserToAddRows = false;
            dgvSearchResults.AllowUserToDeleteRows = false;
            dgvSearchResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSearchResults.MultiSelect = false;
            dgvSearchResults.RowHeadersVisible = false;
            dgvSearchResults.EditMode = DataGridViewEditMode.EditProgrammatically;

            // Fixed dimensions and scrolling
            dgvSearchResults.Height = (30 * 4) + dgvSearchResults.ColumnHeadersHeight;
            dgvSearchResults.ScrollBars = ScrollBars.Vertical;
            dgvSearchResults.RowTemplate.Height = 25;
            dgvSearchResults.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            // Prevent all resizing
            dgvSearchResults.AllowUserToResizeRows = false;
            dgvSearchResults.AllowUserToResizeColumns = false;

            // Add columns with specific sizing rules
            dgvSearchResults.Columns.AddRange(new[]
            {
        new DataGridViewTextBoxColumn // Subject
        {
            DataPropertyName = "Subject",
            HeaderText = "Subject",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            MinimumWidth = 150,
            Resizable = DataGridViewTriState.False
        },
        new DataGridViewTextBoxColumn // Teacher (fill column)
        {
            DataPropertyName = "Teacher",
            HeaderText = "Teacher",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            FillWeight = 45, // Percentage of remaining space
            MinimumWidth = 120,
            Resizable = DataGridViewTriState.False
        },
        new DataGridViewTextBoxColumn // Date
        {
            DataPropertyName = "Date",
            HeaderText = "Date",
            DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd" },
            AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            MinimumWidth = 100,
            Resizable = DataGridViewTriState.False
        },
        new DataGridViewTextBoxColumn // Time (fill column)
        {
            DataPropertyName = "Time",
            HeaderText = "Time",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
            Resizable = DataGridViewTriState.False
        },
        new DataGridViewTextBoxColumn // Room
        {
            DataPropertyName = "Room",
            HeaderText = "Room",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
            FillWeight = 20, // Percentage of remaining space
            MinimumWidth = 80,
            Resizable = DataGridViewTriState.False
        }
    });

            // Visual styling
            dgvSearchResults.DefaultCellStyle.SelectionBackColor = Color.SteelBlue;
            dgvSearchResults.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvSearchResults.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10);
        }

        private async void SearchDelayTimer_Tick(object sender, EventArgs e)
        {
            searchDelayTimer.Stop();
            Debug.WriteLine("[DEBUG] === SEARCH STARTED ===");

            try
            {
                string term = txtSearch.Text.Trim();
                Debug.WriteLine($"[DEBUG] Search term: '{term}'");

                if (string.IsNullOrEmpty(term))
                {
                    Debug.WriteLine("[DEBUG] Empty search term - collapsing panel");
                    splitContainer1.Panel1Collapsed = true;
                    return;
                }

                // Execute search with case-insensitive matching
                var results = await SearchSchedules(term);
                Debug.WriteLine($"[DEBUG] Found {results.Count} matches");

                if (results.Count > 0)
                {
                    splitContainer1.Panel1Collapsed = false;
                    dgvSearchResults.DataSource = results;

                    // Auto-size columns once after data binding
                    dgvSearchResults.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                    Debug.WriteLine($"[UI] Panel expanded. Grid rows: {dgvSearchResults.Rows.Count}");
                }
                else
                {
                    Debug.WriteLine("[DEBUG] No matches found - collapsing panel");
                    splitContainer1.Panel1Collapsed = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] Search failed: {ex.ToString()}");
                MessageBox.Show($"Search error: {ex.Message}");
                splitContainer1.Panel1Collapsed = true;
            }
            finally
            {
                Debug.WriteLine("[DEBUG] === SEARCH COMPLETED ===\n");
            }
        }

        private async Task<List<Schedule>> SearchSchedules(string term)
        {
            var parameters = new[] {
                 new MySqlParameter("@searchTerm", $"%{term}%")
            };

            Debug.WriteLine($"[SQL] Executing query with term: {parameters[0].Value}");

            using (var reader = await DatabaseHelper.ExecuteReaderAsync(
                    @"SELECT id, subject, teacher, date, room, time_in, time_out
        FROM schedules
        WHERE subject LIKE @searchTerm
           OR teacher LIKE @searchTerm
           OR room LIKE @searchTerm", parameters))
            {
                var results = new List<Schedule>();
                while (await reader.ReadAsync())
                {
                    Debug.WriteLine($"[Data] Found match: {reader["subject"]}");
                    results.Add(new Schedule
                    {
                        Id = reader.GetInt32("id"),
                        Subject = reader.GetString("subject"),
                        Teacher = reader.GetString("teacher"),
                        Date = reader.GetDateTime("date"),
                        Room = reader.GetString("room"),
                        TimeIn = reader.GetTimeSpan("time_in"),
                        TimeOut = reader.GetTimeSpan("time_out"),
                    });
                }
                return results;
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            searchDelayTimer.Stop();
            searchDelayTimer.Start();
        }

        private void Calendar_Load(object sender, EventArgs e)
        {
            InitializeDayNames();
            GenerateCalendar(currentDate.Year, currentDate.Month);
            UpdateMonthLabel();
            this.Dock = DockStyle.Fill;
        }

        private void InitializeDayNames()
        {
            // Configure the day names panel
            dayNamesPanel.ColumnCount = 7;
            dayNamesPanel.RowCount = 1;
            dayNamesPanel.ColumnStyles.Clear();
            dayNamesPanel.RowStyles.Clear();

            // Match column widths with calendar grid
            for (int i = 0; i < 7; i++)
            {
                dayNamesPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 7));
            }
            dayNamesPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            // Add day labels
            string[] dayAbbreviations = { "Sunday", "Monday", "Tueday", "Wednesday", "Thursday", "Friday", "Saturday" };
            for (int i = 0; i < 7; i++)
            {
                var lbl = new Label
                {
                    Text = dayAbbreviations[i],
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Arial", 15, FontStyle.Bold),
                    ForeColor = Color.White,
                };
                dayNamesPanel.Controls.Add(lbl, i, 0);
            }
        }

        private async void GenerateCalendar(int year, int month)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            loadingOverlay.Visible = true;
            calendarGrid.Visible = false;
            dayNamesPanel.Visible = false;
            this.SuspendLayout();

            try
            {
                // Async data loading
                await Task.Run(() => LoadMonthlySchedules());

                // Batch control creation
                var days = new List<Day>();
                DateTime firstDay = new DateTime(year, month, 1);
                int daysInMonth = DateTime.DaysInMonth(year, month);
                int firstDayOfWeek = (int)firstDay.DayOfWeek;
                int weeks = (int)Math.Ceiling((firstDayOfWeek + daysInMonth) / 7.0);
                DateTime currentDate = firstDay.AddDays(-firstDayOfWeek);

                // Create all day controls first
                for (int i = 0; i < weeks * 7; i++)
                {
                    var day = new Day
                    {
                        Dock = DockStyle.Fill,
                        DayNumber = currentDate.Day,
                        IsCurrentMonth = currentDate.Month == month,
                        Date = currentDate
                    };

                    if (monthlySchedules.TryGetValue(currentDate.Date, out var dateSchedules))
                        day.LoadSchedules(dateSchedules);

                    days.Add(day);
                    currentDate = currentDate.AddDays(1);
                }

                // Bulk UI update
                Invoke((MethodInvoker)delegate
                {
                    calendarGrid.Controls.Clear();
                    calendarGrid.ColumnStyles.Clear();
                    calendarGrid.RowStyles.Clear();

                    // Setup grid structure
                    calendarGrid.ColumnCount = 7;
                    calendarGrid.RowCount = weeks;

                    for (int i = 0; i < 7; i++)
                        calendarGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 7));

                    for (int i = 0; i < weeks; i++)
                        calendarGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / weeks));

                    // Add all days at once
                    calendarGrid.Controls.AddRange(days.ToArray());
                });
            }
            finally
            {
                Invoke((MethodInvoker)delegate
                {
                    loadingOverlay.Visible = false;
                    calendarGrid.Visible = true;
                    dayNamesPanel.Visible = true;
                    this.ResumeLayout(true);
                });

                sw.Stop();
                Debug.WriteLine($"[Performance] Calendar loaded in {sw.ElapsedMilliseconds}ms");
            }
        }

        private void UpdateMonthLabel()
        {
            lblMonthYear.Text = currentDate.ToString("MMMM yyyy");
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            currentDate = currentDate.AddMonths(-1);
            GenerateCalendar(currentDate.Year, currentDate.Month);
            UpdateMonthLabel();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            currentDate = currentDate.AddMonths(1);
            GenerateCalendar(currentDate.Year, currentDate.Month);
            UpdateMonthLabel();
        }

        private void LoadMonthlySchedules()
        {
            monthlySchedules = GetMonthlySchedules(currentDate);
        }

        private Dictionary<DateTime, List<Schedule>> GetMonthlySchedules(DateTime month)
        {
            var schedules = new Dictionary<DateTime, List<Schedule>>();

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand(
                    @"SELECT date, time_in, time_out, teacher, section, room
              FROM schedules
              WHERE YEAR(date) = @year AND MONTH(date) = @month",
                    conn);
                cmd.Parameters.AddWithValue("@year", month.Year);
                cmd.Parameters.AddWithValue("@month", month.Month);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        try
                        {
                            var date = reader.GetDateTime("date").Date;
                            var section = reader.GetString("section");
                            var room = reader.GetString("room");

                            Debug.WriteLine($"[DB DATA] Date: {date:yyyy-MM-dd} | " +
                   $"Section: {section} | Room: {room}");

                            if (!schedules.ContainsKey(date))
                            {
                                schedules[date] = new List<Schedule>();
                            }

                            schedules[date].Add(new Schedule
                            {
                                TimeIn = reader.GetTimeSpan("time_in"),
                                TimeOut = reader.GetTimeSpan("time_out"),
                                Teacher = reader.GetString("teacher"),
                                Section = section,
                                Room = room
                            });
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"[ERROR] Reading record: {ex.Message}");
                        }
                    }
                }
            }
            return schedules;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void hopeButton1_Click(object sender, EventArgs e)
        {
        }
    }
}