using System;
using Code.Data;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    [Serializable]
    public class SceneSize : VisualizationElementDataBase
    {
        [SerializeField] private float _sceneSize = 10;
        
        public AABB Box => new(_sceneSize * Vector3.one, -_sceneSize * Vector3.one);
    }
}