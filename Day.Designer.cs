namespace Help_Scheduling_and_Teacher_Assignment_Loading_System
{
    partial class Day
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
            this.flpSchedules = new System.Windows.Forms.FlowLayoutPanel();
            this.lblDay = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.panel1.Controls.Add(this.flpSchedules);
            this.panel1.Controls.Add(this.lblDay);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.EdgeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(51)))), ((int)(((byte)(107)))));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(140, 140);
            this.panel1.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.panel1.TabIndex = 0;
            this.panel1.Text = "panel1";
            // 
            // flpSchedules
            // 
            this.flpSchedules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpSchedules.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpSchedules.Location = new System.Drawing.Point(5, 22);
            this.flpSchedules.Margin = new System.Windows.Forms.Padding(0);
            this.flpSchedules.Name = "flpSchedules";
            this.flpSchedules.Size = new System.Drawing.Size(130, 113);
            this.flpSchedules.TabIndex = 1;
            this.flpSchedules.WrapContents = false;
            // 
            // lblDay
            // 
            this.lblDay.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDay.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDay.Location = new System.Drawing.Point(5, 5);
            this.lblDay.Name = "lblDay";
            this.lblDay.Size = new System.Drawing.Size(130, 17);
            this.lblDay.TabIndex = 0;
            this.lblDay.Text = "HI";
            this.lblDay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Day
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Arial", 8F);
            this.Name = "Day";
            this.Size = new System.Drawing.Size(140, 140);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ReaLTaiizor.Controls.Panel panel1;
        private System.Windows.Forms.Label lblDay;
        private System.Windows.Forms.FlowLayoutPanel flpSchedules;
    }
}
