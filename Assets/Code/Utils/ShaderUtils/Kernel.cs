using UnityEngine;

namespace MyFolder.ComputeShaderNM
{
    public class Kernel
    {
        public readonly int ID;
        public readonly Vector3Int ThreadsPerGroup;
        private readonly ComputeShader _computeShader;

        public Kernel(ComputeShader computeShader, string name)
        {
            _computeShader = computeShader;
            ID = computeShader.FindKernel(name);
            ThreadsPerGroup = GetThreadsPerGroup();
        }

        public Vector3Int ComputeThreadGroups(Vector3Int payload)
        {
            return new Vector3Int()
            {
                x = Mathf.CeilToInt((float)payload.x / ThreadsPerGroup.x),
                y = Mathf.CeilToInt((float)payload.y / ThreadsPerGroup.y),
                z = Mathf.CeilToInt((float)payload.z / ThreadsPerGroup.z),
            };
        }
        
        public void Dispatch(int dispatchX = 1, int dispatchY = 1, int dispatchZ = 1)
        {
            Dispatch(new Vector3Int(dispatchX, dispatchY, dispatchZ));
        }

        public void Dispatch(Vector3Int dispatch)
        {
            _computeShader.Dispatch(ID, dispatch.x, dispatch.y, dispatch.z);
        }

        private Vector3Int GetThreadsPerGroup()
        {
            _computeShader.GetKernelThreadGroupSizes(ID,
                out uint threadSizeX,
                out uint threadSizeY,
                out uint threadSizeZ);

            return new Vector3Int((int)threadSizeX, (int)threadSizeY, (int)threadSizeZ);
        }
    }
}