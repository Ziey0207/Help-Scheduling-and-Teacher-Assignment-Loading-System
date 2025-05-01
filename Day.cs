using MySql.Data.MySqlClient;
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
    public partial class Day : UserControl
    {
        private bool isCurrentMonth;
        private static Day _lastSelectedDay; // Track last clicked day
        private static DayPopup _popup; // Shared popup reference
        private DateTime _date;

        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
            }
        }

        private Label CreateScheduleLabel(Schedule schedule)
        {
            var lbl = new Label
            {
                Text = $"{Format12HourTime(schedule.TimeIn)}-{Format12HourTime(schedule.TimeOut)} " +
                       $"{GetLastName(schedule.Teacher)} " +
                       $"{schedule.Section} {schedule.Room}",
                AutoSize = false,
                Height = 20, // Increased height
                Font = new Font("Arial", 10),
                Margin = new Padding(0, 0, 0, 2),
                AutoEllipsis = true
            };

            // Set initial width
            lbl.Width = flpSchedules.ClientSize.Width - 5;

            return lbl;
        }

        private string GetLastName(string fullName)
        {
            var parts = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return parts.Length > 0 ? parts.Last() : fullName;
        }

        public Day()
        {
            InitializeComponent();

            AttachClickToControls(this);

            // Existing handlers
            flpSchedules.SizeChanged += (sender, e) => UpdateLabelWidths();
        }

        private void AttachClickToControls(Control parent)
        {
            // Attach to current control
            parent.Click += DayControl_Click;

            // Recursively attach to all child controls
            foreach (Control child in parent.Controls)
            {
                AttachClickToControls(child);
            }

            // Handle future dynamically added controls
            parent.ControlAdded += (sender, e) => AttachClickToControls(e.Control);
        }

        public int DayNumber
        {
            get { return int.Parse(lblDay.Text); }
            set { lblDay.Text = value.ToString(); }
        }

        private string Format12HourTime(TimeSpan time)
        {
            DateTime tempDate = DateTime.Today.Add(time);
            return tempDate.ToString("hh\\:mm tt"); // Example: "09:00 AM" or "02:30 PM"
        }

        private void UpdateLabelWidths()
        {
            foreach (Control ctrl in flpSchedules.Controls)
            {
                if (ctrl is Label lbl)
                {
                    lbl.Width = flpSchedules.ClientSize.Width - 5;
                }
            }
        }

        public bool IsCurrentMonth
        {
            get => isCurrentMonth;
            set
            {
                isCurrentMonth = value;
                panel1.BackColor = value ? Color.White : Color.LightGray;
                lblDay.ForeColor = value ? Color.Black : Color.Gray;
            }
        }

        private void DayControl_Click(object sender, EventArgs e)
        {
            // Reset previous selection
            if (_lastSelectedDay != null && _lastSelectedDay != this)
            {
                _lastSelectedDay.ResetSelection();
            }

            // Highlight current selection
            panel1.BackColor = Color.LightBlue;
            _lastSelectedDay = this;

            // Show popup window
            ShowEventPopup();
        }

        public void ResetSelection()
        {
            // Restore original colors
            panel1.BackColor = IsCurrentMonth ? Color.White : Color.LightGray;
        }

        public void LoadSchedules(List<Schedule> schedules)
        {
            flpSchedules.Controls.Clear();

            var filtered = schedules
                .OrderBy(s => s.TimeIn)  // Sort by start time
                .Take(3)                 // Still show max 3 entries
                .ToList();

            foreach (var schedule in filtered)
            {
                var lbl = CreateScheduleLabel(schedule);
                flpSchedules.Controls.Add(lbl);
            }

            // Add empty slots if needed
            for (int i = filtered.Count; i < 3; i++)
            {
                flpSchedules.Controls.Add(new Label { Height = 15, Margin = new Padding(0) });
            }
        }

        private void ShowEventPopup()
        {
            if (_popup == null || _popup.IsDisposed)
            {
                _popup = new DayPopup();
                _popup.FormClosed += (sender, e) =>
                {
                    ResetSelection();
                    _lastSelectedDay = null;
                };
                _popup.Show(this.ParentForm); // Show the popup relative to the current control
            }

            // Update popup content
            _popup.UpdatePopupTitle(this.Date);
            _popup.BringToFront();
        }
    }
}