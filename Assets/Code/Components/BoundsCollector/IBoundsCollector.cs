using Code.Data;
using Unity.Collections;

namespace Code.Components.BoundsCollector
{
    public interface IBoundsCollector 
    {
        void CollectTo(NativeArray<AABB> bounds);
        void Dispose() {}
    }
}