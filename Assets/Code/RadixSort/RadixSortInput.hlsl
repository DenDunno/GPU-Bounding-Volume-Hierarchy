#define OUT_OF_BOUND_VALUE -1
#define SORTED_BITS_PER_PASS 2
#include "../BitManipulation.hlsl"

RWStructuredBuffer<int> Input;
uniform int SortLength;
uniform int BitOffset;
static const int4 AllPossibleValues = int4(0, 1, 2, 3);

bool IsInBounds(int index) { return index < SortLength; }
bool IsOutOfBounds(int index) { return index >= SortLength; }

struct RadixSortInput
{
    int InitialValue;
    int ExtractedBits;
    int4 HasPassedMask;

    static int2 __CreateInput(uint globalId)
    {
        int input = Input[globalId];
        return int2(input, ExtractBits(input, BitOffset, SORTED_BITS_PER_PASS));
    }

    static int2 __GetDefault()
    {
        return int2(OUT_OF_BOUND_VALUE, OUT_OF_BOUND_VALUE);
    }

    static int2 __GetParameters(int globalId)
    {
        return globalId < SortLength ?
            __CreateInput(globalId) :
            __GetDefault();
    }

    static RadixSortInput Fetch(int globalId)
    {
        int2 parameters = __GetParameters(globalId);
        RadixSortInput input;
        input.InitialValue = parameters.x;
        input.ExtractedBits = parameters.y;
        input.HasPassedMask = input.ExtractedBits == AllPossibleValues;
        
        return input;
    }
};