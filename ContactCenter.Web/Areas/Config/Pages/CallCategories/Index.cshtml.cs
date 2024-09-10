using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContactCenter.Data;
using ContactCenter.Web.Pages;
using X.PagedList;

namespace ContactCenter.Web.Areas.Config.Pages.CallCategories
{
    public class IndexModel : SysListPageModel<CallCategory>
    {
        public void OnGet(int? p, int? ps)
        {
            Title  = "Call categories";
            var query = Db.CallCategories.AsQueryable();
            List = query.OrderBy(c => c.Id).ToPagedList(p ?? 1, ps ?? DefaultPageSize);
        }
    }
}
