Shader "StencilDebug"
{
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"
        }
        LOD 100
        ZWrite Off Cull Off
        Pass
        {
            Stencil
            {
                Ref 1
                ReadMask 1
                Comp Equal
            }
            
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
            
            half4 Frag(Varyings input) : SV_Target
            {
                const float4 sceneColor = FragBilinear(input);
                
                return sceneColor;
            }
            ENDHLSL
        }
    }
}