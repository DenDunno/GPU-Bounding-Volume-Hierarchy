using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHTestPayload : MonoBehaviour
    {
        [SerializeField] private BVHCluster _bvhCluster;
        [SerializeField] private Sphere[] _spheres;

        [Button]
        public void Rebuild()
        {
        }

        private void Update()
        {
            Rebuild();
        }
    }
}