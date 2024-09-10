using ContactCenter.Data;
using ContactCenter.Web.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContactCenter.Web.Areas.Contacts.Pages
{
    public class AddModel : SysPageModel
    {
        [BindProperty]
        public Contact Contact { get; set; }

        public SelectList Locations { get; private set; }

        public void OnGet()
        {
            Title = PageTitle = "Add new contact";
            Locations = new SelectList(Db.Locations.OrderBy(c => c.Name).ToList(), nameof(Location.Id), nameof(Location.Name));
        }

        public async Task<IActionResult> OnPost()
        {
            Contact.CreationDate=DateTime.Now;
            Contact.CreatorId = CurrentUserId;
            Db.Contacts.Add(Contact);
            await Db.SaveChangesAsync();
            return RedirectToPage("./Details", new { Contact.Id });
        }
    }
}
