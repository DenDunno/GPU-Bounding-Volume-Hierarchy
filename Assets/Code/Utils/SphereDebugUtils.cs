using Code.Components.BoundsCollector;
using Code.Core;
using Code.Data;
using Unity.Collections;

namespace Code.Utils
{
    public abstract class SphereDebugUtils
    {
        public static void DrawSpheresBounds(SpheresData data)
        {
            NativeArray<AABB> bounds = new(data.SpheresCount, Allocator.Temp);
            SingleThreadedBoundsCollector boundsCollector = new(data.Transforms, data.Radiuses);
            boundsCollector.CollectTo(bounds);
            
            for (int i = 0; i < data.SpheresCount; ++i)
            {
                bounds[i].Draw();
            }

            bounds.Dispose();
        }
    }
}