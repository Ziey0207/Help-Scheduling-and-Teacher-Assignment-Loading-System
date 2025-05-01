using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Help_Scheduling_and_Teacher_Assignment_Loading_System
{
    public class Schedule
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Teacher { get; set; }
        public DateTime Date { get; set; }
        public string Room { get; set; }
        public TimeSpan TimeIn { get; set; }
        public TimeSpan TimeOut { get; set; }
        public string Section { get; set; } // Add this line

        public string Time => $"{TimeIn:hh\\:mm} - {TimeOut:hh\\:mm}";
    }
}