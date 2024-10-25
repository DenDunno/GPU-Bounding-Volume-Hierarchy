#ifndef RANGE_HLSL
#define RANGE_HLSL

struct Range
{
    uint Left;
    uint Right;

    uint Size() { return Right - Left + 1; }
    uint HighestDifferingBitIndex() { return firstbithigh(Left ^ Right); }

    static Range Create(const uint index)
    {
        return Create(index, index);
    }

    static Range Create(const uint left, const uint right)
    {
        Range result;
        result.Left = left;
        result.Right = right;

        return result;
    }
};

#endif