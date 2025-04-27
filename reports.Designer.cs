namespace Help_Scheduling_and_Teacher_Assignment_Loading_System
{
    partial class reports
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chartRoom = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartTeacher = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartSection = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chartRoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartTeacher)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartSection)).BeginInit();
            this.SuspendLayout();
            // 
            // chartRoom
            // 
            chartArea1.Name = "ChartArea1";
            this.chartRoom.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartRoom.Legends.Add(legend1);
            this.chartRoom.Location = new System.Drawing.Point(24, 51);
            this.chartRoom.Name = "chartRoom";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartRoom.Series.Add(series1);
            this.chartRoom.Size = new System.Drawing.Size(241, 269);
            this.chartRoom.TabIndex = 0;
            this.chartRoom.Text = "chart1";
            this.chartRoom.Click += new System.EventHandler(this.chartRoom_Click);
            // 
            // chartTeacher
            // 
            chartArea2.Name = "ChartArea1";
            this.chartTeacher.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartTeacher.Legends.Add(legend2);
            this.chartTeacher.Location = new System.Drawing.Point(288, 51);
            this.chartTeacher.Name = "chartTeacher";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartTeacher.Series.Add(series2);
            this.chartTeacher.Size = new System.Drawing.Size(241, 269);
            this.chartTeacher.TabIndex = 1;
            this.chartTeacher.Text = "chart1";
            this.chartTeacher.Click += new System.EventHandler(this.chartTeacher_Click);
            // 
            // chartSection
            // 
            chartArea3.Name = "ChartArea1";
            this.chartSection.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.chartSection.Legends.Add(legend3);
            this.chartSection.Location = new System.Drawing.Point(556, 51);
            this.chartSection.Name = "chartSection";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chartSection.Series.Add(series3);
            this.chartSection.Size = new System.Drawing.Size(241, 269);
            this.chartSection.TabIndex = 2;
            this.chartSection.Text = "chart1";
            this.chartSection.Click += new System.EventHandler(this.chartSection_Click);
            // 
            // reports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(813, 450);
            this.Controls.Add(this.chartSection);
            this.Controls.Add(this.chartTeacher);
            this.Controls.Add(this.chartRoom);
            this.Name = "reports";
            this.Text = "reports";
            this.Load += new System.EventHandler(this.reports_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartRoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartTeacher)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartSection)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartRoom;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartTeacher;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartSection;
    }
}