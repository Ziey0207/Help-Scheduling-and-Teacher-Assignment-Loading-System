namespace Help_Scheduling_and_Teacher_Assignment_Loading_System
{
    partial class ItemRow
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
            this.txtID = new System.Windows.Forms.Label();
            this.txtCourseInfo = new System.Windows.Forms.Label();
            this.panel2 = new ReaLTaiizor.Controls.Panel();
            this.btnEdit = new ReaLTaiizor.Controls.ParrotButton();
            this.btnDelete = new ReaLTaiizor.Controls.ParrotButton();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(51)))), ((int)(((byte)(107)))));
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.txtCourseInfo);
            this.panel1.Controls.Add(this.txtID);
            this.panel1.EdgeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(41)))), ((int)(((byte)(50)))));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(850, 52);
            this.panel1.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.panel1.TabIndex = 0;
            this.panel1.Text = "panel1";
            // 
            // txtID
            // 
            this.txtID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtID.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.txtID.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.txtID.ForeColor = System.Drawing.Color.White;
            this.txtID.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.txtID.Location = new System.Drawing.Point(0, 0);
            this.txtID.Margin = new System.Windows.Forms.Padding(10);
            this.txtID.Name = "txtID";
            this.txtID.Padding = new System.Windows.Forms.Padding(5);
            this.txtID.Size = new System.Drawing.Size(52, 52);
            this.txtID.TabIndex = 9;
            this.txtID.Text = "#";
            this.txtID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtCourseInfo
            // 
            this.txtCourseInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtCourseInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.txtCourseInfo.Font = new System.Drawing.Font("Arial", 10F);
            this.txtCourseInfo.ForeColor = System.Drawing.Color.White;
            this.txtCourseInfo.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.txtCourseInfo.Location = new System.Drawing.Point(56, 0);
            this.txtCourseInfo.Margin = new System.Windows.Forms.Padding(10);
            this.txtCourseInfo.Name = "txtCourseInfo";
            this.txtCourseInfo.Padding = new System.Windows.Forms.Padding(5);
            this.txtCourseInfo.Size = new System.Drawing.Size(639, 52);
            this.txtCourseInfo.TabIndex = 10;
            this.txtCourseInfo.Text = "Course Name:\r\nCouse Description:";
            this.txtCourseInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(15)))), ((int)(((byte)(64)))));
            this.panel2.Controls.Add(this.btnDelete);
            this.panel2.Controls.Add(this.btnEdit);
            this.panel2.EdgeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(51)))), ((int)(((byte)(107)))));
            this.panel2.Location = new System.Drawing.Point(694, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(5);
            this.panel2.Size = new System.Drawing.Size(156, 51);
            this.panel2.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.panel2.TabIndex = 11;
            this.panel2.Text = "panel2";
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(51)))), ((int)(((byte)(107)))));
            this.btnEdit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnEdit.ButtonImage = null;
            this.btnEdit.ButtonStyle = ReaLTaiizor.Controls.ParrotButton.Style.MaterialRounded;
            this.btnEdit.ButtonText = "Edit";
            this.btnEdit.ClickBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(109)))), ((int)(((byte)(224)))));
            this.btnEdit.ClickTextColor = System.Drawing.Color.White;
            this.btnEdit.CornerRadius = 10;
            this.btnEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEdit.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btnEdit.Horizontal_Alignment = System.Drawing.StringAlignment.Center;
            this.btnEdit.HoverBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(52)))), ((int)(((byte)(212)))));
            this.btnEdit.HoverTextColor = System.Drawing.Color.White;
            this.btnEdit.ImagePosition = ReaLTaiizor.Controls.ParrotButton.ImgPosition.Left;
            this.btnEdit.Location = new System.Drawing.Point(6, 8);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Padding = new System.Windows.Forms.Padding(5);
            this.btnEdit.Size = new System.Drawing.Size(68, 34);
            this.btnEdit.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.btnEdit.TabIndex = 14;
            this.btnEdit.TextColor = System.Drawing.Color.White;
            this.btnEdit.TextRenderingType = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.btnEdit.Vertical_Alignment = System.Drawing.StringAlignment.Center;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(51)))), ((int)(((byte)(107)))));
            this.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnDelete.ButtonImage = null;
            this.btnDelete.ButtonStyle = ReaLTaiizor.Controls.ParrotButton.Style.MaterialRounded;
            this.btnDelete.ButtonText = "Delete";
            this.btnDelete.ClickBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(109)))), ((int)(((byte)(224)))));
            this.btnDelete.ClickTextColor = System.Drawing.Color.White;
            this.btnDelete.CornerRadius = 10;
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Horizontal_Alignment = System.Drawing.StringAlignment.Center;
            this.btnDelete.HoverBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(52)))), ((int)(((byte)(212)))));
            this.btnDelete.HoverTextColor = System.Drawing.Color.White;
            this.btnDelete.ImagePosition = ReaLTaiizor.Controls.ParrotButton.ImgPosition.Left;
            this.btnDelete.Location = new System.Drawing.Point(80, 8);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Padding = new System.Windows.Forms.Padding(5);
            this.btnDelete.Size = new System.Drawing.Size(68, 34);
            this.btnDelete.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.btnDelete.TabIndex = 15;
            this.btnDelete.TextColor = System.Drawing.Color.White;
            this.btnDelete.TextRenderingType = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.btnDelete.Vertical_Alignment = System.Drawing.StringAlignment.Center;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // ItemRow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Arial", 10F);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimumSize = new System.Drawing.Size(850, 52);
            this.Name = "ItemRow";
            this.Size = new System.Drawing.Size(850, 52);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ReaLTaiizor.Controls.Panel panel1;
        private System.Windows.Forms.Label txtID;
        private ReaLTaiizor.Controls.Panel panel2;
        private System.Windows.Forms.Label txtCourseInfo;
        private ReaLTaiizor.Controls.ParrotButton btnDelete;
        private ReaLTaiizor.Controls.ParrotButton btnEdit;
    }
}
