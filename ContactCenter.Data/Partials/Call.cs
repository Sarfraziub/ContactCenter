using ContactCenter.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactCenter.Data
{
    partial class Call : INotesContainer
    {
        [NotMapped]
        public CallDirection Direction
        {
            get => (CallDirection)DirectionId;
            set => DirectionId = (int)value;
        }

        public TimeSpan? Duration => EndTime - StartTime;

        public bool IsInbound => Direction == CallDirection.INBOUND;
    }
}
