
int ExtractBits(const int input, const int bitOffset, const int extractedBitsCount)
{
    const int mask = (1 << extractedBitsCount) - 1;
    const int shiftedInput = input >> bitOffset;
    return shiftedInput & mask;
}