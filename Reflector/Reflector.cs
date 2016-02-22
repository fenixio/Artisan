using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Artisan.Tools.Reflector
{
    public abstract class Reflector<T, TSource> : ISerializer
    {
        protected List<Action<T, TSource>> actions;
        protected List<Action<T, TSource>> sets;
        protected Type type;
        protected ISerializerBuilder factory;

        #region Initializers
        public Reflector(ISerializerBuilder builder)
        {
            actions = new List<Action<T, TSource>>();
            sets    = new List<Action<T, TSource>>();
            type    = typeof(T);
            factory = builder;
            GetExpressions();
        }

        private void GetExpressions()
        {
            // first parameter of each expression is the target object
            ParameterExpression targetExp   = Expression.Parameter(typeof(T), "target");
            ParameterExpression sourceExp   = Expression.Parameter(typeof(TSource), "nodeExp");
            ConstantExpression thisExp      = Expression.Constant(this);

            PropertyInfo[] properties       = type.GetProperties();
            if (properties.Length > 0)
            {
                foreach (PropertyInfo property in properties)
                {
                    // instance.Property 
                    MemberExpression propExp              = Expression.Property(targetExp, property);
                    ConstantExpression nameExp            = Expression.Constant(property.Name, typeof(string));
                    InvocationExpression invokeExtractExp = null;
                    InvocationExpression invokePushExp    = null;
                    BinaryExpression assignExtractExp     = null;
                    MethodCallExpression refCallExp       = null;

                    if (property.PropertyType.IsAssignableFrom(typeof(string)))
                    {
                        if (property.CanWrite)
                        {
                            invokeExtractExp = Expression.Invoke( (Expression<Func<Reflector<T, TSource>, TSource, string, string>>)((reflexTyp, nod, nam) => reflexTyp.ParseString(nod, nam)), thisExp, sourceExp, nameExp);
                            assignExtractExp = Expression.Assign(propExp, invokeExtractExp);
                        }

                        invokePushExp = Expression.Invoke( (Expression<Action<Reflector<T, TSource>, TSource, string, string>>)((reflexTyp, nod, nam, val) => reflexTyp.AddString(nod, nam, val)), thisExp, sourceExp, nameExp, propExp);
                    }
                    else if (property.PropertyType.IsAssignableFrom(typeof(bool)))
                    {
                        if (property.CanWrite)
                        {
                            invokeExtractExp = Expression.Invoke((Expression<Func<Reflector<T, TSource>, TSource, string, bool>>)((reflexTyp, nod, nam) => reflexTyp.ParseBool(nod, nam)), thisExp, sourceExp, nameExp);
                            assignExtractExp = Expression.Assign(propExp, invokeExtractExp);
                        }
                        invokePushExp = Expression.Invoke((Expression<Action<Reflector<T, TSource>, TSource, string, bool>>)((reflexTyp, nod, nam, val) => reflexTyp.AddBool(nod, nam, val)), thisExp, sourceExp, nameExp, propExp);
                    }
                    else if (property.PropertyType.IsAssignableFrom(typeof(byte)))
                    {
                        if (property.CanWrite)
                        {
                            invokeExtractExp = Expression.Invoke((Expression<Func<Reflector<T, TSource>, TSource, string, byte>>)((reflexTyp, nod, nam) => reflexTyp.ParseByte(nod, nam)), thisExp, sourceExp, nameExp);
                            assignExtractExp = Expression.Assign(propExp, invokeExtractExp);
                        }
                        invokePushExp = Expression.Invoke((Expression<Action<Reflector<T, TSource>, TSource, string, byte>>)((reflexTyp, nod, nam, val) => reflexTyp.AddByte(nod, nam, val)), thisExp, sourceExp, nameExp, propExp);
                    }
                    else if (property.PropertyType.IsAssignableFrom(typeof(short)))
                    {
                        if (property.CanWrite)
                        {
                            invokeExtractExp = Expression.Invoke((Expression<Func<Reflector<T, TSource>, TSource, string, short>>)((reflexTyp, nod, nam) => reflexTyp.ParseShort(nod, nam)), thisExp, sourceExp, nameExp);
                            assignExtractExp = Expression.Assign(propExp, invokeExtractExp);
                        }
                        invokePushExp = Expression.Invoke((Expression<Action<Reflector<T, TSource>, TSource, string, short>>)((reflexTyp, nod, nam, val) => reflexTyp.AddShort(nod, nam, val)), thisExp, sourceExp, nameExp, propExp);
                    }
                    else if (property.PropertyType.IsAssignableFrom(typeof(int)))
                    {
                        if (property.CanWrite)
                        {
                            invokeExtractExp = Expression.Invoke((Expression<Func<Reflector<T, TSource>, TSource, string, int>>)((reflexTyp, nod, nam) => reflexTyp.ParseInt(nod, nam)), thisExp, sourceExp, nameExp);
                            assignExtractExp = Expression.Assign(propExp, invokeExtractExp);
                        }
                        invokePushExp = Expression.Invoke((Expression<Action<Reflector<T, TSource>, TSource, string, int>>)((reflexTyp, nod, nam, val) => reflexTyp.AddInt(nod, nam, val)), thisExp, sourceExp, nameExp, propExp);
                    }
                    else if (property.PropertyType.IsAssignableFrom(typeof(long)))
                    {
                        if (property.CanWrite)
                        {
                            invokeExtractExp = Expression.Invoke((Expression<Func<Reflector<T, TSource>, TSource, string, long>>)((reflexTyp, nod, nam) => reflexTyp.ParseLong(nod, nam)), thisExp, sourceExp, nameExp);
                            assignExtractExp = Expression.Assign(propExp, invokeExtractExp);
                        }
                        invokePushExp = Expression.Invoke((Expression<Action<Reflector<T, TSource>, TSource, string, long>>)((reflexTyp, nod, nam, val) => reflexTyp.AddLong(nod, nam, val)), thisExp, sourceExp, nameExp, propExp);
                    }
                    else if (property.PropertyType.IsAssignableFrom(typeof(double)))
                    {
                        if (property.CanWrite)
                        {
                            invokeExtractExp = Expression.Invoke((Expression<Func<Reflector<T, TSource>, TSource, string, double>>)((reflexTyp, nod, nam) => reflexTyp.ParseDouble(nod, nam)), thisExp, sourceExp, nameExp);
                            assignExtractExp = Expression.Assign(propExp, invokeExtractExp);
                        }
                        invokePushExp = Expression.Invoke((Expression<Action<Reflector<T, TSource>, TSource, string, double>>)((reflexTyp, nod, nam, val) => reflexTyp.AddDouble(nod, nam, val)), thisExp, sourceExp, nameExp, propExp);
                    }
                    else if (property.PropertyType.IsAssignableFrom(typeof(decimal)))
                    {
                        if (property.CanWrite)
                        {
                            invokeExtractExp = Expression.Invoke((Expression<Func<Reflector<T, TSource>, TSource, string, decimal>>)((reflexTyp, nod, nam) => reflexTyp.ParseDecimal(nod, nam)), thisExp, sourceExp, nameExp);
                            assignExtractExp = Expression.Assign(propExp, invokeExtractExp);
                        }
                        invokePushExp = Expression.Invoke((Expression<Action<Reflector<T, TSource>, TSource, string, decimal>>)((reflexTyp, nod, nam, val) => reflexTyp.AddDecimal(nod, nam, val)), thisExp, sourceExp, nameExp, propExp);
                    }
                    else if (property.PropertyType.IsAssignableFrom(typeof(DateTime)))
                    {
                        if (property.CanWrite)
                        {
                            invokeExtractExp = Expression.Invoke((Expression<Func<Reflector<T, TSource>, TSource, string, DateTime>>)((reflexTyp, nod, nam) => reflexTyp.ParseDateTime(nod, nam)), thisExp, sourceExp, nameExp);
                            assignExtractExp = Expression.Assign(propExp, invokeExtractExp);
                        }
                        invokePushExp = Expression.Invoke((Expression<Action<Reflector<T, TSource>, TSource, string, DateTime>>)((reflexTyp, nod, nam, val) => reflexTyp.AddDateTime(nod, nam, val)), thisExp, sourceExp, nameExp, propExp);
                    }
                    else if (property.PropertyType.IsAssignableFrom(typeof(Guid)))
                    {
                        if (property.CanWrite)
                        {
                            invokeExtractExp = Expression.Invoke((Expression<Func<Reflector<T, TSource>, TSource, string, Guid>>)((reflexTyp, nod, nam) => reflexTyp.ParseGuid(nod, nam)), thisExp, sourceExp, nameExp);
                            assignExtractExp = Expression.Assign(propExp, invokeExtractExp);
                        }
                        invokePushExp = Expression.Invoke((Expression<Action<Reflector<T, TSource>, TSource, string, Guid>>)((reflexTyp, nod, nam, val) => reflexTyp.AddGuid(nod, nam, val)), thisExp, sourceExp, nameExp, propExp);
                    }
                    
                    else if (property.PropertyType.IsEnum)
                    {
                        Type propType = property.PropertyType;
                        ConstantExpression ptExp = Expression.Constant(propType, typeof(Type));
                        if (property.CanWrite)
                        {
                            invokeExtractExp = Expression.Invoke((Expression<Func<Reflector<T, TSource>, TSource, string, Type, object>>)((reflexTyp, nod, nam, prTyp) => reflexTyp.ParseEnum(nod, nam, prTyp)), thisExp, sourceExp, nameExp, ptExp);
                            assignExtractExp = Expression.Assign(propExp, Expression.Convert(invokeExtractExp, propType));
                        }
                        invokePushExp = Expression.Invoke((Expression<Action<Reflector<T, TSource>, TSource, string, int, Type>>)((reflexTyp, nod, nam, val, prTyp) => reflexTyp.AddEnum(nod, nam, val, prTyp)), thisExp, sourceExp, nameExp, Expression.Convert(propExp, typeof(int)), ptExp);
                    }
                    
                    else if (property.PropertyType.IsArray)
                    {
                        Type propType      = property.PropertyType.GetElementType();
                        ISerializer instance = factory.Create(propType);
                        Type propRefType   = instance.GetType();
                        MethodInfo method  = null;
                        Expression instExp = Expression.Constant(instance, propRefType);
                        if (property.CanWrite)
                        {
                            method = propRefType.GetMethod("ParseArray");
                            MethodCallExpression methodCall = Expression.Call(
                                instExp,
                                method,
                                sourceExp, nameExp
                            );
                            assignExtractExp = Expression.Assign(propExp, methodCall);
                        }
                        method = propRefType.GetMethod("AddArray");
                        refCallExp = Expression.Call(
                            instExp,
                            method,
                            propExp, sourceExp, nameExp
                        );
                    }
                    else if (property.PropertyType.GetInterface("IEnumerable") != null)
                    {
                        if (property.PropertyType.IsGenericType)
                        {
                            Type[] ts            = property.PropertyType.GetGenericArguments();
                            Type propType        = ts[0];
                            ISerializer instance = factory.Create(propType);
                            Type propRefType     = instance.GetType();
                            MethodInfo method    = null;

                            Expression instExp   = Expression.Constant(instance, propRefType);
                            if (property.CanWrite)
                            {
                                method = propRefType.GetMethod("ParseList");
                                MethodCallExpression methodCall = Expression.Call(
                                    instExp,
                                    method,
                                    sourceExp, nameExp
                                );
                                assignExtractExp = Expression.Assign(propExp, methodCall);
                            }

                            method = propRefType.GetMethod("AddList");
                            refCallExp = Expression.Call(
                                instExp,
                                method,
                                propExp, sourceExp, nameExp
                            );
                        }
                    }
                    else
                    {
                        Type propType = property.PropertyType;
                        ISerializer instance = factory.Create(propType);
                        Type propRefType     = instance.GetType();
                        MethodInfo method    = null;
                        
                        Expression instExp = Expression.Constant(instance, propRefType);

                        
                        if (property.CanWrite)
                        {
                            method = propRefType.GetMethod("ParseObject");
                            MethodCallExpression methodCall = Expression.Call(
                                instExp,
                                method,
                                sourceExp, nameExp
                            );
                            assignExtractExp = Expression.Assign(propExp, methodCall);
                        }
                        method = propRefType.GetMethod("AddObject");
                        refCallExp = Expression.Call(
                            instExp,
                            method,
                            propExp, sourceExp, nameExp
                        );

                    }

                    if (assignExtractExp != null)
                    {
                        actions.Add(
                            Expression.Lambda<Action<T, TSource>>(
                                assignExtractExp, targetExp, sourceExp
                            ).Compile());

                    }

                    if (invokePushExp != null)
                    {
                        sets.Add(
                            Expression.Lambda<Action<T, TSource>>(
                                invokePushExp, targetExp, sourceExp
                            ).Compile());

                    }

                    if (refCallExp != null)
                    {
                        sets.Add(
                            Expression.Lambda<Action<T, TSource>>(
                                refCallExp, targetExp, sourceExp
                            ).Compile());

                    }


                } // end foreach property
            }// end if property length
        }

        #endregion Initializers

        #region Getters

        public abstract string ParseValue(TSource parentNode, string nodeName, string defaultValue);

        public virtual string ParseString(TSource parentNode, string nodeName)
        {
            return ParseValue(parentNode, nodeName, "");
        }

        public virtual bool ParseBool(TSource parentNode, string nodeName)
        {
            return Convert.ToBoolean(ParseValue(parentNode, nodeName, "false"));
        }

        public virtual byte ParseByte(TSource parentNode, string nodeName)
        {
            return Convert.ToByte(ParseValue(parentNode, nodeName, "0"));
        }

        public virtual short ParseShort(TSource parentNode, string nodeName)
        {
            return Convert.ToInt16(ParseValue(parentNode, nodeName, "0"));
        }

        public virtual int ParseInt(TSource parentNode, string nodeName)
        {
            return Convert.ToInt32(ParseValue(parentNode, nodeName, "0"));
        }

        public virtual long ParseLong(TSource parentNode, string nodeName)
        {
            return Convert.ToInt64(ParseValue(parentNode, nodeName, "0"));
        }

        public virtual double ParseDouble(TSource parentNode, string nodeName)
        {
            return Convert.ToDouble(ParseValue(parentNode, nodeName, "0"));
        }

        public virtual decimal ParseDecimal(TSource parentNode, string nodeName)
        {
            return Convert.ToDecimal(ParseValue(parentNode, nodeName, "0"));
        }

        public virtual DateTime ParseDateTime(TSource parentNode, string nodeName)
        {
            return Convert.ToDateTime(ParseValue(parentNode, nodeName, "01/01/01"));
        }

        public virtual Guid ParseGuid(TSource parentNode, string nodeName)
        {
            return Guid.Parse(ParseValue(parentNode, nodeName, Guid.Empty.ToString()));
        }

        public virtual object ParseEnum(TSource parentNode, string nodeName, Type enumType)
        {
            return Enum.Parse(enumType, ParseValue(parentNode, nodeName, ""));
        }


        #endregion Getters

        #region Setters

        public abstract void AddValue(TSource parentNode, string nodeName, string value);

        public virtual void AddString(TSource parentNode, string nodeName, string value)
        {
            AddValue(parentNode, nodeName, value);
        }

        public virtual void AddBool(TSource parentNode, string nodeName, bool value)
        {
            AddValue(parentNode, nodeName, value.ToString());
        }

        public virtual void AddByte(TSource parentNode, string nodeName, byte value)
        {
            AddValue(parentNode, nodeName, value.ToString());
        }

        public virtual void AddShort(TSource parentNode, string nodeName, short value)
        {
            AddValue(parentNode, nodeName, value.ToString());
        }

        public virtual void AddInt(TSource parentNode, string nodeName, int value)
        {
            AddValue(parentNode, nodeName, value.ToString());
        }

        public virtual void AddLong(TSource parentNode, string nodeName, long value)
        {
            AddValue(parentNode, nodeName, value.ToString());
        }

        public virtual void AddDouble(TSource parentNode, string nodeName, double value)
        {
            AddValue(parentNode, nodeName, value.ToString("0.000"));
        }

        public virtual void AddDecimal(TSource parentNode, string nodeName, decimal value)
        {
            AddValue(parentNode, nodeName, value.ToString("0.000"));
        }

        public virtual void AddDateTime(TSource parentNode, string nodeName, DateTime value)
        {
            AddValue(parentNode, nodeName, value.ToString("yyyy-MM-dd hh:mm:ss.fff"));
        }

        public virtual void AddGuid(TSource parentNode, string nodeName, Guid value)
        {
            AddValue(parentNode, nodeName, value.ToString());
        }

        public virtual void AddEnum(TSource parentNode, string nodeName, int value, Type enumType)
        {
            AddValue(parentNode, nodeName, Enum.GetName(enumType, value));
        }

        #endregion Setters

        #region Deserializers

        public virtual T Deserialize(T item, TSource node)
        {
            return Deserialize( item, node, typeof(T).Name);
        }

        public abstract T Deserialize(T item, TSource node, string childNode);

        public abstract List<T> ParseList(TSource parentNode, string nodeContainer);

        public abstract T ParseObject(TSource parentNode, string nodeName);

        public abstract T[] ParseArray(TSource parentNode, string nodeContainer);

        #endregion Deserializers

        #region Serializers

        public virtual void Serialize(T item, TSource parentNode)
        {
            Serialize(item, parentNode, typeof(T).Name);
        }

        public abstract void Serialize(T item, TSource parentNode, string nodeName);

        public abstract void AddList(IEnumerable<T> items, TSource parentNode, string nodeContainer);

        public abstract void AddObject(T item, TSource parentNode, string nodeName);

        public abstract void AddArray(T[] items, TSource parentNode, string nodeContainer);

        #endregion Serializers

        #region ISerializer

        public virtual object Deserialize(object node)
        {
            return Deserialize(default(T), (TSource)node);
        }

        public virtual object Deserialize(object node, string childNode)
        {
            return Deserialize(default(T), (TSource)node, childNode);
        }

        public virtual void Serialize(object item, object node)
        {
            Serialize((T)item, (TSource)node);
        }

        public virtual void Serialize(object item, object node, string childNode)
        {
            Serialize((T)item, (TSource)node, childNode);
        }

        #endregion ISerializer
    }
}
