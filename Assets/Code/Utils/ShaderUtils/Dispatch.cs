using UnityEngine;

namespace MyFolder.ComputeShaderNM
{
    public class Dispatch
    {
        private readonly Kernel _kernel;
        private readonly Vector3Int _dispatchSize;

        public Dispatch(Kernel kernel, Vector3Int payloadDispatchSize)
        {
            _dispatchSize = GetDispatchSize(payloadDispatchSize);
            _kernel = kernel;
        }

        public void Execute()
        {
            _computeShader.Dispatch(ID, _dispatchSize.x, _dispatchSize.y, _dispatchSize.z);
        }
    }
}