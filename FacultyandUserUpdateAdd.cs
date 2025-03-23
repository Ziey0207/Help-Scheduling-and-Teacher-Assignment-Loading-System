using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Help_Scheduling_and_Teacher_Assignment_Loading_System
{
    public partial class FacultyandUserUpdateAdd : UserControl
    {
        private bool isFaculty, isAdmin;
        private int id;

        public FacultyandUserUpdateAdd(int State, int id = -1)
        {
            InitializeComponent();
            this.id = id;

            switch (State)
            {
                case 0:
                    this.isFaculty = true;
                    FormHeader.Text = "Faculty Update";
                    break;

                case 1:
                    this.isAdmin = true;
                    FormHeader.Text = "User Update";
                    break;
            }
        }

        private void label14_Click(object sender, EventArgs e)
        {
        }
    }
}