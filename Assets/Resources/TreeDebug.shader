Shader "Unlit/TreeDebug"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _DashSize("Dash size", float) = 0.1
        _DashCount("Dash count", Integer) = 1
        _BorderSize("Border size", Range(0, 1)) = 0.1
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"            

            half4 _Color;
            int _DashCount;
            float _DashSize;
            float _BorderSize;

            struct Attributes
            {
                float4 positionOS   : POSITION;                 
                float2 uv   : TEXCOORD0;                 
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float2 uv  : TEXCOORD0;
            };            

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            bool ComputeBorder(float uv)
            {
                return uv < _BorderSize || uv > 1 - _BorderSize;
            }

            int ComputeBorder(float2 uv)
            {
                return ComputeBorder(uv.x) || ComputeBorder(uv.y);
            }

            int ComputeDash(float2 uv)
            {
                int dash = frac(uv.x * _DashCount) < _DashSize ||
                           frac(uv.y * _DashCount) < _DashSize;
                return dash;
            }
            
            half4 frag(Varyings input) : SV_Target
            {
                //return _Color * ComputeBorder(input.uv) * ComputeDash(input.uv);
                return half4(input.uv, 0, 1);
            }
            ENDHLSL
        }
    }
}
