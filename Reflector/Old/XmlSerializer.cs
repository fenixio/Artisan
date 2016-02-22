using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Linq.Expressions;
using System.Reflection;

namespace Artisan.Tools.Reflector
{
    public class XmlSerializer<T> : ISerializer
    {
        private static List<Action<T, XmlNode>> actions;
        private static List<Action<T, XmlNode>> sets;
        private static Type type;

        #region Initializers
        public XmlSerializer()
        {
            actions = new List<Action<T, XmlNode>>();
            sets = new List<Action<T, XmlNode>>();
            type = typeof(T);
            GetExpressions();
        }

        private void GetExpressions()
        {
            // first parameter of each expression is the target object
            ParameterExpression targetExp  = Expression.Parameter(typeof(T), "target");
            ParameterExpression nodeExp    = Expression.Parameter(typeof(XmlNode), "nodeExp");
            ConstantExpression emptyStrExp = Expression.Constant("", typeof(string));
            ConstantExpression zeroExp = Expression.Constant("0", typeof(string));
            ConstantExpression dateExp = Expression.Constant("0001/01/01", typeof(string));

            Expression<Func<XmlSerializer<T>, XmlNode, string, string, string>> extract = (ds, nd, na, dv) => ds.GetValueFromNode(nd, na, dv);
            Expression<Action<XmlSerializer<T>, XmlNode, string, string>> push = (ds, nd, na, dv) => ds.CreateNodeFromValue(nd, na, dv);

            PropertyInfo[] properties = type.GetProperties();
            if (properties.Length > 0)
            {
                foreach (PropertyInfo property in properties)
                {
                    // instance.Property 
                    MemberExpression propExp = Expression.Property(targetExp, property);
                    ConstantExpression nameExp = Expression.Constant(property.Name, typeof(string));
                    InvocationExpression invokeExtractExp = null;
                    InvocationExpression invokePushExp = null;
                    BinaryExpression assignExtractExp = null;
                    MethodCallExpression refCallExp = null;
                    ConstantExpression thisExp = Expression.Constant(this, typeof(XmlSerializer<T>));

                    if (property.PropertyType.IsAssignableFrom(typeof(string)))
                    {
                        if (property.CanWrite)
                        {
                            invokeExtractExp = Expression.Invoke(extract, thisExp, nodeExp, nameExp, emptyStrExp);
                            assignExtractExp = Expression.Assign(propExp, invokeExtractExp);
                        }

                        invokePushExp = Expression.Invoke(push, thisExp, nodeExp, nameExp, propExp);
                    }

                    else if (property.PropertyType.IsAssignableFrom(typeof(byte)))
                    {
                        if (property.CanWrite)
                        {
                            invokeExtractExp = Expression.Invoke(extract, thisExp, nodeExp, nameExp, zeroExp);
                            Expression<Func<string, byte>> parseExp = (s) => byte.Parse(s);
                            InvocationExpression convertExp = InvocationExpression.Invoke(parseExp, invokeExtractExp);
                            assignExtractExp = Expression.Assign(propExp, convertExp);
                        }
                        Expression<Func<byte, string>> valExp = (val) => val.ToString();
                        InvocationExpression tosExp = InvocationExpression.Invoke(valExp, propExp);
                        invokePushExp = Expression.Invoke(push, thisExp, nodeExp, nameExp, tosExp);
                    }
                    else if (property.PropertyType.IsAssignableFrom(typeof(decimal)))
                    {
                        if (property.CanWrite)
                        {
                            invokeExtractExp = Expression.Invoke(extract, thisExp, nodeExp, nameExp, zeroExp);
                            Expression<Func<string, decimal>> parseExp = (s) => decimal.Parse(s);
                            InvocationExpression convertExp = InvocationExpression.Invoke(parseExp, invokeExtractExp);
                            assignExtractExp = Expression.Assign(propExp, convertExp);
                        }
                        Expression<Func<decimal, string>> valExp = (val) => val.ToString("0.000");
                        InvocationExpression tosExp = InvocationExpression.Invoke(valExp, propExp);
                        invokePushExp = Expression.Invoke(push, thisExp, nodeExp, nameExp, tosExp);
                    }
                    else if (property.PropertyType.IsAssignableFrom(typeof(double)))
                    {
                        if (property.CanWrite)
                        {
                            invokeExtractExp = Expression.Invoke(extract, thisExp, nodeExp, nameExp, zeroExp);
                            Expression<Func<string, double>> parseExp = (s) => double.Parse(s);
                            InvocationExpression convertExp = InvocationExpression.Invoke(parseExp, invokeExtractExp);
                            assignExtractExp = Expression.Assign(propExp, convertExp);
                        }
                        Expression<Func<double, string>> valExp = (val) => val.ToString("0.000");
                        InvocationExpression tosExp = InvocationExpression.Invoke(valExp, propExp);
                        invokePushExp = Expression.Invoke(push, thisExp, nodeExp, nameExp, tosExp);
                    }
                    else if (property.PropertyType.IsAssignableFrom(typeof(bool)))
                    {
                        if (property.CanWrite)
                        {
                            invokeExtractExp = Expression.Invoke(extract, nodeExp, nameExp, zeroExp);
                            Expression<Func<string, bool>> parseExp = (s) => bool.Parse(s);
                            InvocationExpression convertExp = InvocationExpression.Invoke(parseExp, invokeExtractExp);
                            assignExtractExp = Expression.Assign(propExp, convertExp);
                        }
                        Expression<Func<bool, string>> valExp = (val) => val.ToString();
                        InvocationExpression tosExp = InvocationExpression.Invoke(valExp, propExp);
                        invokePushExp = Expression.Invoke(push, nodeExp, nameExp, tosExp);
                    }
                    else if (property.PropertyType.IsAssignableFrom(typeof(short)))
                    {
                        if (property.CanWrite)
                        {
                            invokeExtractExp = Expression.Invoke(extract, thisExp, nodeExp, nameExp, zeroExp);
                            Expression<Func<string, short>> parseExp = (s) => short.Parse(s);
                            InvocationExpression convertExp = InvocationExpression.Invoke(parseExp, invokeExtractExp);
                            assignExtractExp = Expression.Assign(propExp, convertExp);
                        }
                        Expression<Func<short, string>> valExp = (val) => val.ToString();
                        InvocationExpression tosExp = InvocationExpression.Invoke(valExp, propExp);
                        invokePushExp = Expression.Invoke(push, thisExp, nodeExp, nameExp, tosExp);
                    }
                    else if (property.PropertyType.IsAssignableFrom(typeof(int)))
                    {
                        if (property.CanWrite)
                        {
                            invokeExtractExp = Expression.Invoke(extract, thisExp, nodeExp, nameExp, zeroExp);
                            Expression<Func<string, int>> parseExp = (s) => int.Parse(s);
                            InvocationExpression convertExp = InvocationExpression.Invoke(parseExp, invokeExtractExp);
                            assignExtractExp = Expression.Assign(propExp, convertExp);
                        }
                        Expression<Func<int, string>> valExp = (val) => val.ToString();
                        InvocationExpression tosExp = InvocationExpression.Invoke(valExp, propExp);
                        invokePushExp = Expression.Invoke(push, thisExp, nodeExp, nameExp, tosExp);
                    }
                    else if (property.PropertyType.IsAssignableFrom(typeof(long)))
                    {
                        if (property.CanWrite)
                        {
                            invokeExtractExp = Expression.Invoke(extract, thisExp, nodeExp, nameExp, zeroExp);
                            Expression<Func<string, long>> parseExp = (s) => long.Parse(s);
                            InvocationExpression convertExp = InvocationExpression.Invoke(parseExp, invokeExtractExp);
                            assignExtractExp = Expression.Assign(propExp, convertExp);
                        }
                        Expression<Func<long, string>> valExp = (val) => val.ToString();
                        InvocationExpression tosExp = InvocationExpression.Invoke(valExp, propExp);
                        invokePushExp = Expression.Invoke(push, thisExp, nodeExp, nameExp, tosExp);
                    }
                    else if (property.PropertyType.IsAssignableFrom(typeof(DateTime)))
                    {
                        if (property.CanWrite)
                        {
                            invokeExtractExp = Expression.Invoke(extract, thisExp, nodeExp, nameExp, dateExp);
                            Expression<Func<string, DateTime>> parseExp = (s) => DateTime.Parse(s);
                            InvocationExpression convertExp = InvocationExpression.Invoke(parseExp, invokeExtractExp);
                            assignExtractExp = Expression.Assign(propExp, convertExp);
                        }
                        Expression<Func<DateTime, string>> valExp = (val) => val.ToString("yyyy-MM-dd hh:mm:ss.fff");
                        InvocationExpression tosExp = InvocationExpression.Invoke(valExp, propExp);
                        invokePushExp = Expression.Invoke(push, thisExp, nodeExp, nameExp, tosExp);
                    }
                    else if (property.PropertyType.IsEnum)
                    {
                        Type propType = property.PropertyType;
                        ConstantExpression ptExp = Expression.Constant(propType, typeof(Type));
                        if (property.CanWrite)
                        {
                            invokeExtractExp = Expression.Invoke(extract, thisExp, nodeExp, nameExp, dateExp);
                            Expression<Func<Type, string, object>> parseExp = (tp, s) => Enum.Parse(tp, s);
                            InvocationExpression convertExp = InvocationExpression.Invoke(parseExp, ptExp, invokeExtractExp);
                            assignExtractExp = Expression.Assign(propExp, Expression.Convert(convertExp, propType));
                        }
                        Expression<Func<Type, int, string>> valExp = (typ, val) => Enum.GetName(typ, val);
                        InvocationExpression tosExp = InvocationExpression.Invoke(valExp, ptExp, Expression.Convert(propExp, typeof(int)));
                        invokePushExp = Expression.Invoke(push, thisExp, nodeExp, nameExp, tosExp);
                    }
                    else if (property.PropertyType.IsArray)
                    {
                        Type propType = property.PropertyType.GetElementType();
                        Type propRefType = (typeof(XmlSerializer<>)).MakeGenericType(propType);
                        MethodInfo method = propRefType.GetMethod("GetArrayFromNode");

                        object instance = XmlSerializerBuilder.Create(propType);
                        Expression instExp = Expression.Constant(instance, propRefType);
                        if (property.CanWrite)
                        {

                            MethodCallExpression methodCall = Expression.Call(
                                instExp,
                                method,
                                nodeExp, nameExp
                            );
                            assignExtractExp = Expression.Assign(propExp, methodCall);
                        }
                        method = propRefType.GetMethod("CreateNodeFromArray");
                        refCallExp = Expression.Call(
                            instExp,
                            method,
                            propExp, nodeExp, nameExp
                        );
                    }
                    else if (property.PropertyType.GetInterface("IEnumerable") != null)
                    {
                        if (property.PropertyType.IsGenericType)
                        {
                            Type[] ts = property.PropertyType.GetGenericArguments();
                            Type propType = ts[0];
                            Type propRefType = (typeof(XmlSerializer<>)).MakeGenericType(propType);
                            MethodInfo method = propRefType.GetMethod("GetListFromNode");

                            object instance = XmlSerializerBuilder.Create(propType);
                            Expression instExp = Expression.Constant(instance, propRefType);
                            if (property.CanWrite)
                            {
                                MethodCallExpression methodCall = Expression.Call(
                                    instExp,
                                    method,
                                    nodeExp, nameExp
                                );
                                assignExtractExp = Expression.Assign(propExp, methodCall);
                            }
                            method = propRefType.GetMethod("CreateNodeFromList");
                            refCallExp = Expression.Call(
                                instExp,
                                method,
                                propExp, nodeExp, nameExp
                            );
                        }
                    }
                    else
                    {
                        Type propType = property.PropertyType;
                        Type propRefType = (typeof(XmlSerializer<>)).MakeGenericType(propType);

                        object instance = XmlSerializerBuilder.Create(propType);
                        Expression instExp = Expression.Constant(instance, propRefType);

                        MethodInfo method = propRefType.GetMethod("GetObjectFromNode");
                        if (property.CanWrite)
                        {
                            MethodCallExpression methodCall = Expression.Call(
                                instExp,
                                method,
                                nodeExp, nameExp
                            );
                            assignExtractExp = Expression.Assign(propExp, methodCall);
                        }
                        method = propRefType.GetMethod("CreateNodeFromObject");
                        refCallExp = Expression.Call(
                            instExp,
                            method,
                            propExp, nodeExp, nameExp
                        );

                    }

                    if (assignExtractExp != null)
                    {
                        actions.Add(
                            Expression.Lambda<Action<T, XmlNode>>(
                                assignExtractExp, targetExp, nodeExp
                            ).Compile());

                    }

                    if (invokePushExp != null)
                    {
                        sets.Add(
                            Expression.Lambda<Action<T, XmlNode>>(
                                invokePushExp, targetExp, nodeExp
                            ).Compile());

                    }

                    if (refCallExp != null)
                    {
                        sets.Add(
                            Expression.Lambda<Action<T, XmlNode>>(
                                refCallExp, targetExp, nodeExp
                            ).Compile());

                    }

                    
                } // end foreach property
            }// end if property length
        }

