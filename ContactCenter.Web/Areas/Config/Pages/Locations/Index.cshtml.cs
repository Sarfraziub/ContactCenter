using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContactCenter.Data;
using ContactCenter.Web.Pages;
using X.PagedList;

namespace ContactCenter.Web.Areas.Config.Pages.Locations
{
    public class IndexModel : SysListPageModel<Location>
    {
        public void OnGet(int? p, int? ps)
        {
            Title  = "Locations";
            var query = Db.Locations.AsQueryable();
            List = query.OrderBy(c => c.Id).ToPagedList(p ?? 1, ps ?? DefaultPageSize);
        }
    }
}
