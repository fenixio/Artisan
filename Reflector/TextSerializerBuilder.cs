using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artisan.Tools.Reflector
{
    public class TextSerializerBuilder : ISerializerBuilder
    {
        private static ConcurrentDictionary<string, ISerializer> index = new ConcurrentDictionary<string, ISerializer>();

        public static readonly TextSerializerBuilder Instance = new TextSerializerBuilder();

        private TextSerializerBuilder()
        {

        }

        public ISerializer Create(Type type)
        {
            string name = type.Name;

            if (!index.ContainsKey(name))
            {
                Type propRefType = (typeof(TextSerializer<>)).MakeGenericType(type);
                index[name] = (ISerializer)Activator.CreateInstance(propRefType);
            }
            return index[name];
        }

        public ISerializer Create<T>()
        {
            Type type = typeof(T);
            string name = type.Name;

            if (!index.ContainsKey(name))
            {
                Type propRefType = (typeof(TextSerializer<>)).MakeGenericType(type);
                index[name] = (ISerializer)Activator.CreateInstance(propRefType);
            }
            return index[name];
        }
    }
}
