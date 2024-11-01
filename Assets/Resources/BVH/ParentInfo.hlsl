#include "Range.hlsl"
#define INVALID_PARENT_ID -1
#pragma warning(disable: 4000)

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
    uint PreviousId;
    uint Id;

    static ParentInfo Create(const uint id, const uint previousParentId, const int min, const int max)
    {
        ParentInfo result;
        result.Range = Range::Create(min, max);
        result.PreviousId = previousParentId;
        result.Id = id;
        
        return result;
    }

    bool HaveBothChildrenBeenProcessed()
    {
        return PreviousId != INVALID_PARENT_ID;
    }
};