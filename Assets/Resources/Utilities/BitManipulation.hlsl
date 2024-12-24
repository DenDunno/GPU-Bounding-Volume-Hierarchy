#ifndef BIT_MANIPULATION_HLSL
#define BIT_MANIPULATION_HLSL

struct BitUtils
{
    static uint ExtractLower31Bits(const uint value)
    {
        return value & 0x7FFFFFFF;
    }

    static uint SetTopBit(const uint value, const uint topBit)
    {
        return ExtractLower31Bits(value) | topBit << 31;;
    }

    static uint SetLower31Bits(const uint value, const uint lowerBits)
    {
        return ExtractLower31Bits(lowerBits) | value & 0x80000000;
    }

    static uint ExtractLowestBit(const uint input)
    {
        return input & (~input - 1u);
    }

    static uint ExtractTopBit(const uint value)
    {
        return value >> 31;
    }

    static int EncodeInt4(const int4 mask)
    {
        return mask.x << 0 | mask.y << 8 | mask.z << 16 | mask.w << 24;
    }

    static int4 DecodeInt4(const int encoded)
    {
        int4 mask;
        mask.x = (encoded >> 0)  & 0xFF;
        mask.y = (encoded >> 8)  & 0xFF;
        mask.z = (encoded >> 16) & 0xFF;
        mask.w = (encoded >> 24) & 0xFF;
        return mask;
    }

    static int ExtractBits(const int input, const int bitOffset, const int extractedBitsCount)
    {
        const int mask = (1 << extractedBitsCount) - 1;
        const int shiftedInput = input >> bitOffset;
        return shiftedInput & mask;
    }

    static uint ExtractLowerBits(const uint input, const uint count)
    {
        uint mask = (1 << count) - 1;
        return input & mask;
    }  
};
#endif