
public struct CollectionComparisonResult<T>
{
    public readonly bool IsEqual;
    public readonly T FirstValue;
    public readonly T SecondValue;
    public readonly int Index;

    public CollectionComparisonResult(bool isEqual, T firstValue, T secondValue, int index)
    {
        IsEqual = isEqual;
        FirstValue = firstValue;
        SecondValue = secondValue;
        Index = index;
    }

    public static implicit operator bool(CollectionComparisonResult<T> result) => result.IsEqual;
}