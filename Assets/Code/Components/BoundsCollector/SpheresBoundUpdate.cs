using System;
using Code.Data;
using Code.Utils.BuffersUtls;
using Unity.Collections;

namespace Code.Components.BoundsCollector
{
    public class SpheresBoundUpdate : IDisposable
    {
        private readonly IBoundsCollector _boundsCollector;
        private readonly SubUpdatesBuffer<AABB> _boundingBoxes;

        public SpheresBoundUpdate(IBoundsCollector boundsCollector, SubUpdatesBuffer<AABB> boundingBoxes)
        {
            _boundsCollector = boundsCollector;
            _boundingBoxes = boundingBoxes;
        }

        public void UpdateBuffer()
        {
            NativeArray<AABB> bounds = _boundingBoxes.BeginWrite();
            _boundsCollector.CollectTo(bounds);
            _boundingBoxes.EndWrite();
        }

        public void Dispose()
        {
            _boundingBoxes.Dispose();
        }
    }
}