#include "..//MortonCode/MortonCode.hlsl"
#include "ParentInfo.hlsl"
#include "BVHNode.hlsl"

StructuredBuffer<MortonCode> SortedMortonCodes;
RWStructuredBuffer<BVHNode> Nodes;
StructuredBuffer<Range> Ranges;
uint _LeavesCount;

ParentInfo ChooseLeftParent(const Range range, const uint nodeIndex)
{
    uint id = range.Left - 1;
    uint previousId = ExchangeParentId(id, range.Right);
    Nodes[id].SetRightChild(nodeIndex);
    return ParentInfo::Create(id, previousId, range.Left, previousId, range.Right);
}

ParentInfo ChooseRightParent(const Range range, const uint nodeIndex)
{
    uint id = range.Right;
    uint previousId = ExchangeParentId(id, range.Left);
    Nodes[id].SetLeftChild(nodeIndex);
    return ParentInfo::Create(id, previousId, range.Right + 1, range.Left, previousId);
}

uint HighestDifferingBitIndex(const uint nodeIndex)
{
    return firstbithigh(SortedMortonCodes[nodeIndex].Value ^
        SortedMortonCodes[nodeIndex + 1].Value);
}

int Delta(const uint nodeIndex)
{
    return HighestDifferingBitIndex(nodeIndex);
}

bool IsParentRight(const Range range)
{
    return range.Left == 0 || (range.Right != _LeavesCount &&
        Delta(range.Right) < Delta(range.Left - 1));
}

ParentInfo ChooseParent(const uint threadId)
{
    const uint nodeIndex = SortedMortonCodes[threadId].ObjectId;
    const Range range = Ranges[nodeIndex];
    
    if (IsParentRight(range))
    {
        return ChooseRightParent(range, nodeIndex);
    }

    return ChooseLeftParent(range, nodeIndex);
}