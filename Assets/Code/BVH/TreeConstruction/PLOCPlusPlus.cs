using Code.Utils.Extensions;
using Code.Utils.ShaderUtils;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class PLOCPlusPLus : ComputeShaderPass, IBVHConstructionAlgorithm
    {
        private readonly uint[] _resetArray = new uint[1];
        private readonly BVHBuffers _buffers;

        public PLOCPlusPLus(ComputeShader shader, BVHBuffers buffers) : base(shader, "Build")
        {
            _buffers = buffers;
        }

        protected override void Setup(IShaderBridge<string> shaderBridge, int kernelId)
        {
            shaderBridge.SetBuffer(kernelId, "MergedNodesCount", _buffers.MergedNodesCount);
            shaderBridge.SetBuffer(kernelId, "ValidNodesCount", _buffers.ValidNodesCount);
            shaderBridge.SetBuffer(kernelId, "BlockCounter", _buffers.BlockCounter);
            shaderBridge.SetBuffer(kernelId, "Nodes", _buffers.Nodes);
            shaderBridge.SetInt("BufferSize", _buffers.Nodes.count);
        }

        protected override void Execute(IShaderBridge<string> shaderBridge, Vector3Int payload)
        {
            int leavesCount = payload.x;
            int treeSize = 0;
            
            while (leavesCount > 1)
            {
                shaderBridge.SetInt("LeavesCount", leavesCount);
                shaderBridge.SetInt("TreeSize", treeSize);
                _buffers.MergedNodesCount.SetData(_resetArray);
                _buffers.ValidNodesCount.SetData(_resetArray);
                _buffers.BlockCounter.SetData(_resetArray);

                Dispatch(leavesCount, payload.y, payload.z);
                
                treeSize += _buffers.MergedNodesCount.FetchValue<int>() * 2;
                leavesCount = _buffers.ValidNodesCount.FetchValue<int>();
            }
        }
    }
}