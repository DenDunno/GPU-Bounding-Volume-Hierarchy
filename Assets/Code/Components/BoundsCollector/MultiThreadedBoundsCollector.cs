using Code.Data;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

namespace Code.Components.BoundsCollector
{
    public class MultiThreadedBoundsCollector : IBoundsCollector
    {
        private TransformAccessArray _transformAccess;
        private NativeArray<float> _radiuses;

        public MultiThreadedBoundsCollector(Transform[] transforms, float[] radiuses)
        {
            _radiuses = new NativeArray<float>(radiuses, Allocator.Persistent);
            _transformAccess = new TransformAccessArray(transforms);
        }

        public void CollectTo(NativeArray<AABB> bounds)
        {
            CollectBoundsJob collectBoundsJob = new(_radiuses, bounds);
            collectBoundsJob.Schedule(_transformAccess);
        }

        public void Dispose()
        {
            _transformAccess.Dispose();
            _radiuses.Dispose();
        }
    }
}