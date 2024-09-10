using System;
using System.Collections.Generic;
using System.Text;

namespace ContactCenter.Lib
{
    public class Note
    {
        public Guid UserId { get; set; }
        public string Comments { get; set; }
        public DateTime Date { get; set; }
        public string Username { get; set; }
        public int StatusId { get; set; }
    }
}
