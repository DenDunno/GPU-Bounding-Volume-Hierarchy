using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    [ExecuteInEditMode]
    public class BVHDebugView : MonoBehaviour
    {
        [SerializeField] private GPUBoundingVolumeHierarchy _bvh;
        [SerializeField] private BVHDebug _debug;
        
        private void OnEnable()
        {
            _bvh.Rebuilt += OnBVHRebuilt;
        }
        
        private void OnDisable()
        {
            _bvh.Rebuilt -= OnBVHRebuilt;
        }

        private void OnBVHRebuilt()
        {
            _debug.Initialize(_bvh.GPUBridge.FetchInnerNodes(), (uint)_bvh.GPUBridge.FetchRoot());
        }
    }
}