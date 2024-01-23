
float GetDepth(float2 uv)
{
    const float depthTextureValue = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv);
    const float sceneDepth = LinearEyeDepth(depthTextureValue, _ZBufferParams);

    return sceneDepth;
}

half4 BlendSrcOneMinusSrc(half4 sourceColor, half4 destinationColor)
{
    return saturate(sourceColor * destinationColor.a + destinationColor * (1 - destinationColor.a));
}

int CheckIfInside(int hitPoint, float origin, float radius)
{
                
}