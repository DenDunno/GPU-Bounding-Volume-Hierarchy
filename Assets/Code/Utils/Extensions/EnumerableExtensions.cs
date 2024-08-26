using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Utils.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsSame<T>(this IList<T> first, IList<T> second)
        {
            for (int i = 0; i < first.Count; ++i)
            {
                if (first[i].Equals(second[i]) == false)
                {
                    return false;
                }
            }

            return true;
        }
        
        public static IList<T> Fill<T>(this IList<T> collection, T value)
        {
            for (int i = 0; i < collection.Count; ++i)
            {
                collection[i] = value;
            }

            return collection;
        }
        
        public static void Print<T>(this IList<T> list, string name)
        {
            Print(list, name, x => x.ToString());
        }
        
        public static void Print<T>(this IList<T> list, string name, Func<T, string> format)
        {
            IEnumerable<string> output = list.Select(format);
            Debug.Log(name + string.Join(", ", output));
        }
    }
}