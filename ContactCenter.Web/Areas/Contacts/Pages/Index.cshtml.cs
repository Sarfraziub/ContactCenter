using ContactCenter.Web.Pages;
using ContactCenter.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Humanizer;
using X.PagedList;

namespace ContactCenter.Web.Areas.Contacts.Pages
{
    public class IndexModel : SysListPageModel<Contact>
    {
        public void OnGet(int? p, int? ps, string q)
        {
            PageTitle = Title = "Contacts";
            var query = Db.Contacts
                .Include(c => c.Location)
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(q))
            {
                QueryString = q;
                q = q.ToLower();
                query = query.Where(x => x.Id.Contains(q) || x.Name.ToLower().Contains(q) || (x.DetailsJson != null && x.DetailsJson.Contains(q)));
            }
            List = query.OrderBy(c => c.Name).ToPagedList(p ?? 1, ps ?? DefaultPageSize);
            PageSubTitle = "contact".ToQuantity(List.TotalItemCount) + " found..";
        }
    }
}
