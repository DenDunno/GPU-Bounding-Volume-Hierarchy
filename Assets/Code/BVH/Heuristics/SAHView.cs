using Code.Components.MortonCodeAssignment;
using TMPro;
using UnityEngine;

namespace Code
{
    [ExecuteInEditMode]
    public class SAHView : MonoBehaviour
    {
        [SerializeField] private GPUBoundingVolumeHierarchy _bvh;
        [SerializeField] private TMP_Text _text;

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
            UpdateView(_bvh.FetchInnerNodes());
        }

        private void UpdateView(BVHNode[] nodes)
        {
            _text.text = $"SAH = {new SurfaceAreaHeuristic(nodes).Compute()}";
        }
    }
}