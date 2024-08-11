using MyFolder.ComputeShaderNM;
using UnityEngine;

namespace Code
{
    public class GPUPrefixSum
    {
        private readonly Kernel _downSweepKernel;
        private readonly Kernel _upSweepKernel;
        private readonly ComputeBuffer _output;
        private readonly ComputeBuffer _input;
        private readonly int _size;

        public GPUPrefixSum(ComputeBuffer input, ComputeShader shader)
        {
            _input = input;
            _size = input.count;
            _output = new ComputeBuffer(input.count, input.stride);
            _upSweepKernel = new Kernel(shader, "UpSweep", new Vector3Int(_size, 1, 1));
            _downSweepKernel = new Kernel(shader, "DownSweep", new Vector3Int(_size, 1, 1));
        }

        public void Execute()
        {
            UpSweep();
        }

        private void UpSweep()
        {
            for (int i = 1; i < _size; i *= 2)
            {
                
            }
        }
    }
}