using System.Collections.Generic;
using System.Linq;
using Code.RenderFeature.Data;
using DefaultNamespace;
using UnityEngine;

public class LeafGenerator
{
    public List<AABBNode> GetNodes(List<AABB> boxes, AABB sceneBox)
    {
        List<AABBNode> codes = boxes.Select(box =>
        {
            Vector3 unitPoint = sceneBox.GetRelativeCoordinates(box.Centroid);
            uint mortonCode = Morton3D(unitPoint.x, unitPoint.y, unitPoint.z);

            return new AABBNode(mortonCode, box);
        }).ToList();
        
        return codes;
    }

    private uint ExpandBits(uint v)
    {
        v = (v * 0x00010001u) & 0xFF0000FFu;
        v = (v * 0x00000101u) & 0x0F00F00Fu;
        v = (v * 0x00000011u) & 0xC30C30C3u;
        v = (v * 0x00000005u) & 0x49249249u;
        return v;
    }

    private uint Morton3D(float x, float y, float z)
    {
        x = Mathf.Min(Mathf.Max(x * 1024.0f, 0.0f), 1023.0f);
        y = Mathf.Min(Mathf.Max(y * 1024.0f, 0.0f), 1023.0f);
        z = Mathf.Min(Mathf.Max(z * 1024.0f, 0.0f), 1023.0f);
        uint xx = ExpandBits((uint)x);
        uint yy = ExpandBits((uint)y);
        uint zz = ExpandBits((uint)z);
        return xx * 4 + yy * 2 + zz;
    }
}