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

namespace ContactCenter.Web.Areas.Config.Pages.Email
{
    public class DetailsModel : SysPageModel
    {
        public EmailConfig EmailConfig { get; set; }
        public string ErrorMessage { get; private set; }
        public bool? IsTestSuccessful { get; private set; }

        public async Task OnGetAsync(Guid ID)
        {
            EmailConfig = await Db.EmailConfigs.FirstOrDefaultAsync(c => c.Id == ID);
            Title = $"Email config details: {EmailConfig.Name}";
            PageTitle = EmailConfig.Name;
        }

        public async Task<IActionResult> OnGetStatusAsync(Guid ID)
        {
            EmailConfig = await Db.EmailConfigs.FirstOrDefaultAsync(c => c.Id == ID);
            EmailConfig.IsActive = !EmailConfig.IsActive;
            await Db.SaveChangesAsync();
            return RedirectToPage("./Details", new { ID });
        }

        public async Task<IActionResult> OnGetSendTest(Guid ID)
        {
            EmailConfig = await Db.EmailConfigs.FirstOrDefaultAsync(c => c.Id == ID);
            Title = $"Email config details: {EmailConfig.Name}";
            var emailSender = new EmailSender(EmailConfig.GetOptions());
            try
            {
                await emailSender.SendEmailAsync(CurrentUser.Email, "Test message", "Test message from wCyber Contact Center!");
                IsTestSuccessful = true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                IsTestSuccessful = false;
            }
            return Page();
        }
    }
}
