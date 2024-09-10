using ContactCenter.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactCenter.Data
{
    partial class Ticket
    {
        [NotMapped]
        public Lib.TicketStatus Status
        {
            get => (Lib.TicketStatus)StatusId;
            set => StatusId = (int)value;
        }

        [NotMapped]
        public Lib.TicketType Type
        {
            get => (Lib.TicketType)TypeId;
            set => TypeId = (int)value;
        }
    }
}
