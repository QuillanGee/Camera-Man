Shader "Custom/ScreenSpaceOutline"
{
    Properties
    {
        _MainColor ("Main Color", Color) = (1,1,1,1)  // Color of the object
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)  // Color for the outline
        _OutlineWidth ("Outline Width", Range(1, 10)) = 2.0  // Thickness of the outline
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            Name "BASE"
            Tags { "LightMode" = "ForwardBase" }
            Cull Back

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float3 normal : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
            };

            // Properties
            float4 _MainColor;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.normal = v.normal;
                o.screenPos = ComputeScreenPos(o.pos);  // Store screen space position
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return _MainColor;
            }

            ENDCG
        }

        Pass
        {
            Name "OUTLINE"
            Tags { "LightMode" = "Always" }
            Cull Front  // Render backfaces to get the silhouette

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment fragOutline
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float4 screenPos : TEXCOORD0;
            };

            // Properties
            float4 _OutlineColor;
            float _OutlineWidth;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeScreenPos(o.pos);  // Store screen space position
                return o;
            }

            fixed4 fragOutline(v2f i) : SV_Target
            {
                // Screen space derivative functions to detect edge outlines
                float2 screenUV = i.screenPos.xy / i.screenPos.w;
                float2 offset1 = screenUV + float2(_OutlineWidth, 0);
                float2 offset2 = screenUV + float2(0, _OutlineWidth);

                // Use ddx/ddy to detect depth changes (silhouette)
                float depth1 = tex2Dproj(_CameraDepthTexture, offset1).r;
                float depth2 = tex2Dproj(_CameraDepthTexture, offset2).r;

                float diff1 = abs(i.screenPos.z - depth1);
                float diff2 = abs(i.screenPos.z - depth2);

                // If either derivative is large, it's an edge
                float edge = step(0.001, diff1) + step(0.001, diff2);

                // Apply the outline color where the edge is detected
                return edge > 0 ? _OutlineColor : float4(0, 0, 0, 0);
            }

            ENDCG
        }
    }

    FallBack "Diffuse"
}