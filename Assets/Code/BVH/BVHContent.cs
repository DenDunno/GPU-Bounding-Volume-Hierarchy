using System;
using System.Collections.Generic;
using Code.Data;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    [Serializable]
    public class BVHContent
    {
        [SerializeField] public List<AABB> BoundingBoxes;

        public int Count => BoundingBoxes.Count;
    }
}