using System.Collections.Generic;
using DefaultNamespace.Code.GeometryGeneration;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHHierarchyInputListener : MonoBehaviour, IObjectPlacementListener
    {
        [SerializeField] private BVHBakery _bvhBakery;
        
        public void Accept(IReadOnlyList<GameObject> objects)
        {
            // _bvhBakery.Data.BoxesInput.List = new List<GameObject>(objects);
            // _bvhBakery.Bake();
        }
    }
}