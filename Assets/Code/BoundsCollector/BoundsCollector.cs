using System;
using Unity.Collections;

namespace Code.SpheresBoundsCollector
{
    public abstract class BoundsCollector : IDisposable
    {
        private NativeArray<BoundingBox> _bounds;

        protected BoundsCollector(int size)
        {
            _bounds = new NativeArray<BoundingBox>(size, Allocator.Persistent);
        }

        public NativeArray<BoundingBox> Collect()
        {
            OnCollect(_bounds);

            return _bounds;
        }

        public void Dispose()
        {
            _bounds.Dispose();
            OnDispose();
        }
        
        protected abstract void OnCollect(NativeArray<BoundingBox> bounds);
        protected virtual void OnDispose() {}
    }
}