using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Shared
{
    public class PagedResult<T>
    {
        public int PageNumber { get; set; } // current page
        public int PageSize { get; set; } // items per page

        public int TotalCount { get; set; } // total items before pagination

        public int TotalPages =>
            (int)Math.Ceiling((double)TotalCount / PageSize); // total pages

        public IReadOnlyList<T> Data { get; set; } // data for the current page
    }
}
