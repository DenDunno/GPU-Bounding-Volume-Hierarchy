Shader "Unlit/DottedLine"
{
    Properties
    {
        _Color("Main color", Color) = (1,1,1,1)
        _Thickness("Thickness", Range(0, 1)) = 0.1
        _GapCount("Gap count", Integer) = 3
        _GapSize("Gap size", float) = 0.1
        _Frequency("_Frequency", float) = 0.1
        _Phase("_Phase", float) = 0.1
        _Offset("Offset", float) = 0
        _MainTex("Main texture", 2D) = "white" {}
        _Radius("Radius", Range(0.00001, 1)) = 0.1
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"
        }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma enable_d3d11_debug_symbols
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 samplePosition : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            half4 _Color;
            int _GapCount;
            float _Thickness;
            float _GapSize;
            float _Offset;
            float _Radius;
            float _Frequency = 10.0; // Controls the number of dots
            float _Phase = 0.0; // Controls the rotation of the circle
            float2 _RectangleSize;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.samplePosition = (2 * v.uv - 1) * _RectangleSize / 2;
                return o;
            }

            float2 projectUvOntoSdfRectangle(float2 worldUv, float radius)
            {
                const float2 offset = _RectangleSize / 2;
                worldUv.x = clamp(worldUv.x, -offset.x + radius, offset.x - radius);
                worldUv.y = clamp(worldUv.y, -offset.y + radius, offset.y - radius);
                return worldUv;
            }

            float evaluateSDF(float2 uv)
            {
                const float maxRadius = min(_RectangleSize.x, _RectangleSize.y) / 2;
                const float radius = lerp(0, maxRadius, _Radius);
                const float thickness = lerp(0, radius, _Thickness);

                const float2 projectedUv = projectUvOntoSdfRectangle(uv, radius);
                const float sdf = length(uv - projectedUv) - radius;

                return sdf < 0;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                 float angle = atan2(i.samplePosition.y, i.samplePosition.x);
                if (angle < 0.0)
                {
                    angle += 2.0 * 3.14f;
                }
                
                float wave = 0.5 * sin(_Frequency * angle + _Phase) + 0.5;
                const int angleStep = step(_GapSize, wave);

                return _Color * evaluateSDF(i.samplePosition);
            }
            ENDCG
        }
    }
}