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
    public partial class Day : UserControl
    {
        private bool isCurrentMonth;
        private static Day _lastSelectedDay; // Track last clicked day
        private static DayPopup _popup; // Shared popup reference

        public DateTime Date { get; set; } // Property to hold the date of the day

        public Day()
        {
            InitializeComponent();

            // Add click handlers for both the control and the label
            this.Click += DayControl_Click;
            lblDay.Click += DayControl_Click;
        }

        public int DayNumber
        {
            get { return int.Parse(lblDay.Text); }
            set { lblDay.Text = value.ToString(); }
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