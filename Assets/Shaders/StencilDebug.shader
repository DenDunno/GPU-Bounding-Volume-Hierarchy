Shader "StencilDebug"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        [IntRange] _StencilID ("Stencil ID", Range(0, 255)) = 0
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"
        }
        LOD 100

        Stencil
        {
            Ref [_StencilID]
            Comp Equal
        }
        
        Pass
        {
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            half4 _Color;
            
            half4 Frag(Varyings input) : SV_Target
            {
                return _Color;
            }
            ENDHLSL
        }
    }
}