using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContactCenter.Data;
using ContactCenter.Lib;
using ContactCenter.Web.Pages;
using X.PagedList;

namespace ContactCenter.Web.Areas.Config.Pages.Users
{
    [Authorize(Roles = nameof(UserRole.ADMIN))]
    public class AddModel : SysPageModel
    {

        [BindProperty]
        public User NewUser { get; set; }

        public void OnGet()
        {
            Title = PageTitle = "Add new user..";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            NewUser.LoginId = NewUser.LoginId.ToLower().Trim();
            var duplicateUser = await Db.Users.FirstOrDefaultAsync(c => c.LoginId == NewUser.LoginId);
            if (duplicateUser != null)
            {
                Title = PageTitle = "Add new user..";
                ModelState.AddModelError($"{nameof(NewUser)}.{nameof(NewUser.LoginId)}", "A user with the same Login Id already exists!");
                return Page();
            }
            NewUser.Id = Guid.NewGuid();
            NewUser.CreationDate = DateTime.Now;
            NewUser.SecurityStamp = Guid.NewGuid().ToString();
            NewUser.CreatorId = CurrentUser.Id;
            Db.Users.Add(NewUser);
            NewUser.Agent = new Agent
            {
                CreationDate = NewUser.CreationDate,
                CreatorId = CurrentUserId,
                Extension = NewUser.Extension,
                Status = AgentStatus.OFFLINE,
            };
            await Db.SaveChangesAsync();
            return RedirectToPage("./Details", new { NewUser.Id });
        }
    }
}