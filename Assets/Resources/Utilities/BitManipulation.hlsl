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
    return input & (-input);
}

uint ExtractTopBit(const uint value)
{
    return value >> 31;
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
