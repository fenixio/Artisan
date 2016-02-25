using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Artisan.Tools.Store
{
    public class QueryContraints<T> : IQueryConstraints<T> where T : StorableObject
    {
        private int pageNumber;

        private int pageSize;

        private List<SortDirection> sortDirs;

        private List<Expression<Func<T, object>>> sortExps;

        public QueryContraints()
        {
            pageNumber = 0;
            pageSize = 0;
            sortDirs = new List<SortDirection>();
            sortExps = new List<Expression<Func<T, object>>>();
        }

        public IQueryConstraints<T> Page(int pageNumber, int pageSize)
        {
            this.pageNumber = pageNumber;
            this.pageSize = pageSize;
            return this;
        }

        public IQueryConstraints<T> SortBy(Expression<Func<T, object>> property)
        {
            return SortBy(property, SortDirection.ASC);
        }

        public IQueryConstraints<T> SortBy(Expression<Func<T, object>> property, SortDirection sortDirection)
        {
            sortDirs.Add(sortDirection);
            sortExps.Add(property);
            return this;
        }

        public IQueryable<T> ApplyTo(IQueryable<T> query) 
        {
            if (sortExps.Count > 0)
            {
                var q = query;
                if (sortDirs[0] == SortDirection.ASC)
                {
                    q = q.OrderBy(sortExps[0]);
                }
                else
                {
                    q = q.OrderByDescending(sortExps[0]);
                }
                for (int i = 1; i < sortExps.Count; i++)
                {
                    if (sortDirs[i] == SortDirection.ASC)
                    {
                        q = q.OrderBy(sortExps[i]);
                    }
                    else
                    {
                        q = q.OrderByDescending(sortExps[i]);
                    }
                }
                if (pageSize > 0)
                {
                    q = q.Skip(pageNumber * pageSize).Take(pageSize);
                }
                return q;
            }
            else
            {
                return query;
            }
        }
    }
}
