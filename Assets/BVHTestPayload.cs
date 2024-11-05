using System.Linq;
using Code.Data;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
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