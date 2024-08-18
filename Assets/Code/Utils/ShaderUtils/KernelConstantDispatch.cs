using UnityEngine;

namespace MyFolder.ComputeShaderNM
{
    public class KernelConstantDispatch
    {
        public readonly Vector3Int ThreadGroups;
        private readonly Kernel _kernel;

        public KernelConstantDispatch(Kernel kernel, Vector3Int payload)
        {
            ThreadGroups = kernel.ComputeThreadGroups(payload);
            _kernel = kernel;
        }
        
        public void Execute()
        {
            _kernel.Dispatch(ThreadGroups);
        }
    }
}