using Code.Components.MortonCodeAssignment.TestTree;
using EditorWrapper;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(StaticBVH))]
    public class BVHVisualization : InEditorLifetime
    {
        [SerializeField] private VisualizationData _data;
        [HideInInspector] [SerializeField] private BVHNode[] _nodes;
        [HideInInspector] [SerializeField] private int _root;
        private StaticBVH _bvh;
        private IDrawable _drawable;

        private void Awake()
        {
            _bvh = GetComponent<StaticBVH>();
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
            Debug.Log("BVHVisualization enabled");
            if (_nodes != null && _nodes.Length != 0)
            {
                TreeNode root = new TreeCalculator(_nodes, (uint)_root).Compute();
                BinaryTreeSize treeSize = new(root);
                treeSize.Initialize();
                
                _drawable = new DrawableIfTrue(new DrawableComposite(new IDrawable[]
                {
                    new DrawableIfTrue(new BinaryTreeVisualization(_data.BinaryTree, root, _nodes.Length, treeSize),
                        _data.BinaryTree.Show),
                    new DrawableIfTrue(new BVHTreeVisualization(_nodes, treeSize.Height, root, _data.BVHTree),
                        _data.BVHTree.Show),
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