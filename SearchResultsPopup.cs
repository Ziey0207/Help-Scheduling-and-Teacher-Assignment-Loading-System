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
    public partial class SearchResultsPopup : Form
    {
        public SearchResultsPopup()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;
            this.TopMost = true;

            this.dgvResults.AllowUserToAddRows = false;
            this.dgvResults.RowHeadersVisible = false;
            this.dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvResults.Columns.Add("Date", "Date");
            this.dgvResults.Columns.Add("Subject", "Subject");
            this.dgvResults.Columns.Add("Teacher", "Teacher");
            this.dgvResults.Columns.Add("Room", "Room");
            this.dgvResults.Columns.Add("Time", "Time");
            // In SearchResultsPopup constructor:
            // Single Deactivate event
            this.Deactivate += (s, e) =>
            {
                this.Hide(); // Uses the overridden Hide() method above
                if (this.Tag is Calendar calendar)
                    calendar.StopSearchTimer();
            };
        }

        public new void Hide()
        {
            Console.WriteLine("Hiding SearchResultsPopup");
            this.TopMost = false; // Stop forcing the popup to stay on top
            base.Hide();
            Console.WriteLine($"Popup visibility after Hide(): {this.Visible}");
        }

        protected override void OnDeactivate(EventArgs e)
        {
            Console.WriteLine("Popup deactivated.");
            base.OnDeactivate(e);
            this.Hide();
            if (this.Tag is Calendar calendar)
            {
                Console.WriteLine("Stopping calendar timer.");
                calendar.StopSearchTimer();
            }
        }

        public void LoadResults(List<Schedule> results)
        {
            dgvResults.Rows.Clear();
            foreach (var schedule in results)
            {
                dgvResults.Rows.Add(
                    schedule.Date.ToString("d"),
                    schedule.Subject,
                    schedule.Teacher,
                    schedule.Room,
                    $"{schedule.TimeIn} - {schedule.TimeOut}"
                );
            }
        }
    }
}