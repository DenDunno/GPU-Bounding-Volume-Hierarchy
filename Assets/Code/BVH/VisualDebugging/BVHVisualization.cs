using Code.Components.MortonCodeAssignment.TestTree;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(GPUBoundingVolumeHierarchy))]
    public class BVHVisualization : MonoBehaviour
    {
        [SerializeField] private BinaryTreeDebug _binaryTreeDebug;
        [SerializeField] private BVHTreeDebug _bvhTreeDebug;
        private GPUBoundingVolumeHierarchy _bvh;

        private void Awake()
        {
            _bvh = GetComponent<GPUBoundingVolumeHierarchy>();
        }

        private void OnEnable()
        {
            _bvh.RebuiltEvent.AddListener(OnBVHRebuilt);
        }
        
        private void OnDisable()
        {
            _bvh.RebuiltEvent.RemoveListener(OnBVHRebuilt);
        }

        private void OnBVHRebuilt()
        {
            BVHNode[] innerNodes = _bvh.GPUBridge.FetchInnerNodes();
            uint root = (uint)_bvh.GPUBridge.FetchRoot();
            
            BinaryTree tree = new(new TreeCalculator(innerNodes, root).Compute());
            _binaryTreeDebug.Initialize(innerNodes.Length, tree.Root);
            _bvhTreeDebug.Initialize(tree, innerNodes);
        }

        private void OnDrawGizmos()
        {
            _binaryTreeDebug.TryDraw();
            _bvhTreeDebug.TryDraw();
        }
    }
}