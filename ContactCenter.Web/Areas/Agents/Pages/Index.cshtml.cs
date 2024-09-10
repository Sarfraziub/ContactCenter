using ContactCenter.Data;
using ContactCenter.Web.Pages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ContactCenter.Web.Areas.Agents.Pages
{
    public class IndexModel : SysPageModel
    {
        private readonly SignInManager<User> _signInManager;
        public IndexModel(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public List<Agent> Agents { get; private set; }
        public async Task OnGetAsync()
        {
            Agents = await Db.Agents
                .Include(c => c.Creator)
                .OrderBy(c => c.Extension)
                .ToListAsync();
            Title = PageTitle = "Agents..";
        }

        public async Task<IActionResult> OnPostCheckInAsync()
        {
            var agent = Db.Agents.FirstOrDefault(c => c.Id == CurrentUserId);
            if (agent.IsOnline)
            {
                agent.Status = Lib.AgentStatus.OFFLINE;
                var session = await Db.AgentSessions.FirstOrDefaultAsync(c => c.CheckoutTime == null);
                if (session != null) session.CheckoutTime = DateTime.Now;
            }
            else if (agent.IsOffline)
            {
                agent.Status = Lib.AgentStatus.ONLINE;
                agent.AgentSessions.Add(new AgentSession
                {
                    Id = Guid.NewGuid(),
                    CheckInTime = DateTime.Now,
                });
            }
            await Db.SaveChangesAsync();
            await _signInManager.SignInAsync(CurrentUser, false);
            return RedirectToPage("/Index", new { area = "" });
        }
    }
}
