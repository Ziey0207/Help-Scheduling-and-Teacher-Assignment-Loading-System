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
                Control controlRemove = Area.Controls[1];
                Area.Controls.Remove(controlRemove);
                controlRemove.Dispose();

                AreaHome areaHome = new AreaHome();
                areaHome.Dock = DockStyle.Fill;
                AreaUsed = true;
                Option = 0;
                Area.Controls.Add(areaHome);
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
                CourseList courseList1 = new CourseList(0);
                AreaUsed = true;
                Option = 1;
                Area.Controls.Add(courseList1);
            }
            else if (AreaUsed && !(Option == 1))
            {
                Control controlRemove = Area.Controls[1];
                Area.Controls.Remove(controlRemove);
                controlRemove.Dispose();

                CourseList courseList1 = new CourseList(0);
                AreaUsed = true;
                Option = 1;
                Area.Controls.Add(courseList1);
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
                CourseList courseList1 = new CourseList(1);
                AreaUsed = true;
                Option = 2;
                Area.Controls.Add(courseList1);
            }
            else if (AreaUsed && !(Option == 2))
            {
                Control controlRemove = Area.Controls[1];
                Area.Controls.Remove(controlRemove);
                controlRemove.Dispose();

                CourseList courseList1 = new CourseList(1);
                AreaUsed = true;
                Option = 2;
                Area.Controls.Add(courseList1);
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
                FacultyListandUsersList List = new FacultyListandUsersList(0);
                AreaUsed = true;
                Option = 3;
                Area.Controls.Add(List);
            }
            else if (AreaUsed && !(Option == 3))
            {
                Control controlRemove = Area.Controls[1];
                Area.Controls.Remove(controlRemove);
                controlRemove.Dispose();

                FacultyListandUsersList List = new FacultyListandUsersList(0);
                AreaUsed = true;
                Option = 3;
                Area.Controls.Add(List);
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
                FacultyListandUsersList List = new FacultyListandUsersList(1);
                AreaUsed = true;
                Option = 4;
                Area.Controls.Add(List);
            }
            else if (AreaUsed && !(Option == 4))
            {
                Control controlRemove = Area.Controls[1];
                Area.Controls.Remove(controlRemove);
                controlRemove.Dispose();

                FacultyListandUsersList List = new FacultyListandUsersList(1);
                AreaUsed = true;
                Option = 4;
                Area.Controls.Add(List);
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
                ScheduleCalendar List = new ScheduleCalendar();
                AreaUsed = true;
                Option = 5;
                Area.Controls.Add(List);
            }
            else if (AreaUsed && !(Option == 5))
            {
                Control controlRemove = Area.Controls[1];
                Area.Controls.Remove(controlRemove);
                controlRemove.Dispose();

                ScheduleCalendar List = new ScheduleCalendar();
                AreaUsed = true;
                Option = 5;
                Area.Controls.Add(List);
            }
            else
            {
                return;
            }
        }
    }
}