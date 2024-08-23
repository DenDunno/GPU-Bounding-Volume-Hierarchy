using UnityEngine;
using UnityEngine.Rendering;

namespace Code.Utils.ShaderUtils
{
    public class CommandBufferComputeShaderBridge : IShaderBridge<int>, IShaderBridge<string>
    {
        private readonly CommandBuffer _commandBuffer;
        private readonly ComputeShader _shader;

        public CommandBufferComputeShaderBridge(CommandBuffer commandBuffer, ComputeShader shader)
        {
            _commandBuffer = commandBuffer;
            _shader = shader;
        }

        public void SetInt(int key, int value) => _commandBuffer.SetComputeIntParam(_shader, key, value);
        public void SetInt(string key, int value) => _commandBuffer.SetComputeIntParam(_shader, key, value);
        public void SetFloat(int key, float value) => _commandBuffer.SetComputeFloatParam(_shader, key, value);
        public void SetFloat(string key, float value) => _commandBuffer.SetComputeFloatParam(_shader, key, value);
        public void SetVector(int key, Vector4 value) => _commandBuffer.SetComputeVectorParam(_shader, key, value);
        public void SetVector(string key, Vector4 value) => _commandBuffer.SetComputeVectorParam(_shader, key, value);
        public void SetBuffer(int kernelId, int key, ComputeBuffer value) => _commandBuffer.SetComputeBufferParam(_shader, kernelId, key, value);
        public void SetBuffer(int kernelId, string key, ComputeBuffer value) => _commandBuffer.SetComputeBufferParam(_shader, kernelId, key, value);
    }
}