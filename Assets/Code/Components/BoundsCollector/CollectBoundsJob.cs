using Code.Data;
using Unity.Burst;
using Unity.Collections;
using UnityEngine.Jobs;

namespace Code.Components.BoundsCollector
{
    public struct CollectBoundsJob : IJobParallelForTransform
    {
        [ReadOnly] private readonly NativeArray<float> _radiuses;
        [WriteOnly] private NativeArray<AABB> _boundingBoxes;

        public CollectBoundsJob(NativeArray<float> radiuses, NativeArray<AABB> boundingBoxes)
        {
            _boundingBoxes = boundingBoxes;
            _radiuses = radiuses;
        }

        [BurstCompile]
        public void Execute(int index, TransformAccess transform)
        {
            SphereBoundsCalculator boundsCalculator = new(transform.position, _radiuses[index]);
            _boundingBoxes[index] = boundsCalculator.Evaluate();
        }
    }
}