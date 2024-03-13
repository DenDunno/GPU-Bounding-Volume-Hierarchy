using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

namespace Code.SpheresBoundsCollector
{
    public class MultiThreadedBoundsCollector : BoundsCollector
    {
        private TransformAccessArray _transformAccess;
        private NativeArray<float> _radiuses;

        public MultiThreadedBoundsCollector(Transform[] transforms, float[] radiuses) : base(radiuses.Length)
        {
            _radiuses = new NativeArray<float>(radiuses, Allocator.Persistent);
            _transformAccess = new TransformAccessArray(transforms);
        }

        protected override void OnCollect(NativeArray<BoundingBox> bounds)
        {
            CollectBoundsJob collectBoundsJob = new(_radiuses, bounds);
            collectBoundsJob.Schedule(_transformAccess);
        }

        protected override void OnDispose()
        {
            _transformAccess.Dispose();
            _radiuses.Dispose();
        }
    }
}