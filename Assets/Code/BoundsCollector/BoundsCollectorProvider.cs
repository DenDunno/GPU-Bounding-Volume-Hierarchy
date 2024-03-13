using UnityEngine;

namespace Code.SpheresBoundsCollector
{
    public class BoundsCollectorProvider
    {
        public BoundsCollector Create(Transform[] transforms, float[] radiuses)
        {
            if (Application.isPlaying)
            {
                return new MultiThreadedBoundsCollector(transforms, radiuses);
            }

            return new SingleThreadedBoundsCollector(transforms, radiuses);
        }
    }
}