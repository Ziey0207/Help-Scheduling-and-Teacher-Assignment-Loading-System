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
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }
    }
}