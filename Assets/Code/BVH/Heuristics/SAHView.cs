using Code.Components.MortonCodeAssignment;
using TMPro;
using UnityEngine;

namespace Code
{
    public class SAHView : MonoBehaviour
    {
        [SerializeField] private BVHBakery _bvhBakery;
        [SerializeField] private TMP_Text _text;

        private void UpdateView(BVHNode[] nodes)
        {
            _text.text = $"SAH = {new SurfaceAreaHeuristic(nodes).Compute()}";
        }
    }
}