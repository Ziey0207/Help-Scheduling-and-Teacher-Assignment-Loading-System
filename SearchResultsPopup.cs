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
            this.ControlBox = false;

            Console.WriteLine("Configuring DataGridView as read-only");
            this.dgvResults.AllowUserToAddRows = false;
            this.dgvResults.ReadOnly = true;
            this.dgvResults.EditMode = DataGridViewEditMode.EditProgrammatically;
            this.dgvResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvResults.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgvResults.AllowUserToResizeRows = false;

            this.dgvResults.RowHeadersVisible = false;
            this.dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvResults.Columns.Add("Date", "Date");
            this.dgvResults.Columns.Add("Subject", "Subject");
            this.dgvResults.Columns.Add("Teacher", "Teacher");
            this.dgvResults.Columns.Add("Room", "Room");
            this.dgvResults.Columns.Add("Time", "Time");
        }

        public new DialogResult ShowDialog(IWin32Window owner)
        {
            this.Show(owner as Form);
            return DialogResult.None; // Dialog result doesn't matter
        }

        public new void Hide()
        {
            Console.WriteLine("Hiding SearchResultsPopup");
            this.TopMost = false; // Stop forcing the popup to stay on top
            base.Hide();
            Console.WriteLine($"Popup visibility after Hide(): {this.Visible}");
        }

        public new void Show(IWin32Window owner)
        {
            Console.WriteLine("Show() called on SearchResultsPopup");
            base.Show(owner);

            // When shown, attach to the parent form's events
            if (owner is Form parentForm)
            {
                Console.WriteLine("Attaching to parent form events");
                // Use owner's Deactivate event to hide
                parentForm.Deactivate += ParentForm_Deactivate;

                // Handle tab switching
                foreach (Control control in parentForm.Controls)
                {
                    if (control is TabControl tabControl)
                    {
                        Console.WriteLine("Found TabControl - attaching to SelectedIndexChanged");
                        tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
                    }
                }
            }
        }

        private void ParentForm_Deactivate(object sender, EventArgs e)
        {
            Console.WriteLine("Parent form deactivated - hiding popup");
            this.Hide();
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine("Tab switched - hiding popup");
            this.Hide();
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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Console.WriteLine("SearchResultsPopup closing");
            // Remove event handlers to prevent memory leaks
            if (Owner is Form parentForm)
            {
                parentForm.Deactivate -= ParentForm_Deactivate;

                foreach (Control control in parentForm.Controls)
                {
                    if (control is TabControl tabControl)
                    {
                        tabControl.SelectedIndexChanged -= TabControl_SelectedIndexChanged;
                    }
                }
            }
            base.OnFormClosing(e);
        }
    }
}