using System;
using Unity.Collections;
using UnityEngine;

namespace Code.Utils.BuffersUtls
{
    public class SubUpdatesBuffer<T> : IDisposable where T : struct
    {
        private readonly int _size;

        public SubUpdatesBuffer(int size, int stride)
        {
            Value = new ComputeBuffer(size, stride, ComputeBufferType.Structured, ComputeBufferMode.SubUpdates);
            _size = size;
        }

        public ComputeBuffer Value { get; }

        public NativeArray<T> BeginWrite() 
        {
            return Value.BeginWrite<T>(0, _size);
        }

        public void EndWrite() 
        {
            Value.EndWrite<T>(_size);
        }

        public void Dispose()
        {
            Value.Dispose();
        }
    }
}