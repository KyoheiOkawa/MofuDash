Shader "Sprites/ScrollSprite"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_Speed("Speed",Float) = 1.0
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
	    [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
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
	float _Speed;

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
	    float2 texcoord = IN.texcoord;
	    texcoord.x -= _Speed * _Time;

		fixed4 normal = SampleSpriteTexture(texcoord);

		return normal*_Color;
	}
		ENDCG
	}
	}
}
