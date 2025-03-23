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
    public partial class ItemRow : UserControl
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }

        public event EventHandler<int> EditClicked;

        public event EventHandler<int> DeleteClicked;

        public ItemRow()
        {
            InitializeComponent();
        }

        // Method to update the UI with course data
        public void UpdateCourseData()
        {
            txtCourseInfo.Text = $"{CourseName}\n{CourseDescription}";
        }

        // Edit button click event
        private void btnEdit_Click(object sender, EventArgs e)
        {
            EditClicked?.Invoke(this, CourseId);
        }

        // Delete button click event
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteClicked?.Invoke(this, CourseId);
        }
    }
}