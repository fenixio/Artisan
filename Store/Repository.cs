using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Artisan.Tools.Exceptions;

namespace Artisan.Tools.Store
{
    public class Repository<T> : IRepository<T>
        where T : StorableObject, new()
    {

        private DbContext ctx;

        public Repository()
        {
            ctx = null;
        }

        public Repository(DbContext context)
        {
            ctx = context;
        }

        public virtual T this[int id] {
            get
            {
                return Get(id);
            }
            set
            {
                Save(value);
            }
        }

        public virtual T Get(int id)
        {
            T entity = Context.Set<T>().FirstOrDefault(e => e.Id == id);

            return entity;
        }

        public virtual PagedResult<T> Search(
            Expression<Func<T, bool>> where, int skip, int take
        )
        {

            var query = Context.Set<T>().Skip(skip).Take(take).Where(where);


            return new PagedResult<T>(query.ToList(), query.Count());
        }

        public IEnumerable<T> Search(
            Expression<Func<T, bool>> filter = null,
            IQueryConstraints<T> queryConstraints = null,
            string includeProperties = "")
        {
            bool ctxCreated = false;
            DbContext localCtx = null;
            try
            {
                if (ctx == null)
                {
                    localCtx = Context.Factory.Get(typeof(T));
                    ctxCreated = true;
                }

                IQueryable<T> query = localCtx.Set<T>();

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }

                if (queryConstraints != null)
                {

                    return queryConstraints.ApplyTo(query).ToList();
                }
                else
                {
                    return query.ToList();
                }
            }
            catch(Exception ex)
            {
                throw new AppException(ex, "Failed executing repositiry search");
            }
            finally
            {
                if(ctxCreated)
                    ctx.Dispose();
            }
        }


        public virtual T New()
        {
            throw new NotImplementedException();
        }

        public virtual void Save(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual void Delete(T entity)
        {
            throw new NotImplementedException();
        }

    }
}
