using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.RenderFeature.Data
{
    public class SharedBuffers : IDisposable
    {
        public readonly ComputeBuffer VisibleSpheres;
        public readonly ComputeBuffer BoundingBoxes;
        public readonly ComputeBuffer Spheres;

        public SharedBuffers(int maxSpheres)
        {
            VisibleSpheres = new ComputeBuffer(maxSpheres, sizeof(int), ComputeBufferType.Counter);
            Spheres = new ComputeBuffer(maxSpheres, SphereData.GetSize());
            //BoundingBoxes = new ComputeBuffer(maxSpheres, AABB.GetSize());
        }

        public int SpheresCount { get; private set; }

        public void Update(List<SphereData> sphereData)
        {
            SpheresCount = sphereData.Count;
            Spheres.SetData(sphereData);
        }

        public void Dispose()
        {
            VisibleSpheres.Dispose();
            BoundingBoxes.Dispose();
            Spheres.Dispose();
        }
    }
}