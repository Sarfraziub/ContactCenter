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

namespace ContactCenter.Web.Areas.Config.Pages.CallCategories
{
    public class AddModel : SysPageModel
    {

        [BindProperty]
        public CallCategory Category { get; set; }

        public SelectList Parents { get; set; }
        public void OnGet()
        {
            Title = PageTitle = "Add new category..";
            Parents = new SelectList(Db.CallCategories.Where(c => c.ParentId == null).OrderBy(c => c.Name).ToList(), nameof(CallCategory.Id), nameof(CallCategory.Name));
        }

        public async Task<IActionResult> OnPost()
        {
            Category.Id = Guid.NewGuid();
            Category.CreatorId = CurrentUserId;
            Category.CreationDate = DateTime.Now;
            Db.CallCategories.Add(Category);
            await Db.SaveChangesAsync();
            return RedirectToPage("./Details", new { Category.Id });
        }
    }
}
