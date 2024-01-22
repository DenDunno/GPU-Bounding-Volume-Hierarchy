using System.Collections.Generic;
using UnityEngine;

namespace Code.RenderFeature
{
    public class IntersectingSpheresManager : MonoBehaviour
    {
        private List<SphereData> _spheres;

        public int UpdateBuffer(SphereData sphere, int index)
        {
            if (index == -1)
            {
                index = Add(sphere);
            }
            
            return index;
        }

        private int Add(SphereData sphere)
        { 
            _spheres.Add(sphere);

            return _spheres.Count - 1;
        }

        public void Remove(SphereData sphere, int index)
        {
            _spheres[index] = _spheres[^1];
            _spheres.RemoveAt(_spheres.Count - 1);
        }
    }
}