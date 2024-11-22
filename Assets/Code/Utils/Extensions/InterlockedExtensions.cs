using System;
using System.Threading;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Code.Utils.Extensions
{
    public static class InterlockedExtensions
    {
        public static unsafe void InterlockedMin(this NativeArray<int> array, int index, int value)
        {
            Min(ref ((int*)array.GetUnsafePtr())[index], value);
        }

        public static void Min(ref int location, int value)
        {
            int initialValue, computedValue;
            do
            {
                initialValue = location;
                computedValue = Math.Min(initialValue, value);
                if (initialValue == computedValue)
                {
                    break;
                }
            } while (Interlocked.CompareExchange(ref location, computedValue, initialValue) != initialValue);
        }
    }
}