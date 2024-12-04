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

            uint currentValue;

            while (true)
            {
                // Read the current value atomically
                currentValue = Volatile.Read(ref *elementPtr);

                // If the current value is already less than or equal to the new value, no update is needed
                if (currentValue <= value)
                {
                    break;
                }

                // Attempt to replace the current value with the new minimum value atomically
                uint previousValue = InterlockedCompareExchange(ref *elementPtr, value, currentValue);

                // If the exchange was successful, exit the loop
                if (previousValue == currentValue)
                {
                    break;
                }

                // Another thread updated the value; retry with the new current value
                // The loop will continue until the exchange is successful or no update is needed
            }
        }

        private static unsafe uint InterlockedCompareExchange(ref uint location, uint value, uint comparand)
        {
            // Use UnsafeUtility to reinterpret the uints as ints for the Interlocked.CompareExchange method
            int result = Interlocked.CompareExchange(
                ref UnsafeUtility.As<uint, int>(ref location),
                UnsafeUtility.As<uint, int>(ref value),
                UnsafeUtility.As<uint, int>(ref comparand));

            // Reinterpret the result back to uint
            return UnsafeUtility.As<int, uint>(ref result);
        }
    }
}