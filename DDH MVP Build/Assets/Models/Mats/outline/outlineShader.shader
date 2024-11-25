Shader "Custom/OutlineShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineThickness ("Outline Thickness", Float) = 0.03
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            // Draw the object with the outline
            Name "Outline"
            Cull Front
            ZWrite On
            ZTest LEqual
            ColorMask RGB
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float4 color : COLOR;
            };

            float _OutlineThickness;
            float4 _OutlineColor;

            v2f vert(appdata_t v)
            {
                v2f o;
                float3 norm = normalize(mul((float3x3)unity_WorldToObject, v.normal));
                float3 pos = v.vertex.xyz + norm * _OutlineThickness;

                o.pos = UnityObjectToClipPos(float4(pos, 1.0));
                o.color = _OutlineColor;
                return o;
            }

            half4 frag(v2f i) : COLOR
            {
                return i.color;
            }
            ENDCG
        }
        
        Pass
        {
            // Regular rendering pass
            Name "Main"
            Cull Back
            ZWrite On
            ZTest LEqual
            ColorMask RGB
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vertMain
            #pragma fragment fragMain
            #include "UnityCG.cginc"

            sampler2D _MainTex;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vertMain(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 fragMain(v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}