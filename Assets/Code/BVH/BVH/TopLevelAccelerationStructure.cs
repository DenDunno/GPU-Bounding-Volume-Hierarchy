using EditorWrapper;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    [ExecuteInEditMode]
    public class TopLevelAccelerationStructure : MonoBehaviour
    {
        [SerializeField] private BottomLevelAccelerationStructure _bvhCluster;
        [SerializeField] private BVHData _bvhData;
        private IDrawable _visualization;

        public void Initialize(BottomLevelAccelerationStructure bvhCluster)
        {
            _bvhCluster = bvhCluster;
        }

        private void OnValidate()
        {
            if (_bvhCluster != null)
            {
                _visualization = new DrawableIfTrue(
                    new VisualizationFactory(_bvhData).Create(_bvhCluster.Tree, 0),
                    _bvhData.Visualization.Show);   
            }
        }

        private void OnDrawGizmos()
        {
            _visualization?.Draw();
        }

        private void OnDestroy()
        {
            FindFirstObjectByType<BoundingVolumeHierarchy>().Remove(this);
        }
    }
}