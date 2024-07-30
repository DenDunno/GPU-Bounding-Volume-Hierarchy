using UnityEngine;

namespace MyFolder.ComputeShaderNM
{
    public class Kernel
    {
        public readonly int ID;
        private readonly ComputeShader _computeShader;
        private readonly Vector3Int _dispatchSize;

        public Kernel(ComputeShader computeShader, string name, Vector3Int payloadDispatchSize)
        {
            _computeShader = computeShader;
            ID = computeShader.FindKernel(name);
            _dispatchSize = GetDispatchSize(payloadDispatchSize);
        }

        private Vector3Int GetDispatchSize(Vector3Int payloadDispatchSize)
        {
            _computeShader.GetKernelThreadGroupSizes(ID, 
                out uint threadSizeX,
                out uint threadSizeY,
                out uint threadSizeZ);

            return new Vector3Int()
            {
                x = Mathf.CeilToInt((float)payloadDispatchSize.x / threadSizeX),
                y = Mathf.CeilToInt((float)payloadDispatchSize.y / threadSizeY),
                z = Mathf.CeilToInt((float)payloadDispatchSize.z / threadSizeZ),
            };
        }

        public void Dispatch()
        {
            _computeShader.Dispatch(ID, _dispatchSize.x, _dispatchSize.y, _dispatchSize.z);
        }
    }
}