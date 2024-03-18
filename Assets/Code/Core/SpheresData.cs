using System;
using System.Linq;
using UnityEngine;

namespace Code.Core
{
    [Serializable]
    public class SpheresData : ISphereDataValidateCallback
    {
        [SerializeField] private Sphere[] _spheres = Array.Empty<Sphere>();
        [SerializeField] private int _maxSpheres = 100;
        
        public Transform[] Transforms => _spheres.Select(x => x.transform).ToArray();
        public float[] Radiuses { get; private set; }
        public int SpheresCount => _spheres.Length;
        public int MaxSpheres => _maxSpheres;
        
        public void Initialize()
        {
            Radiuses = _spheres.Select(x => x.Data.Radius).ToArray();

            for (int i = 0; i < _spheres.Length; ++i)
            {
                _spheres[i].InjectOnChangedCallback(this, i);
            }
        }

        public void UpdateByIndex(int index)
        {
            Radiuses[index] = _spheres[index].Data.Radius;
        }
    }
}