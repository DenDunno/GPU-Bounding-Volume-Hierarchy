
struct ConstructionResult
{
    uint NodeIndex;
    uint RangeSize;

    static ConstructionResult Create(uint nodeIndex, uint rangeSize)
    {
        ConstructionResult result;
        result.NodeIndex = nodeIndex;
        result.RangeSize = rangeSize;
        
        return result;
    }
};
