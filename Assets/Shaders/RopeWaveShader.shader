Shader "Custom/RopeWaveShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BumpAmplitude ("BumpHeight", Range(0.0, 1.0)) = 0.5
        _BumpWidth ("BumpWidth", Range(0.01, 0.5)) = 0.1
        _BumpSpeed ("Speed", Range(0.1, 5.0)) = 1.0
        _BumpDirection ("Direction", Float) = 1.0
        _Color ("Color", Color) = (0.4, 0.25, 0.15, 1.0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float _BumpAmplitude;
            float _BumpWidth;
            float _BumpSpeed;
            float _BumpDirection;
            fixed4 _Color;
            sampler2D _MainTex;

            // struct appdata_full {
            //     float4 vertex : POSITION;
            // };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata_full v) {
                v2f o;
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                float bumpCenterPosition = worldPos.x + _Time.y * _BumpSpeed * _BumpDirection; 
                float distance = abs(worldPos.x - bumpCenterPosition);
                float attenuation = exp(-(distance * distance) / (_BumpWidth * _BumpWidth));
                float yOffset = _BumpAmplitude * attenuation;
                worldPos.y += yOffset;
                o.pos = mul(UNITY_MATRIX_VP, worldPos);
                o.uv = v.texcoord;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                return col;
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
}
