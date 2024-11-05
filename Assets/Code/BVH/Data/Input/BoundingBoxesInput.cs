using System;
using System.Collections.Generic;
using Code.Data;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    [Serializable]
    public class BoundingBoxesInput : IValidatedData
    {
        [SerializeField] private bool _buildFromMesh = true;
        [SerializeField] private List<MonoBehaviour> _value;
        [SerializeField] private MeshFilter _meshFilter;

        public bool IsValid()
        {
            return _buildFromMesh ? 
                _meshFilter != null : 
                _value is { Count: > 0 };
        }

        public IBoundingBoxesInput Value => _buildFromMesh ? 
            new InputFromMesh(_meshFilter.sharedMesh) :
            new ManualBoundingBoxesInput(_value);

        public int Count => _buildFromMesh ?
            _meshFilter.sharedMesh.triangles.Length / 3 : 
            _value.Count;

        public void OnValidate()
        {
            for (int i = 0; i < _value.Count; ++i)
            {
                if (_value[i] is IAABBProvider == false)
                {
                    Debug.LogError($"GameObject {_value[i].gameObject.name} must implement IAABBProvider");
                    _value.RemoveAt(i--);
                }
            }
        }
    }
}