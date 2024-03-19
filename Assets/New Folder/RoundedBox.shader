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
                float2 worldUv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            half4 _Color;
            half4 _InsideColor;
            half4 _OutsideColor;
            int _GapCount;
            int _Frequency;
            float _GapSize;
            float _Thickness;
            float _Phase;
            float _Offset;
            float _Radius;
            float _LineDistance;
            float _LineThickness;
            float _DashSize;
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
                return componentWiseEdgeDistance.y;
            }


            float msign(in float x) { return (x < 0.0) ? -1.0 : 1.0; }

            float2 paBox(in float2 p, in float2 b, in float r)
            {
                float2 q = abs(p) - b;

                return float2(

                    // u = distance along perimeter
                    (3.0 + msign(p.x)) * (b.x + b.y + 1.570796 * r) + msign(p.y * p.x) *
                    (b.y + ((q.y > 0.0) ? r * ((q.x > 0.0) ? atan2(q.y, q.x) : 1.570796) + max(-q.x, 0.0) : q.y)),

                    // v = distance to box
                    min(max(q.x, q.y), 0.0) + length(max(q, 0.0)) - r);
            }

            float4 paBox(in float2 p, in float2 b, in float r, in float s)
            {
                float2 q = abs(p) - b;

                float l = b.x + b.y + 1.570796 * r;

                float k1 = min(max(q.x, q.y), 0.0) + length(max(q, 0.0)) - r;
                float k2 = ((q.x > 0.0) ? atan2(q.y, q.x) : 1.570796);
                float k3 = 3.0 + 2.0 * sign(min(p.x, -p.y)) - sign(p.x);
                float k4 = sign(p.x * p.y);
                float k5 = r * k2 + max(-q.x, 0.0);

                float ra = s * round(k1 / s);

                float l2 = l + 1.570796 * ra;

                return float4(k1 - ra,
                              k3 * l2 + k4 * (b.y + ((q.y > 0.0) ? k5 + k2 * ra : q.y)),
                              4.0 * l2,
                              k1);
            }

            float getRadius()
            {
                const float maxRadius = min(_RectangleSize.x, _RectangleSize.y) / 2;
                return lerp(0, maxRadius, _Radius);
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
                const float radius = getRadius();
                const float band = _Phase;
                float4 boxResult = paBox(i.worldUv, _RectangleSize / 2 - radius, radius, band);
                float sdf = boxResult.w;
                float borderCoordinate = boxResult.y;
                borderCoordinate *= floor(boxResult.z / band) * (band / boxResult.z);
                borderCoordinate += _Offset;
                int outline = sdf < 0 && sdf > -_Thickness;

                float perimeter = getRoundedRectanglePerimeter(_RectangleSize, radius);
                float dashSize = perimeter / _Frequency;

                int dash = borderCoordinate % dashSize < dashSize * _DashSize;
                float coordinate = invLerp(0, perimeter, borderCoordinate);

                return outline * dash * fixed4(coordinate.xxx, 1);
            }
            ENDCG
        }
    }
}


/*
float4 paBox(in float2 p, in float2 b, in float r, in float s)
            {
                float2 q = abs(p) - b;

                float l = b.x + b.y + 1.570796 * r;

                float k1 = min(max(q.x, q.y), 0.0) + length(max(q, 0.0)) - r;
                float k2 = ((q.x > 0.0) ? atan2(q.y, q.x) : 1.570796);
                float k3 = 3.0 + 2.0 * sign(min(p.x, -p.y)) - sign(p.x);
                float k4 = sign(p.x * p.y);
                float k5 = r * k2 + max(-q.x, 0.0);

                float ra = s * round(k1 / s);

                float l2 = l + 1.570796 * ra;

                return float4(k1 - ra,
                              k3 * l2 + k4 * (b.y + ((q.y > 0.0) ? k5 + k2 * ra : q.y)),
                              4.0 * l2,
                              k1);
            }


fixed4 getRedGreenBox(float2 worldUv)
            {
                const float4 result = paBox(worldUv, float2(100, 100), _Radius, _Thickness);
                const float sdf = result.w;

                const fixed4 color = lerp(_InsideColor, _OutsideColor, step(0, sdf));
                const float distanceChange = fwidth(sdf) * 0.5;
                const float majorLineDistance = abs(frac(sdf / _LineDistance + 0.5) - 0.5) * _LineDistance;
                const float majorLines = smoothstep(_LineThickness - distanceChange, _LineThickness + distanceChange,majorLineDistance);

                float4 col = majorLines * color;
                const float band = _Thickness;
                float2 q = result.xy;
                q.y *= floor(result.z / band) * (band / result.z);
                // optional - ensure periodicity, but break physicallity
                q.y -= _Time.z * 10; // animate circles
                float2 uv = frac(q / band + 0.5) - 0.5; // draw circles
                float l = length(uv);
                col *= 0.1 + 0.9 * smoothstep(0.10, 0.11, l);

                return col;
            }

            float4 getCircle(float2 uv, float2 radius)
            {
                float sdf = length(uv) - radius;
                return step(sdf, 0) * float4(1, 0, 0, 1);
            }
*/