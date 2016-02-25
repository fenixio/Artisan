using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artisan.Tools.Store
{
    public interface IRepository<T>
        where T : StorableObject, new()
    {

        T this[int id] { get; set; }

        T Get(int id);

        T New();

        void Save(T entity);

        void Delete(T entity);

    }
}
