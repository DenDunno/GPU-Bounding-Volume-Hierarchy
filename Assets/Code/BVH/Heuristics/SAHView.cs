using Code.Components.MortonCodeAssignment;
using TMPro;
using UnityEngine;

namespace Code
{
    public class SAHView : MonoBehaviour
    {
        [SerializeField] private GPUBoundingVolumeHierarchy _bvh;
        [SerializeField] private TMP_Text _text;

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
            UpdateView(_bvh.GPUBridge.FetchInnerNodes());
        }

        private void UpdateView(BVHNode[] nodes)
        {
            _text.text = $"SAH = {new SurfaceAreaHeuristic(nodes).Compute()}";
        }
    }
}