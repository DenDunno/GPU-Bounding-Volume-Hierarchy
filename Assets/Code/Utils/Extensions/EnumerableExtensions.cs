using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Utils.Extensions
{
    public static class EnumerableExtensions
    {
        public static CollectionComparisonResult<T> IsSame<T>(this IList<T> first, IList<T> second)
        {
            for (int i = 0; i < first.Count; ++i)
            {
                if (first[i].Equals(second[i]) == false)
                {
                    return new CollectionComparisonResult<T>(isEqual: false, first, second, i);
                }
            }

            return new CollectionComparisonResult<T>(isEqual: true, first, second, default);
        }
        
        public static IList<T> Fill<T>(this IList<T> collection, T value)
        {
            for (int i = 0; i < collection.Count; ++i)
            {
                collection[i] = value;
            }

            return collection;
        }
        
        public static void Print<T>(this IList<T> list)
        {
            Print(list, string.Empty);
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
        
        public static void Shuffle<T>(this IList<T> ts)
        {
            int count = ts.Count;
            int last = count - 1;
            for (int i = 0; i < last; ++i)
            {
                int r = UnityEngine.Random.Range(i, count);
                (ts[i], ts[r]) = (ts[r], ts[i]);
            }
        }

        public static int[] GetRandomOrderedArray(int size)
        {
            int[] array = new int[size];
            for (int i = 0; i < array.Length; ++i)
            {
                array[i] = i;
            }
            
            array.Shuffle();

            return array;
        }
    }
}