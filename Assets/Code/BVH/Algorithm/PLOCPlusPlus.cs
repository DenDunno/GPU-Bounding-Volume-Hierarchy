using Code.Utils.Extensions;
using Code.Utils.ShaderUtils;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class PLOCPlusPLus : ComputeShaderPass, IBVHConstructionAlgorithm
    {
        private readonly BVHBuffers _buffers;

        public PLOCPlusPLus(ComputeShader shader, BVHBuffers buffers) : base(shader, "Build")
        {
            _buffers = buffers;
        }

        protected override void Setup(IShaderBridge<string> shaderBridge, int kernelId)
        {
            shaderBridge.SetBuffer(kernelId, "MergedNodesCount", _buffers.MergedNodesCount);
            shaderBridge.SetBuffer(kernelId, "ValidNodesCount", _buffers.ValidNodesCount);
            shaderBridge.SetBuffer(kernelId, "SortedMortonCodes", _buffers.MortonCodes);
            shaderBridge.SetBuffer(kernelId, "BlockCounter", _buffers.BlockCounter);
            shaderBridge.SetBuffer(kernelId, "ParentIds", _buffers.ParentIds);
            shaderBridge.SetBuffer(kernelId, "TreeSize", _buffers.TreeSize);
            shaderBridge.SetBuffer(kernelId, "RootIndex", _buffers.Root);
            shaderBridge.SetBuffer(kernelId, "Nodes", _buffers.Nodes);
            shaderBridge.SetBuffer(kernelId, "Test", _buffers.Test);
            shaderBridge.SetInt("BufferSize", _buffers.Nodes.count);
        }

        protected override void Execute(IShaderBridge<string> shaderBridge, Vector3Int payload)
        {
            int leavesCount = payload.x;
            int safetyCheckMax = 100;
            int iterations = 0;
            int treeSize = 0;
            
            while (leavesCount > 1 && iterations < safetyCheckMax)
            {
                shaderBridge.SetInt("LeavesCount", leavesCount);
                
                _buffers.TreeSize.SetData(new[] { treeSize });
                _buffers.MergedNodesCount.SetData(new uint[1]);
                _buffers.BlockCounter.SetData(new uint[1]);
                _buffers.ValidNodesCount.SetData(new uint[1]);
                
                Dispatch(leavesCount, payload.y, payload.z);
                
                treeSize += _buffers.MergedNodesCount.FetchValue<int>() * 2;
                leavesCount = _buffers.ValidNodesCount.FetchValue<int>();
                iterations++;
            }
            
            if (iterations >= safetyCheckMax)
            {
                Debug.LogError("BVH construction error. Termination");
            }
        }
    }
}