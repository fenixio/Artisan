using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Linq.Expressions;
using System.Reflection;

namespace Artisan.Tools.Reflector
{
    public class XmlSerializer<T> : Reflector<T, XmlNode>
    {
        public XmlSerializer() : base(XmlSerializerBuilder.Instance)
        {

        }

        public override T Deserialize(T item, XmlNode node, string childNode)
        {
            if (item == null)
            {
                item = Activator.CreateInstance<T>();
            }
            XmlNode child = node.SelectSingleNode(childNode);
            actions.ForEach(
                 e => e(item, child)
            );
            return item;
        }

        public override string ParseValue(XmlNode parentNode, string nodeName, string defaultValue)
        {
            XmlNode node = parentNode.SelectSingleNode(nodeName);
            if (node != null)
            {
                return node.InnerText;
            }
            else
                return defaultValue;
        }

        public override List<T> ParseList(XmlNode parentNode, string nodeContainer)
        {
            List<T> items = new List<T>();
            XmlNode containerNode = parentNode.SelectSingleNode(nodeContainer);
            if (containerNode != null)
            {
                XmlNode node = containerNode.FirstChild;
                while (node != null)
                {
                    T item = default(T);
                    items.Add(Deserialize(item, node, "."));
                    node = node.NextSibling;
                }
            }
            return items;
        }

        public override void Serialize(T item, XmlNode parentNode, string nodeName)
        {
            if (item != null)
            {
                XmlNode node = parentNode.OwnerDocument.CreateElement(nodeName);
                sets.ForEach(
                     e => e(item, node)
                );
                parentNode.AppendChild(node);
            }
        }

        public override void AddValue(XmlNode parentNode, string nodeName, string value)
        {
            XmlNode node = parentNode.OwnerDocument.CreateElement(nodeName);
            node.InnerText = value;
            parentNode.AppendChild(node);
        }


        public override void AddList(IEnumerable<T> items, XmlNode parentNode, string nodeContainer)
        {
            if (items != null)
            {
                XmlNode containerNode = parentNode.OwnerDocument.CreateElement(nodeContainer);
                foreach (var item in items)
                {
                    Serialize(item, containerNode);
                }
                parentNode.AppendChild(containerNode);
            }
        }

        public override T ParseObject(XmlNode parentNode, string nodeName)
        {
            T item = default(T);
            item = Deserialize(item, parentNode, nodeName);
            return item;
        }

        public override T[] ParseArray(XmlNode parentNode, string nodeContainer)
        {
            return ParseList(parentNode, nodeContainer).ToArray();
        }

        public override void AddObject(T item, XmlNode parentNode, string nodeName)
        {
            if (item != null)
            {
                Serialize(item, parentNode, nodeName);
            }
        }

        public override void AddArray(T[] items, XmlNode parentNode, string nodeContainer)
        {
            AddList(items, parentNode, nodeContainer);
        }
    }
}
