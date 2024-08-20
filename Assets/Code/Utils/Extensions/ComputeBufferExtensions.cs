using System;
using UnityEngine;

namespace Code.Utils.Extensions
{
    public static class ComputeBufferExtensions
    {
        public static void PrintInt(this ComputeBuffer computeBuffer, string name)
        {
            Print<int>(computeBuffer, name, element => element.ToString());
        }
        
        public static void Print<T>(this ComputeBuffer computeBuffer, string name, Func<T, string> format)
        {
            T[] elements = new T[computeBuffer.count];
            computeBuffer.GetData(elements);
            elements.Print(name, format);
        }
    }
}