
float GetDepth(float2 uv, sampler2D depthTexture)
{
    const float depthTextureValue = SAMPLE_DEPTH_TEXTURE(depthTexture, uv);
    const float sceneDepth = LinearEyeDepth(depthTextureValue, _ZBufferParams);

    return sceneDepth;
}

half4 BlendSrcOneMinusSrc(half4 sourceColor, half4 destinationColor)
{
    return saturate(sourceColor * destinationColor.a + destinationColor * (1 - destinationColor.a));
}

int CheckIfOutside(float3 hitPoint, float3 origin, float radius)
{
    return (pow((hitPoint.r - origin.r), 2) + pow((hitPoint.g - origin.g), 2) + pow((hitPoint.b - origin.b), 2)) >= radius * radius; 
}