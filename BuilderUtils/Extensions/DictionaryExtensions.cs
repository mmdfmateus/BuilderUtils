using System;
using System.Collections.Generic;
using System.Text;

namespace BuilderUtils.Extensions
{
    public static class DictionaryExtensions
    {
        public static Dictionary<TKey, TValue> Add<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, KeyValuePair<TKey, TValue> keyValuePair)
        {
            dictionary.Add(keyValuePair.Key, keyValuePair.Value);
            return dictionary;
        }
    }
}
