using ContactCenter.Lib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContactCenter.Web.Pages
{
    public class IndexModel : SysPageModel
    {
        public int InCalls { get; set; }
        public int OutCalls { get; set; }
        public int Agents { get; set; }
        public int OTickets { get; set; }
        public int CTickets { get; set; }

        public async Task OnGetAsync()
        {
            Title = "Dashboard";
            var today = DateTime.Today;
            var callsQuery = Db.Calls.Where(c => c.StartTime >= today);
            var ticketsQuery = Db.Tickets.AsQueryable();
            Agents = await Db.Agents.CountAsync(c => c.StatusId == (int)AgentStatus.ONLINE);

            if (User.IsAgent())
            {
                callsQuery = callsQuery.Where(c => c.AgentId == CurrentUserId);
                ticketsQuery = ticketsQuery.Where(c => c.CreatorId == CurrentUserId.ToString());
            }

            InCalls = await callsQuery.CountAsync(c => c.DirectionId == (int)CallDirection.INBOUND);
            OutCalls = await callsQuery.CountAsync(c => c.DirectionId == (int)CallDirection.OUTBOUND);

            OTickets = await ticketsQuery.CountAsync(c => c.StatusId == (int)TicketStatus.IN_PROGRESS || c.StatusId == (int)TicketStatus.PENDING);
            CTickets = await ticketsQuery.CountAsync(c => c.CreationDate > today && c.StatusId == (int)TicketStatus.RESOLVED);
        }
    }
}