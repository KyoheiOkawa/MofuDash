// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "GUI/StageText Shader" {
	Properties{
		_MainTex("Font Texture", 2D) = "white" {}
	_Color("Text Color", Color) = (1,1,1,1)
	}

		SubShader{

		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		Lighting Off Cull Off ZTest Always ZWrite On Fog{ Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha

		Pass{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest

#include "UnityCG.cginc"

		struct appdata_t {
		float4 vertex : POSITION;
		fixed4 color : COLOR;
		float2 texcoord : TEXCOORD0;
	};

	struct v2f {
		float4 vertex : POSITION;
		fixed4 color : COLOR;
		float2 texcoord : TEXCOORD0;
	};

	sampler2D _MainTex;
	uniform float4 _MainTex_ST;
	uniform fixed4 _Color;

	v2f vert(appdata_t v)
	{
		v2f o;
		v.vertex *= 0.025;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.color = v.color * _Color;
		o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
		return o;
	}

	fixed4 frag(v2f i) : COLOR
	{
	fixed4 col = i.color;

	col.a *= UNITY_SAMPLE_1CHANNEL(_MainTex, i.texcoord);
	if (col.a != 1.0)
		discard;

	return col;
	}
		ENDCG
	}
	}
}