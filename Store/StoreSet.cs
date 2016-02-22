using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Artisan.Tools.Store
{
    public class StoreSet<T> : ISet<T> where T: StorableObject, new()
    {
        protected Type elementType;
        protected Type keyType;
        protected Type contextType;

        protected virtual DbContext CreateDb()
        {
            return (DbContext)Activator.CreateInstance(contextType);
        }

        public StoreSet( Type contextType ) 
        {
            this.contextType = contextType;
            elementType = typeof(T);
        }

        public int Count
        {
            get
            {
                using (var db = CreateDb())
                {
                    return db.Set<T>().Count();
                }
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool Add(T item)
        {
            bool retVal = false;
            if (item != null)
            {
                using (var db = CreateDb())
                {
                    
                    T o = db.Set<T>().FirstOrDefault(s => ((StorableObject)s).Id == (((StorableObject)item).Id));
                    if (o == default(T))
                    {
                        db.Set<T>().Add(item);
                        retVal = true;
                    }
                    else
                    {
                        new Copier<T>().Copy(item, o);
                    }
                    db.SaveChanges();
                }
            }
            return retVal;
        }

        public void Clear()
        {
            using (var db = CreateDb())
            {

            }
        }

        public bool Contains(T item)
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().Contains(item);
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            using (var db = CreateDb())
            {
                db.Set<T>().Except(other);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            using (var db = CreateDb())
            {
                db.Set<T>().Remove(item);
                db.SaveChanges();
            }
            return true;
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public void UnionWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        void ICollection<T>.Add(T item)
        {
            using (var db = CreateDb())
            {
                db.Set<T>().Remove(item);
                db.SaveChanges();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool All(Func<T, bool> predicate)
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().Count(predicate) == db.Set<T>().Count();

            }
        }

        public bool Any()
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().Count()>0;

            }
        }
        public bool Any( Func<T, bool> predicate)
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().Count(predicate) > 0;

            }
        }
        public IEnumerable<T> AsEnumerable()
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().AsEnumerable();

            }
        }

        public double? Average(Func<T, double> selector)
        {
            double? val = null;
            using (var db = CreateDb())
            {
                val = new double?( db.Set<T>().Average(selector));
            }
            return val;
        }
        //
        // Summary:
        //     Computes the average of a sequence of nullable System.Decimal values that are
        //     obtained by invoking a transform function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values to calculate the average of.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        // Returns:
        //     The average of the sequence of values, or null if the source sequence is empty
        //     or contains only values that are null.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        //
        //   T:System.OverflowException:
        //     The sum of the elements in the sequence is larger than System.Decimal.MaxValue.
        public decimal? Average(Expression<Func<T, decimal?>> selector)
        {
            decimal? val = null;
            using (var db = CreateDb())
            {
                val = db.Set<T>().Average<T>(selector);
            }
            return val;
        }



        //
        // Summary:
        //     Returns distinct elements from a sequence by using the default equality comparer
        //     to compare values.
        //
        // Parameters:
        //   source:
        //     The sequence to remove duplicate elements from.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 that contains distinct elements from
        //     the source sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.
        public IEnumerable<T> Distinct()
        {
            using (var db = CreateDb())
            {
                var q = db.Set<T>().Distinct();
                return q.AsEnumerable();
            }
        }
        //
        // Summary:
        //     Returns distinct elements from a sequence by using a specified System.Collections.Generic.IEqualityComparer`1
        //     to compare values.
        //
        // Parameters:
        //   source:
        //     The sequence to remove duplicate elements from.
        //
        //   comparer:
        //     An System.Collections.Generic.IEqualityComparer`1 to compare values.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 that contains distinct elements from
        //     the source sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.
        public IEnumerable<T> Distinct(IEqualityComparer<T> comparer)
        {
            using (var db = CreateDb())
            {
                var q = db.Set<T>().Distinct(comparer);
                return q.AsEnumerable();
            }
        }


        //
        // Summary:
        //     Returns the first element of a sequence.
        //
        // Parameters:
        //   source:
        //     The System.Collections.Generic.IEnumerable`1 to return the first element of.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        // Returns:
        //     The first element in the specified sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.
        //
        //   T:System.InvalidOperationException:
        //     The source sequence is empty.
        public T First()
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().First();
            }
        }
        //
        // Summary:
        //     Returns the first element in a sequence that satisfies a specified condition.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable`1 to return an element from.
        //
        //   predicate:
        //     A function to test each element for a condition.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        // Returns:
        //     The first element in the sequence that passes the test in the specified predicate
        //     function.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or predicate is null.
        //
        //   T:System.InvalidOperationException:
        //     No element satisfies the condition in predicate.-or-The source sequence is empty.
        public T First(Func<T, bool> predicate)
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().First(predicate);
            }
        }
        //
        // Summary:
        //     Returns the first element of a sequence, or a default value if the sequence contains
        //     no elements.
        //
        // Parameters:
        //   source:
        //     The System.Collections.Generic.IEnumerable`1 to return the first element of.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        // Returns:
        //     default(T) if source is empty; otherwise, the first element in source.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.
        public T FirstOrDefault()
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().FirstOrDefault();
            }
        }
        //
        // Summary:
        //     Returns the first element of the sequence that satisfies a condition or a default
        //     value if no such element is found.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable`1 to return an element from.
        //
        //   predicate:
        //     A function to test each element for a condition.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        // Returns:
        //     default(T) if source is empty or if no element passes the test specified
        //     by predicate; otherwise, the first element in source that passes the test specified
        //     by predicate.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or predicate is null.
        public T FirstOrDefault(Func<T, bool> predicate)
        {
            T t = default(T);

            using (var db = CreateDb())
            {
                //db.Configuration.LazyLoadingEnabled = false;
                t = db.Set<T>().FirstOrDefault(predicate);

            }
            return t;

        }

        public T FirstOrDefault(Func<T, bool> predicate, string related)
        {
            T t = default(T);

            using (var db = CreateDb())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                t = db.Set<T>().Include(related).FirstOrDefault(predicate);

                //var data = db.Set<T>().Include(related).Where(predicate);
            }
            return t;

        }


        //
        // Summary:
        //     Groups the elements of a sequence according to a specified key selector function.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable`1 whose elements to group.
        //
        //   keySelector:
        //     A function to extract the key for each element.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        //   TKey:
        //     The type of the key returned by keySelector.
        //
        // Returns:
        //     An IEnumerable<IGrouping<TKey, T>> in C# or IEnumerable(Of IGrouping(Of
        //     TKey, T)) in Visual Basic where each System.Linq.IGrouping`2 object contains
        //     a sequence of objects and a key.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or keySelector is null.
        public IEnumerable<IGrouping<TKey, T>> GroupBy<TKey>(Func<T, TKey> keySelector)
        {
            using (var db = CreateDb())
            {
                var q = db.Set<T>().GroupBy<T,TKey>(keySelector);

                return q.ToList();
            }
        }
        //
        // Summary:
        //     Groups the elements of a sequence according to a specified key selector function
        //     and compares the keys by using a specified comparer.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable`1 whose elements to group.
        //
        //   keySelector:
        //     A function to extract the key for each element.
        //
        //   comparer:
        //     An System.Collections.Generic.IEqualityComparer`1 to compare keys.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        //   TKey:
        //     The type of the key returned by keySelector.
        //
        // Returns:
        //     An IEnumerable<IGrouping<TKey, T>> in C# or IEnumerable(Of IGrouping(Of
        //     TKey, T)) in Visual Basic where each System.Linq.IGrouping`2 object contains
        //     a collection of objects and a key.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or keySelector is null.
        public IEnumerable<IGrouping<TKey, T>> GroupBy<TKey>(Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            using (var db = CreateDb())
            {
                var q = db.Set<T>().GroupBy<T, TKey>(keySelector, comparer);

                return q.ToList();
            }
        }
        //
        // Summary:
        //     Groups the elements of a sequence according to a specified key selector function
        //     and creates a result value from each group and its key.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable`1 whose elements to group.
        //
        //   keySelector:
        //     A function to extract the key for each element.
        //
        //   resultSelector:
        //     A function to create a result value from each group.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        //   TKey:
        //     The type of the key returned by keySelector.
        //
        //   TResult:
        //     The type of the result value returned by resultSelector.
        //
        // Returns:
        //     A collection of elements of type TResult where each element represents a projection
        //     over a group and its key.
        public IEnumerable<TResult> GroupBy<TKey, TResult>(Func<T, TKey> keySelector, Func<TKey, IEnumerable<T>, TResult> resultSelector)
        {
            using (var db = CreateDb())
            {
                var q = db.Set<T>().GroupBy<T, TKey, TResult>(keySelector, resultSelector);

                return q.ToList();
            }
        }
        //
        // Summary:
        //     Groups the elements of a sequence according to a specified key selector function
        //     and projects the elements for each group by using a specified function.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable`1 whose elements to group.
        //
        //   keySelector:
        //     A function to extract the key for each element.
        //
        //   elementSelector:
        //     A function to map each source element to an element in the System.Linq.IGrouping`2.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        //   TKey:
        //     The type of the key returned by keySelector.
        //
        //   TElement:
        //     The type of the elements in the System.Linq.IGrouping`2.
        //
        // Returns:
        //     An IEnumerable<IGrouping<TKey, TElement>> in C# or IEnumerable(Of IGrouping(Of
        //     TKey, TElement)) in Visual Basic where each System.Linq.IGrouping`2 object contains
        //     a collection of objects of type TElement and a key.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or keySelector or elementSelector is null.
        public IEnumerable<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Func<T, TKey> keySelector, Func<T, TElement> elementSelector)
        {
            using (var db = CreateDb())
            {
                var q = db.Set<T>().GroupBy<T, TKey, TElement>(keySelector, elementSelector);

                return q.ToList();
            }
        }
        //
        // Summary:
        //     Groups the elements of a sequence according to a key selector function. The keys
        //     are compared by using a comparer and each group's elements are projected by using
        //     a specified function.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable`1 whose elements to group.
        //
        //   keySelector:
        //     A function to extract the key for each element.
        //
        //   elementSelector:
        //     A function to map each source element to an element in an System.Linq.IGrouping`2.
        //
        //   comparer:
        //     An System.Collections.Generic.IEqualityComparer`1 to compare keys.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        //   TKey:
        //     The type of the key returned by keySelector.
        //
        //   TElement:
        //     The type of the elements in the System.Linq.IGrouping`2.
        //
        // Returns:
        //     An IEnumerable<IGrouping<TKey, TElement>> in C# or IEnumerable(Of IGrouping(Of
        //     TKey, TElement)) in Visual Basic where each System.Linq.IGrouping`2 object contains
        //     a collection of objects of type TElement and a key.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or keySelector or elementSelector is null.
        public IEnumerable<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Func<T, TKey> keySelector, Func<T, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            using (var db = CreateDb())
            {
                var q = db.Set<T>().GroupBy<T, TKey, TElement>(keySelector, elementSelector, comparer);

                return q.ToList();
            }
        }
        //
        // Summary:
        //     Groups the elements of a sequence according to a specified key selector function
        //     and creates a result value from each group and its key. The keys are compared
        //     by using a specified comparer.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable`1 whose elements to group.
        //
        //   keySelector:
        //     A function to extract the key for each element.
        //
        //   resultSelector:
        //     A function to create a result value from each group.
        //
        //   comparer:
        //     An System.Collections.Generic.IEqualityComparer`1 to compare keys with.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        //   TKey:
        //     The type of the key returned by keySelector.
        //
        //   TResult:
        //     The type of the result value returned by resultSelector.
        //
        // Returns:
        //     A collection of elements of type TResult where each element represents a projection
        //     over a group and its key.
        public IEnumerable<TResult> GroupBy<TKey, TResult>( Func<T, TKey> keySelector, Func<TKey, IEnumerable<T>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            using (var db = CreateDb())
            {
                var q = db.Set<T>().GroupBy<T, TKey, TResult>(keySelector, resultSelector, comparer);

                return q.ToList();
            }
        }
        //
        // Summary:
        //     Groups the elements of a sequence according to a specified key selector function
        //     and creates a result value from each group and its key. The elements of each
        //     group are projected by using a specified function.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable`1 whose elements to group.
        //
        //   keySelector:
        //     A function to extract the key for each element.
        //
        //   elementSelector:
        //     A function to map each source element to an element in an System.Linq.IGrouping`2.
        //
        //   resultSelector:
        //     A function to create a result value from each group.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        //   TKey:
        //     The type of the key returned by keySelector.
        //
        //   TElement:
        //     The type of the elements in each System.Linq.IGrouping`2.
        //
        //   TResult:
        //     The type of the result value returned by resultSelector.
        //
        // Returns:
        //     A collection of elements of type TResult where each element represents a projection
        //     over a group and its key.
        public IEnumerable<TResult> GroupBy<TKey, TElement, TResult>( Func<T, TKey> keySelector, Func<T, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
        {
            using (var db = CreateDb())
            {
                var q = db.Set<T>().GroupBy<T, TKey, TElement, TResult>(keySelector, elementSelector, resultSelector);

                return q.ToList();
            }
        }
        //
        // Summary:
        //     Groups the elements of a sequence according to a specified key selector function
        //     and creates a result value from each group and its key. Key values are compared
        //     by using a specified comparer, and the elements of each group are projected by
        //     using a specified function.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable`1 whose elements to group.
        //
        //   keySelector:
        //     A function to extract the key for each element.
        //
        //   elementSelector:
        //     A function to map each source element to an element in an System.Linq.IGrouping`2.
        //
        //   resultSelector:
        //     A function to create a result value from each group.
        //
        //   comparer:
        //     An System.Collections.Generic.IEqualityComparer`1 to compare keys with.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        //   TKey:
        //     The type of the key returned by keySelector.
        //
        //   TElement:
        //     The type of the elements in each System.Linq.IGrouping`2.
        //
        //   TResult:
        //     The type of the result value returned by resultSelector.
        //
        // Returns:
        //     A collection of elements of type TResult where each element represents a projection
        //     over a group and its key.
        public IEnumerable<TResult> GroupBy<TKey, TElement, TResult>( Func<T, TKey> keySelector, Func<T, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            using (var db = CreateDb())
            {
                var q = db.Set<T>().GroupBy<T, TKey, TElement, TResult>(keySelector, elementSelector, resultSelector, comparer);

                return q.ToList();
            }
        }

        //
        // Summary:
        //     Returns the last element of a sequence.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable`1 to return the last element of.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        // Returns:
        //     The value at the last position in the source sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.
        //
        //   T:System.InvalidOperationException:
        //     The source sequence is empty.
        public T Last()
        {
            T val = default(T);
            using (var db = CreateDb())
            {
                val = db.Set<T>().Last();
            }
            return val;
        }
        //
        // Summary:
        //     Returns the last element of a sequence that satisfies a specified condition.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable`1 to return an element from.
        //
        //   predicate:
        //     A function to test each element for a condition.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        // Returns:
        //     The last element in the sequence that passes the test in the specified predicate
        //     function.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or predicate is null.
        //
        //   T:System.InvalidOperationException:
        //     No element satisfies the condition in predicate.-or-The source sequence is empty.
        public T Last(Func<T, bool> predicate)
        {
            T val = default(T);
            using (var db = CreateDb())
            {
                val = db.Set<T>().Last(predicate);
            }
            return val;
        }
        //
        // Summary:
        //     Returns the last element of a sequence, or a default value if the sequence contains
        //     no elements.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable`1 to return the last element of.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        // Returns:
        //     default(T) if the source sequence is empty; otherwise, the last element
        //     in the System.Collections.Generic.IEnumerable`1.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.
        public T LastOrDefault()
        {
            T val = default(T);
            using (var db = CreateDb())
            {
                val = db.Set<T>().LastOrDefault();
            }
            return val;
        }
        //
        // Summary:
        //     Returns the last element of a sequence that satisfies a condition or a default
        //     value if no such element is found.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable`1 to return an element from.
        //
        //   predicate:
        //     A function to test each element for a condition.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        // Returns:
        //     default(T) if the sequence is empty or if no elements pass the test in
        //     the predicate function; otherwise, the last element that passes the test in the
        //     predicate function.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or predicate is null.
        public T LastOrDefault(Func<T, bool> predicate)
        {
            T val = default(T);
            using (var db = CreateDb())
            {
                val = db.Set<T>().LastOrDefault(predicate);
            }
            return val;
        }
        //
        // Summary:
        //     Returns an System.Int64 that represents the total number of elements in a sequence.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable`1 that contains the elements to be
        //     counted.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        // Returns:
        //     The number of elements in the source sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.
        //
        //   T:System.OverflowException:
        //     The number of elements exceeds System.Int64.MaxValue.
        public long LongCount()
        {
            long count = 0;
            using (var db = CreateDb())
            {
                count = db.Set<T>().LongCount();
            }
            return count;

        }
        //
        // Summary:
        //     Returns an System.Int64 that represents how many elements in a sequence satisfy
        //     a condition.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable`1 that contains the elements to be
        //     counted.
        //
        //   predicate:
        //     A function to test each element for a condition.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        // Returns:
        //     A number that represents how many elements in the sequence satisfy the condition
        //     in the predicate function.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or predicate is null.
        //
        //   T:System.OverflowException:
        //     The number of matching elements exceeds System.Int64.MaxValue.
        public long LongCount(Func<T, bool> predicate)
        {
            long count = 0;
            using (var db = CreateDb())
            {
                count = db.Set<T>().LongCount(predicate);
            }
            return count;
        }


        //
        // Summary:
        //     Invokes a transform function on each element of a sequence and returns the maximum
        //     System.Double value.
        //
        // Parameters:
        //   source:
        //     A sequence of values to determine the maximum value of.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        // Returns:
        //     The maximum value in the sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        //
        //   T:System.InvalidOperationException:
        //     source contains no elements.
        public double Max(Func<T, double> selector)
        {
            double val = 0;
            using (var db = CreateDb())
            {
                val = db.Set<T>().Max(selector);
            }
            return val;

        }
        //
        // Summary:
        //     Invokes a transform function on each element of a sequence and returns the maximum
        //     nullable System.Double value.
        //
        // Parameters:
        //   source:
        //     A sequence of values to determine the maximum value of.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        // Returns:
        //     The value of type Nullable<Double> in C# or Nullable(Of Double) in Visual Basic
        //     that corresponds to the maximum value in the sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        public double? Max(Func<T, double?> selector)
        {
            double? val = 0;
            using (var db = CreateDb())
            {
                val = db.Set<T>().Max(selector);
            }
            return val;

        }
        //
        // Summary:
        //     Invokes a transform function on each element of a sequence and returns the maximum
        //     System.Decimal value.
        //
        // Parameters:
        //   source:
        //     A sequence of values to determine the maximum value of.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        // Returns:
        //     The maximum value in the sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        //
        //   T:System.InvalidOperationException:
        //     source contains no elements.
        public decimal Max(Func<T, decimal> selector)
        {
            decimal val = 0;
            using (var db = CreateDb())
            {
                val = db.Set<T>().Max(selector);
            }
            return val;

        }
        //
        // Summary:
        //     Invokes a transform function on each element of a sequence and returns the maximum
        //     nullable System.Decimal value.
        //
        // Parameters:
        //   source:
        //     A sequence of values to determine the maximum value of.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        // Returns:
        //     The value of type Nullable<Decimal> in C# or Nullable(Of Decimal) in Visual Basic
        //     that corresponds to the maximum value in the sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        public decimal? Max( Func<T, decimal?> selector)
        {
            decimal? val = 0;
            using (var db = CreateDb())
            {
                val = db.Set<T>().Max(selector);
            }
            return val;
        }
        //
        // Summary:
        //     Invokes a transform function on each element of a generic sequence and returns
        //     the maximum resulting value.
        //
        // Parameters:
        //   source:
        //     A sequence of values to determine the maximum value of.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        //   TResult:
        //     The type of the value returned by selector.
        //
        // Returns:
        //     The maximum value in the sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        public TResult Max<TResult>( Func<T, TResult> selector)
        {
            TResult val = default(TResult);
            using (var db = CreateDb())
            {
                val = db.Set<T>().Max(selector);
            }
            return val;
        }

        public double Min(Func<T, double> selector)
        {
            double val = 0;
            using (var db = CreateDb())
            {
                val = db.Set<T>().Min(selector);
            }
            return val;
        }
        //
        // Summary:
        //     Invokes a transform function on each element of a sequence and returns the minimum
        //     nullable System.Double value.
        //
        // Parameters:
        //   source:
        //     A sequence of values to determine the minimum value of.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        // Returns:
        //     The value of type Nullable<Double> in C# or Nullable(Of Double) in Visual Basic
        //     that corresponds to the minimum value in the sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        public double? Min( Func<T, double?> selector)
        {
            double? val = 0;
            using (var db = CreateDb())
            {
                val = db.Set<T>().Min(selector);
            }
            return val;
        }

        //
        // Summary:
        //     Invokes a transform function on each element of a sequence and returns the minimum
        //     System.Double value.
        //
        // Parameters:
        //   source:
        //     A sequence of values to determine the minimum value of.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        // Returns:
        //     The minimum value in the sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        //
        //   T:System.InvalidOperationException:
        //     source contains no elements.
        public decimal Min(Func<T, decimal> selector)
        {
            decimal val = 0;
            using (var db = CreateDb())
            {
                val = db.Set<T>().Min(selector);
            }
            return val;

        }

        public decimal? Min(Func<T, decimal?> selector)
        {
            decimal? val = 0;
            using (var db = CreateDb())
            {
                val = db.Set<T>().Min(selector);
            }
            return val;

        }

        public TResult Min<TResult>(Func<T, TResult> selector)
        {
            TResult val = default(TResult);
            using (var db = CreateDb())
            {
                val = db.Set<T>().Min(selector);
            }
            return val;
        }

        //
        // Summary:
        //     Sorts the elements of a sequence in ascending order by using a specified comparer.
        //
        // Parameters:
        //   source:
        //     A sequence of values to order.
        //
        //   keySelector:
        //     A function to extract a key from an element.
        //
        //   comparer:
        //     An System.Collections.Generic.IComparer`1 to compare keys.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        //   TKey:
        //     The type of the key returned by keySelector.
        //
        // Returns:
        //     An System.Linq.IOrderedEnumerable`1 whose elements are sorted according to a
        //     key.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or keySelector is null.
        public IOrderedEnumerable<T> OrderBy<TKey>(Func<T, TKey> keySelector, IComparer<TKey> comparer)
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().OrderBy<T, TKey>(keySelector, comparer);
            }
        }
        //
        // Summary:
        //     Sorts the elements of a sequence in descending order according to a key.
        //
        // Parameters:
        //   source:
        //     A sequence of values to order.
        //
        //   keySelector:
        //     A function to extract a key from an element.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        //   TKey:
        //     The type of the key returned by keySelector.
        //
        // Returns:
        //     An System.Linq.IOrderedEnumerable`1 whose elements are sorted in descending order
        //     according to a key.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or keySelector is null.
        public IOrderedEnumerable<T> OrderByDescending<TKey>(Func<T, TKey> keySelector)
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().OrderByDescending<T, TKey>(keySelector);
            }
        }
        //
        // Summary:
        //     Sorts the elements of a sequence in descending order by using a specified comparer.
        //
        // Parameters:
        //   source:
        //     A sequence of values to order.
        //
        //   keySelector:
        //     A function to extract a key from an element.
        //
        //   comparer:
        //     An System.Collections.Generic.IComparer`1 to compare keys.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        //   TKey:
        //     The type of the key returned by keySelector.
        //
        // Returns:
        //     An System.Linq.IOrderedEnumerable`1 whose elements are sorted in descending order
        //     according to a key.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or keySelector is null.
        public IOrderedEnumerable<T> OrderByDescending<TKey>( Func<T, TKey> keySelector, IComparer<TKey> comparer)
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().OrderByDescending<T, TKey>(keySelector, comparer);
            }
        }


        //
        // Summary:
        //     Projects each element of a sequence into a new form.
        //
        // Parameters:
        //   source:
        //     A sequence of values to invoke a transform function on.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        //   TResult:
        //     The type of the value returned by selector.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 whose elements are the result of
        //     invoking the transform function on each element of source.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        public IEnumerable<TResult> Select<TResult>( Func<T, TResult> selector)
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().Select<T, TResult>(selector);
            }
        }

        //
        // Summary:
        //     Projects each element of a sequence to an System.Collections.Generic.IEnumerable`1
        //     and flattens the resulting sequences into one sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values to project.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        //   TResult:
        //     The type of the elements of the sequence returned by selector.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 whose elements are the result of
        //     invoking the one-to-many transform function on each element of the input sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        public IEnumerable<TResult> SelectMany<TResult>(Func<T, IEnumerable<TResult>> selector)
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().SelectMany<T, TResult>(selector);
            }
        }
        //
        // Summary:
        //     Projects each element of a sequence to an System.Collections.Generic.IEnumerable`1,
        //     and flattens the resulting sequences into one sequence. The index of each source
        //     element is used in the projected form of that element.
        //
        // Parameters:
        //   source:
        //     A sequence of values to project.
        //
        //   selector:
        //     A transform function to apply to each source element; the second parameter of
        //     the function represents the index of the source element.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        //   TResult:
        //     The type of the elements of the sequence returned by selector.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 whose elements are the result of
        //     invoking the one-to-many transform function on each element of an input sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        public IEnumerable<TResult> SelectMany<TResult>(Func<T, int, IEnumerable<TResult>> selector)
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().SelectMany(selector);
            }
        }
        //
        // Summary:
        //     Projects each element of a sequence to an System.Collections.Generic.IEnumerable`1,
        //     flattens the resulting sequences into one sequence, and invokes a result selector
        //     function on each element therein.
        //
        // Parameters:
        //   source:
        //     A sequence of values to project.
        //
        //   collectionSelector:
        //     A transform function to apply to each element of the input sequence.
        //
        //   resultSelector:
        //     A transform function to apply to each element of the intermediate sequence.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        //   TCollection:
        //     The type of the intermediate elements collected by collectionSelector.
        //
        //   TResult:
        //     The type of the elements of the resulting sequence.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 whose elements are the result of
        //     invoking the one-to-many transform function collectionSelector on each element
        //     of source and then mapping each of those sequence elements and their corresponding
        //     source element to a result element.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or collectionSelector or resultSelector is null.
        public IEnumerable<TResult> SelectMany<TCollection, TResult>( Func<T, IEnumerable<TCollection>> collectionSelector, Func<T, TCollection, TResult> resultSelector)
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().SelectMany(collectionSelector, resultSelector);
            }
        }
        //
        // Summary:
        //     Projects each element of a sequence to an System.Collections.Generic.IEnumerable`1,
        //     flattens the resulting sequences into one sequence, and invokes a result selector
        //     function on each element therein. The index of each source element is used in
        //     the intermediate projected form of that element.
        //
        // Parameters:
        //   source:
        //     A sequence of values to project.
        //
        //   collectionSelector:
        //     A transform function to apply to each source element; the second parameter of
        //     the function represents the index of the source element.
        //
        //   resultSelector:
        //     A transform function to apply to each element of the intermediate sequence.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        //   TCollection:
        //     The type of the intermediate elements collected by collectionSelector.
        //
        //   TResult:
        //     The type of the elements of the resulting sequence.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 whose elements are the result of
        //     invoking the one-to-many transform function collectionSelector on each element
        //     of source and then mapping each of those sequence elements and their corresponding
        //     source element to a result element.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or collectionSelector or resultSelector is null.
        public IEnumerable<TResult> SelectMany<TCollection, TResult>( Func<T, int, IEnumerable<TCollection>> collectionSelector, Func<T, TCollection, TResult> resultSelector)
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().SelectMany(collectionSelector, resultSelector);
            }
        }

        //
        // Summary:
        //     Bypasses a specified number of elements in a sequence and then returns the remaining
        //     elements.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable`1 to return elements from.
        //
        //   count:
        //     The number of elements to skip before returning the remaining elements.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 that contains the elements that occur
        //     after the specified index in the input sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.
        public IEnumerable<T> Skip(int count)
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().Skip(count);
            }
        }

        //
        // Summary:
        //     Bypasses elements in a sequence as long as a specified condition is true and
        //     then returns the remaining elements.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable`1 to return elements from.
        //
        //   predicate:
        //     A function to test each element for a condition.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 that contains the elements from the
        //     input sequence starting at the first element in the linear series that does not
        //     pass the test specified by predicate.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or predicate is null.
        public IEnumerable<T> SkipWhile( Func<T, bool> predicate)
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().SkipWhile(predicate);
            }
        }

        //
        // Summary:
        //     Bypasses elements in a sequence as long as a specified condition is true and
        //     then returns the remaining elements. The element's index is used in the logic
        //     of the predicate function.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable`1 to return elements from.
        //
        //   predicate:
        //     A function to test each source element for a condition; the second parameter
        //     of the function represents the index of the source element.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 that contains the elements from the
        //     input sequence starting at the first element in the linear series that does not
        //     pass the test specified by predicate.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or predicate is null.
        public IEnumerable<T> SkipWhile(Func<T, int, bool> predicate)
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().SkipWhile(predicate);
            }
        }

        //
        // Summary:
        //     Computes the sum of the sequence of System.Decimal values that are obtained by
        //     invoking a transform function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values that are used to calculate a sum.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        // Returns:
        //     The sum of the projected values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        //
        //   T:System.OverflowException:
        //     The sum is larger than System.Decimal.MaxValue.
        public decimal Sum(Func<T, decimal> selector)
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().Sum(selector);
            }
        }
        //
        // Summary:
        //     Computes the sum of the sequence of nullable System.Double values that are obtained
        //     by invoking a transform function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values that are used to calculate a sum.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        // Returns:
        //     The sum of the projected values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        public double? Sum( Func<T, double?> selector)
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().Sum(selector);
            }
        }

        //
        // Summary:
        //     Returns a specified number of contiguous elements from the start of a sequence.
        //
        // Parameters:
        //   source:
        //     The sequence to return elements from.
        //
        //   count:
        //     The number of elements to return.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 that contains the specified number
        //     of elements from the start of the input sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.
        public IEnumerable<T> Take(int count)
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().Take(count);
            }
        }
        //
        // Summary:
        //     Returns elements from a sequence as long as a specified condition is true.
        //
        // Parameters:
        //   source:
        //     A sequence to return elements from.
        //
        //   predicate:
        //     A function to test each element for a condition.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 that contains the elements from the
        //     input sequence that occur before the element at which the test no longer passes.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or predicate is null.
        public IEnumerable<T> TakeWhile( Func<T, bool> predicate)
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().TakeWhile(predicate);
            }
        }
        //
        // Summary:
        //     Returns elements from a sequence as long as a specified condition is true. The
        //     element's index is used in the logic of the predicate function.
        //
        // Parameters:
        //   source:
        //     The sequence to return elements from.
        //
        //   predicate:
        //     A function to test each source element for a condition; the second parameter
        //     of the function represents the index of the source element.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 that contains elements from the input
        //     sequence that occur before the element at which the test no longer passes.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or predicate is null.
        public IEnumerable<T> TakeWhile(Func<T, int, bool> predicate)
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().TakeWhile(predicate);
            }
        }




        //
        // Summary:
        //     Creates an array from a System.Collections.Generic.IEnumerable`1.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable`1 to create an array from.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        // Returns:
        //     An array that contains the elements from the input sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.
        public T[] ToArray()
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().ToArray();
            }
        }
        //
        // Summary:
        //     Creates a System.Collections.Generic.Dictionary`2 from an System.Collections.Generic.IEnumerable`1
        //     according to a specified key selector function.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable`1 to create a System.Collections.Generic.Dictionary`2
        //     from.
        //
        //   keySelector:
        //     A function to extract a key from each element.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        //   TKey:
        //     The type of the key returned by keySelector.
        //
        // Returns:
        //     A System.Collections.Generic.Dictionary`2 that contains keys and values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or keySelector is null.-or-keySelector produces a key that is null.
        //
        //   T:System.ArgumentException:
        //     keySelector produces duplicate keys for two elements.
        public Dictionary<TKey, T> ToDictionary<TKey>( Func<T, TKey> keySelector)
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().ToDictionary(keySelector);
            }
        }
        //
        // Summary:
        //     Creates a System.Collections.Generic.Dictionary`2 from an System.Collections.Generic.IEnumerable`1
        //     according to a specified key selector function and key comparer.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable`1 to create a System.Collections.Generic.Dictionary`2
        //     from.
        //
        //   keySelector:
        //     A function to extract a key from each element.
        //
        //   comparer:
        //     An System.Collections.Generic.IEqualityComparer`1 to compare keys.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        //   TKey:
        //     The type of the keys returned by keySelector.
        //
        // Returns:
        //     A System.Collections.Generic.Dictionary`2 that contains keys and values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or keySelector is null.-or-keySelector produces a key that is null.
        //
        //   T:System.ArgumentException:
        //     keySelector produces duplicate keys for two elements.
        public Dictionary<TKey, T> ToDictionary<TKey>(Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().ToDictionary(keySelector, comparer);
            }
        }
        //
        // Summary:
        //     Creates a System.Collections.Generic.Dictionary`2 from an System.Collections.Generic.IEnumerable`1
        //     according to specified key selector and element selector functions.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable`1 to create a System.Collections.Generic.Dictionary`2
        //     from.
        //
        //   keySelector:
        //     A function to extract a key from each element.
        //
        //   elementSelector:
        //     A transform function to produce a result element value from each element.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        //   TKey:
        //     The type of the key returned by keySelector.
        //
        //   TElement:
        //     The type of the value returned by elementSelector.
        //
        // Returns:
        //     A System.Collections.Generic.Dictionary`2 that contains values of type TElement
        //     selected from the input sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or keySelector or elementSelector is null.-or-keySelector produces a key
        //     that is null.
        //
        //   T:System.ArgumentException:
        //     keySelector produces duplicate keys for two elements.
        public Dictionary<TKey, TElement> ToDictionary<TKey, TElement>(Func<T, TKey> keySelector, Func<T, TElement> elementSelector)
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().ToDictionary(keySelector, elementSelector);
            }
        }
        //
        // Summary:
        //     Creates a System.Collections.Generic.Dictionary`2 from an System.Collections.Generic.IEnumerable`1
        //     according to a specified key selector function, a comparer, and an element selector
        //     function.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable`1 to create a System.Collections.Generic.Dictionary`2
        //     from.
        //
        //   keySelector:
        //     A function to extract a key from each element.
        //
        //   elementSelector:
        //     A transform function to produce a result element value from each element.
        //
        //   comparer:
        //     An System.Collections.Generic.IEqualityComparer`1 to compare keys.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        //   TKey:
        //     The type of the key returned by keySelector.
        //
        //   TElement:
        //     The type of the value returned by elementSelector.
        //
        // Returns:
        //     A System.Collections.Generic.Dictionary`2 that contains values of type TElement
        //     selected from the input sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or keySelector or elementSelector is null.-or-keySelector produces a key
        //     that is null.
        //
        //   T:System.ArgumentException:
        //     keySelector produces duplicate keys for two elements.
        public Dictionary<TKey, TElement> ToDictionary<TKey, TElement>( Func<T, TKey> keySelector, Func<T, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().ToDictionary(keySelector, elementSelector, comparer);
            }
        }
        //
        // Summary:
        //     Creates a System.Collections.Generic.List`1 from an System.Collections.Generic.IEnumerable`1.
        //
        // Parameters:
        //   source:
        //     The System.Collections.Generic.IEnumerable`1 to create a System.Collections.Generic.List`1
        //     from.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        // Returns:
        //     A System.Collections.Generic.List`1 that contains elements from the input sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.
        public List<T> ToList()
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().ToList();
            }
        }
        //
        // Summary:
        //     Creates a System.Linq.Lookup`2 from an System.Collections.Generic.IEnumerable`1
        //     according to a specified key selector function.
        //
        // Parameters:
        //   source:
        //     The System.Collections.Generic.IEnumerable`1 to create a System.Linq.Lookup`2
        //     from.
        //
        //   keySelector:
        //     A function to extract a key from each element.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        //   TKey:
        //     The type of the key returned by keySelector.
        //
        // Returns:
        //     A System.Linq.Lookup`2 that contains keys and values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or keySelector is null.
        public ILookup<TKey, T> ToLookup<TKey>(Func<T, TKey> keySelector)
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().ToLookup(keySelector);
            }
        }
        //
        // Summary:
        //     Creates a System.Linq.Lookup`2 from an System.Collections.Generic.IEnumerable`1
        //     according to a specified key selector function and key comparer.
        //
        // Parameters:
        //   source:
        //     The System.Collections.Generic.IEnumerable`1 to create a System.Linq.Lookup`2
        //     from.
        //
        //   keySelector:
        //     A function to extract a key from each element.
        //
        //   comparer:
        //     An System.Collections.Generic.IEqualityComparer`1 to compare keys.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        //   TKey:
        //     The type of the key returned by keySelector.
        //
        // Returns:
        //     A System.Linq.Lookup`2 that contains keys and values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or keySelector is null.
        public ILookup<TKey, T> ToLookup<TKey>( Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().ToLookup(keySelector, comparer);
            }
        }
        //
        // Summary:
        //     Creates a System.Linq.Lookup`2 from an System.Collections.Generic.IEnumerable`1
        //     according to specified key selector and element selector functions.
        //
        // Parameters:
        //   source:
        //     The System.Collections.Generic.IEnumerable`1 to create a System.Linq.Lookup`2
        //     from.
        //
        //   keySelector:
        //     A function to extract a key from each element.
        //
        //   elementSelector:
        //     A transform function to produce a result element value from each element.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        //   TKey:
        //     The type of the key returned by keySelector.
        //
        //   TElement:
        //     The type of the value returned by elementSelector.
        //
        // Returns:
        //     A System.Linq.Lookup`2 that contains values of type TElement selected from the
        //     input sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or keySelector or elementSelector is null.
        public ILookup<TKey, TElement> ToLookup<TKey, TElement>( Func<T, TKey> keySelector, Func<T, TElement> elementSelector)
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().ToLookup(keySelector, elementSelector);
            }
        }
        //
        // Summary:
        //     Creates a System.Linq.Lookup`2 from an System.Collections.Generic.IEnumerable`1
        //     according to a specified key selector function, a comparer and an element selector
        //     function.
        //
        // Parameters:
        //   source:
        //     The System.Collections.Generic.IEnumerable`1 to create a System.Linq.Lookup`2
        //     from.
        //
        //   keySelector:
        //     A function to extract a key from each element.
        //
        //   elementSelector:
        //     A transform function to produce a result element value from each element.
        //
        //   comparer:
        //     An System.Collections.Generic.IEqualityComparer`1 to compare keys.
        //
        // Type parameters:
        //   T:
        //     The type of the elements of source.
        //
        //   TKey:
        //     The type of the key returned by keySelector.
        //
        //   TElement:
        //     The type of the value returned by elementSelector.
        //
        // Returns:
        //     A System.Linq.Lookup`2 that contains values of type TElement selected from the
        //     input sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or keySelector or elementSelector is null.
        public ILookup<TKey, TElement> ToLookup<TKey, TElement>( Func<T, TKey> keySelector, Func<T, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().ToLookup(keySelector, elementSelector, comparer);
            }
        }

        public IEnumerable<T> Where( Func<T, int, bool> predicate)
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().Where(predicate);
            }
        }
        public IEnumerable<T> Where( Func<T, bool> predicate)
        {
            using (var db = CreateDb())
            {
                return db.Set<T>().Where(predicate);
            }
        }
        /*
        public IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {


        }
        */

    }
}
