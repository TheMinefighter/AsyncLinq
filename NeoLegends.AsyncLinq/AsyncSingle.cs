using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static async Task<T> SingleAsync<T>(this Task<IEnumerable<T>> collection)
        {
            Contract.Requires<ArgumentNullException>(collection != null);

            return (await collection.ConfigureAwait(false)).Single();
        }

        public static Task<T> SingleAsync<T>(this IEnumerable<Task<T>> collection)
        {
            Contract.Requires<ArgumentNullException>(collection != null);

            List<Task<T>> workingCopy = collection.ToList();
            return Task.WhenAny(workingCopy).ContinueWith(t =>
            {
                workingCopy.Remove(t.Result);
                if (workingCopy.Any())
                {
                    throw new InvalidOperationException("Sequence contains more than one element");
                }
                else
                {
                    return t.Result.Result;
                }
            });
        }

        public static async Task<T> SingleAsync<T>(this Task<IEnumerable<T>> collection, Func<T, bool> predicate)
        {
            Contract.Requires<ArgumentNullException>(collection != null);
            Contract.Requires<ArgumentNullException>(predicate != null);

            return (await collection.ConfigureAwait(false)).Single(predicate);
        }

        public static async Task<T> SingleAsync<T>(this IEnumerable<Task<T>> collection, Func<T, bool> predicate)
        {
            Contract.Requires<ArgumentNullException>(collection != null);
            Contract.Requires<ArgumentNullException>(predicate != null);
            bool any = false;
            T instanceFound = default(T); //default will never be returned only initializing that C# is happy
            foreach (Task<T> item in collection)
            {
                T awaitedItem = await item.ConfigureAwait(false);
                if (predicate(awaitedItem))
                {
                    if (any)
                    {
                        throw new InvalidOperationException("Sequence contains more than one matching element");
                    }

                    any = true;
                    instanceFound = awaitedItem;
                }
            }

            if (!any)
            {
                throw new InvalidOperationException("Sequence contains no matching element");
            }

            return instanceFound;
        }
        
        public static async Task<T> SingleAsync<T>(this IEnumerable<Task<T>> collection, Func<T, Task<bool>> predicate)
        {
            Contract.Requires<ArgumentNullException>(collection != null);
            Contract.Requires<ArgumentNullException>(predicate != null);
            bool any = false;
            T instanceFound = default(T); //default will never be returned only initializing that C# is happy
            foreach (Task<T> item in collection)
            {
                T awaitedItem = await item.ConfigureAwait(false);
                if (await predicate(awaitedItem).ConfigureAwait(false))
                {
                    if (any)
                    {
                        throw new InvalidOperationException("Sequence contains more than one matching element");
                    }

                    any = true;
                    instanceFound = awaitedItem;
                }
            }

            if (!any)
            {
                throw new InvalidOperationException("Sequence contains no matching element");
            }

            return instanceFound;
        }
        
        public static async Task<T> SingleAsync<T>(this Task<IEnumerable<T>> collection, Func<T, Task<bool>> predicate)
        {
            Contract.Requires<ArgumentNullException>(collection != null);
            Contract.Requires<ArgumentNullException>(predicate != null);
            bool any = false;
            T instanceFound = default(T); //default will never be returned only initializing that C# is happy
            foreach (T item in await collection.ConfigureAwait(false))
            {
                if (await predicate(item).ConfigureAwait(false))
                {
                    if (any)
                    {
                        throw new InvalidOperationException("Sequence contains more than one matching element");
                    }

                    any = true;
                    instanceFound = item;
                }
            }

            if (!any)
            {
                throw new InvalidOperationException("Sequence contains no matching element");
            }

            return instanceFound;
        }
        
        public static async Task<T> SingleAsync<T>(this IEnumerable<T> collection, Func<T, Task<bool>> predicate)
        {
            Contract.Requires<ArgumentNullException>(collection != null);
            Contract.Requires<ArgumentNullException>(predicate != null);
            bool any = false;
            T instanceFound = default(T); //default will never be returned only initializing that C# is happy
            foreach (T item in collection)
            {
                if (await predicate(item).ConfigureAwait(false))
                {
                    if (any)
                    {
                        throw new InvalidOperationException("Sequence contains more than one matching element");
                    }

                    any = true;
                    instanceFound = item;
                }
            }

            if (!any)
            {
                throw new InvalidOperationException("Sequence contains no matching element");
            }

            return instanceFound;
        }
    }
}