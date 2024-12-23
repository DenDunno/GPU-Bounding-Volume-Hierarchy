#ifndef BIT_MANIPULATION_HLSL
#define BIT_MANIPULATION_HLSL

uint ExtractLower31Bits(const uint value)
{
    return value & 0x7FFFFFFF;
}

uint SetTopBit(const uint value, const uint topBit)
{
    return ExtractLower31Bits(value) | topBit << 31;;
}

uint SetLower31Bits(const uint value, const uint lowerBits)
{
    return ExtractLower31Bits(lowerBits) | value & 0x80000000;
}

uint ExtractLowestBit(uint input)
{
    return input & (~input - 1u);
}

uint ExtractTopBit(const uint value)
{
    return value >> 31;
}

int EncodeInt4(int4 mask)
{
    return mask.x << 0 | mask.y << 8 | mask.z << 16 | mask.w << 24;
}

int4 DecodeInt4(int encoded)
{
    int4 mask;
    mask.x = (encoded >> 0)  & 0xFF;
    mask.y = (encoded >> 8)  & 0xFF;
    mask.z = (encoded >> 16) & 0xFF;
    mask.w = (encoded >> 24) & 0xFF;
    return mask;
}

int ExtractBits(const int input, const int bitOffset, const int extractedBitsCount)
{
    const int mask = (1 << extractedBitsCount) - 1;
    const int shiftedInput = input >> bitOffset;
    return shiftedInput & mask;
}

uint ExtractLowerBits(uint input, uint count)
{
    uint mask = (1 << count) - 1;
    return input & mask;
}
#endif