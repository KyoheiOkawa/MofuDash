Shader "Sprites/Block"
{
	Properties
	{
		_MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint",Color) = (1,1,1,1)
		_MinusColor ("MinusColor",Color) = (.5,.5,.5,.5)
	}
	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}
		
			Cull Off
			Lighting Off
			ZWrite Off
			Blend One OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color  : COLOR;
				float2 uv : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
				float2 uv : TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			fixed4 _Color;
			fixed4 _MinusColor;
			
			v2f vert (appdata IN)
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.uv = IN.uv;
				OUT.color = IN.color * _Color;

				return OUT;
			}

			fixed4 SampleSpriteTexture(float2 uv)
			{
				fixed4 color = tex2D(_MainTex, uv);

				return color;
			}
			
			fixed4 frag (v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture(IN.uv) * IN.color;
			c.r = abs(c.r - _MinusColor.r);
			c.g = abs(c.g - _MinusColor.g);
			c.b = abs(c.b - _MinusColor.b);
			c.rgb *= c.a;
			return c;
			}
			ENDCG
		}
	}
}
