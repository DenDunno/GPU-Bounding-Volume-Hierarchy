using System;
using Code.SpheresBoundsCollector;

namespace Code
{
    public class SpheresComponents : IDisposable
    {
        public readonly BoundsCollector BoundsCollector;
        private bool _wasDisposed;

        public SpheresComponents(SpheresData data)
        {
            BoundsCollector = new BoundsCollectorProvider().Create(data.Transforms, data.Radiuses);
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
                BoundsCollector.Dispose(); 
            }
        }
    }
}