using MySql.Data.MySqlClient;
using ReaLTaiizor.Controls;
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
    public partial class Calendar : UserControl
    {
        private bool _suppressSearchPopup = false; // Add this
        private DateTime currentDate;
        private SearchResultsPopup searchPopup;
        private Timer searchDelayTimer;

        public Calendar()
        {
            InitializeComponent();
            InitializeSearch();
            InitializeCalendar();

            calendarGrid.ColumnCount = 0;
            calendarGrid.RowCount = 0;

            currentDate = DateTime.Now;
        }

        public bool IsSearchPopupVisible()
        {
            return searchPopup?.Visible ?? false;
        }

        public bool IsPointInSearchPopup(Point point)
        {
            if (!IsSearchPopupVisible())
                return false;

            // Convert to popup's coordinates for proper bounds check
            Point popupPoint = searchPopup.PointToClient(this.PointToScreen(point));
            return searchPopup.ClientRectangle.Contains(popupPoint);
        }

        public bool IsPointInSearchTextBox(Point point)
        {
            // Convert to textbox's coordinates for proper bounds check
            Point textBoxPoint = txtSearch.PointToClient(this.PointToScreen(point));
            return txtSearch.ClientRectangle.Contains(textBoxPoint);
        }

        private void InitializeCalendar()
        {
            calendarGrid.ColumnCount = 0;
            calendarGrid.RowCount = 0;
            currentDate = DateTime.Now;
        }

        internal void HideSearchPopup()
        {
            if (searchPopup.Visible)
                searchPopup.Visible = false;
        }

        private void InitializeSearch()
        {
            Console.WriteLine("InitializeSearch called");

            // Search box events
            txtSearch.TextChanged += TxtSearch_TextChanged;
            txtSearch.Click += TxtSearch_Click;
            txtSearch.GotFocus += TxtSearch_GotFocus; // Add focus handler
            txtSearch.LostFocus += TxtSearch_LostFocus; // Add focus lost handler

            // Search delay timer
            searchDelayTimer = new Timer { Interval = 500 };
            searchDelayTimer.Tick += SearchDelayTimer_Tick;

            // Initialize popup
            searchPopup = new SearchResultsPopup
            {
                Visible = false,
                Size = new Size(400, 200),
                Tag = this
            };

            // Add click handler for the popup
            searchPopup.Click += SearchPopup_Click;

            // Handle DGV clicks in the popup
            if (searchPopup.Controls["dgvResults"] is DataGridView dgv)
            {
                Console.WriteLine("Attaching click handler to DataGridView");
                dgv.CellClick += DgvResults_CellClick;
            }
            else
            {
                Console.WriteLine("WARNING: Could not find dgvResults control in popup");
            }
        }

        private void TxtSearch_GotFocus(object sender, EventArgs e)
        {
            Console.WriteLine("Search textbox got focus");
            if (!string.IsNullOrEmpty(txtSearch.Text) && !_suppressSearchPopup)
            {
                Console.WriteLine("Search has text and not suppressed - showing results");
                ShowSearchResults(txtSearch.Text);
            }
        }

        private void TxtSearch_LostFocus(object sender, EventArgs e)
        {
            Console.WriteLine("Search textbox lost focus");
            // Don't hide the popup here - let the click handlers manage it
        }

        private void DgvResults_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine($"DataGridView cell clicked: Row {e.RowIndex}, Column {e.ColumnIndex}");
            // Don't hide popup or change focus - let user interact with the results
            // If you want to perform an action when they select a row, do it here
        }

        private void SearchPopup_Click(object sender, EventArgs e)
        {
            Console.WriteLine("SearchPopup clicked - maintaining focus");
            // Prevent focus change
            txtSearch.Focus();
        }

        private void TxtSearch_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Search textbox clicked");
            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                Console.WriteLine("Search has text - showing results");
                ShowSearchResults(txtSearch.Text);
            }
            // Keep focus on the textbox
            txtSearch.Focus();
        }

        private void Calendar_Load(object sender, EventArgs e)
        {
            InitializeDayNames();
            GenerateCalendar(currentDate.Year, currentDate.Month);
            UpdateMonthLabel();
            this.Dock = DockStyle.Fill;

            this.MouseDown += Calendar_MouseDown;
            calendarGrid.MouseDown += Calendar_MouseDown;
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

        private void GenerateCalendar(int year, int month)
        {
            calendarGrid.Controls.Clear();
            calendarGrid.RowStyles.Clear();
            calendarGrid.ColumnStyles.Clear();

            // Setup columns
            for (int i = 0; i < 7; i++)
            {
                calendarGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 7));
            }

            DateTime firstDay = new DateTime(year, month, 1);
            int daysInMonth = DateTime.DaysInMonth(year, month);
            int firstDayOfWeek = (int)firstDay.DayOfWeek;

            int weeks = (int)Math.Ceiling((firstDayOfWeek + daysInMonth) / 7.0);
            calendarGrid.RowCount = weeks;

            // Setup rows
            for (int i = 0; i < weeks; i++)
            {
                calendarGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / weeks));
            }

            DateTime currentDate = firstDay.AddDays(-firstDayOfWeek);

            for (int i = 0; i < weeks * 7; i++)
            {
                int row = i / 7;
                int col = i % 7;

                Day dayControl = new Day
                {
                    Dock = DockStyle.Fill,
                    DayNumber = currentDate.Day,
                    IsCurrentMonth = currentDate.Month == month,
                    Date = currentDate // Set the date property
                };

                dayControl.MouseDown += Day_MouseDown;

                calendarGrid.Controls.Add(dayControl, col, row);
                currentDate = currentDate.AddDays(1);
            }
        }

        public void StopSearchTimer()
        {
            Console.WriteLine("StopSearchTimer called. Timer Enabled: " + searchDelayTimer.Enabled);
            searchDelayTimer.Stop();
        }

        private void Day_MouseDown(object sender, MouseEventArgs e)
        {
            searchPopup.Visible = false;
            searchDelayTimer.Stop();
        }

        private void UpdateMonthLabel()
        {
            lblMonthYear.Text = currentDate.ToString("MMMM yyyy");
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            currentDate = currentDate.AddMonths(-1);
            GenerateCalendar(currentDate.Year, currentDate.Month);
            searchPopup.Visible = false; // Hide popup when changing month
            searchDelayTimer.Stop();
            UpdateMonthLabel();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            currentDate = currentDate.AddMonths(1);
            GenerateCalendar(currentDate.Year, currentDate.Month);
            searchPopup.Visible = false; // Hide popup when changing month
            searchDelayTimer.Stop();
            UpdateMonthLabel();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            Console.WriteLine("Text changed: " + txtSearch.Text.Trim());
            searchDelayTimer.Stop();
            searchDelayTimer.Start();
        }

        private void SearchDelayTimer_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("Search timer tick - performing search");
            searchDelayTimer.Stop();
            ShowSearchResults(txtSearch.Text.Trim());
            // Make sure search maintains focus
            txtSearch.Focus();
        }

        private void ShowSearchResults(string term)
        {
            Console.WriteLine($"ShowSearchResults called with term: '{term}'");

            if (string.IsNullOrEmpty(term))
            {
                Console.WriteLine("Term is empty - hiding popup");
                searchPopup.Hide();
                searchDelayTimer.Stop();
                return;
            }

            searchDelayTimer.Stop();
            Console.WriteLine("Search timer stopped");

            // Position popup relative to search box
            Point screenCoords = txtSearch.PointToScreen(new Point(0, txtSearch.Height));
            searchPopup.Location = screenCoords;
            Console.WriteLine($"Positioning popup at screen coordinates: {screenCoords}");

            // Load and show results
            var results = SearchSchedules(term);
            Console.WriteLine($"Found {results.Count} search results");

            searchPopup.LoadResults(results);

            if (results.Count > 0)
            {
                if (!searchPopup.Visible)
                {
                    Console.WriteLine("Showing popup (was hidden)");
                    searchPopup.Show(this.ParentForm);
                    Console.WriteLine($"Popup visibility after Show(): {searchPopup.Visible}");

                    // Ensure textbox keeps focus
                    Console.WriteLine("Refocusing textbox");
                    txtSearch.Focus();
                }
                else
                {
                    Console.WriteLine("Popup already visible - maintaining state");
                }
            }
            else
            {
                Console.WriteLine("No results found - hiding popup");
                searchPopup.Hide();
            }
        }

        private List<Schedule> SearchSchedules(string term)
        {
            var parameters = new[] {
        new MySqlParameter("@searchTerm", $"%{term}%")
    };

            using (var reader = DatabaseHelper.ExecuteReader(
                @"SELECT * FROM schedules
        WHERE subject LIKE @searchTerm
           OR teacher LIKE @searchTerm
           OR room LIKE @searchTerm", parameters))
            {
                var results = new List<Schedule>();
                while (reader.Read())
                {
                    results.Add(new Schedule
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Subject = reader["subject"].ToString(),
                        Teacher = reader["teacher"].ToString(),
                        Date = Convert.ToDateTime(reader["date"]),
                        Room = reader["room"].ToString(),
                        TimeIn = reader["time_in"].ToString(),
                        TimeOut = reader["time_out"].ToString()
                    });
                }
                return results;
            }
        }

        private void Calendar_MouseDown(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Calendar_MouseDown triggered");

            if (searchPopup.Visible)
            {
                Point clickPoint = (sender == this)
                    ? e.Location
                    : this.PointToClient((sender as Control).PointToScreen(e.Location));

                Console.WriteLine($"Click at point: {clickPoint}, checking against search popup and textbox");

                bool inPopup = IsPointInSearchPopup(clickPoint);
                bool inTextBox = IsPointInSearchTextBox(clickPoint);

                Console.WriteLine($"Click in popup: {inPopup}, Click in textbox: {inTextBox}");

                if (!inPopup && !inTextBox)
                {
                    Console.WriteLine("Click outside search areas - hiding popup");
                    _suppressSearchPopup = true;
                    searchPopup.Hide();
                    searchDelayTimer.Stop();

                    Console.WriteLine("Setting focus to calendar");
                    this.Focus();
                    this.ParentForm?.Activate();

                    Console.WriteLine("Scheduling reset of suppress flag");
                    Task.Delay(200).ContinueWith(t =>
                    {
                        _suppressSearchPopup = false;
                        Console.WriteLine("Reset suppress flag to false");
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                }
                else if (inTextBox)
                {
                    Console.WriteLine("Click in search box - maintaining focus");
                    txtSearch.Focus();
                }
            }
        }
    }
}