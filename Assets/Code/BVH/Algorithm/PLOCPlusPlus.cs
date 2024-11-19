using Code.Utils.ShaderUtils;
using MyFolder.ComputeShaderNM;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class PLOCPlusPLus : ComputeShaderPass, IBVHConstructionAlgorithm
    {
        private readonly BVHBuffers _buffers;

        public PLOCPlusPLus(ComputeShader shader, BVHBuffers buffers) : base(shader, "RunPlocPlusPlus")
        {
            _buffers = buffers;
        }
        
        protected override void Setup(Kernel kernel, IShaderBridge<string> shaderBridge)
        {
            shaderBridge.SetBuffer(kernel.ID, "SortedMortonCodes", _buffers.MortonCodes);
            shaderBridge.SetBuffer(kernel.ID, "ParentIds", _buffers.ParentIds);
            shaderBridge.SetBuffer(kernel.ID, "RootIndex", _buffers.Root);
            shaderBridge.SetBuffer(kernel.ID, "Nodes", _buffers.Nodes);
        }

        protected override void OnPreDispatch(IShaderBridge<string> shaderBridge, Vector3Int payload)
        {
            shaderBridge.SetInt("LeavesCount", payload.x);
        }
    }
}