using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContactCenter.Data;
using ContactCenter.Web.Pages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContactCenter.Web.Areas.Config.Pages.Locations
{
    public class EditModel : SysPageModel
    {
        [BindProperty]
        public Location Location { get; set; }

        public SelectList Parents { get; set; }

        public async Task OnGetAsync(Guid ID)
        {
            Location = await Db.Locations.FindAsync(ID);
            Title = $"Edit location: {Location.Name}";
            Parents = new SelectList(Db.Locations.Where(c => c.ParentId == null && c.Id != ID).OrderBy(c => c.Name).ToList(), nameof(Data.Location.Id), nameof(Data.Location.Name));
            PageTitle = Location.Name;
        }

        public async Task<IActionResult> OnPostAsync(Guid Id)
        {
            Location = await Db.Locations.FindAsync(Id);
            if (await TryUpdateModelAsync(Location, nameof(Location), p => p.Name, p => p.ParentId))
            {
                await Db.SaveChangesAsync();
                return RedirectToPage("./Details", new { Location.Id });
            }
            return Page();
        }
    }
}
