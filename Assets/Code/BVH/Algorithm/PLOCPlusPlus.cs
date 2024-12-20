using System;
using Code.Utils.Extensions;
using Code.Utils.ShaderUtils;
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

        protected override void Setup(IShaderBridge<string> shaderBridge, int kernelId)
        {
            shaderBridge.SetBuffer(kernelId, "ValidNodesCount", _buffers.ValidNodesCount);
            shaderBridge.SetBuffer(kernelId, "SortedMortonCodes", _buffers.MortonCodes);
            shaderBridge.SetBuffer(kernelId, "BlockCounter", _buffers.BlockCounter);
            shaderBridge.SetBuffer(kernelId, "BlockOffset", _buffers.BlockOffset);
            shaderBridge.SetBuffer(kernelId, "ParentIds", _buffers.ParentIds);
            shaderBridge.SetBuffer(kernelId, "TreeSize", _buffers.TreeSize);
            shaderBridge.SetBuffer(kernelId, "RootIndex", _buffers.Root);
            shaderBridge.SetBuffer(kernelId, "Nodes", _buffers.Nodes);
            shaderBridge.SetBuffer(kernelId, "Tree", _buffers.Tree);
            shaderBridge.SetBuffer(kernelId, "Test", _buffers.Test);
        }

        protected override void Execute(IShaderBridge<string> shaderBridge, Vector3Int payload)
        {
            int leavesCount = payload.x;
            int treeSize = 0;
            int iterations = 0;
            int safetyCheckMax = 30;

            // _buffers.Tree.Print<BVHNode>("Tree before:\n", x => $"{x}\n");
            _buffers.Nodes.Print<BVHNode>("Nodes before:\n", x => $"{x}\n");

            while (leavesCount > 1 && iterations < safetyCheckMax)
            {
                shaderBridge.SetInt("LeavesCount", leavesCount);
                
                _buffers.TreeSize.SetData(new[] { treeSize });
                _buffers.BlockOffset.SetData(new uint[1]);
                _buffers.BlockCounter.SetData(new uint[1]);
                _buffers.ValidNodesCount.SetData(new uint[1]);

                Dispatch(leavesCount, payload.y, payload.z);
                 _buffers.Tree.Print<BVHNode>($"Iteration: {iterations}. Tree after:\n", x => $"{x}\n");
                //_buffers.Nodes.Print<BVHNode>($"Iteration: {iterations}. Nodes after:\n", x => $"{x}\n");
                //_buffers.Test.Print<int>($"Iteration: {iterations}. Test:\n", x => x.ToString());

                int validNodes = _buffers.ValidNodesCount.FetchValue<int>();
                treeSize += _buffers.BlockOffset.FetchValue<int>();
                leavesCount = validNodes;
                iterations++;
            }
            //_buffers.Nodes.Print<BVHNode>($"Iteration: {iterations}. Nodes after:\n", x => $"{x}\n");
            Debug.Log($"Iterations: {iterations}");
            if (iterations >= safetyCheckMax)
            {
                Debug.LogError("BVH construction error. Termination");
            }
        }
    }
}