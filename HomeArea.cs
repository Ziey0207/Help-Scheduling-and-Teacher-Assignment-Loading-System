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
    public partial class AreaHome : UserControl
    {
        public AreaHome()
        {
            InitializeComponent();
            lblWelcome.Text = "Welcome!";
        }

        private void AreaHome_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
        }

        public void SetWelcomeMessage(string message)
        {
            // Assuming you have a label called lblWelcome in your AreaHome control
            lblWelcome.Text = message;
        }
    }
}