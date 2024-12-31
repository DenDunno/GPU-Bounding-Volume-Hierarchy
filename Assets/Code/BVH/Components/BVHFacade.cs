using System;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHFacade : IDisposable
    {
        private readonly BVHComponents _components;

        public BVHFacade(BVHData data, IBoundingBoxesInput boxesInput, BVHShaders shaders)
        {
            _components = new BVHComponents(data, shaders, boxesInput);
        }
        
        public void Initialize()
        {
            _components.Algorithm.Initialize();
        }

        public BVHNode[] FetchTree()
        {
            return _components.GPUBridge.FetchTree();
        }
        
        public void Rebuild()
        {
            _components.Buffers.Boxes.SetData(_components.BoxesInput.Calculate());
            _components.Algorithm.Execute(_components.Buffers.Size);
        }

        public void Dispose()
        {
            _components.Algorithm.Dispose();
        }
    }
}