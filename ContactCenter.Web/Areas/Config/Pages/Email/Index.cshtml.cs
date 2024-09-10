using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContactCenter.Data;
using ContactCenter.Web.Pages;
using X.PagedList;

namespace ContactCenter.Web.Areas.Config.Pages.Email
{
    public class IndexModel : SysListPageModel<EmailConfig>
    {
        public void OnGet(int? p, int? ps)
        {
            Title  = "Email configs..";
            var query = Db.EmailConfigs.AsQueryable();
            List = query.OrderByDescending(c => c.CreationDate).ToPagedList(p ?? 1, ps ?? DefaultPageSize);
        }
    }
}
