using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artisan.Tools.Reflector
{
    public interface ISerializer
    {
        object Deserialize(object node);

        object Deserialize(object node, string childNode);

        void Serialize(object item, object node);

        void Serialize(object item, object node, string childNode);
    }
}
