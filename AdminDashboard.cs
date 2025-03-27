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

        public bool AreaUsed = false;
        private int Option;

        public AdminDashboard(string sessionToken = null)
        {
            InitializeComponent();
            _currentSessionToken = sessionToken;

            _adminId = DatabaseHelper.GetAdminIdBySession(sessionToken) ?? -1;

            InitializeSessionTracking();
            btnHome_Click(null, null);
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

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Logout();
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

        private void btnHome_Click(object sender, EventArgs e)
        {
            if (!AreaUsed)
            {
                AreaHome areaHome = new AreaHome();
                string adminName = DatabaseHelper.GetAdminNameById(_adminId);
                areaHome.SetWelcomeMessage($"Welcome, {adminName}!");
                AreaUsed = true;
                Option = 0;
                SwitchingArea.Controls.Add(areaHome);
            }
            else if (AreaUsed && !(Option == 0))
            {
                Control controlRemove = SwitchingArea.Controls[0];
                SwitchingArea.Controls.Remove(controlRemove);
                controlRemove.Dispose();

                AreaHome areaHome = new AreaHome();
                string adminName = DatabaseHelper.GetAdminNameById(_adminId);
                areaHome.SetWelcomeMessage($"Welcome, {adminName}!");
                AreaUsed = true;
                Option = 0;
                SwitchingArea.Controls.Add(areaHome);
            }
            else
            {
                return;
            }
        }

        private void btnCourse_Click(object sender, EventArgs e)
        {
            if (!AreaUsed)
            {
                CourseList Course = new CourseList(0);
                AreaUsed = true;
                Option = 1;
                SwitchingArea.Controls.Add(Course);
            }
            else if (AreaUsed && !(Option == 1))
            {
                Control controlRemove = SwitchingArea.Controls[0];
                SwitchingArea.Controls.Remove(controlRemove);
                controlRemove.Dispose();

                CourseList Course = new CourseList(0);
                AreaUsed = true;
                Option = 1;
                SwitchingArea.Controls.Add(Course);
            }
            else
            {
                return;
            }
        }

        private void btnSubject_Click(object sender, EventArgs e)
        {
            //gagamitin ko nlng same user control na courselist cause ive got no time hoe
            if (!AreaUsed)
            {
                CourseList Subject = new CourseList(1);
                AreaUsed = true;
                Option = 2;
                SwitchingArea.Controls.Add(Subject);
            }
            else if (AreaUsed && !(Option == 2))
            {
                Control controlRemove = SwitchingArea.Controls[0];
                SwitchingArea.Controls.Remove(controlRemove);
                controlRemove.Dispose();

                CourseList Subject = new CourseList(1);
                AreaUsed = true;
                Option = 2;
                SwitchingArea.Controls.Add(Subject);
            }
            else
            {
                return;
            }
        }

        private void btnFaculty_Click(object sender, EventArgs e)
        {
            //gagamitin ko nlng same user control na courselist cause ive got no time hoe
            if (!AreaUsed)
            {
                FacultyListandUsersList Faculty = new FacultyListandUsersList(0);
                AreaUsed = true;
                Option = 3;
                SwitchingArea.Controls.Add(Faculty);
            }
            else if (AreaUsed && !(Option == 3))
            {
                Control controlRemove = SwitchingArea.Controls[0];
                SwitchingArea.Controls.Remove(controlRemove);
                controlRemove.Dispose();

                FacultyListandUsersList Faculty = new FacultyListandUsersList(0);
                AreaUsed = true;
                Option = 3;
                SwitchingArea.Controls.Add(Faculty);
            }
            else
            {
                return;
            }
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            //gagamitin ko nlng same user control na courselist cause ive got no time hoe
            if (!AreaUsed)
            {
                FacultyListandUsersList User = new FacultyListandUsersList(1);
                AreaUsed = true;
                Option = 4;
                SwitchingArea.Controls.Add(User);
            }
            else if (AreaUsed && !(Option == 4))
            {
                Control controlRemove = SwitchingArea.Controls[0];
                SwitchingArea.Controls.Remove(controlRemove);
                controlRemove.Dispose();

                FacultyListandUsersList User = new FacultyListandUsersList(1);
                AreaUsed = true;
                Option = 4;
                SwitchingArea.Controls.Add(User);
            }
            else
            {
                return;
            }
        }

        private void btnSchedule_Click(object sender, EventArgs e)
        {
            //gagamitin ko nlng same user control na courselist cause ive got no time hoe
            if (!AreaUsed)
            {
                ScheduleCalendar Schedule = new ScheduleCalendar();
                AreaUsed = true;
                Option = 5;
                SwitchingArea.Controls.Add(Schedule);
            }
            else if (AreaUsed && !(Option == 5))
            {
                Control controlRemove = SwitchingArea.Controls[0];
                SwitchingArea.Controls.Remove(controlRemove);
                controlRemove.Dispose();

                ScheduleCalendar Schedule = new ScheduleCalendar();
                AreaUsed = true;
                Option = 5;
                SwitchingArea.Controls.Add(Schedule);
            }
            else
            {
                return;
            }
        }

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