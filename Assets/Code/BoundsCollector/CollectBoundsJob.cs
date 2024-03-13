using Unity.Burst;
using Unity.Collections;
using UnityEngine.Jobs;

namespace Code.SpheresBoundsCollector
{
    public struct CollectBoundsJob : IJobParallelForTransform
    {
        [ReadOnly] private readonly NativeArray<float> _radiuses;
        [WriteOnly] private NativeArray<BoundingBox> _boundingBoxes;

        public CollectBoundsJob(NativeArray<float> radiuses, NativeArray<BoundingBox> boundingBoxes)
        {
            _radiuses = radiuses;
            _boundingBoxes = boundingBoxes;
        }

        [BurstCompile]
        public void Execute(int index, TransformAccess transform)
        {
            SphereBoundsCalculator boundsCalculator = new(transform.position, _radiuses[index]);
            _boundingBoxes[index] = boundsCalculator.Evaluate();
        }
    }
}