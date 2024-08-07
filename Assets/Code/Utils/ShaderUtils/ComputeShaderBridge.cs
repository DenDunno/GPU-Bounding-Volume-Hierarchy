using UnityEngine;

namespace Code.Utils.ShaderUtils
{
    public class ComputeShaderBridge : IShaderBridge
    {
        private readonly ComputeShader _computeShader;

        public ComputeShaderBridge(ComputeShader computeShader) => _computeShader = computeShader;

        public void SetInt(int id, int value) => _computeShader.SetInt(id, value);
        public void SetFloat(int id, float value) => _computeShader.SetFloat(id, value);
        public void SetVector(int id, Vector4 value) => _computeShader.SetVector(id, value);
        public void SetBuffer(int kernelId, int bufferId, ComputeBuffer value) =>
            _computeShader.SetBuffer(kernelId, bufferId, value);
    }
}