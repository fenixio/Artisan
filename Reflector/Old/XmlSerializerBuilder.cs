using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artisan.Tools.Reflector
{
    public static class XmlSerializerBuilder
    {
        private static Dictionary<string, object> index = new Dictionary<string, object>();

        public static ISerializer Create(Type type)
        {
            string name = type.Name;

            if (!index.ContainsKey(name))
            {
                Type propRefType = (typeof(XmlSerializer<>)).MakeGenericType(type);
                index[name] = Activator.CreateInstance(propRefType);
            }
            return (ISerializer)index[name];
        }

        public static XmlSerializer<T> Create<T>()
        {
            Type type = typeof(T);
            string name = type.Name;

            if (!index.ContainsKey(name))
            {
                Type propRefType = (typeof(XmlSerializer<>)).MakeGenericType(type);
                index[name] = Activator.CreateInstance(propRefType);
            }
            return (XmlSerializer<T>)index[name];
        }
    }
}
