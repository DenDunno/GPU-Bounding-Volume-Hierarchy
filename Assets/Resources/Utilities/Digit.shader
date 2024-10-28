Shader "Custom/DigitShader"
{
    Properties
    {
        _Value ("Value", Int) = 0
        _Size ("Size", Float) = 1.0
        _Offset ("Offset", Vector) = (0,0,0,0)
        _Color ("Color", Color) = (1,1,1,1)
        _DigitOffset ("Digit Offset", Float) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // Uniforms (Properties)
            int _Value;
            float _Size;
            float4 _Offset;
            float4 _Color;
            float _DigitOffset;

            // Input structure
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            // Output structure
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            // Vertex shader
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // Constants
            static const float kCharBlank = 12.0;
            static const float kCharMinus = 11.0;
            static const float kCharDecimalPoint = 10.0;

            // Function to check if a point is inside a rectangle
            float InRect(float2 vUV, float4 vRect)
            {
                float2 vTestMin = step(vRect.xy, vUV.xy);
                float2 vTestMax = step(vUV.xy, vRect.zw);	
                float2 vTest = vTestMin * vTestMax;
                return vTest.x * vTest.y;
            }

            // Function to sample a digit
            float SampleDigit(float fDigit, float2 vUV)
            {
                const float x0 = 0.0 / 4.0;
                const float x1 = 1.0 / 4.0;
                const float x2 = 2.0 / 4.0;
                const float x3 = 3.0 / 4.0;
                const float x4 = 4.0 / 4.0;
                const float y0 = 0.0 / 5.0;
                const float y1 = 1.0 / 5.0;
                const float y2 = 2.0 / 5.0;
                const float y3 = 3.0 / 5.0;
                const float y4 = 4.0 / 5.0;
                const float y5 = 5.0 / 5.0;

                float4 vRect0 = float4(0.0,0.0,0.0,0.0);
                float4 vRect1 = float4(0.0,0.0,0.0,0.0);
                float4 vRect2 = float4(0.0,0.0,0.0,0.0);

                if(fDigit < 0.5) { // 0
                    vRect0 = float4(x0, y0, x3, y5); vRect1 = float4(x1, y1, x2, y4);
                }
                else if(fDigit < 1.5) { // 1
                    vRect0 = float4(x1, y0, x2, y5); vRect1 = float4(x0, y0, x0, y0);
                }
                else if(fDigit < 2.5) { // 2
                    vRect0 = float4(x0, y0, x3, y5); vRect1 = float4(x0, y3, x2, y4); vRect2 = float4(x1, y1, x3, y2);
                }
                else if(fDigit < 3.5) { // 3
                    vRect0 = float4(x0, y0, x3, y5); vRect1 = float4(x0, y3, x2, y4); vRect2 = float4(x0, y1, x2, y2);
                }
                else if(fDigit < 4.5) { // 4
                    vRect0 = float4(x0, y1, x2, y5); vRect1 = float4(x1, y2, x2, y5); vRect2 = float4(x2, y0, x3, y3);
                }
                else if(fDigit < 5.5) { // 5
                    vRect0 = float4(x0, y0, x3, y5); vRect1 = float4(x1, y3, x3, y4); vRect2 = float4(x0, y1, x2, y2);
                }
                else if(fDigit < 6.5) { // 6
                    vRect0 = float4(x0, y0, x3, y5); vRect1 = float4(x1, y3, x3, y4); vRect2 = float4(x1, y1, x2, y2);
                }
                else if(fDigit < 7.5) { // 7
                    vRect0 = float4(x0, y0, x3, y5); vRect1 = float4(x0, y0, x2, y4);
                }
                else if(fDigit < 8.5) { // 8
                    vRect0 = float4(x0, y0, x3, y5); vRect1 = float4(x1, y1, x2, y2); vRect2 = float4(x1, y3, x2, y4);
                }
                else if(fDigit < 9.5) { // 9
                    vRect0 = float4(x0, y0, x3, y5); vRect1 = float4(x1, y3, x2, y4); vRect2 = float4(x0, y1, x2, y2);
                }
                else if(fDigit < 10.5) { // '.'
                    vRect0 = float4(x1, y0, x2, y1);
                }
                else if(fDigit < 11.5) { // '-' 
                    vRect0 = float4(x0, y2, x3, y3);
                }	
                float fResult = InRect(vUV, vRect0) + InRect(vUV, vRect1) + InRect(vUV, vRect2);
                return fmod(fResult, 2.0);
            }

            // Function to print the value
            float PrintValue(float2 vStringCharCoords, float fValue, float fMaxDigits, float fDecimalPlaces)
            {
                float fAbsValue = abs(fValue);
                float fStringCharIndex = floor(vStringCharCoords.x);
                float fLog10Value = log2(fAbsValue) / log2(10.0);
                float fBiggestDigitIndex = max(floor(fLog10Value), 0.0);

                float fDigitCharacter = kCharBlank;
                float fDigitIndex = fMaxDigits - fStringCharIndex;
                if(fDigitIndex > (-fDecimalPlaces - 1.5)) {
                    if(fDigitIndex > fBiggestDigitIndex) {
                        if(fValue < 0.0) {
                            if(fDigitIndex < (fBiggestDigitIndex+1.5)) {
                                fDigitCharacter = kCharMinus;
                            }
                        }
                    }
                    else {
                        if(fDigitIndex == -1.0) {
                            if(fDecimalPlaces > 0.0) {
                                fDigitCharacter = kCharDecimalPoint;
                            }
                        }
                        else {
                            if(fDigitIndex < 0.0) {
                                fDigitIndex += 1.0;
                            }
                            float fDigitValue = (fAbsValue / pow(10.0, fDigitIndex));
                            fDigitCharacter = fmod(floor(0.0001 + fDigitValue), 10.0);
                        }
                    }
                }
                float2 vCharPos = float2(frac(vStringCharCoords.x), vStringCharCoords.y);
                return SampleDigit(fDigitCharacter, vCharPos);	
            }

            // Overloaded PrintValue function
            float PrintValue(float2 fragCoord, float2 vPixelCoords, float2 vFontSize, float fValue, float fMaxDigits, float fDecimalPlaces)
            {
                float2 adjust = (fragCoord.xy - vPixelCoords) / vFontSize;
                return PrintValue(adjust, fValue, fMaxDigits, fDecimalPlaces);
            }

            // Function to get the number of digits
            int getDigits(int number)
            {
                if (number == 0)
                    return 1;
                else
                    return (int)(floor(log10(abs((float)number))) + 1);
            }

            // Fragment shader
            fixed4 frag (v2f i) : SV_Target
            {
                float2 textureCoordinates = i.uv;
                float2 vFontSize = float2(_Size, _Size);
                float2 numberOffset = _Offset.xy;
                int value = _Value;

                // Adjust the number offset based on the number of digits
                numberOffset.x = _Offset.x + (float)(getDigits(value)) * _DigitOffset;
                if (value == 0)
                    numberOffset.x += _DigitOffset;

                // Calculate the number color
                float numberColor = PrintValue(textureCoordinates, numberOffset, vFontSize, (float)value, 6.0, 0.0);
                float4 circleColor = _Color;

                // Create a circular mask
                float distance = pow(textureCoordinates.x - 0.5f, 2) + pow(textureCoordinates.y - 0.5f, 2) - 0.45f * 0.45f; 
                circleColor.a = smoothstep(0.01f, 0.0f, distance);

                // Output the final color
                return circleColor + float4(numberColor, numberColor, numberColor, 0);
            }
            ENDCG
        }
    }
}
