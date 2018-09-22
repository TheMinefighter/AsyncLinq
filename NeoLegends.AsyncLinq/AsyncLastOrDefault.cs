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
        public static async Task<T> LastOrDefaultAsync<T>(this Task<IEnumerable<T>> collection)
        {
            Contract.Requires<ArgumentNullException>(collection != null);

            return (await collection.ConfigureAwait(false)).LastOrDefault();
        }

        public static Task<T> LastOrDefaultAsync<T>(this IEnumerable<Task<T>> collection)
        {
            // Even though this method does nothing more than the regular .LastOrDefault
            // we have it because it would be confusing if it was missing.

            Contract.Requires<ArgumentNullException>(collection != null);

            return collection.LastOrDefault();
        }

        public static async Task<T> LastOrDefaultAsync<T>(this Task<IEnumerable<T>> collection, Func<T, bool> predicate)
        {
            Contract.Requires<ArgumentNullException>(collection != null);
            Contract.Requires<ArgumentNullException>(predicate != null);

            return (await collection.ConfigureAwait(false)).LastOrDefault(predicate);
        }

        public static async Task<T> LastOrDefaultAsync<T>(this IEnumerable<Task<T>> collection, Func<T, bool> predicate)
        {
            Contract.Requires<ArgumentNullException>(collection != null);
            Contract.Requires<ArgumentNullException>(predicate != null);

            foreach (Task<T> task in collection.Reverse())
            {
                T result = await task.ConfigureAwait(false);
                if (predicate(result))
                {
                    return result;
                }
            }

            return default(T);
        }
        
        public static async Task<T> LastOrDefaultAsync<T>(this IEnumerable<Task<T>> collection, Func<T, Task<bool>> predicate)
        {
            Contract.Requires<ArgumentNullException>(collection != null);
            Contract.Requires<ArgumentNullException>(predicate != null);

            foreach (Task<T> task in collection.Reverse())
            {
                T result = await task.ConfigureAwait(false);
                if (await predicate(result).ConfigureAwait(false))
                {
                    return result;
                }
            }

            return default(T);
        }
        
        public static async Task<T> LastOrDefaultAsync<T>(this Task<IEnumerable<T>> collection, Func<T, Task<bool>> predicate)
        {
            Contract.Requires<ArgumentNullException>(collection != null);
            Contract.Requires<ArgumentNullException>(predicate != null);

            foreach (T item in (await collection.ConfigureAwait(false)).Reverse())
            {
                if (await predicate(item).ConfigureAwait(false))
                {
                    return item;
                }
            }

            return default(T);
        }
        
        public static async Task<T> LastOrDefaultAsync<T>(this IEnumerable<T> collection, Func<T, Task<bool>> predicate)
        {
            Contract.Requires<ArgumentNullException>(collection != null);
            Contract.Requires<ArgumentNullException>(predicate != null);

            foreach (T item in collection.Reverse())
            {
                if (await predicate(item).ConfigureAwait(false))
                {
                    return item;
                }
            }

            return default(T);
        }
    }
}
