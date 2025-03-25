using ReaLTaiizor.Extension;
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
    public partial class AdminDashboard : Form
    {
        public bool AreaUsed = false;
        private int Option;

        public AdminDashboard()
        {
            InitializeComponent();
            btnHome_Click(null, null);
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            if (!AreaUsed)
            {
                AreaHome areaHome = new AreaHome();
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
    }
}