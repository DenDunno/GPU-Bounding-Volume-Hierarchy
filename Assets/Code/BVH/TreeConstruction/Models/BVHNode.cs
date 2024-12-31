using System;
using Code.Data;
using UnityEngine;

[Serializable]
public struct BVHNode
{
    public uint Left;
    public uint Right;
    public AABB Box;

    public override string ToString()
    {
        return $"LefChild = {(int)Left} RightChild = {(int)Right} {Box}";
    }

    public float ComputeSurfaceArea()
    {
        Vector3 size = Box.Max - Box.Min;
        return 2f * (size.x * size.y + size.y * size.z + size.z * size.x);
    }

    public static int GetSize()
    {
        return AABB.GetSize() + sizeof(float) + sizeof(float);
    }
};