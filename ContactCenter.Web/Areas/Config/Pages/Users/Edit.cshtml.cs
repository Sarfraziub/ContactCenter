using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContactCenter.Data;
using ContactCenter.Web.Pages;
using X.PagedList;
using ContactCenter.Lib;

namespace ContactCenter.Web.Areas.Config.Pages.Users
{
    [Authorize(Roles = nameof(UserRole.ADMIN))]
    public class EditModel : SysPageModel
    {

        [BindProperty]
        public User SelectedUser { get; private set; }

        public async Task OnGetAsync(Guid ID)
        {
            SelectedUser = await Db.Users.FirstOrDefaultAsync(c => c.Id == ID);
            Title = $"Edit user: {SelectedUser.Name}";
            PageTitle = SelectedUser.Name;
        }

        public async Task<IActionResult> OnPostAsync(Guid ID)
        {
            SelectedUser = await Db.Users.FirstOrDefaultAsync(c => c.Id == ID);
            if (await TryUpdateModelAsync(SelectedUser, "", p => p.Name, p => p.Email, p => p.Role, p => p.IsActive, p => p.Mobile))
            {
                await Db.SaveChangesAsync();
                return RedirectToPage("./Details", new { SelectedUser.Id });
            }
            return Page();
        }
    }
}