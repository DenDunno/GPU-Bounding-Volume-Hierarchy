#include "..//MortonCode/MortonCode.hlsl"
#include "ParentInfo.hlsl"

StructuredBuffer<MortonCode> MortonCodes;
uint _LeavesCount;

ParentInfo ChooseLeftParent(const Range range)
{
    uint previousParentId = ExchangeParentId(range.Left - 1, range.Right);
    return ParentInfo::Create(previousParentId, range.Left, previousParentId, range.Right);
}

ParentInfo ChooseRightParent(const Range range)
{
    uint previousParentId = ExchangeParentId(range.Right, range.Left);
    return ParentInfo::Create(previousParentId, range.Right + 1, range.Left, previousParentId);
}

uint HighestDifferingBitIndex(const uint nodeIndex)
{
    return firstbithigh(MortonCodes[nodeIndex].Value ^
        MortonCodes[nodeIndex + 1].Value);
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

ParentInfo ChooseParent(const Range range)
{
    if (IsParentRight(range))
    {
        return ChooseRightParent(range);
    }

    return ChooseLeftParent(range);
}
