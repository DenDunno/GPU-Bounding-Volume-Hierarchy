using Code.Data;

struct BVHNode
{
    AABB Box;
    private uint X;
    private uint Y;
    
    public uint ExtractLower31Bits(uint input) => input & 0x7FFFFFFF;
    public uint SetTopBit(uint input, uint topBit) => ExtractLower31Bits(input) | topBit << 31;
    public uint SetLower31Bits(uint input, uint lowerBits) => ExtractLower31Bits(lowerBits) | input & 0x80000000;
    public uint ExtractTopBit(uint input) => input >> 31;

    public bool IsLeaf() { return ExtractTopBit(X) == 1; }
    public uint LeftChild() { return X; }
    public uint RightChild() { return Y; }
    public uint TriangleIndex() { return LeftChild(); }
    public uint TriangleCount() { return RightChild(); }

    public void MarkAsLeaf() { __SetIsLeaf(1); }
    public void MarkAsInternalNode() { __SetIsLeaf(0); }
    public void __SetIsLeaf(uint value) { X = SetTopBit(X, value); }
    public void SetLeftChild(uint value) { X = SetLower31Bits(X, value); }
    public void SetRightChild(uint value) { X = SetLower31Bits(Y, value); }

    public override string ToString()
    {
        return $"LefChild = {LeftChild()} RightChild = {RightChild()} {Box}";
    }
};