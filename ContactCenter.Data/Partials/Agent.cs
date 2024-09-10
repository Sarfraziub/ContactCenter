using ContactCenter.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactCenter.Data
{
    partial class Agent
    {
        [NotMapped]
        public AgentStatus Status
        {
            get => (AgentStatus)StatusId;
            set => StatusId = (int)value;
        }

        public bool IsOnline => Status == AgentStatus.ONLINE;
        public bool IsOffline => Status == AgentStatus.OFFLINE;
        public bool IsDeactivated => Status == AgentStatus.DEACTIVATED;

    }
}
