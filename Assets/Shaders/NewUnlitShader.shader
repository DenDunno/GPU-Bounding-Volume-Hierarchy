Shader "Raytracing"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Raytracing.hlsl"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            half4 _CameraParams;
            StructuredBuffer<Sphere> _Spheres;
            int _SpheresCount;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 GetSphereColor()
            {
                return fixed4(1,0,0,1);
            }
            
            fixed4 IterateSpheres(Ray ray)
            {
                fixed4 color = fixed4(0,0,0,0);
                Sphere sphere;
                sphere.position = float3(0,0,0);
                sphere.radius = 2;
                
                // for (int i = 0; i < _SpheresCount; ++i)
                // {
                //     const HitResult hitResult = HitSphere(ray, _Spheres[i]);
                //     const fixed4 sphereColor = hitResult.success ? GetSphereColor() : fixed4(0,0,0,0);
                //     
                //     color += sphereColor;
                // }

                const HitResult hitResult = HitSphere(ray, sphere);
                const fixed4 sphereColor = hitResult.success ? GetSphereColor() : fixed4(0,0,0,0);
                color += sphereColor;

                return color;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                return fixed4(1,1,1,1);
                const Ray ray = GetInitialRay(i.uv, _CameraParams);
                const fixed4 color = IterateSpheres(ray);

                return fixed4(1,0,0,1);
                return color;
            }
            ENDHLSL
        }
    }
}