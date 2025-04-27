namespace Help_Scheduling_and_Teacher_Assignment_Loading_System
{
    partial class Calendar
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new ReaLTaiizor.Controls.Panel();
            this.panel3 = new ReaLTaiizor.Controls.Panel();
            this.calendarGrid = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new ReaLTaiizor.Controls.Panel();
            this.txtSearch = new ReaLTaiizor.Controls.HopeTextBox();
            this.dayNamesPanel = new System.Windows.Forms.TableLayoutPanel();
            this.btnPrev = new ReaLTaiizor.Controls.ParrotButton();
            this.btnNext = new ReaLTaiizor.Controls.ParrotButton();
            this.lblMonthYear = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(51)))), ((int)(((byte)(63)))));
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.EdgeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(41)))), ((int)(((byte)(50)))));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(1176, 716);
            this.panel1.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.panel1.TabIndex = 0;
            this.panel1.Text = "panel1";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Teal;
            this.panel3.Controls.Add(this.calendarGrid);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.EdgeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(41)))), ((int)(((byte)(50)))));
            this.panel3.Location = new System.Drawing.Point(5, 83);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(5);
            this.panel3.Size = new System.Drawing.Size(1166, 628);
            this.panel3.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.panel3.TabIndex = 2;
            this.panel3.Text = "panel3";
            // 
            // calendarGrid
            // 
            this.calendarGrid.ColumnCount = 1;
            this.calendarGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1156F));
            this.calendarGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.calendarGrid.Location = new System.Drawing.Point(5, 5);
            this.calendarGrid.Name = "calendarGrid";
            this.calendarGrid.RowCount = 1;
            this.calendarGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 618F));
            this.calendarGrid.Size = new System.Drawing.Size(1156, 618);
            this.calendarGrid.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel2.Controls.Add(this.txtSearch);
            this.panel2.Controls.Add(this.dayNamesPanel);
            this.panel2.Controls.Add(this.btnPrev);
            this.panel2.Controls.Add(this.btnNext);
            this.panel2.Controls.Add(this.lblMonthYear);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.EdgeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(41)))), ((int)(((byte)(50)))));
            this.panel2.Location = new System.Drawing.Point(5, 5);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(5);
            this.panel2.Size = new System.Drawing.Size(1166, 78);
            this.panel2.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.panel2.TabIndex = 1;
            this.panel2.Text = "panel2";
            // 
            // txtSearch
            // 
            this.txtSearch.BackColor = System.Drawing.Color.White;
            this.txtSearch.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(55)))), ((int)(((byte)(66)))));
            this.txtSearch.BorderColorA = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.txtSearch.BorderColorB = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(223)))), ((int)(((byte)(230)))));
            this.txtSearch.Font = new System.Drawing.Font("Arial", 9F);
            this.txtSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(49)))), ((int)(((byte)(51)))));
            this.txtSearch.Hint = "";
            this.txtSearch.Location = new System.Drawing.Point(5, 5);
            this.txtSearch.MaxLength = 32767;
            this.txtSearch.Multiline = false;
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PasswordChar = '\0';
            this.txtSearch.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtSearch.SelectedText = "";
            this.txtSearch.SelectionLength = 0;
            this.txtSearch.SelectionStart = 0;
            this.txtSearch.Size = new System.Drawing.Size(224, 30);
            this.txtSearch.TabIndex = 47;
            this.txtSearch.TabStop = false;
            this.txtSearch.UseSystemPasswordChar = false;
            // 
            // dayNamesPanel
            // 
            this.dayNamesPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dayNamesPanel.ColumnCount = 1;
            this.dayNamesPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1156F));
            this.dayNamesPanel.Font = new System.Drawing.Font("Arial", 10F);
            this.dayNamesPanel.Location = new System.Drawing.Point(5, 40);
            this.dayNamesPanel.Name = "dayNamesPanel";
            this.dayNamesPanel.RowCount = 1;
            this.dayNamesPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 618F));
            this.dayNamesPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 618F));
            this.dayNamesPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 618F));
            this.dayNamesPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 618F));
            this.dayNamesPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 618F));
            this.dayNamesPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 618F));
            this.dayNamesPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 618F));
            this.dayNamesPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 618F));
            this.dayNamesPanel.Size = new System.Drawing.Size(1156, 32);
            this.dayNamesPanel.TabIndex = 46;
            this.dayNamesPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // btnPrev
            // 
            this.btnPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrev.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(51)))), ((int)(((byte)(107)))));
            this.btnPrev.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnPrev.ButtonImage = null;
            this.btnPrev.ButtonStyle = ReaLTaiizor.Controls.ParrotButton.Style.MaterialRounded;
            this.btnPrev.ButtonText = "Previous";
            this.btnPrev.ClickBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(109)))), ((int)(((byte)(224)))));
            this.btnPrev.ClickTextColor = System.Drawing.Color.White;
            this.btnPrev.CornerRadius = 10;
            this.btnPrev.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrev.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btnPrev.Horizontal_Alignment = System.Drawing.StringAlignment.Center;
            this.btnPrev.HoverBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(52)))), ((int)(((byte)(212)))));
            this.btnPrev.HoverTextColor = System.Drawing.Color.White;
            this.btnPrev.ImagePosition = ReaLTaiizor.Controls.ParrotButton.ImgPosition.Left;
            this.btnPrev.Location = new System.Drawing.Point(939, 0);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Padding = new System.Windows.Forms.Padding(5);
            this.btnPrev.Size = new System.Drawing.Size(106, 34);
            this.btnPrev.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.btnPrev.TabIndex = 45;
            this.btnPrev.TextColor = System.Drawing.Color.White;
            this.btnPrev.TextRenderingType = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.btnPrev.Vertical_Alignment = System.Drawing.StringAlignment.Center;
            this.btnPrev.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(51)))), ((int)(((byte)(107)))));
            this.btnNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnNext.ButtonImage = null;
            this.btnNext.ButtonStyle = ReaLTaiizor.Controls.ParrotButton.Style.MaterialRounded;
            this.btnNext.ButtonText = "Next";
            this.btnNext.ClickBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(109)))), ((int)(((byte)(224)))));
            this.btnNext.ClickTextColor = System.Drawing.Color.White;
            this.btnNext.CornerRadius = 10;
            this.btnNext.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNext.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btnNext.Horizontal_Alignment = System.Drawing.StringAlignment.Center;
            this.btnNext.HoverBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(52)))), ((int)(((byte)(212)))));
            this.btnNext.HoverTextColor = System.Drawing.Color.White;
            this.btnNext.ImagePosition = ReaLTaiizor.Controls.ParrotButton.ImgPosition.Left;
            this.btnNext.Location = new System.Drawing.Point(1055, 0);
            this.btnNext.Name = "btnNext";
            this.btnNext.Padding = new System.Windows.Forms.Padding(5);
            this.btnNext.Size = new System.Drawing.Size(106, 34);
            this.btnNext.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.btnNext.TabIndex = 44;
            this.btnNext.TextColor = System.Drawing.Color.White;
            this.btnNext.TextRenderingType = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.btnNext.Vertical_Alignment = System.Drawing.StringAlignment.Center;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // lblMonthYear
            // 
            this.lblMonthYear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMonthYear.AutoSize = true;
            this.lblMonthYear.Font = new System.Drawing.Font("Arial", 15F);
            this.lblMonthYear.ForeColor = System.Drawing.Color.White;
            this.lblMonthYear.Location = new System.Drawing.Point(528, 5);
            this.lblMonthYear.Name = "lblMonthYear";
            this.lblMonthYear.Size = new System.Drawing.Size(109, 23);
            this.lblMonthYear.TabIndex = 0;
            this.lblMonthYear.Text = "Month/year";
            // 
            // Calendar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "Calendar";
            this.Size = new System.Drawing.Size(1176, 716);
            this.Load += new System.EventHandler(this.Calendar_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Calendar_MouseDown);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ReaLTaiizor.Controls.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel calendarGrid;
        private ReaLTaiizor.Controls.Panel panel2;
        private ReaLTaiizor.Controls.Panel panel3;
        private System.Windows.Forms.Label lblMonthYear;
        private ReaLTaiizor.Controls.ParrotButton btnPrev;
        private ReaLTaiizor.Controls.ParrotButton btnNext;
        private System.Windows.Forms.TableLayoutPanel dayNamesPanel;
        private ReaLTaiizor.Controls.HopeTextBox txtSearch;
    }
}
