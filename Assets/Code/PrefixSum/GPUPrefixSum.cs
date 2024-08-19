using UnityEngine;

namespace Code
{
    public class GPUPrefixSum : IGPUPrefixSum
    {
        public readonly Vector3Int ThreadGroups;
        private readonly GPUPrefixSumCommon _common;
        private readonly ComputeBuffer _input;
        private readonly int _size;

        public GPUPrefixSum(ComputeBuffer input, GPUPrefixSumCommon common)
        {
            _input = input;
            _common = common;
            _size = input.count;
            ThreadGroups = _common.ScanKernel.ComputeThreadGroups(_size);
        }

        private void SetupShader()
        {
            _common.Bridge.SetBuffer(_common.ScanKernel.ID, "Result", _input);
            _common.Bridge.SetInt("InputSize", _size);
        }

        public void Dispatch()
        {
            SetupShader();
            _common.ScanKernel.Dispatch(ThreadGroups);
        }
    }
}