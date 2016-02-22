using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artisan.Tools.Reflector
{
    public interface ISerializerBuilder
    {
        ISerializer Create(Type type);

        ISerializer Create<T>();
    }
}
