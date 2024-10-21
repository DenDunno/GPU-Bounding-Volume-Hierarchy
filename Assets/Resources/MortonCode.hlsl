
// Expands a 10-bit integer into 30 bits
// by inserting 2 zeros after each bit.
uint expandBits(unsigned int v)
{
    v = (v * 0x00010001u) & 0xFF0000FFu;
    v = (v * 0x00000101u) & 0x0F00F00Fu;
    v = (v * 0x00000011u) & 0xC30C30C3u;
    v = (v * 0x00000005u) & 0x49249249u;
    return v;
}

// Calculates a 30-bit Morton code for the
// given 3D point located within the unit cube [0,1].
uint morton3D(float3 input)
{
    input.x = min(max(input.x * 1024.0f, 0.0f), 1023.0f);
    input.y = min(max(input.y * 1024.0f, 0.0f), 1023.0f);
    input.z = min(max(input.z * 1024.0f, 0.0f), 1023.0f);
    uint xx = expandBits((uint)input.x);
    uint yy = expandBits((uint)input.y);
    uint zz = expandBits((uint)input.z);
    return xx * 4 + yy * 2 + zz;
}