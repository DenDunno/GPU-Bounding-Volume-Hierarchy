
#ifndef SORTING_TYPE
#error "SORTING_TYPE is not defined. Please define 'SORTING_TYPE' before including RadixSort library"
#endif

int FetchSortingValue(SORTING_TYPE bufferInput);
SORTING_TYPE GetDefault();

#define OUT_OF_BOUND_VALUE -1
#define SORTED_BITS_PER_PASS 2
#include "..//Utilities/BitManipulation.hlsl"

RWStructuredBuffer<SORTING_TYPE> Input;
uniform int SortLength;
uniform int BitOffset;
static const int4 AllPossibleValues = int4(0, 1, 2, 3);

bool IsInBounds(const int index) { return index < SortLength; }
bool IsOutOfBounds(const int index) { return index >= SortLength; }

int ExtractBits(SORTING_TYPE bufferInput)
{
    int input = FetchSortingValue(bufferInput);
    return ExtractBits(input, BitOffset, SORTED_BITS_PER_PASS);
}

struct RadixSortInput
{
    SORTING_TYPE InitialValue;
    int ExtractedBits;
    int4 HasPassedMask;
    
    static RadixSortInput Fetch(const int id)
    {
        RadixSortInput input;
        
        if (IsInBounds(id))
        {
            input.InitialValue = Input[id];
            input.ExtractedBits = ExtractBits(input.InitialValue);
        }
        else
        {
            input.InitialValue = GetDefault();
            input.ExtractedBits = OUT_OF_BOUND_VALUE;   
        }
        
        input.HasPassedMask = input.ExtractedBits == AllPossibleValues;
        return input;
    }
};
