
using System.Collections.Generic;

public struct CollectionComparisonResult<T>
{
    public readonly IList<T> SecondCollection;
    public readonly IList<T> FirstCollection;
    public readonly bool IsEqual;
    public readonly int Index;

    public CollectionComparisonResult(bool isEqual, IList<T> firstCollection, IList<T> secondCollection, int index)
    {
        SecondCollection = secondCollection;
        FirstCollection = firstCollection;
        IsEqual = isEqual;
        Index = index;
    }

    public T FirstValue => FirstCollection[Index];
    public T SecondValue => SecondCollection[Index];

    public static implicit operator bool(CollectionComparisonResult<T> result) => result.IsEqual;
}