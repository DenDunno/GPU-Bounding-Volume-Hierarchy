
#define ALL_POSSIBLE_VALUES 4 // SORTED_BITS_PER_PASS ^ 2
groupshared int OffsetTable[ALL_POSSIBLE_VALUES + 1];

void InitializeOffsetTable()
{
    OffsetTable[0] = 0;
}

void AddToOffsetTable(int sortValue, const int totalElementsInGroup)
{
    OffsetTable[sortValue + 1] = totalElementsInGroup + OffsetTable[sortValue];
}