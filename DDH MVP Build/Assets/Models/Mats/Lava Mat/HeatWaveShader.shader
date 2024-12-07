Shader "Custom/HeatWave"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Distortion ("Distortion", 2D) = "white" {}
        _DistortionStrength ("Distortion Strength", Range(0, 1)) = 0.1
        _Speed ("Wave Speed", Range(0, 5)) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _Distortion;
            float _DistortionStrength;
            float _Speed;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Use Unity's built-in _Time variable for animation
                float time = _Time.y;
                float2 distortionUV = i.uv + (tex2D(_Distortion, i.uv).rg * 2.0 - 1.0) * _DistortionStrength;
                distortionUV.y += sin(time * _Speed + i.uv.x * 10.0) * 0.01;
                fixed4 col = tex2D(_MainTex, distortionUV);
                return col;
            }
            ENDCG
        }
    }
}