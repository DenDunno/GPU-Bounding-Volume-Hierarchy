using Code.Utils.Extensions;
using UnityEngine;
using UnityEngine.Rendering;

namespace Code.Utils.ShaderUtils
{
    public class CommandBufferComputeShaderBridge : IShaderBridge<int>, IShaderBridge<string>
    {
        private readonly CommandBuffer _commands;
        private readonly ComputeShader _shader;

        public CommandBufferComputeShaderBridge(CommandBuffer commands, ComputeShader shader)
        {
            _commands = commands;
            _shader = shader;
        }

        public void SetInt(int key, int value) => _commands.SetComputeIntParam(_shader, key, value);
        public void SetInt(string key, int value) => _commands.SetComputeIntParam(_shader, key, value);
        public void SetFloat(int key, float value) => _commands.SetComputeFloatParam(_shader, key, value);
        public void SetFloat(string key, float value) => _commands.SetComputeFloatParam(_shader, key, value);
        public void SetVector(int key, Vector4 value) => _commands.SetComputeVectorParam(_shader, key, value);
        public void SetVector(string key, Vector4 value) => _commands.SetComputeVectorParam(_shader, key, value);
        public void SetColor(int key, Color value) => SetVector(key, value.ToVector4());
        public void SetColor(string key, Color value) => SetVector(key, value.ToVector4());
        public void SetBuffer(int kernelId, int key, ComputeBuffer value) => _commands.SetComputeBufferParam(_shader, kernelId, key, value);
        public void SetBuffer(int kernelId, string key, ComputeBuffer value) => _commands.SetComputeBufferParam(_shader, kernelId, key, value);
    }
}