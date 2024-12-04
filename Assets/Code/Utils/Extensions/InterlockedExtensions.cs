using System;
using System.Threading;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Code.Utils.Extensions
{
    public static class InterlockedExtensions
    {
        public static unsafe void InterlockedMin(this NativeArray<uint> array, uint index, uint value)
        {
            InterlockedMin(array, (int)index, value);
        }

        public static unsafe void InterlockedMin(this NativeArray<uint> array, int index, uint value)
        {
            // Get a pointer to the underlying array data
            uint* ptr = (uint*)NativeArrayUnsafeUtility.GetUnsafePtr(array);
            // Get a pointer to the specific element
            uint* elementPtr = ptr + index;

            uint currentValue, newValue;

            do
            {
                // Read the current value atomically
                currentValue = Volatile.Read(ref *elementPtr);

                // Determine the new value as the minimum
                newValue = Math.Min(currentValue, value);

                // If the current value is already the minimum, no update is needed
                if (currentValue == newValue)
                {
                    break;
                }

                // Attempt to replace the current value with the new minimum value atomically
            } while (Interlocked.CompareExchange(
                         ref *(int*)elementPtr,
                         *(int*)&newValue,
                         *(int*)&currentValue) != *(int*)&currentValue);
        }
    }
}