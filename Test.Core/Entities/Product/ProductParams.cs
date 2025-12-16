using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Entities.Product
{
    public class ProductParams
    {
        public string? Sort { get; set; }          // sorting
        public int? CategoryId { get; set; }      // filter by category
        public int TotalCount { get; set; }       // total items before pagination
        public string? Search { get; set; }        // keyword search

        public int MaxPageSize { get; set; } = 10;

        private int _pageSize = 3;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }

        public int PageNumber { get; set; } = 1;  // which page
    }
}
