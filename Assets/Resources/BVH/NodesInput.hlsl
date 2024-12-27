#ifndef NODES_INPUT_HLSL
#define NODES_INPUT_HLSL
#include "BVHNode.hlsl"

RWStructuredBuffer<uint> RootIndex;
RWStructuredBuffer<BVHNode> Nodes;
uint LeavesCount;
uint BufferSize;

// layout: innerNodes (n - 1) | Root (1) | leaves (n) = 2n
uint BufferLastIndex() { return BufferSize - 1; }
uint InnerNodes() { return LeavesCount - 1; }

bool IsRoot(uint rangeSize) { return rangeSize == LeavesCount; }
bool IsNotRoot(uint rangeSize) { return IsRoot(rangeSize) == false; }
bool IsInBounds(uint id) { return id < LeavesCount; }
bool IsInBounds(int id) { return id >= 0 && id < int(LeavesCount); }

void TrySetRoot(uint nodeIndex, uint rangeSize)
{
    if (IsRoot(rangeSize))
    {
        RootIndex[0] = nodeIndex;
    }
}
#endif
