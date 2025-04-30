namespace Help_Scheduling_and_Teacher_Assignment_Loading_System
{
    partial class DayPopup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.airForm1 = new ReaLTaiizor.Forms.AirForm();
            this.panel1 = new ReaLTaiizor.Controls.Panel();
            this.cmbCourse = new ReaLTaiizor.Controls.HopeComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbSubjects = new ReaLTaiizor.Controls.HopeComboBox();
            this.cmbTeachers = new ReaLTaiizor.Controls.HopeComboBox();
            this.airButton3 = new ReaLTaiizor.Controls.AirButton();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.airButton2 = new ReaLTaiizor.Controls.AirButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbRooms = new ReaLTaiizor.Controls.HopeComboBox();
            this.btnSave = new ReaLTaiizor.Controls.AirButton();
            this.dgvEvents = new System.Windows.Forms.DataGridView();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbSections = new ReaLTaiizor.Controls.HopeComboBox();
            this.cmbTimeIn = new ReaLTaiizor.Controls.HopeComboBox();
            this.cmbTimeOut = new ReaLTaiizor.Controls.HopeComboBox();
            this.airForm1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEvents)).BeginInit();
            this.SuspendLayout();
            // 
            // airForm1
            // 
            this.airForm1.BackColor = System.Drawing.Color.White;
            this.airForm1.BorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.airForm1.Controls.Add(this.panel1);
            this.airForm1.Controls.Add(this.dgvEvents);
            this.airForm1.Customization = "AAAA/1paWv9ycnL/";
            this.airForm1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.airForm1.Font = new System.Drawing.Font("Arial", 12F);
            this.airForm1.Image = null;
            this.airForm1.Location = new System.Drawing.Point(0, 0);
            this.airForm1.MinimumSize = new System.Drawing.Size(112, 35);
            this.airForm1.Movable = true;
            this.airForm1.Name = "airForm1";
            this.airForm1.NoRounding = false;
            this.airForm1.Padding = new System.Windows.Forms.Padding(10);
            this.airForm1.Sizable = true;
            this.airForm1.Size = new System.Drawing.Size(709, 600);
            this.airForm1.SmartBounds = true;
            this.airForm1.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.airForm1.TabIndex = 0;
            this.airForm1.Text = "Day Events";
            this.airForm1.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.airForm1.Transparent = false;
            this.airForm1.Click += new System.EventHandler(this.airForm1_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.cmbTimeOut);
            this.panel1.Controls.Add(this.cmbTimeIn);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.cmbSections);
            this.panel1.Controls.Add(this.cmbCourse);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.cmbSubjects);
            this.panel1.Controls.Add(this.cmbTeachers);
            this.panel1.Controls.Add(this.airButton3);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.airButton2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cmbRooms);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.EdgeColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(10, 38);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(689, 271);
            this.panel1.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.panel1.TabIndex = 15;
            this.panel1.Text = "panel1";
            // 
            // cmbCourse
            // 
            this.cmbCourse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmbCourse.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbCourse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCourse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbCourse.Font = new System.Drawing.Font("Arial", 10F);
            this.cmbCourse.FormattingEnabled = true;
            this.cmbCourse.ItemHeight = 30;
            this.cmbCourse.Location = new System.Drawing.Point(11, 168);
            this.cmbCourse.Name = "cmbCourse";
            this.cmbCourse.Size = new System.Drawing.Size(272, 36);
            this.cmbCourse.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 10F);
            this.label6.Location = new System.Drawing.Point(12, 149);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 16);
            this.label6.TabIndex = 14;
            this.label6.Text = "Course";
            // 
            // cmbSubjects
            // 
            this.cmbSubjects.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmbSubjects.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbSubjects.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSubjects.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbSubjects.Font = new System.Drawing.Font("Arial", 10F);
            this.cmbSubjects.FormattingEnabled = true;
            this.cmbSubjects.ItemHeight = 30;
            this.cmbSubjects.Location = new System.Drawing.Point(8, 34);
            this.cmbSubjects.Name = "cmbSubjects";
            this.cmbSubjects.Size = new System.Drawing.Size(272, 36);
            this.cmbSubjects.TabIndex = 2;
            // 
            // cmbTeachers
            // 
            this.cmbTeachers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmbTeachers.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbTeachers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTeachers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbTeachers.Font = new System.Drawing.Font("Arial", 10F);
            this.cmbTeachers.FormattingEnabled = true;
            this.cmbTeachers.ItemHeight = 30;
            this.cmbTeachers.Location = new System.Drawing.Point(8, 101);
            this.cmbTeachers.Name = "cmbTeachers";
            this.cmbTeachers.Size = new System.Drawing.Size(272, 36);
            this.cmbTeachers.TabIndex = 4;
            // 
            // airButton3
            // 
            this.airButton3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.airButton3.Customization = "7e3t//Ly8v/r6+v/5ubm/+vr6//f39//p6en/zw8PP8UFBT/gICA/w==";
            this.airButton3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.airButton3.Image = null;
            this.airButton3.Location = new System.Drawing.Point(8, 224);
            this.airButton3.Name = "airButton3";
            this.airButton3.NoRounding = false;
            this.airButton3.Size = new System.Drawing.Size(63, 28);
            this.airButton3.TabIndex = 12;
            this.airButton3.Text = "Delete";
            this.airButton3.Transparent = false;
            this.airButton3.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 10F);
            this.label5.Location = new System.Drawing.Point(551, 149);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 16);
            this.label5.TabIndex = 11;
            this.label5.Text = "Time Out";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 10F);
            this.label4.Location = new System.Drawing.Point(415, 149);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 16);
            this.label4.TabIndex = 10;
            this.label4.Text = "Time In";
            // 
            // airButton2
            // 
            this.airButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.airButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.airButton2.Customization = "7e3t//Ly8v/r6+v/5ubm/+vr6//f39//p6en/zw8PP8UFBT/gICA/w==";
            this.airButton2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.airButton2.Image = null;
            this.airButton2.Location = new System.Drawing.Point(621, 224);
            this.airButton2.Name = "airButton2";
            this.airButton2.NoRounding = false;
            this.airButton2.Size = new System.Drawing.Size(63, 28);
            this.airButton2.TabIndex = 9;
            this.airButton2.Text = "Clear";
            this.airButton2.Transparent = false;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 10F);
            this.label3.Location = new System.Drawing.Point(416, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "Room";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 10F);
            this.label2.Location = new System.Drawing.Point(9, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Teacher";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 10F);
            this.label1.Location = new System.Drawing.Point(8, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "Subject";
            // 
            // cmbRooms
            // 
            this.cmbRooms.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbRooms.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmbRooms.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbRooms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRooms.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbRooms.Font = new System.Drawing.Font("Arial", 10F);
            this.cmbRooms.FormattingEnabled = true;
            this.cmbRooms.ItemHeight = 30;
            this.cmbRooms.Location = new System.Drawing.Point(419, 34);
            this.cmbRooms.Name = "cmbRooms";
            this.cmbRooms.Size = new System.Drawing.Size(265, 36);
            this.cmbRooms.TabIndex = 3;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Customization = "7e3t//Ly8v/r6+v/5ubm/+vr6//f39//p6en/zw8PP8UFBT/gICA/w==";
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSave.Image = null;
            this.btnSave.Location = new System.Drawing.Point(548, 224);
            this.btnSave.Name = "btnSave";
            this.btnSave.NoRounding = false;
            this.btnSave.Size = new System.Drawing.Size(67, 28);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.Transparent = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dgvEvents
            // 
            this.dgvEvents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEvents.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvEvents.Location = new System.Drawing.Point(10, 309);
            this.dgvEvents.Name = "dgvEvents";
            this.dgvEvents.Size = new System.Drawing.Size(689, 281);
            this.dgvEvents.TabIndex = 14;
            this.dgvEvents.SelectionChanged += new System.EventHandler(this.dgvEvents_SelectionChanged);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 10F);
            this.label7.Location = new System.Drawing.Point(416, 82);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 16);
            this.label7.TabIndex = 16;
            this.label7.Text = "Section";
            // 
            // cmbSections
            // 
            this.cmbSections.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSections.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmbSections.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbSections.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSections.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbSections.Font = new System.Drawing.Font("Arial", 10F);
            this.cmbSections.FormattingEnabled = true;
            this.cmbSections.ItemHeight = 30;
            this.cmbSections.Location = new System.Drawing.Point(419, 101);
            this.cmbSections.Name = "cmbSections";
            this.cmbSections.Size = new System.Drawing.Size(265, 36);
            this.cmbSections.TabIndex = 15;
            // 
            // cmbTimeIn
            // 
            this.cmbTimeIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbTimeIn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmbTimeIn.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbTimeIn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTimeIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbTimeIn.Font = new System.Drawing.Font("Arial", 10F);
            this.cmbTimeIn.FormattingEnabled = true;
            this.cmbTimeIn.ItemHeight = 30;
            this.cmbTimeIn.Location = new System.Drawing.Point(419, 168);
            this.cmbTimeIn.Name = "cmbTimeIn";
            this.cmbTimeIn.Size = new System.Drawing.Size(129, 36);
            this.cmbTimeIn.TabIndex = 17;
            // 
            // cmbTimeOut
            // 
            this.cmbTimeOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbTimeOut.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmbTimeOut.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbTimeOut.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTimeOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbTimeOut.Font = new System.Drawing.Font("Arial", 10F);
            this.cmbTimeOut.FormattingEnabled = true;
            this.cmbTimeOut.ItemHeight = 30;
            this.cmbTimeOut.Location = new System.Drawing.Point(554, 168);
            this.cmbTimeOut.Name = "cmbTimeOut";
            this.cmbTimeOut.Size = new System.Drawing.Size(130, 36);
            this.cmbTimeOut.TabIndex = 18;
            // 
            // DayPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 600);
            this.Controls.Add(this.airForm1);
            this.Font = new System.Drawing.Font("Arial", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(112, 35);
            this.Name = "DayPopup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Day Events";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.Load += new System.EventHandler(this.DayPopup_Load);
            this.airForm1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEvents)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ReaLTaiizor.Forms.AirForm airForm1;
        private ReaLTaiizor.Controls.AirButton btnSave;
        private ReaLTaiizor.Controls.HopeComboBox cmbTeachers;
        private ReaLTaiizor.Controls.HopeComboBox cmbRooms;
        private ReaLTaiizor.Controls.HopeComboBox cmbSubjects;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private ReaLTaiizor.Controls.AirButton airButton2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private ReaLTaiizor.Controls.AirButton airButton3;
        private System.Windows.Forms.DataGridView dgvEvents;
        private ReaLTaiizor.Controls.Panel panel1;
        private ReaLTaiizor.Controls.HopeComboBox cmbCourse;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private ReaLTaiizor.Controls.HopeComboBox cmbSections;
        private ReaLTaiizor.Controls.HopeComboBox cmbTimeOut;
        private ReaLTaiizor.Controls.HopeComboBox cmbTimeIn;
    }
}