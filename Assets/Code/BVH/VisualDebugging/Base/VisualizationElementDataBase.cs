using System;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    [Serializable]
    public abstract class VisualizationElementDataBase
    {
        [field: SerializeField] public bool Show { get; private set; }
    }
}