using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Help_Scheduling_and_Teacher_Assignment_Loading_System.AddEdit_userControls
{
    public partial class AE_User : UserControl
    {
        private bool _editMode = false;
        private int _currentId = -1;

        public AE_User()
        {
            InitializeComponent();
            SetAddMode();
        }

        public void SetAddMode()
        {
            _editMode = false;
            _currentId = -1;

            // Update UI for add mode
            btnSave.Text = "Add";
            btnCancel.Visible = false;

            // Clear fields
            txtUsername.Text = "";
            txtEmail.Text = "";
            txtContact.Text = "";
            txtPassword.Text = "";

            // Reset fields
        }

        public void SetEditMode(int id, DataRow data)
        {
            _editMode = true;
            _currentId = id;

            // Update UI for edit mode
            btnSave.Text = "Update";
            btnCancel.Visible = true;

            // Populate fields from data
            txtUsername.Text = data["username"].ToString();
            txtEmail.Text = data["email"].ToString();
            txtContact.Text = data["contact_number"].ToString();
            txtPassword.Text = data["password"].ToString();
        }

        private void AE_User_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetAddMode();
        }
    }
}