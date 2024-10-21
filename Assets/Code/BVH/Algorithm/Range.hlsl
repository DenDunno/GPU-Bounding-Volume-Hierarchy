
struct Range
{
    int Left;
    int Right;

    int Size() { return Right - Left + 1; }

    static Range Create(int startIndex)
    {
        Range result;
        result.Left = startIndex;
        result.Right = startIndex;

        return result;
    }
};
