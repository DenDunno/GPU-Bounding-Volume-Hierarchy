Shader "Unlit/RoundedBox"
{
    Properties
    {
        _Color("Main color", Color) = (1,1,1,1)
        _InsideColor("_InsideColor", Color) = (1,1,1,1)
        _OutsideColor("_OutsideColor", Color) = (1,1,1,1)
        _Thickness("Thickness", Range(0, 1)) = 0.1
        _GapCount("Gap count", Integer) = 3
        _GapSize("Gap size", float) = 0.1
        _Frequency("_Frequency", float) = 0.1
        _Phase("_Phase", float) = 0.1
        _LineDistance("_LineDistance", float) = 0.1
        _LineThickness("_LineThickness", float) = 0.1
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
                float2 worldUv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            half4 _Color;
            half4 _InsideColor;
            half4 _OutsideColor;
            int _GapCount;
            float _GapSize;
            float _Offset;
            float _Radius;
            float _LineDistance;
            float _LineThickness;
            float2 _RectangleSize;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldUv = (2 * v.uv - 1) * _RectangleSize / 2;
                //o.worldUv = (2 * v.uv - 1);
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

                const float2 projectedUv = projectUvOntoSdfRectangle(uv, radius);
                const float sdf = length(uv - projectedUv) - radius;

                return sdf < 0;
            }

            float rectangle(float2 samplePosition, float2 halfSize)
            {
                float2 componentWiseEdgeDistance = abs(samplePosition) - halfSize;
                float outsideDistance = length(max(componentWiseEdgeDistance, 0));
                float insideDistance = min(max(componentWiseEdgeDistance.x, componentWiseEdgeDistance.y), 0);
                return outsideDistance + insideDistance;
            }

            float rectangleASd(float2 uv, float2 size)
            {
                float2 componentWiseEdgeDistance = abs(uv) - size;
                return componentWiseEdgeDistance.x;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                const float sdf = rectangle(i.worldUv, float2(100, 100));
                const fixed4 color = lerp(_InsideColor, _OutsideColor, step(0, sdf));
                const float distanceChange = fwidth(sdf) * 0.5;
                const float majorLineDistance = abs(frac(sdf / _LineDistance + 0.5) - 0.5) * _LineDistance;
                const float majorLines = smoothstep(_LineThickness - distanceChange, _LineThickness + distanceChange, majorLineDistance);

                return majorLines * color;
            }
            ENDCG
        }
    }
}