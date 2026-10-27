using System;
using System.Collections.Generic;
using System.Text;


namespace DataAccess.Model
{
    public class OrderingPause
    {
        public int PauseId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Reason { get; set; }
    }
}
