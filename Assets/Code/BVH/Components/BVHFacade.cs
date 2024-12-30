using System;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHFacade : IDisposable
    {
        public readonly BVHComponents Components;

        public BVHFacade(BVHData data, BVHShaders shaders)
        {
            Components = new BVHComponents(data, shaders);
        }
        
        public void Initialize()
        {
            Components.Algorithm.Initialize();
            Components.Buffers.Boxes.SetData(Components.BoxesInput.Calculate());
        }

        public void Rebuild()
        {
            Components.Algorithm.Execute(Components.Buffers.Size);
        }

        public void Dispose()
        {
            Components.Algorithm.Dispose();
        }
    }
}