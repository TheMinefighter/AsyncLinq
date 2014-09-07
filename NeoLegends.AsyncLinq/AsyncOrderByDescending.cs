﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    public static class AsyncOrderByDescending
    {
        public static async Task<IEnumerable<TSource>> OrderByDescendingAsync<TSource, TKey>(
            this Task<IEnumerable<TSource>> collection, 
            Func<TSource, TKey> selector)
        {
            Contract.Requires<ArgumentNullException>(collection != null);
            Contract.Requires<ArgumentNullException>(selector != null);

            return (await collection).OrderByDescending(selector);
        }

        public static async Task<IEnumerable<TSource>> OrderByDescendingAsync<TSource, TKey>(
            this IEnumerable<Task<TSource>> collection, 
            Func<TSource, TKey> selector)
        {
            Contract.Requires<ArgumentNullException>(collection != null);
            Contract.Requires<ArgumentNullException>(selector != null);

            return (await Task.WhenAll(collection)).OrderByDescending(selector);
        }

        public static async Task<IEnumerable<TSource>> OrderByDescendingAsync<TSource, TKey>(
            this Task<IEnumerable<TSource>> collection, 
            Func<TSource, TKey> selector, 
            IComparer<TKey> comparer)
        {
            Contract.Requires<ArgumentNullException>(collection != null);
            Contract.Requires<ArgumentNullException>(selector != null);
            Contract.Requires<ArgumentNullException>(comparer != null);

            return (await collection).OrderByDescending(selector, comparer);
        }

        public static async Task<IEnumerable<TSource>> OrderByDescendingAsync<TSource, TKey>(
            this IEnumerable<Task<TSource>> collection, 
            Func<TSource, TKey> selector, 
            IComparer<TKey> comparer)
        {
            Contract.Requires<ArgumentNullException>(collection != null);
            Contract.Requires<ArgumentNullException>(selector != null);
            Contract.Requires<ArgumentNullException>(comparer != null);

            return (await Task.WhenAll(collection)).OrderByDescending(selector, comparer);
        }
    }
}