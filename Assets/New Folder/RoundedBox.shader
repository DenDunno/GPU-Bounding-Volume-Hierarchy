Shader "Unlit/RoundedBox"
{
    Properties
    {
        _Color("Main color", Color) = (1,1,1,1)
        _InsideColor("_InsideColor", Color) = (1,1,1,1)
        _OutsideColor("_OutsideColor", Color) = (1,1,1,1)
        _Thickness("Thickness", float) = 0.1
        _GapCount("Gap count", Integer) = 3
        _GapSize("Gap size", float) = 0.1
        _Frequency("_Frequency", Integer) = 10
        _Phase("_Phase", float) = 0.1
        _LineDistance("_LineDistance", float) = 0.1
        _LineThickness("_LineThickness", float) = 0.1
        _Offset("Offset", float) = 0
        _DashSize("Dash size", Range(0, 1)) = 0.5
        _MainTex("Main texture", 2D) = "white" {}
        _Radius("Radius", Range(0, 1)) = 0.1
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
            int _Frequency;
            float _Thickness;
            float _Phase;
            float _Offset;
            float _Radius;
            float2 _RectangleSize;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.samplePosition = (2 * v.uv - 1) * _RectangleSize;
                return o;
            }

            float getRoundedBoxSdf(float2 position, float2 size, float radius)
            {
                float2 boxPosition = abs(position) - size + radius;
                return min(max(boxPosition.x, boxPosition.y), 0.0) + length(max(boxPosition, 0.0)) - radius;
            }

            float getRoundedRectanglePerimeter(float2 rect, float radius)
            {
                const float firstSide = rect.x - 2 * radius;
                const float secondSide = rect.y - 2 * radius;
                const float perimeter = 2.0 * (firstSide + secondSide + UNITY_PI * radius);

                return perimeter;
            }

            float invLerp(float from, float to, float value)
            {
                return (value - from) / (to - from);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                const float maxRadius = min(_RectangleSize.x, _RectangleSize.y);
                const float radius = lerp(0, maxRadius, _Radius);
                const float thickness = lerp(0, maxRadius, _Thickness);
                const float sdf = getRoundedBoxSdf(i.samplePosition, _RectangleSize, radius);
                const int outline = sdf < 0 && sdf > -thickness;

                return outline * _Color;
                return fixed4(sdf.xxx, 1);
            }
            ENDCG
        }
    }
}