using UnityEngine;

namespace Code.Components.BoundsCollector
{
    public class BoundsCollectorProvider
    {
        public IBoundsCollector Create(Transform[] transforms, float[] radiuses)
        {
            if (Application.isPlaying)
            {
                return new MultiThreadedBoundsCollector(transforms, radiuses);
            }

            return new SingleThreadedBoundsCollector(transforms, radiuses);
        }
    }
}