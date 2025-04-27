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
            Console.WriteLine("SearchResultsPopup constructor called");
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;
            this.TopMost = true;

            Console.WriteLine("Configuring DataGridView as read-only");
            this.dgvResults.AllowUserToAddRows = false;
            this.dgvResults.ReadOnly = true;
            this.dgvResults.EditMode = DataGridViewEditMode.EditProgrammatically;
            this.dgvResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

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
                Console.WriteLine("SearchResultsPopup Deactivate event fired");
                if (this.Tag is Calendar calendar)
                {
                    Console.WriteLine("Stopping search timer from Deactivate event");
                    calendar.StopSearchTimer();
                }
            };
        }

        public new void Hide()
        {
            Console.WriteLine("Hiding SearchResultsPopup");
            this.TopMost = false; // Stop forcing the popup to stay on top
            base.Hide();
            Console.WriteLine($"Popup visibility after Hide(): {this.Visible}");
        }

        public void LoadResults(List<Schedule> results)
        {
            Console.WriteLine($"LoadResults called with {results.Count} schedules");
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
            Console.WriteLine($"Loaded {dgvResults.Rows.Count} rows into the grid");
        }
    }
}