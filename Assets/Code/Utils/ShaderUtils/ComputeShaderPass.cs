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
            Setup(_kernel, _shaderBridge);
        }
        
        public void Execute(int payload)
        {
            Execute(payload, 1, 1);
        }
        
        public void Execute(int payloadX, int payloadY, int payloadZ)
        {
            OnPreDispatch(_shaderBridge, new Vector3Int(payloadX, payloadY, payloadZ));
            _kernel.DispatchPayload(payloadX, payloadY, payloadZ);
        }

        protected virtual void Setup(Kernel kernel, IShaderBridge<string> shaderBridge) {}
        protected virtual void OnPreDispatch(IShaderBridge<string> shaderBridge, Vector3Int payload) {}
    }
}