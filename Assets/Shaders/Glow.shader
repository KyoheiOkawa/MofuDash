// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Sprites/Glow"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		_ShineLocation("ShineLocation", Range(0,1)) = 0
		_ShineWidth("ShineWidth", Range(0,1)) = 0
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
	}

	SubShader
	{
		Tags 
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType"="Transparent"
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
			#pragma multi_compile _ PIXELSNAP_ON
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			fixed4 _Color;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				o.color = v.color * _Color;

#ifdef PIXELSNAP_ON
				o.vertex = UnityPixelSnap(o.vertex);
#endif

				return o;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			float _AlphaSplitEnabled;
			float _ShineLocation;
			float _ShineWidth;

			fixed4 SampleSpriteTexture(float2 uv)
			{
				fixed4 color = tex2D(_MainTex, uv);

#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
					color.a = tex2D(_AlphaTex, uv).r;
#endif

				float lowLevel = _ShineLocation - _ShineWidth;
				float highLevel = _ShineLocation + _ShineWidth;
				float currentDistaceProjection = (uv.x + uv.y) / 2;
				if (currentDistaceProjection > lowLevel && currentDistaceProjection < highLevel)
				{
					float whitePower = 1 - (abs(currentDistaceProjection - _ShineLocation) / _ShineWidth);
					color.rgb += color.a * whitePower;
				}

				return color;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 c = SampleSpriteTexture(i.texcoord) * i.color;
				c.rgb *= c.a;

				return c;
			}
			ENDCG
		}
	}
}
