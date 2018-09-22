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
        public static async Task<int> CountAsync<T>(this Task<IEnumerable<T>> collection)
        {
            Contract.Requires<ArgumentNullException>(collection != null);

            return (await collection.ConfigureAwait(false)).Count();
        }

        public static async Task<int> CountAsync<T>(this Task<IEnumerable<T>> collection, Func<T, bool> predicate)
        {
            Contract.Requires<ArgumentNullException>(collection != null);
            Contract.Requires<ArgumentNullException>(predicate != null);

            return (await collection.ConfigureAwait(false)).Count(predicate);
        }

        public static async Task<int> CountAsync<T>(this IEnumerable<Task<T>> collection, Func<T, bool> predicate)
        {
            Contract.Requires<ArgumentNullException>(collection != null);
            Contract.Requires<ArgumentNullException>(predicate != null);

            return (await Task.WhenAll(collection).ConfigureAwait(false)).Count(predicate);
        }

        public static async Task<int> CountAsync<T>(this Task<IEnumerable<T>> collection, Func<T, Task<bool>> predicate)
        {
            Contract.Requires<ArgumentNullException>(collection != null);
            Contract.Requires<ArgumentNullException>(predicate != null);

            IEnumerable<T> evaluatedCollection = await collection.ConfigureAwait(false);
            int count = 0;
            foreach (T item in evaluatedCollection) {
                if (await predicate(item).ConfigureAwait(false)) {
                    count++;
                }
            }
            return count;
        }
        
        public static async Task<int> CountAsync<T>(this IEnumerable<T> collection, Func<T, Task<bool>> predicate)
        {
            Contract.Requires<ArgumentNullException>(collection != null);
            Contract.Requires<ArgumentNullException>(predicate != null);

            int count = 0;
            foreach (T item in collection) {
                if (await predicate(item).ConfigureAwait(false)) {
                    count++;
                }
            }
            return count;
        }

        public static async Task<int> CountAsync<T>(this IEnumerable<Task<T>> collection, Func<T, Task<bool>> predicate)
        {
            Contract.Requires<ArgumentNullException>(collection != null);
            Contract.Requires<ArgumentNullException>(predicate != null);

            int count = 0;
            foreach (Task<T> item in collection) {
                if (await predicate(await item.ConfigureAwait(false)).ConfigureAwait(false)) {
                    count++;
                }
            }
            return count;
        }
    }
}
