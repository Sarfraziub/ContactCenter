using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContactCenter.Data;
using ContactCenter.Web.Pages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContactCenter.Web.Areas.Config.Pages.TicketCategories
{
    public class EditModel : SysPageModel
    {
        [BindProperty]
        public TicketCategory Category { get; set; }

        public SelectList Parents { get; set; }

        public async Task OnGetAsync(Guid ID)
        {
            Category = await Db.TicketCategories.FindAsync(ID);
            Title = $"Edit category: {Category.Name}";
            Parents = new SelectList(Db.TicketCategories.Where(c => c.ParentId == null && c.Id != ID).OrderBy(c => c.Name).ToList(), nameof(CallCategory.Id), nameof(CallCategory.Name));
            PageTitle = Category.Name;
        }

        public async Task<IActionResult> OnPostAsync(Guid Id)
        {
            Category = await Db.TicketCategories.FindAsync(Id);
            if (await TryUpdateModelAsync(Category, nameof(Category), p => p.Name, p => p.ParentId))
            {
                await Db.SaveChangesAsync();
                return RedirectToPage("./Details", new { Category.Id });
            }
            return Page();
        }
    }
}
