using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContactCenter.Data;
using ContactCenter.Web.Pages;
using wCyber.Helpers.Identity;

namespace ContactCenter.Web.Areas.Config.Pages.CallCategories
{
    public class DetailsModel : SysPageModel
    {
        public CallCategory Category { get; set; }
        public async Task OnGetAsync(Guid Id)
        {
            Category = await Db.CallCategories
                .Include(c => c.Creator)
                .Include(c => c.Parent)
                .Include(c => c.InverseParent)
                .FirstOrDefaultAsync(c => c.Id == Id);
            Title = PageTitle = Category.Name;
        }
    }
}
