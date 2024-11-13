Shader "Unlit/Background"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainColor ("Color", Color) = (0, 0, 1.0, 1.0)
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
            fixed4 _MainColor;

            fixed greyscale(fixed channel, fixed str)
            {
                fixed g = dot(channel, fixed3(0.299, 0.587, 0.114));
                return lerp(channel, g, str);
            }

            fixed3 greyscale3(fixed3 color, fixed str)
            {
                fixed g = dot(color, fixed3(0.299, 0.587, 0.114));
                return lerp(color, fixed3(g, g, g), str);
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 p = -1.0 + 2.0 * i.uv * 10.0;
                float t = _Time * 10.0;

                // main code, *original shader by: 'Plasma' by Viktor Korsun (2011)
                float x = p.x;
                float y = p.y;
                float mov0 = x+y+cos(sin(t)*2.0)*100.+sin(x/100.)*1000.;
                float mov1 = y / 0.9 +  t;
                float mov2 = x / 0.2;
                float c1 = abs(sin(mov1+t)/2.+mov2/2.-mov1-mov2+t);
                float c2 = abs(sin(c1+sin(mov0/1000.+t)+sin(y/40.+t)+sin((x+y)/100.)*3.));
                float c3 = abs(sin(c2+cos(mov1+mov2+c2)+cos(mov2)+sin(x/1000.)));
                fixed4 c = fixed4(c1, c2, c3, 1);

                c *= _MainColor;

                return c;
            }
            ENDCG
        }
    }
}
