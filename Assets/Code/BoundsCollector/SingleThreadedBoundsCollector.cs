using Unity.Collections;
using UnityEngine;

namespace Code.SpheresBoundsCollector
{
    public class SingleThreadedBoundsCollector : BoundsCollector
    {
        private readonly Transform[] _transforms;
        private readonly float[] _radiuses;

        public SingleThreadedBoundsCollector(Transform[] transforms, float[] radiuses) : base(radiuses.Length)
        {
            _transforms = transforms;
            _radiuses = radiuses;
        }

        protected override void OnCollect(NativeArray<BoundingBox> bounds)
        {
            for (int i = 0; i < _transforms.Length; ++i)
            {
                SphereBoundsCalculator boundsCalculator = new(_transforms[i].position, _radiuses[i]);
                bounds[i] = boundsCalculator.Evaluate();
            }
        }
    }
}