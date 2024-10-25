
uint Expand10BitValueTo30Bit(uint input)
{
    input = input * 0x00010001u & 0xFF0000FFu;
    input = input * 0x00000101u & 0x0F00F00Fu;
    input = input * 0x00000011u & 0xC30C30C3u;
    input = input * 0x00000005u & 0x49249249u;
    return input;
}

uint Compute30BitCode(float3 normalizedInput)
{
    normalizedInput.x = min(max(normalizedInput.x * 1024.0f, 0.0f), 1023.0f);
    normalizedInput.y = min(max(normalizedInput.y * 1024.0f, 0.0f), 1023.0f);
    normalizedInput.z = min(max(normalizedInput.z * 1024.0f, 0.0f), 1023.0f);
    uint xx = Expand10BitValueTo30Bit((uint)normalizedInput.x);
    uint yy = Expand10BitValueTo30Bit((uint)normalizedInput.y);
    uint zz = Expand10BitValueTo30Bit((uint)normalizedInput.z);
    return xx * 4 + yy * 2 + zz;
}

struct MortonCode
{
    uint ObjectId;
    uint Value;

    static MortonCode Create(uint objectId, float3 input)
    {
        MortonCode output;
        output.Value = Compute30BitCode(input);
        output.ObjectId = objectId;

        return output;
    }
};