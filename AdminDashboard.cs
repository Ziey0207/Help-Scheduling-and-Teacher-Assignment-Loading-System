using ReaLTaiizor.Extension;
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
    public partial class AdminDashboard : Form
    {
        // Add with your existing variables
        private Timer _sessionTimer;

        private const int SESSION_TIMEOUT_MINUTES = 30; // 30 minute timeout
        private DateTime _lastActivityTime;
        private string _currentSessionToken;
        private bool _explicitLogout = false;
        private int _adminId;

        private enum ContentArea
        {
            Home,
            Course,
            Subject,
            Faculty,
            Users,
            Schedule,
            Room,    // New
            Section,  // New
            ViewRooms
        }

        public AdminDashboard(string sessionToken = null)
        {
            InitializeComponent();
            _currentSessionToken = sessionToken;
            _adminId = DatabaseHelper.GetAdminIdBySession(sessionToken) ?? -1;
            InitializeSessionTracking();
            ShowContentArea(ContentArea.Home);
        }

        private void InitializeSessionTracking()
        {
            // Track user activity
            this.MouseMove += (s, e) => _lastActivityTime = DateTime.Now;
            this.KeyPress += (s, e) => _lastActivityTime = DateTime.Now;

            // Initialize timer
            _sessionTimer = new Timer { Interval = 60000 }; // Check every minute
            _sessionTimer.Tick += CheckSessionTimeout;
            _sessionTimer.Start();
            _lastActivityTime = DateTime.Now;
        }

        private void CheckSessionTimeout(object sender, EventArgs e)
        {
            if ((DateTime.Now - _lastActivityTime).TotalMinutes >= SESSION_TIMEOUT_MINUTES)
            {
                _sessionTimer.Stop();
                MessageBox.Show("Session expired due to inactivity");
                Logout();
            }
        }

        private void CloseDayPopups()
        {
            var openPopups = Application.OpenForms.OfType<DayPopup>().ToList();
            foreach (var popup in openPopups)
            {
                popup.Close();
            }
        }

        private void Logout()
        {
            Debug.WriteLine("Logout initiated");

            // Record logout in database
            if (!string.IsNullOrEmpty(_currentSessionToken))
            {
                DatabaseHelper.RecordLogout(_currentSessionToken);
            }

            // Clear session token (regardless of RememberMe)
            Properties.Settings.Default.CurrentSessionToken = "";
            Properties.Settings.Default.Save();

            // Show login form and close dashboard
            _explicitLogout = true;
            var homeForm = Application.OpenForms.OfType<Home>().FirstOrDefault();
            this.Close();

            if (homeForm != null)
            {
                homeForm.Show();
            }
        }

        private void btnLogout_Click(object sender, EventArgs e) => Logout();

        private void ShowContentArea(ContentArea area)
        {
            CloseDayPopups();

            // Clear current content
            SwitchingArea.Controls.Clear();

            Control contentControl = null;

            switch (area)
            {
                case ContentArea.Home:
                    contentControl = CreateHomeArea();
                    break;

                case ContentArea.Faculty:
                    contentControl = new ListCRUD(0); // 0 for Faculty
                    break;

                case ContentArea.Users:
                    contentControl = new ListCRUD(1); // 1 for Users
                    break;

                case ContentArea.Course:
                    contentControl = new ListCRUD(2); // 2 for Course
                    break;

                case ContentArea.Subject:
                    contentControl = new ListCRUD(3); // 3 for Subject
                    break;

                case ContentArea.Room:
                    contentControl = new ListCRUD(4); // 4 = Room
                    break;

                case ContentArea.Section:
                    contentControl = new ListCRUD(5); // 5 = Section
                    break;

                case ContentArea.Schedule:
                    contentControl = new Calendar();
                    break;

                case ContentArea.ViewRooms:
                    contentControl = new room();
                    break;
            }

            if (contentControl != null)
            {
                contentControl.Dock = DockStyle.Fill;
                SwitchingArea.Controls.Add(contentControl);
            }
        }

        private Control CreateHomeArea()
        {
            var areaHome = new AreaHome();
            string adminName = DatabaseHelper.GetAdminNameById(_adminId);
            areaHome.SetWelcomeMessage($"Welcome, {adminName}!");
            return areaHome;
        }

        // Button click handlers
        private void btnHome_Click(object sender, EventArgs e) => ShowContentArea(ContentArea.Home);

        private void btnCourse_Click(object sender, EventArgs e) => ShowContentArea(ContentArea.Course);

        private void btnSubject_Click(object sender, EventArgs e) => ShowContentArea(ContentArea.Subject);

        private void btnFaculty_Click(object sender, EventArgs e) => ShowContentArea(ContentArea.Faculty);

        private void btnUsers_Click(object sender, EventArgs e) => ShowContentArea(ContentArea.Users);

        private void btnSchedule_Click(object sender, EventArgs e) => ShowContentArea(ContentArea.Schedule);

        private void btnRoom_Click(object sender, EventArgs e) => ShowContentArea(ContentArea.Room);

        private void btnSection_Click(object sender, EventArgs e) => ShowContentArea(ContentArea.Section);

        private void btnViewRooms_Click(object sender, EventArgs e) => ShowContentArea(ContentArea.ViewRooms);

        private void AdminDashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            Debug.WriteLine($"Dashboard closing - RememberMe: {Properties.Settings.Default.RememberMe}");

            if (!string.IsNullOrEmpty(_currentSessionToken))
            {
                if (_explicitLogout)
                {
                    Debug.WriteLine("Recording explicit logout");
                    DatabaseHelper.RecordLogout(_currentSessionToken);
                }
                else
                {
                    Debug.WriteLine("Recording implicit logout (X button)");
                    DatabaseHelper.RecordLogout(_currentSessionToken, explicitLogout: false);

                    Debug.WriteLine("RememberMe true - closing app completely");
                    Application.Exit();
                    return;
                }
            }
        }
    }
}