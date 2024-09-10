using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContactCenter.Data;
using ContactCenter.Web.Pages;
using wCyber.Helpers.Identity;

namespace ContactCenter.Web.Areas.Config.Pages.Email
{
    public class AddModel : SysPageModel
    {

        [BindProperty]
        public EmailConfig NewEmailConfig { get; set; }

        public void OnGet()
        {
            Title = PageTitle = "Add new email config..";
        }

        public async Task<IActionResult> OnPost(int[] TargetIds)
        {
            NewEmailConfig.Id = Guid.NewGuid();
            NewEmailConfig.CreationDate = DateTime.Now;
            NewEmailConfig.CreatorId = CurrentUserId;
            NewEmailConfig.ComputeHash();
            if (TargetIds != null) NewEmailConfig.TargetId = TargetIds.Sum();
            Db.EmailConfigs.Add(NewEmailConfig);
            await Db.SaveChangesAsync();
            return RedirectToPage("./Details", new { NewEmailConfig.Id });
        }
    }
}
