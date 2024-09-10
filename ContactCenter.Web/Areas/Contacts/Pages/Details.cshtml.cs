using ContactCenter.Data;
using ContactCenter.Web.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContactCenter.Web.Areas.Contacts.Pages
{
    public class DetailsModel : SysPageModel
    {
        public Contact Contact { get; set; }

        public async Task OnGetAsync(string Id)
        {
            Contact = await Db.Contacts
                .Include(c => c.Creator)
                .Include(c => c.Tickets).ThenInclude(c => c.Assignee)
                .Include(c => c.Tickets).ThenInclude(c => c.Location)
                .Include(c => c.Calls).ThenInclude(c => c.Agent)
                .Include(c => c.Calls).ThenInclude(c => c.Category)
                .FirstOrDefaultAsync(c => c.Id == Id);
            PageTitle = "Contact details";
            Title = $"Contact details: {Id}";
            PageSubTitle = Id;
        }
    }
}
