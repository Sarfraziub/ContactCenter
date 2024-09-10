using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using wCyber.Lib.FileStorage;
using Microsoft.EntityFrameworkCore;
using ContactCenter.Data;

namespace ContactCenter.Web.Pages
{
    [Authorize]
    public class SysPageModel : PageModel
    {
        public Guid CurrentUserId => Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        static bool IsDbCreated;
        EDRSMContext _db;
        public EDRSMContext Db
        {
            get
            {
                if (_db == null)
                {
                    _db = Request.HttpContext.RequestServices.GetService<EDRSMContext>();
                    if (!IsDbCreated)
                    {
                        _db.Database.Migrate();
                        IsDbCreated = true;
                    }
                }
                return _db;
            }
        }

        public Agent CurrentAgent => CurrentUser.Agent;

        User _currentUser;
        public User CurrentUser
        {
            get
            {
                _currentUser ??= Db.Users
                        .Include(c => c.Agent)
                        .FirstOrDefault(c => c.Id == CurrentUserId);
                return _currentUser;
            }
        }

        IFileStore _filestore;
        protected IFileStore FileStore
        {
            get
            {
                if (_filestore == null) _filestore = Request.HttpContext.RequestServices.GetService<IFileStore>();
                return _filestore;
            }
        }

        [ViewData]
        public string Title { get; protected set; }

        [ViewData]
        public string PageTitle { get; protected set; }
        [ViewData]
        public string PageSubTitle { get; protected set; }

        [ViewData]
        public string SideNavPath { get; protected set; }

        [ViewData]
        public bool OverrideNav { get; protected set; }

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            Title = GetType().Namespace[(GetType().Namespace.LastIndexOf(".") + 1)..];
            if (Title == "Pages" && GetType().Namespace.Contains("Areas"))
                Title = GetType().Namespace.Replace(".Pages", "")[(GetType().Namespace.Replace(".Pages", "").LastIndexOf(".") + 1)..];
            base.OnPageHandlerExecuting(context);
        }
    }
}
