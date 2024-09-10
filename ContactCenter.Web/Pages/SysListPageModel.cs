using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace ContactCenter.Web.Pages
{
    public class SysListPageModel<T> : SysPageModel where T : class
    {
        [ViewData]
        public string SearchPlaceholder { get; protected set; }
        [ViewData]
        public string QueryString { get; protected set; }
        public IPagedList<T> List { get; protected set; }
        protected int DefaultPageSize { get; set; } = 50;
    }
}
