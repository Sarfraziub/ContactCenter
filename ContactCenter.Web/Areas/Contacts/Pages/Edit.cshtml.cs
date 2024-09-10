using ContactCenter.Data;
using ContactCenter.Web.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ContactCenter.Web.Areas.Contacts.Pages
{
    public class EditModel : SysPageModel
    {
        [BindProperty]
        public Contact Contact { get; set; }
        public SelectList Locations { get; private set; }

        public async Task OnGetAsync(string Id)
        {
            PageTitle = "Edit contact";
            Title = $"Edit contact: {Id}";
            PageSubTitle = Id;
            Contact = await Db.Contacts.FirstOrDefaultAsync(c => c.Id == Id);
            Locations = new SelectList(Db.Locations.OrderBy(c => c.Name).ToList(), nameof(Location.Id), nameof(Location.Name));
        }

        public async Task<IActionResult> OnPost(string Id)
        {
           Contact = await Db.Contacts.FirstOrDefaultAsync(c => c.Id == Id);
            if (await TryUpdateModelAsync(Contact, nameof(Contact),
                p => p.Name,
                p => p.LocationId,
                p => p.Address,
                p => p.Email,
                p => p.Company
                ))
            {
                await Db.SaveChangesAsync();
            }
            return RedirectToPage("./Details", new { Contact.Id });
        }

    }
}
