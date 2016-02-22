using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Artisan.Tools.Store
{
    public class Copier<T> where T : StorableObject, new()
    {
        protected List<Action<T, T>> sets;
        protected Type type;

        #region Initializers
        public Copier()
        {
            sets = new List<Action<T, T>>();
            type = typeof(T);
            GetExpressions();
        }

        private void GetExpressions()
        {
            // first parameter of each expression is the target object
            ParameterExpression targetExp = Expression.Parameter(typeof(T), "target");
            ParameterExpression sourceExp = Expression.Parameter(typeof(T), "source");
            ConstantExpression thisExp = Expression.Constant(this);

            PropertyInfo[] properties = type.GetProperties();
            if (properties.Length > 0)
            {
                foreach (PropertyInfo property in properties)
                {

                    if (property.CanWrite)
                    {
                        // instance.Property 
                        MemberExpression propTarExp = Expression.Property(targetExp, property);
                        MemberExpression propSrcExp = Expression.Property(sourceExp, property);
                        BinaryExpression assignExtractExp = null;
                        
                        if (property.PropertyType.IsArray)
                        {
                            Type propType = property.PropertyType.GetElementType();
                            Type propRefType = (typeof(Copier<>)).MakeGenericType(propType);
                            object instance = Activator.CreateInstance(propRefType);
                            //Type propRefType   = instance.GetType();
                            MethodInfo method = null;
                            Expression instExp = Expression.Constant(instance, propRefType);

                            method = propRefType.GetMethod("CopyArray");
                            MethodCallExpression methodCall = Expression.Call(
                                instExp,
                                method,
                                propTarExp, propSrcExp
                            );

                            sets.Add(
                                Expression.Lambda<Action<T, T>>(
                                    methodCall, targetExp, sourceExp
                            ).Compile());
                        }
                        else if (property.PropertyType.GetInterface("IEnumerable") != null)
                        {
                            if (property.PropertyType.IsGenericType)
                            {
                                Type[] ts = property.PropertyType.GetGenericArguments();
                                Type propType = ts[0];
                                Type propRefType = (typeof(Copier<>)).MakeGenericType(propType);
                                object instance = Activator.CreateInstance(propRefType);
                                //ISerializer instance = factory.Create(propType);
                                //Type propRefType     = instance.GetType();
                                MethodInfo method = null;

                                Expression instExp = Expression.Constant(instance, propRefType);
                                method = propRefType.GetMethod("CopyList");
                                MethodCallExpression methodCall = Expression.Call(
                                    instExp,
                                    method,
                                    propTarExp, propSrcExp
                                );

                                sets.Add(
                                    Expression.Lambda<Action<T, T>>(
                                        methodCall, targetExp, sourceExp
                                ).Compile());
                            }
                        }
                        if (property.PropertyType.IsSubclassOf(typeof(StorableObject)))
                        {
                            Type propType    = property.PropertyType;
                            Type propRefType = (typeof(Copier<>)).MakeGenericType(propType);
                            object instance  = Activator.CreateInstance(propRefType);
                            //ISerializer instance = factory.Create(propType);
                            //Type propRefType     = instance.GetType();
                            MethodInfo method = null;

                            Expression instExp = Expression.Constant(instance, propRefType);
                            method             = propRefType.GetMethod("Copy", new Type[] {propType, propType});
                            MethodCallExpression methodCall = Expression.Call(
                                instExp,
                                method,
                                propTarExp, propSrcExp
                            );

                            sets.Add(
                                Expression.Lambda<Action<T, T>>(
                                    methodCall, targetExp, sourceExp
                            ).Compile());
                        }
                        else
                        {
                            assignExtractExp = Expression.Assign(propTarExp, propSrcExp);

                            sets.Add(
                                Expression.Lambda<Action<T, T>>(
                                    assignExtractExp, targetExp, sourceExp
                                ).Compile());

                        }

                    }

                } // end foreach property
            }// end if property length
        }

        #endregion Initializers

        public T Copy(T source)
        {
            return Copy( null, source);
        }

        public T Copy(T target, T source )
        {
            if (target == null)
                target = new T();

            sets.ForEach(
                     e => e(target, source)
                );

            return target;
        }

        public void CopyList(List<T> targets, List<T> sources)
        {
            if (sources != null)
            {
                List<T> srcLst = sources.ToList();

                if (targets == null)
                {
                    targets = new List<T>();
                }
                for (int i= targets.Count-1; i>=0; i--) 
                {
                    int srcIndex = srcLst.FindIndex(e => e.Id == targets[i].Id);
                    if (srcIndex >= 0)
                    {
                        Copy(srcLst[srcIndex], targets[i]);
                        srcLst.RemoveAt(srcIndex);
                    }
                    else
                    {
                        targets.RemoveAt(i);
                    }
                }

                foreach( var src in srcLst)
                {
                    targets.Add(Copy(src));
                }
            }
        }

        public void CopyArray(T[] targets, T[] sources)
        {
            if (sources != null)
            {
                List<T> srcLst = sources.ToList();

                List<T> trgLst = targets.ToList();
                if (targets == null)
                {
                    trgLst = new List<T>();
                }
                else
                    trgLst = targets.ToList();

                for (int i = trgLst.Count-1; i >= 0; i--)
                {
                    int srcIndex = srcLst.FindIndex(e => e.Id == targets[i].Id);
                    if (srcIndex >= 0)
                    {
                        Copy(srcLst[srcIndex], targets[i]);
                        srcLst.RemoveAt(srcIndex);
                    }
                    else
                    {
                        trgLst.RemoveAt(i);
                    }
                }

                foreach (var src in srcLst)
                {
                    trgLst.Add(Copy(src));
                }

                targets = trgLst.ToArray();
            }
        }

    }
}
