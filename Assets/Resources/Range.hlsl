
struct Range
{
    int Left;
    int Right;

    int Size() { return Right - Left + 1; }
    int HighestDifferingBitIndex() { return firstbithigh(Left ^ Right); }

    static Range Create(int index)
    {
        return Create(index, index);
    }

    static Range Create(int left, int right)
    {
        Range result;
        result.Left = left;
        result.Right = right;

        return result;
    }
};
