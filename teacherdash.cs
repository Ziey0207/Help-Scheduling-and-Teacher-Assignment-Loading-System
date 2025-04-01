using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Help_Scheduling_and_Teacher_Assignment_Loading_System
{
    public partial class teacherdash : Form
    {
        string connString = "server=localhost;database=school_management;uid=root;pwd=;Allow Zero Datetime=True;Convert Zero Datetime=True;";
        private DateTime currentMonth = DateTime.Now;
        public teacherdash()
        {
            InitializeComponent();
        }

        private void teacherdash_Load(object sender, EventArgs e)
        {
       
        }

        public void LoadScheduleToGrid()
        {
          
        }
        private void LoadCalendar()
        {
         
        }



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
         
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
  
        }
    }
}
