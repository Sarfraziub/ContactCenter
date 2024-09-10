using ContactCenter.Data;
using ContactCenter.Web.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContactCenter.Web.Areas.Tickets.Pages
{
    public class DetailsModel : SysPageModel
    {
        public Ticket Ticket { get; private set; }
        public async Task OnGet(string Id)
        {
            Ticket = await Db.Tickets
                .Include(c => c.Contact)
                .Include(c => c.Location)
                .Include(c => c.Assignee)
                .Include(c => c.Category)
                .FirstOrDefaultAsync(c => c.Number == Id);
            PageTitle = "Ticket details";
            PageSubTitle = Id;
            PageTitle = $"Ticket details: {Id}";
        }
    }
}
