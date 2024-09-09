#define OUT_OF_BOUND_VALUE -100
#define SORTED_BITS_PER_PASS 2
#include "../BitManipulation.hlsl"

RWStructuredBuffer<int> Input;
uniform int SortLength;
uniform int BitOffset;

bool IsInBounds(int index) { return index < SortLength; }
bool IsOutOfBounds(int index) { return IsInBounds(index) == false; }

struct RadixSortInput
{
    int InitialValue;
    int ExtractedBits;

    static int2 CreateInputInternal(uint globalId)
    {
        int input = Input[globalId];
        return int2(input, ExtractBits(input, BitOffset, SORTED_BITS_PER_PASS));
    }

    static int2 GetDefaultInternal()
    {
        return int2(OUT_OF_BOUND_VALUE, OUT_OF_BOUND_VALUE);
    }

    static RadixSortInput Fetch(int globalId)
    {
        int2 parameters = globalId < SortLength ? CreateInputInternal(globalId) : GetDefaultInternal();
        RadixSortInput input;
        input.InitialValue = parameters.x;
        input.ExtractedBits = parameters.y;

        return input;
    }
};