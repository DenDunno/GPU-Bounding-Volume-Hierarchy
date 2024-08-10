using UnityEngine;

namespace Code
{
    public class GPUPrefixSum
    {
        private readonly ComputeBuffer _output;
        private readonly ComputeBuffer _input;

        public GPUPrefixSum(ComputeBuffer input)
        {
            _output = new ComputeBuffer(input.count, input.stride);
            _input = input;
        }

        public void Execute()
        {
            
        }
    }
}