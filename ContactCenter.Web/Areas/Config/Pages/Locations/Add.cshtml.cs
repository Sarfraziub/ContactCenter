using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContactCenter.Data;
using ContactCenter.Web.Pages;
using wCyber.Helpers.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContactCenter.Web.Areas.Config.Pages.Locations
{
    public class AddModel : SysPageModel
    {

        [BindProperty]
        public Location Location { get; set; }

        public SelectList Parents { get; set; }
        public void OnGet()
        {
            Title = PageTitle = "Add new location..";
            Parents = new SelectList(Db.Locations.Where(c => c.ParentId == null).OrderBy(c => c.Name).ToList(), nameof(Data.Location.Id), nameof(Data.Location.Name));
        }

        public async Task<IActionResult> OnPost()
        {
            Location.Id = Guid.NewGuid();
            Location.CreatorId = CurrentUserId;
            Location.CreationDate = DateTime.Now;
            Db.Locations.Add(Location);
            await Db.SaveChangesAsync();
            return RedirectToPage("./Details", new { Location.Id });
        }
    }
}
