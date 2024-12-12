using Code.Utils.Extensions;
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

        void IBVHConstructionAlgorithm.Execute(int leavesCount)
        {
            int treeSize = 0;
            int safetyCheck = 0;
            int safetyCheckMax = 30;
            
            while (leavesCount > 1 && safetyCheck++ < safetyCheckMax)
            {
                _buffers.TreeSize.SetData(new[] { treeSize });
                _buffers.BlockOffset.SetData(new uint[1]);
                _buffers.BlockCounter.SetData(new uint[1]);

                _buffers.Tree.Print<BVHNode>("Tree before: \n", x => $"{x}\n");
                _buffers.Nodes.Print<BVHNode>("Nodes before: \n", x => $"{x}\n");
                Execute(leavesCount);
                _buffers.Tree.Print<BVHNode>("Tree after: \n", x => $"{x}\n");
                _buffers.Nodes.Print<BVHNode>("Nodes: \n", x => $"{x}\n");
                
                int mergedNodes = _buffers.BlockOffset.FetchValue<int>();
                leavesCount -= mergedNodes;
                treeSize += mergedNodes * 2;
            }

            if (safetyCheck >= safetyCheckMax)
            {
                Debug.LogError("BVH construction error. Termination");
            }
        }

        protected override void Setup(Kernel kernel, IShaderBridge<string> shaderBridge)
        {
            shaderBridge.SetBuffer(kernel.ID, "SortedMortonCodes", _buffers.MortonCodes);
            shaderBridge.SetBuffer(kernel.ID, "BlockCounter", _buffers.BlockCounter);
            shaderBridge.SetBuffer(kernel.ID, "BlockOffset", _buffers.BlockOffset);
            shaderBridge.SetBuffer(kernel.ID, "ParentIds", _buffers.ParentIds);
            shaderBridge.SetBuffer(kernel.ID, "TreeSize", _buffers.TreeSize);
            shaderBridge.SetBuffer(kernel.ID, "RootIndex", _buffers.Root);
            shaderBridge.SetBuffer(kernel.ID, "Nodes", _buffers.Nodes);
            shaderBridge.SetBuffer(kernel.ID, "Tree", _buffers.Tree);
        }

        protected override void OnPreDispatch(IShaderBridge<string> shaderBridge, Vector3Int payload)
        {
            shaderBridge.SetInt("LeavesCount", payload.x);
        }
    }
}