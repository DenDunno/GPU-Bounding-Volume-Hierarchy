using System;
using Code.Utils.ShaderUtils;
using Code.Utils.ShaderUtils.Buffer;
using MyFolder.ComputeShaderNM;
using UnityEngine;

namespace Code
{
    public class GPURadixSort : IDisposable
    {
        private readonly CachedShaderBridge _shaderBridge;
        private readonly ComputeBuffer _input;
        private readonly Kernel _kernel;
        private readonly int[] _output;
        
        public GPURadixSort(int size, ComputeShader sortShader)
        {
            _shaderBridge = new CachedShaderBridge(new ComputeShaderBridge(sortShader));
            _kernel = new Kernel(sortShader, "CSMain", new Vector3Int(size, 1, 1));
            _input = new ComputeBuffer(size, sizeof(int));
            _output = new int[size];
        }

        public void Initialize<TCollection>(SetDataOperation<TCollection> setDataOperation)
        {
            setDataOperation.Execute(_input);
            _shaderBridge.SetBuffer(_kernel.ID, "Input", _input);
        }

        public int[] Execute()
        {
            _kernel.Dispatch();
            _input.GetData(_output);

            return _output;
        }

        public void Dispose()
        {
            _input.Dispose();
        }
    }
}