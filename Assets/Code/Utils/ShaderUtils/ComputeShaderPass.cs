using Code.Utils.ShaderUtils;
using MyFolder.ComputeShaderNM;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public abstract class ComputeShaderPass
    {
        private readonly IShaderBridge<string> _shaderBridge;
        private readonly Kernel _kernel;

        protected ComputeShaderPass(ComputeShader shader, string kernelName)
        {
            _shaderBridge = new CachedShaderBridge(new ComputeShaderBridge(shader));
            _kernel = new Kernel(shader, kernelName);
        }

        public void Prepare()
        {
            Setup(_shaderBridge, _kernel.ID);
        }

        public void Execute(int payload)
        {
            Execute(payload, 1, 1);
        }

        public void Execute(int payloadX, int payloadY, int payloadZ)
        {
            Execute(_shaderBridge, new Vector3Int(payloadX, payloadY, payloadZ));
        }

        protected virtual void Execute(IShaderBridge<string> shaderBridge, Vector3Int payload)
        {
            OnPreDispatch(_shaderBridge, payload);
            Dispatch(payload.x, payload.y, payload.z);
        }

        protected void Dispatch(int payloadX = 1, int payloadY = 1, int payloadZ = 1)
        {
            _kernel.DispatchPayload(payloadX, payloadY, payloadZ);
        }

        protected virtual void Setup(IShaderBridge<string> shaderBridge, int kernelId) {}
        protected virtual void OnPreDispatch(IShaderBridge<string> shaderBridge, Vector3Int payload) {}
    }
}