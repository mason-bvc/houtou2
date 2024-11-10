Shader "Unlit/Background2"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float ii = _Time;
                float2 uv = i.uv * 2.0 - 1.0;
                fixed4 c = fixed4(1.0, 1.0, 1.0, 1.0);
                float d = length(uv);
                float a = atan2(uv.y, uv.x) + sin(ii * 0.2) * 0.5;

                uv.x = cos(a) * d;
                uv.y = sin(a) * d;
                d -= ii;
                uv.x += sin(uv.y * 2.0 + ii) * 0.1;
                uv += sin(uv * 1234.567 + ii) * 0.0005;
                c.r = abs(fmod(uv.y + uv.x * 2.0 * d, uv.x * 1.1));

                return c.rrra;
            }
            ENDCG
        }
    }
}
