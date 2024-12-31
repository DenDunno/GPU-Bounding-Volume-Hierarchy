#include "..//MortonCode/MortonCode.hlsl"
#include "NodesInput.hlsl"
#include "ParentInfo.hlsl"

StructuredBuffer<MortonCode> SortedMortonCodes;

ParentInfo ChooseLeftParent(const Range range, const uint nodeIndex)
{
    uint id = range.Left - 1;
    uint previousId = ExchangeParentId(id, range.Right);
    Nodes[id].Right = nodeIndex;
    return ParentInfo::Create(id, previousId, previousId, range.Right);
}

ParentInfo ChooseRightParent(const Range range, const uint nodeIndex)
{
    uint id = range.Right;
    uint previousId = ExchangeParentId(id, range.Left);
    Nodes[id].Left = nodeIndex;
    return ParentInfo::Create(id, previousId, range.Left, previousId);
}

uint HighestDifferingBitIndex(const uint nodeIndex)
{
    return firstbithigh(SortedMortonCodes[nodeIndex].Value ^
        SortedMortonCodes[nodeIndex + 1].Value);
}

uint Delta(const uint nodeIndex)
{
    return HighestDifferingBitIndex(nodeIndex);
}

bool IsParentRight(const Range range)
{
    return range.Left == 0 || (range.Right != InnerNodes() &&
        Delta(range.Right) < Delta(range.Left - 1));
}

ParentInfo ChooseParent(const uint threadId, const Range range)
{
    if (IsParentRight(range))
    {
        return ChooseRightParent(range, threadId);
    }

    return ChooseLeftParent(range, threadId);
}