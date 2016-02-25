using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Artisan.Tools.Store
{
    public enum SortDirection
    {
           ASC,
           DESC
    }

    /// <summary>
    /// Typed paging
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public interface IQueryConstraints<T> where T : StorableObject
    {

        /// <summary>
        /// Use paging
        /// </summary>
        /// <param name="pageNumber">Page to get (one based index).</param>
        /// <param name="pageSize">Number of items per page.</param>
        /// <returns>Current instance</returns>
        IQueryConstraints<T> Page(int pageNumber, int pageSize);

        /// <summary>
        /// Property to sort by (ascending)
        /// </summary>
        /// <param name="property">The property.</param>
        IQueryConstraints<T> SortBy(Expression<Func<T, object>> property);

        /// <summary>
        /// Property to sort by (ascending)
        /// </summary>
        /// <param name="property">The property.</param>
        IQueryConstraints<T> SortBy(Expression<Func<T, object>> property, SortDirection sortDirection);


        IQueryable<T> ApplyTo(IQueryable<T> query)

    }
}
