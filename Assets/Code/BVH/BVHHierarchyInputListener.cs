using System.Collections.Generic;
using DefaultNamespace.Code.GeometryGeneration;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHHierarchyInputListener : MonoBehaviour, IObjectPlacementListener
    {
        [SerializeField] private StaticBVH _bvh;
        
        public void Accept(IReadOnlyList<GameObject> objects)
        {
            _bvh.Data.BoxesInput.List = new List<GameObject>(objects);
        }
    }
}