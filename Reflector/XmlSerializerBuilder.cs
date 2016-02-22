using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artisan.Tools.Reflector
{
    public class XmlSerializerBuilder : ISerializerBuilder
    {
        private static ConcurrentDictionary<string, ISerializer> index = new ConcurrentDictionary<string, ISerializer>();

        public static readonly XmlSerializerBuilder Instance = new XmlSerializerBuilder();

        private XmlSerializerBuilder()
        {

        }

        public ISerializer Create(Type type)
        {
            string name = type.Name;

            if (!index.ContainsKey(name))
            {
                Type propRefType = (typeof(XmlSerializer<>)).MakeGenericType(type);
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
                Type propRefType = (typeof(XmlSerializer<>)).MakeGenericType(type);
                index[name] = (ISerializer)Activator.CreateInstance(propRefType);
            }
            return index[name];
        }
    }
}
