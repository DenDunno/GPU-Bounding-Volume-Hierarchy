using System.Collections.Generic;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class BoundingVolumeHierarchy : MonoBehaviour
    {
        [SerializeField] private List<TopLevelAccelerationStructure> _topLevelStructures;

        public void Add(TopLevelAccelerationStructure topLevelStructure)
        {
            _topLevelStructures.Add(topLevelStructure);
        }

        public void Remove(TopLevelAccelerationStructure topLevelAccelerationStructure)
        {
            _topLevelStructures.Remove(topLevelAccelerationStructure);
        }
    }
}