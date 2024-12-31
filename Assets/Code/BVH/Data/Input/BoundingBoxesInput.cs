using System;
using System.Collections.Generic;
using System.Linq;
using Code.Data;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    [Serializable]
    public class BoundingBoxesInput : IValidatedData
    {
        [SerializeField] private bool _buildFromMesh = true;
        [SerializeField] private MeshFilter _meshFilter;
        public List<GameObject> List;

        public bool IsValid()
        {
            return _buildFromMesh ? 
                _meshFilter != null : 
                List is { Count: > 0 };
        }

        public IBoundingBoxesInput Value => _buildFromMesh ? 
            new InputFromMesh(_meshFilter.sharedMesh) :
            new ManualBoundingBoxesInput(List.Select(x => x.GetComponent<IAABBProvider>()).ToArray());

        public void OnValidate()
        {
            for (int i = 0; i < List.Count; ++i)
            {
                if (List[i].GetComponent<IAABBProvider>() == null)
                {
                    Debug.LogError($"GameObject {List[i].gameObject.name} must implement IAABBProvider");
                    List.RemoveAt(i--);
                }
            }
        }
    }
}