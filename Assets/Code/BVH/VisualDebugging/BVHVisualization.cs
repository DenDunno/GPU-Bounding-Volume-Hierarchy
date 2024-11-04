using Code.Components.MortonCodeAssignment.TestTree;
using EditorWrapper;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(GPUBoundingVolumeHierarchy))]
    public class BVHVisualization : InEditorLifetime
    {
        [SerializeField] private VisualizationData _data;
        [HideInInspector] [SerializeField] private BVHNode[] _nodes;
        [HideInInspector] [SerializeField] private int _root;
        private GPUBoundingVolumeHierarchy _bvh;
        private IDrawable _drawable;
        
        private void Awake()
        {
            _bvh = GetComponent<GPUBoundingVolumeHierarchy>();
        }

        private void OnEnable()
        {
            _bvh.RebuiltEvent.AddListener(OnRebuilt);
        }

        private void OnDisable()
        {
            _bvh.RebuiltEvent.RemoveListener(OnRebuilt);
        }

        protected override void Reassemble()
        {
            if (_nodes != null && _nodes.Length != 0)
            {
                TreeNode root = new TreeCalculator(_nodes, (uint)_root).Compute();
                BinaryTreeVisualization binaryTree = new(_data.BinaryTree, root, _nodes.Length);
                binaryTree.Initialize();   
                
                _drawable = new DrawableIfTrue(new DrawableComposite(new IDrawable[]
                {
                    new DrawableIfTrue(binaryTree, _data.BinaryTree.Show),
                }), enabled);
            }
        }

        private void OnRebuilt()
        {
            _nodes = _bvh.GPUBridge.FetchInnerNodes();
            _root = _bvh.GPUBridge.FetchRoot();
            Reassemble();
        }
        
        private void OnDrawGizmos()
        {
            _drawable?.Draw();   
        }
    }
}