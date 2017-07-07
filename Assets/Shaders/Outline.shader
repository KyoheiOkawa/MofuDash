// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Sprites/Outline"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
	[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		_Threshold("Threshold", Range(0.1, 1)) = 0.1
		_OutLineColor("Outline Color", Color) = (1, 1, 1, 1)
		_Radius("Radius", Range(0.1, 100)) = 10
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
		Fog{ Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile DUMMY PIXELSNAP_ON
#include "UnityCG.cginc"

		struct appdata
	{
		float4 vertex   : POSITION;
		float4 color    : COLOR;
		float2 texcoord : TEXCOORD0;
	};

	struct v2f
	{
		float4 vertex   : SV_POSITION;
		fixed4 color : COLOR;
		float2 texcoord : TEXCOORD0;
	};

	fixed4 _Color;
	sampler2D _MainTex;
	float _Threshold;
	fixed4 _OutLineColor;
	float _Radius;

	v2f vert(appdata IN)
	{
		v2f OUT;
		OUT.vertex = UnityObjectToClipPos(IN.vertex);
		OUT.texcoord = IN.texcoord;
		OUT.color = IN.color;
#ifdef PIXELSNAP_ON
		OUT.vertex = UnityPixelSnap(OUT.vertex);
#endif

		return OUT;
	}

	sampler2D _AlphaTex;
	float _AlphaSplitEnabled;

	fixed4 SampleSpriteTexture(float2 uv)
	{
		fixed4 color = tex2D(_MainTex, uv);

#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
		if (_AlphaSplitEnabled)
		{
			color.a = tex2D(_AlphaTex, uv).r;
		}
#endif

		return color;
	}

	fixed4 frag(v2f IN) : SV_Target
	{
		float radius = _Radius;

	fixed4 accum = float4(0, 0, 0, 0);
	fixed4 normal = float4(0, 0, 0, 0);

	normal = SampleSpriteTexture(IN.texcoord);

	for (float i = 1.0; i <= radius; i += 1.0f) {
		accum += SampleSpriteTexture(float2(IN.texcoord.x - 0.01f * i, IN.texcoord.y - 0.01f * i));
		accum += SampleSpriteTexture(float2(IN.texcoord.x + 0.01f * i, IN.texcoord.y - 0.01f * i));
		accum += SampleSpriteTexture(float2(IN.texcoord.x + 0.01f * i, IN.texcoord.y + 0.01f * i));
		accum += SampleSpriteTexture(float2(IN.texcoord.x - 0.01f * i, IN.texcoord.y + 0.01f * i));
	}

	accum.rgb = _OutLineColor.rgb * _OutLineColor.a * accum.a * 0.95f;
	float opacity = ((1.0f - normal.a) / radius) * ((_CosTime.w+ 1.0)*.5 / _Threshold);

	normal = (accum * opacity) + (normal * normal.a);

	return normal * _Color;
	}
		ENDCG
	}
	}
}