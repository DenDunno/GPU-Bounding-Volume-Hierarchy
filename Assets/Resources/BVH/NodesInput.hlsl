#include "BVHNode.hlsl"

RWStructuredBuffer<uint> RootIndex;
RWStructuredBuffer<BVHNode> Nodes;
uint LeavesCount;

uint InnerNodes() { return LeavesCount - 1; }
uint ComputeLeafIndex(uint threadId) { return InnerNodes() + threadId; }

bool IsRoot(uint rangeSize) { return rangeSize == LeavesCount; }
bool IsNotRoot(uint rangeSize) { return IsRoot(rangeSize) == false; }

void TrySetRoot(uint nodeIndex, uint rangeSize)
{
    if (IsRoot(rangeSize))
    {
        RootIndex[0] = nodeIndex;
    }
}
