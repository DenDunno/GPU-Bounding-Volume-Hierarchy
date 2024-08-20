using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Utils.Extensions
{
    public static class EnumerableExtensions
    {
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