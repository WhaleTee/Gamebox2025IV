using System;
using System.Collections.Generic;

namespace Extensions
{
    public static class Dictionary
    {
        public static void Populate<TKey, TValue, TKeys>
            (this Dictionary<TKey, TValue> dictionary, TKeys keys)
            where TKeys : IEnumerable<TKey>
        {
            foreach (var key in keys)
                dictionary.Add(key, default);
        }

        public static void Populate<TKey, TValue, TKeys>
    (this Dictionary<TKey, TValue> dictionary, TKeys keys, Func<TKey, TValue> factory)
    where TKeys : IEnumerable<TKey>
        {
            foreach (var key in keys)
                dictionary.Add(key, factory(key));
        }
    }
}
