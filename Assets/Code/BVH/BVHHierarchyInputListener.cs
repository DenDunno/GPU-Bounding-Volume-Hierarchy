using System.Collections.Generic;
using DefaultNamespace.Code.GeometryGeneration;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHHierarchyInputListener : MonoBehaviour, IObjectPlacementListener
    {
        [SerializeField] private BVHCluster _bvhCluster;
        
        public void Accept(IReadOnlyList<GameObject> objects)
        {
            _bvhCluster.Data.BoxesInput.List = new List<GameObject>(objects);
            _bvhCluster.Bake();
        }
    }
}