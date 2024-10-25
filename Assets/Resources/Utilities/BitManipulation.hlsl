
int ExtractBits(int input, int bitOffset, int extractedBitsCount)
{
    const int mask = (1 << extractedBitsCount) - 1;
    const int shiftedInput = input >> bitOffset;
    return shiftedInput & mask;
}