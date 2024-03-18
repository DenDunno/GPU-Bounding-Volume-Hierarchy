using Code.Data;
using Unity.Collections;
using UnityEngine;

namespace Code.Components.BoundsCollector
{
    public class SingleThreadedBoundsCollector : IBoundsCollector
    {
        private readonly Transform[] _transforms;
        private readonly float[] _radiuses;

        public SingleThreadedBoundsCollector(Transform[] transforms, float[] radiuses) 
        {
            _transforms = transforms;
            _radiuses = radiuses;
        }

        public void CollectTo(NativeArray<AABB> bounds)
        {
            for (int i = 0; i < _transforms.Length; ++i)
            {
                SphereBoundsCalculator boundsCalculator = new(_transforms[i].position, _radiuses[i]);
                bounds[i] = boundsCalculator.Evaluate();
            }
        }
    }
}