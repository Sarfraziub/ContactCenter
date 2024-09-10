using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContactCenter.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using wCyber.Helpers.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;

namespace ContactCenter.Web
{
    public static class DataExtensions
    {
        public static async Task SendActivationEmail(this User user, EmailSender emailSender, HttpContext context, IUrlHelper Url, bool sendAsync = true)
        {

            if (emailSender == null) return;

            var _userManager = context.RequestServices.GetService<UserManager<User>>();
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var url = Url.Page(
                "/Account/ResetPassword",
                pageHandler: null,
                values: new { code, email = user.LoginId, area="Identity" },
                protocol: context.Request.Scheme);

            var HostEnvironment = context.RequestServices.GetService<IWebHostEnvironment>();
            string Body = File.ReadAllText(Path.Combine(HostEnvironment.WebRootPath, "email", "AccountActivation.html"));
            Body = Body.Replace("[GREETING]", $"Dear {user.Name}");
            Body = Body.Replace("[URL]", url);
            Body = Body.Replace("[USERNAME]", user.Email);
            if (sendAsync)
            {
                new Task(async () =>
                {
                    try
                    {
                        await emailSender.SendEmailAsync(user.Email, "Contact Center account activation", Body);
                    }
                    catch { }
                }).Start();
            }
            else
            {
                emailSender.SendEmail(user.Email, "Contact Center account activation", Body);
            }
        }
    }
}
