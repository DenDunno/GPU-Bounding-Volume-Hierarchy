using System;
using System.Linq;
using Code.View;
using UnityEngine;

namespace Code
{
    [Serializable]
    public class SpheresData
    {
        [SerializeField] private Sphere[] _spheres = Array.Empty<Sphere>();

        public Transform[] Transforms => _spheres.Select(x => x.transform).ToArray();
        public float[] Radiuses { get; private set; }
        public int SpheresCount => _spheres.Length;

        public void Initialize()
        {
            Radiuses = _spheres.Select(x => x.Data.Radius).ToArray();
        }
    }
}