        #endregion Initializers

        #region Deserializers

        public T Deserialize(T item, XmlNode node)
        {
            return Deserialize(item, node, typeof(T).Name);
        }

        public T Deserialize(T item, XmlNode node, string childNode)
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



        public string GetValueFromNode(XmlNode parentNode, string nodeName, string defaultValue)
        {
            XmlNode node = parentNode.SelectSingleNode(nodeName);
            if (node != null)
            {
                return node.InnerText;
            }
            else
                return defaultValue;
        }

        public T GetObjectFromNode(XmlNode parentNode, string nodeName)
        {
            T item = default(T);
            item = Deserialize(item, parentNode, nodeName);
            return item;
        }

        public List<T> GetListFromNode(XmlNode parentNode, string nodeContainer)
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
                    //items.Add(reflector.Deserialize(item, node, "."));
                    node = node.NextSibling;
                }
            }
            return items;
        }

        public T[] GetArrayFromNode(XmlNode parentNode, string nodeContainer)
        {
            return GetListFromNode(parentNode, nodeContainer).ToArray();
        }

        #endregion Deserializers

        #region Serializers

        public void Serialize(T item, XmlNode parentNode)
        {
            Serialize(item, parentNode, typeof(T).Name);
        }

        public static void Serialize(T item, XmlNode parentNode, string nodeName)
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

        public void CreateNodeFromValue(XmlNode parentNode, string nodeName, string value)
        {
            XmlNode node = parentNode.OwnerDocument.CreateElement(nodeName);
            node.InnerText = value;
            parentNode.AppendChild(node);
        }

        public void CreateNodeFromObject(T item, XmlNode parentNode, string nodeName)
        {
            if (item != null)
            {
                Serialize(item, parentNode, nodeName);
            }
        }

        public void CreateNodeFromList(IEnumerable<T> items, XmlNode parentNode, string nodeContainer)
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


        public void CreateNodeFromArray(T[] items, XmlNode parentNode, string nodeContainer)
        {
            CreateNodeFromList(items, parentNode, nodeContainer);
        }

        #endregion Serializers

        #region ISerializer

        public object Deserialize(object node)
        {
            return Deserialize( default(T), (XmlNode)node);
        }

        public object Deserialize(object node, string childNode)
        {
            return Deserialize(default(T), (XmlNode)node, childNode);
        }

        public void Serialize(object item, object node)
        {
            Serialize((T)item, (XmlNode)node);
        }

        public void Serialize(object item, object node, string childNode)
        {
            Serialize((T)item, (XmlNode)node, childNode);
        }

        #endregion ISerializer
    }
}
