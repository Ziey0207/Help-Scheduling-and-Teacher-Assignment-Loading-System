using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReaLTaiizor.Controls;

namespace Help_Scheduling_and_Teacher_Assignment_Loading_System
{
    public partial class FacultyListandUsersList : UserControl
    {
        private bool isFaculty, isUsers;

        public FacultyListandUsersList(int FacultyorUsers)
        {
            InitializeComponent();

            switch (FacultyorUsers)
            {
                case 0:
                    this.isFaculty = true;
                    txtHeaderMain.Text = "Faculty List";
                    break;

                case 1:
                    this.isUsers = true;
                    txtHeaderMain.Text = "Users List";
                    break;
            }
        }

        private void FacultyListandUsersList_Load(object sender, EventArgs e)
        {
        }
    }
}