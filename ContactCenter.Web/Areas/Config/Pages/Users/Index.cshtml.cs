using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContactCenter.Data;
using ContactCenter.Web.Pages;
using X.PagedList;

namespace ContactCenter.Web.Areas.Config.Pages.Users
{
    public class IndexModel : SysListPageModel<User>
    {
        public void OnGet(int? p, int? ps)
        {
            SearchPlaceholder = "Search users..";
            var query = Db.Users.AsQueryable();
            List = query.OrderBy(c => c.Name).ToPagedList(p ?? 1, ps ?? DefaultPageSize);
        }
    }
}