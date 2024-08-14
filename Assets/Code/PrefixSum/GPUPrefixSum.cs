using Code.Utils.ShaderUtils;
using MyFolder.ComputeShaderNM;
using UnityEngine;

namespace Code
{
    public class GPUPrefixSum
    {
        private readonly IShaderBridge<string> _shaderBridge;
        private readonly Kernel _downSweepKernel;
        private readonly Kernel _upSweepKernel;
        private readonly ComputeBuffer _input;
        private readonly int _size;

        public GPUPrefixSum(ComputeBuffer input, ComputeShader shader)
        {
            _shaderBridge = new CachedShaderBridge(new ComputeShaderBridge(shader));
            _downSweepKernel = new Kernel(shader, "DownSweep");
            _upSweepKernel = new Kernel(shader, "UpSweep");
            _size = input.count;
            _input = input;
        }

        public void Initialize()
        {
            _shaderBridge.SetBuffer(_upSweepKernel.ID, "Result", _input);
            _shaderBridge.SetInt("LastIndex", _size - 1);
        }

        public void Dispatch()
        {
            UpSweep();
            DownSweep();
        }

        private void UpSweep()
        {
            for (int step = 1; step < _size; step *= 2)
            {
                int threadsTotal = Mathf.CeilToInt(_size / (step * 2f));
                _shaderBridge.SetInt("Step", step);
                _upSweepKernel.Dispatch(threadsTotal);
            }
        }

        private void DownSweep()
        {
            for (int i = 1; i < _size; i *= 2)
            {
            }
        }
    }
}