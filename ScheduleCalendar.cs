using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using MySql.Data.MySqlClient;

namespace Help_Scheduling_and_Teacher_Assignment_Loading_System
{

    public partial class ScheduleCalendar: UserControl
    {
        int month, year;
        public static int static_month, static_year;
        public ScheduleCalendar()
        {
            InitializeComponent();
        }

        private void ScheduleCalendar_Load(object sender, EventArgs e)
        {
            displaDays();
        }
        public void displaDays() 
        { 

            DateTime now = DateTime.Now;
            month = now.Month;
            year = now.Year;

            String monthname = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
            lbdate.Text = monthname + " " + year;

            static_month = month;
            static_year = year;
            DateTime startofthemonth = new DateTime(year, month, 1);
            int days = DateTime.DaysInMonth(year, month);
            int dayoftheweek = Convert.ToInt32(startofthemonth.DayOfWeek.ToString("d")) + 1;

            for (int i = 1; i < dayoftheweek; i++)
            {
                UserControlBlank ucb = new UserControlBlank();
                daycontainers.Controls.Add(ucb);
            }

            for (int i = 1; i <= days; i++)
            {
                DateTime currentDate = new DateTime(year, month, i);
                UserControlDays ucd = new UserControlDays(currentDate);  // Ipasok ang eksaktong petsa
                ucd.days(i);
                daycontainers.Controls.Add(ucd);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            daycontainers.Controls.Clear();
            UserControlDays.static_day = null; 

                        month--;


            String monthname = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
            lbdate.Text = monthname + " " + year;

            DateTime startofthemonth = new DateTime(year, month, 1);
            int days = DateTime.DaysInMonth(year, month);
            int dayoftheweek = Convert.ToInt32(startofthemonth.DayOfWeek.ToString("d")) + 1;

            for (int i = 1; i < dayoftheweek; i++)
            {
                UserControlBlank ucb = new UserControlBlank();
                daycontainers.Controls.Add(ucb);
            }

            for (int i = 1; i <= days; i++)
            {
                DateTime currentDate = new DateTime(year, month, i);
                UserControlDays ucd = new UserControlDays(currentDate);  // Ipasok ang eksaktong petsa
                ucd.days(i);
                daycontainers.Controls.Add(ucd);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            daycontainers.Controls.Clear();
            UserControlDays.static_day = null; 

                        month++;
   

            String monthname = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
            lbdate.Text = monthname + " " + year;

            DateTime startofthemonth = new DateTime(year, month, 1);
            int days = DateTime.DaysInMonth(year, month);
            int dayoftheweek = Convert.ToInt32(startofthemonth.DayOfWeek.ToString("d")) + 1;

            for (int i = 1; i < dayoftheweek; i++)
            {
                UserControlBlank ucb = new UserControlBlank();
                daycontainers.Controls.Add(ucb);
            }

            for (int i = 1; i <= days; i++)
            {
                DateTime currentDate = new DateTime(year, month, i);
                UserControlDays ucd = new UserControlDays(currentDate);  // Ipasok ang eksaktong petsa
                ucd.days(i);
                daycontainers.Controls.Add(ucd);
            }
        }
    }
}
