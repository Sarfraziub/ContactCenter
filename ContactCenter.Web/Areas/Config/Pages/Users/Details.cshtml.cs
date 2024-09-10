using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContactCenter.Data;
using wCyber.Helpers.Identity;
using ContactCenter.Web.Pages;
using X.PagedList;
using ContactCenter.Lib;

namespace ContactCenter.Web.Areas.Config.Pages.Users
{
    public class DetailsModel : SysPageModel
    {
        public User SelectedUser { get; private set; }

        public bool IsCurrentUser => SelectedUser.Id == CurrentUser.Id;

        public async Task OnGetAsync(Guid? ID)
        {
            if (ID == null) ID = CurrentUserId;
            SelectedUser = await Db.Users
                .FirstOrDefaultAsync(c => c.Id == ID);
            Title = $"User details: {SelectedUser.Name}";
            PageTitle = SelectedUser.Name;
        }

        public async Task<IActionResult> OnGetStatusAsync(Guid ID)
        {
            //if (!User.IsAdmin()) return Unauthorized();
            SelectedUser = await Db.Users.FindAsync(ID);
            SelectedUser.IsActive = !SelectedUser.IsActive;
            await Db.SaveChangesAsync();
            return RedirectToPage("./Details", new { ID });
        }

        public async Task<IActionResult> OnGetSendActivationAsync(Guid? Id)
        {
            if (Id == null) Id = CurrentUser.Id;
            EmailSender EmailSender = null;
            var config = await Db.EmailConfigs.FirstOrDefaultAsync(c => c.IsActive && (c.TargetId & (int)EmailConfigTarget.LOGIN_ACCOUNT_MANAGEMENT) == (int)EmailConfigTarget.LOGIN_ACCOUNT_MANAGEMENT);
            if (config != null) EmailSender = new EmailSender(config.GetOptions());

            SelectedUser = await Db.Users
                .FirstOrDefaultAsync(c => c.Id == Id);
            await SelectedUser.SendActivationEmail(EmailSender, HttpContext, Url, false);
            return RedirectToPage("./Details", new { Id });
        }
    }
}