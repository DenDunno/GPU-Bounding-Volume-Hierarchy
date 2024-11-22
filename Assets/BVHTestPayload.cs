using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHTestPayload : MonoBehaviour
    {
        [SerializeField] private StaticBVH _bvh;
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