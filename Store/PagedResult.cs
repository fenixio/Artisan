using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artisan.Tools.Store
{
    public class PagedResult<T>
    {
        private IEnumerable<T> items;
        private int totalCount;

        public IEnumerable<T> Items
        {
            get { return items; }
        }

        public int TotalCount
        {
            get { return totalCount; }
        }


        public PagedResult(IEnumerable<T> items, int totalCount)
        {
            this.items = items;
            this.totalCount = totalCount;
        }

    }
}
