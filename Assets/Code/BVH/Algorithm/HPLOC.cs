using Code.Components.MortonCodeAssignment;
using Code.Utils.ShaderUtils;
using MyFolder.ComputeShaderNM;
using UnityEngine;

public class HPLOC : ComputeShaderPass, IBVHConstructionAlgorithm
{
    private readonly BVHBuffers _buffers;

    public HPLOC(ComputeShader shader, BVHBuffers buffers) : base(shader, "Build")
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