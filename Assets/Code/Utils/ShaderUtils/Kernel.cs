using UnityEngine;

namespace MyFolder.ComputeShaderNM
{
    public class Kernel
    {
        public readonly int ID;
        public readonly Vector3Int ThreadSizes;
        private readonly ComputeShader _computeShader;

        public Kernel(ComputeShader computeShader, string name)
        {
            _computeShader = computeShader;
            ID = computeShader.FindKernel(name);
            ThreadSizes = GetThreadSizes();
        }

        public Vector3Int ComputeOptimalDispatchSize(Vector3Int payloadDispatchSize)
        {
            return new Vector3Int()
            {
                x = Mathf.CeilToInt((float)payloadDispatchSize.x / ThreadSizes.x),
                y = Mathf.CeilToInt((float)payloadDispatchSize.y / ThreadSizes.y),
                z = Mathf.CeilToInt((float)payloadDispatchSize.z / ThreadSizes.z),
            };
        }
        
        public void Dispatch(int dispatchX = 1, int dispatchY = 1, int dispatchZ = 1)
        {
            Dispatch(new Vector3Int(dispatchX, dispatchY, dispatchZ));
        }

        private void Dispatch(Vector3Int dispatch)
        {
            _computeShader.Dispatch(ID, dispatch.x, dispatch.y, dispatch.z);
        }

        private Vector3Int GetThreadSizes()
        {
            _computeShader.GetKernelThreadGroupSizes(ID,
                out uint threadSizeX,
                out uint threadSizeY,
                out uint threadSizeZ);

            return new Vector3Int((int)threadSizeX, (int)threadSizeY, (int)threadSizeZ);
        }
    }
}