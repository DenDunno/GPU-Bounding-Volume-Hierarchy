using System;
using Code.Components.BoundsCollector;
using Code.Components.MortonCodeAssignment;

namespace Code.Core
{
    public class SpheresComponents : IDisposable
    {
        public readonly SpheresBoundUpdate SpheresBoundUpdate;
        public readonly MortonCodeAssignment MortonCodeAssignment;
        private bool _wasDisposed;

        public SpheresComponents(SpheresData data, SphereBuffers buffers)
        {
            IBoundsCollector boundsCollector = new BoundsCollectorProvider().Create(data.Transforms, data.Radiuses);
            SpheresBoundUpdate = new SpheresBoundUpdate(boundsCollector, buffers.BoundingBoxes);
            MortonCodeAssignment = new MortonCodeAssignment(buffers.BoundingBoxes.Value, buffers.Nodes);
        }

        public void Initialize()
        {
        }

        public void Dispose()
        {
            CleanUp();
            GC.SuppressFinalize(this);
        }

        ~SpheresComponents()
        {
            CleanUp();
        }

        private void CleanUp()
        {
            if (_wasDisposed == false)
            {
                _wasDisposed = true;
                SpheresBoundUpdate.Dispose();
            }
        }
    }
}