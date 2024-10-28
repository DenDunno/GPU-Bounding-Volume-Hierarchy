
#include "..//MortonCode/MortonCode.hlsl"
#define SORTING_TYPE MortonCode

int FetchSortingValue(SORTING_TYPE bufferInput)
{
    return bufferInput.Value;
}

SORTING_TYPE GetDefault()
{
    MortonCode code;
    code.Value = -1;
    code.ObjectId = -1;
    return code;
}