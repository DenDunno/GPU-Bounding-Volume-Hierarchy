#include "Range.hlsl"
#define INVALID_PARENT_ID -1

RWStructuredBuffer<uint> ParentIds;

uint ExchangeParentId(const uint id, const uint value)
{
    uint previousParentId = 0;
    InterlockedExchange(ParentIds[id], value, previousParentId);

    return previousParentId;
}

struct ParentInfo
{
    Range Range;
    int SplitIndex;
    int PreviousId;
    
    static ParentInfo Create(const int previousParentId, const int splitIndex, const int min, const int max)
    {
        ParentInfo result;
        result.SplitIndex = splitIndex;
        result.Range = Range::Create(min, max);
        result.PreviousId = previousParentId;

        return result;
    }

    bool HaveBothChildrenBeenProcessed()
    {
        return PreviousId != INVALID_PARENT_ID;
    }
};