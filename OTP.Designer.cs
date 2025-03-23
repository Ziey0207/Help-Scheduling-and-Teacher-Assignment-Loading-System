namespace Help_Scheduling_and_Teacher_Assignment_Loading_System
{
    partial class OTP
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
            this.lblOTP = new System.Windows.Forms.Label();
            this.txtOTP = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblOTP
            // 
            this.lblOTP.AutoSize = true;
            this.lblOTP.Location = new System.Drawing.Point(74, 116);
            this.lblOTP.Name = "lblOTP";
            this.lblOTP.Size = new System.Drawing.Size(29, 13);
            this.lblOTP.TabIndex = 0;
            this.lblOTP.Text = "OTP";
            // 
            // txtOTP
            // 
            this.txtOTP.Location = new System.Drawing.Point(129, 113);
            this.txtOTP.Name = "txtOTP";
            this.txtOTP.Size = new System.Drawing.Size(100, 20);
            this.txtOTP.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(129, 149);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Verify OTP";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // OTP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 402);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtOTP);
            this.Controls.Add(this.lblOTP);
            this.Name = "OTP";
            this.Text = "OTP";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblOTP;
        private System.Windows.Forms.TextBox txtOTP;
        private System.Windows.Forms.Button button1;
    }
}