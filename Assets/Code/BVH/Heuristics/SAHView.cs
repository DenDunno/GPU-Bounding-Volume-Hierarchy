using Code.Components.MortonCodeAssignment;
using TMPro;
using UnityEngine;

namespace Code
{
    public class SAHView : MonoBehaviour
    {
        [SerializeField] private BVHCluster _bvhCluster;
        [SerializeField] private TMP_Text _text;

        private void OnEnable()
        {
            _bvhCluster.RebuiltEvent.AddListener(OnBVHRebuilt);
        }
        
        private void OnDisable()
        {
            _bvhCluster.RebuiltEvent.RemoveListener(OnBVHRebuilt);
        }

        private void OnBVHRebuilt()
        {
            UpdateView(_bvhCluster.GPUBridge.FetchTree());
        }

        private void UpdateView(BVHNode[] nodes)
        {
            _text.text = $"SAH = {new SurfaceAreaHeuristic(nodes).Compute()}";
        }
    }
}