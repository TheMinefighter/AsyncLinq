﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static async Task<T> LastAsync<T>(this Task<IEnumerable<T>> collection)
        {
            Contract.Requires<ArgumentNullException>(collection != null);

            return (await collection.ConfigureAwait(false)).Last();
        }

        public static async Task<T> LastAsync<T>(this Task<IEnumerable<T>> collection, Func<T, bool> predicate)
        {
            Contract.Requires<ArgumentNullException>(collection != null);
            Contract.Requires<ArgumentNullException>(predicate != null);

            return (await collection.ConfigureAwait(false)).Last(predicate);
        }

        public static async Task<T> LastAsync<T>(this IEnumerable<Task<T>> collection, Func<T, bool> predicate)
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

            throw new InvalidOperationException("Sequence contains no matching element");
        }

        public static async Task<T> LastFinishedAsync<T>(this IEnumerable<Task<T>> collection)
        {
            Contract.Requires<ArgumentNullException>(collection != null);

            List<Task<T>> workingCopy = collection.ToList();
            Task<T> current = null;
            while (workingCopy.Any())
            {
                current = await Task.WhenAny(workingCopy).ConfigureAwait(false);
                workingCopy.Remove(current);
            }
            return current.Result;
        }

        public static async Task<T> LastFinishedAsync<T>(this IEnumerable<Task<T>> collection, Func<T, bool> predicate)
        {
            Contract.Requires<ArgumentNullException>(collection != null);
            Contract.Requires<ArgumentNullException>(predicate != null);

            List<Task<T>> workingCopy = collection.ToList();
            Task<T> current = null;
            while (workingCopy.Any())
            {
                Task<T> finishedTask = await Task.WhenAny(workingCopy).ConfigureAwait(false);
                if (predicate(finishedTask.Result))
                {
                    current = finishedTask;
                    workingCopy.Remove(current);
                }
            }
            return current.Result;
        }
        
        public static async Task<T> LastAsync<T>(this IEnumerable<Task<T>> collection, Func<T, Task<bool>> predicate)
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

            throw new InvalidOperationException("Sequence contains no matching element");
        }
        
        public static async Task<T> LastAsync<T>(this Task<IEnumerable<T>> collection, Func<T, Task<bool>> predicate)
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

            throw new InvalidOperationException("Sequence contains no matching element");
        }
        
        public static async Task<T> LastAsync<T>(this IEnumerable<T> collection, Func<T, Task<bool>> predicate)
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

            throw new InvalidOperationException("Sequence contains no matching element");
        }
        
        public static async Task<T> LastFinishedAsync<T>(this IEnumerable<Task<T>> collection, Func<T, Task<bool>> predicate)
        {
            Contract.Requires<ArgumentNullException>(collection != null);
            Contract.Requires<ArgumentNullException>(predicate != null);

            List<Task<T>> workingCopy = collection.ToList();
            Task<T> current = null;
            while (workingCopy.Any())
            {
                Task<T> finishedTask = await Task.WhenAny(workingCopy).ConfigureAwait(false);
                if (await predicate(finishedTask.Result).ConfigureAwait(false))
                {
                    current = finishedTask;
                    workingCopy.Remove(current);
                }
            }
            return current.Result;
        }
    }
}
