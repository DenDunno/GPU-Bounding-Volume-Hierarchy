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

    protected override void Setup(IShaderBridge<string> shaderBridge, int kernelId)
    {
        shaderBridge.SetBuffer(kernelId, "SortedMortonCodes", _buffers.MortonCodes);
        shaderBridge.SetBuffer(kernelId, "ParentIds", _buffers.ParentIds);
        shaderBridge.SetBuffer(kernelId, "RootIndex", _buffers.Root);
        shaderBridge.SetBuffer(kernelId, "Nodes", _buffers.Nodes);
    }

    protected override void OnPreDispatch(IShaderBridge<string> shaderBridge, Vector3Int payload)
    {
        shaderBridge.SetInt("LeavesCount", payload.x);
    }
}