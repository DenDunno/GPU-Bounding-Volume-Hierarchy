using UnityEngine;

namespace MyFolder.ComputeShaderNM
{
    public class Kernel
    {
        public readonly int ID;
        public readonly Vector3Int ThreadsPerGroup;
        private readonly ComputeShader _computeShader;
        public readonly string Name;

        public Kernel(ComputeShader computeShader, string name)
        {
            Name = name;
            _computeShader = computeShader;
            ID = computeShader.FindKernel(name);
            ThreadsPerGroup = GetThreadsPerGroup();
        }

        public Vector3Int ComputeThreadGroups(int payloadX = 1, int payloadY = 1, int payloadZ = 1)
        {
            return ComputeThreadGroups(new Vector3Int(payloadX, payloadY, payloadZ));
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
        
        public void Dispatch(int threadGroupX = 1, int threadGroupY = 1, int threadGroupZ = 1)
        {
            Dispatch(new Vector3Int(threadGroupX, threadGroupY, threadGroupZ));
        }

        public void Dispatch(Vector3Int threadGroup)
        {
            _computeShader.Dispatch(ID, threadGroup.x, threadGroup.y, threadGroup.z);
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