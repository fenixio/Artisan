using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Artisan.Tools.Reflector
{
    public class TextSerializer<T> : Reflector<T, StringBuilder>
    {
        public TextSerializer() : base(TextSerializerBuilder.Instance)
        {

        }

        public override T Deserialize(T item, StringBuilder node, string childNode)
        {
            throw new NotImplementedException();
        }

        public override string ParseValue(StringBuilder parentNode, string nodeName, string defaultValue)
        {
            throw new NotImplementedException();
        }

        public override List<T> ParseList(StringBuilder parentNode, string nodeContainer)
        {
            throw new NotImplementedException();
        }

        public override void Serialize(T item, StringBuilder parentNode, string nodeName)
        {
            if (item != null)
            {
                //parentNode.AppendLine();
                parentNode.Append("{");
                StringBuilder node = new StringBuilder();
                node.AppendLine("{");
                sets.ForEach(
                     e => e(item, node)
                );
                if (node[node.Length - 3] == ',') node.Remove(node.Length - 3, 1);
                node.Append("}");
                parentNode.AppendFormat( "{0}:{1}", typeof(T).Name, node.ToString());
                parentNode.AppendLine("},");
            }
        }

        public override void AddValue(StringBuilder parentNode, string nodeName, string value)
        {
            parentNode.AppendFormat("{0}:{1},", nodeName, value);
            parentNode.AppendLine();
        }


        public override void AddList(IEnumerable<T> items, StringBuilder parentNode, string nodeContainer)
        {
            if (items != null)
            {
                parentNode.AppendFormat("{0}:[", nodeContainer);
                parentNode.AppendLine();
                StringBuilder nodes = new StringBuilder();
                foreach (var item in items)
                {
                    Serialize(item, nodes);
                    //nodes.AppendLine(",");
                }
                if (nodes[nodes.Length - 1] == ',') nodes.Remove(nodes.Length - 1, 1);
                parentNode.AppendLine(nodes.ToString());
                parentNode.AppendLine("]");
            }
        }

        public override T ParseObject(StringBuilder parentNode, string nodeName)
        {
            throw new NotImplementedException();
        }

        public override T[] ParseArray(StringBuilder parentNode, string nodeContainer)
        {
            throw new NotImplementedException();
        }

        public override void AddObject(T item, StringBuilder parentNode, string nodeName)
        {
            if (item != null)
            {
                Serialize(item, parentNode, nodeName);
            }
        }

        public override void AddArray(T[] items, StringBuilder parentNode, string nodeContainer)
        {
            AddList(items, parentNode, nodeContainer);
        }
    }
}
