using UnityEngine;

namespace MyFolder.ComputeShaderNM
{
    public class Kernel
    {
        public readonly int ID;
        public readonly Vector3Int DispatchSize;
        private readonly ComputeShader _computeShader;

        public Kernel(ComputeShader computeShader, string name, Vector3Int payloadDispatchSize)
        {
            _computeShader = computeShader;
            ID = computeShader.FindKernel(name);
            DispatchSize = GetDispatchSize(payloadDispatchSize);
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
            _computeShader.Dispatch(ID, DispatchSize.x, DispatchSize.y, DispatchSize.z);
        }
    }
}