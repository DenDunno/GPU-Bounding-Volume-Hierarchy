using UnityEngine;

namespace MyFolder.ComputeShaderNM
{
    public class Kernel
    {
        private readonly ComputeShader _computeShader;
        public readonly int ID;

        public Kernel(ComputeShader computeShader, string name)
        {
            _computeShader = computeShader;
            ID = computeShader.FindKernel(name);
        }

        private Vector3Int ComputeOptimalDispatchSize(Vector3Int payloadDispatchSize)
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
        
        public void Dispatch(Vector3Int dispatch)
        {
            _computeShader.Dispatch(ID, dispatch.x, dispatch.y, dispatch.z);
        }
    }
}