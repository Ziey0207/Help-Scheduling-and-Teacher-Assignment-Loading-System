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
            this.panel2 = new ReaLTaiizor.Controls.Panel();
            this.panel1 = new ReaLTaiizor.Controls.Panel();
            this.flpDays = new System.Windows.Forms.FlowLayoutPanel();
            this.chkMon = new ReaLTaiizor.Controls.HopeCheckBox();
            this.chkTue = new ReaLTaiizor.Controls.HopeCheckBox();
            this.chkWed = new ReaLTaiizor.Controls.HopeCheckBox();
            this.chkThu = new ReaLTaiizor.Controls.HopeCheckBox();
            this.chkFri = new ReaLTaiizor.Controls.HopeCheckBox();
            this.chkSat = new ReaLTaiizor.Controls.HopeCheckBox();
            this.chkSun = new ReaLTaiizor.Controls.HopeCheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbFrequency = new ReaLTaiizor.Controls.HopeComboBox();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.cmbTimeOut = new ReaLTaiizor.Controls.HopeComboBox();
            this.cmbTimeIn = new ReaLTaiizor.Controls.HopeComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbSections = new ReaLTaiizor.Controls.HopeComboBox();
            this.cmbCourse = new ReaLTaiizor.Controls.HopeComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbSubjects = new ReaLTaiizor.Controls.HopeComboBox();
            this.cmbTeachers = new ReaLTaiizor.Controls.HopeComboBox();
            this.airButton3 = new ReaLTaiizor.Controls.AirButton();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnClear = new ReaLTaiizor.Controls.AirButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbRooms = new ReaLTaiizor.Controls.HopeComboBox();
            this.btnSave = new ReaLTaiizor.Controls.AirButton();
            this.dgvEvents = new System.Windows.Forms.DataGridView();
            this.airForm1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.flpDays.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEvents)).BeginInit();
            this.SuspendLayout();
            // 
            // airForm1
            // 
            this.airForm1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(15)))), ((int)(((byte)(40)))));
            this.airForm1.BorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.airForm1.Controls.Add(this.panel2);
            this.airForm1.Customization = "////////wP/UNEj/";
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
            this.airForm1.Size = new System.Drawing.Size(862, 873);
            this.airForm1.SmartBounds = true;
            this.airForm1.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.airForm1.TabIndex = 0;
            this.airForm1.Text = "Scheduels";
            this.airForm1.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.airForm1.Transparent = false;
            this.airForm1.Click += new System.EventHandler(this.airForm1_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(51)))), ((int)(((byte)(63)))));
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.dgvEvents);
            this.panel2.EdgeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(41)))), ((int)(((byte)(50)))));
            this.panel2.Location = new System.Drawing.Point(10, 31);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(5);
            this.panel2.Size = new System.Drawing.Size(842, 832);
            this.panel2.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.panel2.TabIndex = 19;
            this.panel2.Text = "panel2";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.flpDays);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.cmbFrequency);
            this.panel1.Controls.Add(this.dtpEndDate);
            this.panel1.Controls.Add(this.dtpStartDate);
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
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cmbRooms);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.EdgeColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(5, 5);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(832, 315);
            this.panel1.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.panel1.TabIndex = 15;
            this.panel1.Text = "panel1";
            // 
            // flpDays
            // 
            this.flpDays.Controls.Add(this.chkMon);
            this.flpDays.Controls.Add(this.chkTue);
            this.flpDays.Controls.Add(this.chkWed);
            this.flpDays.Controls.Add(this.chkThu);
            this.flpDays.Controls.Add(this.chkFri);
            this.flpDays.Controls.Add(this.chkSat);
            this.flpDays.Controls.Add(this.chkSun);
            this.flpDays.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpDays.Location = new System.Drawing.Point(3, 105);
            this.flpDays.Name = "flpDays";
            this.flpDays.Size = new System.Drawing.Size(218, 165);
            this.flpDays.TabIndex = 35;
            // 
            // chkMon
            // 
            this.chkMon.AutoSize = true;
            this.chkMon.CheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.chkMon.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkMon.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(198)))), ((int)(((byte)(202)))));
            this.chkMon.DisabledStringColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(187)))), ((int)(((byte)(189)))));
            this.chkMon.Enable = true;
            this.chkMon.EnabledCheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.chkMon.EnabledStringColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.chkMon.EnabledUncheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(158)))), ((int)(((byte)(161)))));
            this.chkMon.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.chkMon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(49)))), ((int)(((byte)(51)))));
            this.chkMon.Location = new System.Drawing.Point(3, 3);
            this.chkMon.Name = "chkMon";
            this.chkMon.Size = new System.Drawing.Size(92, 20);
            this.chkMon.TabIndex = 22;
            this.chkMon.Text = "Monday";
            this.chkMon.UseVisualStyleBackColor = true;
            // 
            // chkTue
            // 
            this.chkTue.AutoSize = true;
            this.chkTue.CheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.chkTue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkTue.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(198)))), ((int)(((byte)(202)))));
            this.chkTue.DisabledStringColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(187)))), ((int)(((byte)(189)))));
            this.chkTue.Enable = true;
            this.chkTue.EnabledCheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.chkTue.EnabledStringColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.chkTue.EnabledUncheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(158)))), ((int)(((byte)(161)))));
            this.chkTue.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.chkTue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(49)))), ((int)(((byte)(51)))));
            this.chkTue.Location = new System.Drawing.Point(3, 34);
            this.chkTue.Name = "chkTue";
            this.chkTue.Size = new System.Drawing.Size(92, 20);
            this.chkTue.TabIndex = 23;
            this.chkTue.Text = "Tuesday";
            this.chkTue.UseVisualStyleBackColor = true;
            // 
            // chkWed
            // 
            this.chkWed.AutoSize = true;
            this.chkWed.CheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.chkWed.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkWed.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(198)))), ((int)(((byte)(202)))));
            this.chkWed.DisabledStringColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(187)))), ((int)(((byte)(189)))));
            this.chkWed.Enable = true;
            this.chkWed.EnabledCheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.chkWed.EnabledStringColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.chkWed.EnabledUncheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(158)))), ((int)(((byte)(161)))));
            this.chkWed.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.chkWed.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(49)))), ((int)(((byte)(51)))));
            this.chkWed.Location = new System.Drawing.Point(3, 65);
            this.chkWed.Name = "chkWed";
            this.chkWed.Size = new System.Drawing.Size(115, 20);
            this.chkWed.TabIndex = 24;
            this.chkWed.Text = "Wednesday";
            this.chkWed.UseVisualStyleBackColor = true;
            // 
            // chkThu
            // 
            this.chkThu.AutoSize = true;
            this.chkThu.CheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.chkThu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkThu.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(198)))), ((int)(((byte)(202)))));
            this.chkThu.DisabledStringColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(187)))), ((int)(((byte)(189)))));
            this.chkThu.Enable = true;
            this.chkThu.EnabledCheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.chkThu.EnabledStringColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.chkThu.EnabledUncheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(158)))), ((int)(((byte)(161)))));
            this.chkThu.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.chkThu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(49)))), ((int)(((byte)(51)))));
            this.chkThu.Location = new System.Drawing.Point(3, 96);
            this.chkThu.Name = "chkThu";
            this.chkThu.Size = new System.Drawing.Size(99, 20);
            this.chkThu.TabIndex = 25;
            this.chkThu.Text = "Thursday";
            this.chkThu.UseVisualStyleBackColor = true;
            // 
            // chkFri
            // 
            this.chkFri.AutoSize = true;
            this.chkFri.CheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.chkFri.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkFri.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(198)))), ((int)(((byte)(202)))));
            this.chkFri.DisabledStringColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(187)))), ((int)(((byte)(189)))));
            this.chkFri.Enable = true;
            this.chkFri.EnabledCheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.chkFri.EnabledStringColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.chkFri.EnabledUncheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(158)))), ((int)(((byte)(161)))));
            this.chkFri.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.chkFri.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(49)))), ((int)(((byte)(51)))));
            this.chkFri.Location = new System.Drawing.Point(3, 127);
            this.chkFri.Name = "chkFri";
            this.chkFri.Size = new System.Drawing.Size(78, 20);
            this.chkFri.TabIndex = 26;
            this.chkFri.Text = "Friday";
            this.chkFri.UseVisualStyleBackColor = true;
            // 
            // chkSat
            // 
            this.chkSat.AutoSize = true;
            this.chkSat.CheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.chkSat.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkSat.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(198)))), ((int)(((byte)(202)))));
            this.chkSat.DisabledStringColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(187)))), ((int)(((byte)(189)))));
            this.chkSat.Enable = true;
            this.chkSat.EnabledCheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.chkSat.EnabledStringColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.chkSat.EnabledUncheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(158)))), ((int)(((byte)(161)))));
            this.chkSat.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.chkSat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(49)))), ((int)(((byte)(51)))));
            this.chkSat.Location = new System.Drawing.Point(118, 3);
            this.chkSat.Name = "chkSat";
            this.chkSat.Size = new System.Drawing.Size(97, 20);
            this.chkSat.TabIndex = 27;
            this.chkSat.Text = "Saturday";
            this.chkSat.UseVisualStyleBackColor = true;
            // 
            // chkSun
            // 
            this.chkSun.AutoSize = true;
            this.chkSun.CheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.chkSun.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkSun.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(198)))), ((int)(((byte)(202)))));
            this.chkSun.DisabledStringColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(187)))), ((int)(((byte)(189)))));
            this.chkSun.Enable = true;
            this.chkSun.EnabledCheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.chkSun.EnabledStringColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.chkSun.EnabledUncheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(158)))), ((int)(((byte)(161)))));
            this.chkSun.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.chkSun.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(49)))), ((int)(((byte)(51)))));
            this.chkSun.Location = new System.Drawing.Point(118, 34);
            this.chkSun.Name = "chkSun";
            this.chkSun.Size = new System.Drawing.Size(87, 20);
            this.chkSun.TabIndex = 28;
            this.chkSun.Text = "Sunday";
            this.chkSun.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial", 10F);
            this.label10.Location = new System.Drawing.Point(5, 54);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(22, 16);
            this.label10.TabIndex = 32;
            this.label10.Text = "To";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial", 10F);
            this.label9.Location = new System.Drawing.Point(5, 5);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 16);
            this.label9.TabIndex = 31;
            this.label9.Text = "From";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial", 10F);
            this.label8.Location = new System.Drawing.Point(227, 197);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 16);
            this.label8.TabIndex = 30;
            this.label8.Text = "Frequency";
            // 
            // cmbFrequency
            // 
            this.cmbFrequency.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbFrequency.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmbFrequency.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbFrequency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFrequency.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbFrequency.Font = new System.Drawing.Font("Arial", 10F);
            this.cmbFrequency.FormattingEnabled = true;
            this.cmbFrequency.ItemHeight = 30;
            this.cmbFrequency.Items.AddRange(new object[] {
            "Once",
            "Weekly"});
            this.cmbFrequency.Location = new System.Drawing.Point(230, 216);
            this.cmbFrequency.Name = "cmbFrequency";
            this.cmbFrequency.Size = new System.Drawing.Size(130, 36);
            this.cmbFrequency.TabIndex = 29;
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndDate.Location = new System.Drawing.Point(3, 73);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(200, 26);
            this.dtpEndDate.TabIndex = 21;
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStartDate.Location = new System.Drawing.Point(3, 24);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(200, 26);
            this.dtpStartDate.TabIndex = 20;
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
            this.cmbTimeOut.Location = new System.Drawing.Point(646, 153);
            this.cmbTimeOut.Name = "cmbTimeOut";
            this.cmbTimeOut.Size = new System.Drawing.Size(130, 36);
            this.cmbTimeOut.TabIndex = 18;
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
            this.cmbTimeIn.Location = new System.Drawing.Point(511, 153);
            this.cmbTimeIn.Name = "cmbTimeIn";
            this.cmbTimeIn.Size = new System.Drawing.Size(129, 36);
            this.cmbTimeIn.TabIndex = 17;
            this.cmbTimeIn.SelectedIndexChanged += new System.EventHandler(this.cmbTimeIn_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 10F);
            this.label7.Location = new System.Drawing.Point(508, 67);
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
            this.cmbSections.Location = new System.Drawing.Point(510, 86);
            this.cmbSections.Name = "cmbSections";
            this.cmbSections.Size = new System.Drawing.Size(265, 36);
            this.cmbSections.TabIndex = 15;
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
            this.cmbCourse.Location = new System.Drawing.Point(230, 153);
            this.cmbCourse.Name = "cmbCourse";
            this.cmbCourse.Size = new System.Drawing.Size(272, 36);
            this.cmbCourse.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 10F);
            this.label6.Location = new System.Drawing.Point(227, 134);
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
            this.cmbSubjects.Location = new System.Drawing.Point(230, 24);
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
            this.cmbTeachers.Location = new System.Drawing.Point(230, 86);
            this.cmbTeachers.Name = "cmbTeachers";
            this.cmbTeachers.Size = new System.Drawing.Size(272, 36);
            this.cmbTeachers.TabIndex = 4;
            // 
            // airButton3
            // 
            this.airButton3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.airButton3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.airButton3.Customization = "7e3t//Ly8v/r6+v/5ubm/+vr6//f39//p6en/zw8PP8UFBT/gICA/w==";
            this.airButton3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.airButton3.Image = null;
            this.airButton3.Location = new System.Drawing.Point(3, 279);
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
            this.label5.Location = new System.Drawing.Point(643, 134);
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
            this.label4.Location = new System.Drawing.Point(507, 134);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 16);
            this.label4.TabIndex = 10;
            this.label4.Text = "Time In";
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClear.Customization = "7e3t//Ly8v/r6+v/5ubm/+vr6//f39//p6en/zw8PP8UFBT/gICA/w==";
            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnClear.Image = null;
            this.btnClear.Location = new System.Drawing.Point(764, 279);
            this.btnClear.Name = "btnClear";
            this.btnClear.NoRounding = false;
            this.btnClear.Size = new System.Drawing.Size(63, 28);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "Clear";
            this.btnClear.Transparent = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 10F);
            this.label3.Location = new System.Drawing.Point(508, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "Room";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 10F);
            this.label2.Location = new System.Drawing.Point(227, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Teacher";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 10F);
            this.label1.Location = new System.Drawing.Point(227, 5);
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
            this.cmbRooms.Location = new System.Drawing.Point(511, 24);
            this.cmbRooms.Name = "cmbRooms";
            this.cmbRooms.Size = new System.Drawing.Size(264, 36);
            this.cmbRooms.TabIndex = 3;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Customization = "7e3t//Ly8v/r6+v/5ubm/+vr6//f39//p6en/zw8PP8UFBT/gICA/w==";
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSave.Image = null;
            this.btnSave.Location = new System.Drawing.Point(691, 279);
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
            this.dgvEvents.Location = new System.Drawing.Point(5, 320);
            this.dgvEvents.Name = "dgvEvents";
            this.dgvEvents.Size = new System.Drawing.Size(832, 507);
            this.dgvEvents.TabIndex = 14;
            this.dgvEvents.SelectionChanged += new System.EventHandler(this.dgvEvents_SelectionChanged);
            // 
            // DayPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(862, 873);
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
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.flpDays.ResumeLayout(false);
            this.flpDays.PerformLayout();
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
        private ReaLTaiizor.Controls.AirButton btnClear;
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
        private ReaLTaiizor.Controls.Panel panel2;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private ReaLTaiizor.Controls.HopeCheckBox chkTue;
        private ReaLTaiizor.Controls.HopeCheckBox chkMon;
        private ReaLTaiizor.Controls.HopeCheckBox chkSun;
        private ReaLTaiizor.Controls.HopeCheckBox chkSat;
        private ReaLTaiizor.Controls.HopeCheckBox chkFri;
        private ReaLTaiizor.Controls.HopeCheckBox chkThu;
        private ReaLTaiizor.Controls.HopeCheckBox chkWed;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private ReaLTaiizor.Controls.HopeComboBox cmbFrequency;
        private System.Windows.Forms.FlowLayoutPanel flpDays;
    }
}