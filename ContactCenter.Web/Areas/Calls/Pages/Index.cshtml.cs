using ContactCenter.Web.Pages;
using ContactCenter.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList;
using Microsoft.EntityFrameworkCore;
using Humanizer;

namespace ContactCenter.Web.Areas.Calls.Pages
{
    public class IndexModel : SysListPageModel<Call>
    {
        public void OnGet(int? p, int? ps, string q, Guid? Id)
        {
            PageTitle = Title = "Calls";
            var query = Db.Calls
                .Include(x => x.Ticket)
                .Include(x => x.Agent.Creator)
                .Include(x => x.Category)
                .Include(x => x.Contact)
                .AsQueryable();
            if (Id != null)
            {
                query = query.Where(x => x.Id == Id);
            }
            List = query.OrderByDescending(c => c.StartTime).ToPagedList(p ?? 1, ps ?? DefaultPageSize);
            PageSubTitle = "Call".ToQuantity(List.TotalItemCount) + " found..";
        }
    }
}
