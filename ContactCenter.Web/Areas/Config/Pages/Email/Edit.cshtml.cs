using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContactCenter.Data;
using ContactCenter.Web.Pages;

namespace ContactCenter.Web.Areas.Config.Pages.Email
{
    public class EditModel : SysPageModel
    {
        [BindProperty]
        public EmailConfig EmailConfig { get; set; }

        public async Task OnGetAsync(Guid ID)
        {
            EmailConfig = await Db.EmailConfigs.FindAsync(ID);
            Title = $"Edit email config: {EmailConfig.Name}";
            PageTitle = EmailConfig.Name;
        }

        public async Task<IActionResult> OnPostAsync(Guid ID, int[] TargetIds)
        {
            var newPwd = EmailConfig.Password;
            EmailConfig = await Db.EmailConfigs.FindAsync(ID);
            if (await TryUpdateModelAsync(EmailConfig, nameof(EmailConfig), p => p.Name, p => p.IsActive, p => p.SenderId, p => p.Host, p => p.Port, p => p.EnableSsl, p => p.Username, p => p.SenderDisplayName))
            {
                EmailConfig.TargetId = 0;
                if (TargetIds != null) EmailConfig.TargetId = TargetIds.Sum();
                if (!string.IsNullOrWhiteSpace(newPwd))
                {
                    EmailConfig.Password = newPwd;
                    EmailConfig.Hash = EmailConfig.ComputeHash();
                }
                await Db.SaveChangesAsync();
                return RedirectToPage("./Details", new { EmailConfig.Id });
            }
            return Page();
        }
    }
}
