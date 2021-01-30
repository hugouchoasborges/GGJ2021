// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "2D Custom/SpriteCustom" 
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}

		[Header(Color Blend)]
		_Color("Color Blend", Color) = (1.0, 1.0, 1.0, 1.0)
		_ColorBlend("Blend Amount", Range(0.0, 1.0)) = 0.0

		[Header(Grayscale)]
		_OuterGrayscaleBlend("Outer GS Blend", Range(0.0, 1.0)) = 0.0
		_InnerGrayscaleBlend("Inner GS Blend", Range(0.0, 1.0)) = 0.0
		_GrayscaleRadius("GS Radius", float) = 0.0
		_GrayscaleCenterPosition("GS Center Position", vector) = (0, 0, 0, 0)
		_GrayscaleCenterRadius("GS Center Radius", float) = 0.0
	}

	//Base layer
	Subshader
	{
		Tags
		{
			"Queve" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True" 
		}

		Cull Off 
		Lighting Off
		ZWrite Off
		Fog {Mode Off}
		Blend One OneMinusSrcAlpha

		Pass
		{

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile DUMMY PIXELSNAP_ON
			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 color : COLOR;
				float2 texcoord: TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 worldPos : TEXCOORD1;
				float3 normal : NORMAL;
				fixed4 color : COLOR;
				half2 texcoord : TEXCOORD0;
			};

			fixed4 _Color;
			fixed _ColorBlend;
			float PixelSnap;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.worldPos = mul(unity_ObjectToWorld, IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color;// * _Color;

				#if PIXELSNAP_ON
					OUT.vertex = UnityPixelSnap(OUT.vertex);
				#endif

				if(PixelSnap > 0.1)
					OUT.vertex = UnityPixelSnap(OUT.vertex);
				return OUT;
			}

			sampler2D _MainTex;
			fixed3 _GrayscaleCenterPosition;
			fixed _GrayscaleCenterRadius;
			fixed _InnerGrayscaleBlend;
			fixed _OuterGrayscaleBlend;
			fixed _GrayscaleRadius;

			fixed NormalizeCoord(fixed coord, fixed bounds)
			{
				fixed index = floor(coord / (1.0 / bounds));
				index = clamp(index, 0, index);
				coord -= (index * (1.0 / bounds));
				coord *= bounds;
				return coord;
			}

			fixed GetGrayscaleBlend(fixed3 pos)
			{
				fixed dist = distance(pos, _GrayscaleCenterPosition);
				//fixed dist = abs(_GrayscaleCenterPosition.x - pos.x);
				if(dist < _GrayscaleCenterRadius) return _InnerGrayscaleBlend;
				if(dist > _GrayscaleCenterRadius + _GrayscaleRadius) return _OuterGrayscaleBlend;
				fixed factor = (dist - _GrayscaleCenterRadius) / _GrayscaleRadius;
				return lerp(_InnerGrayscaleBlend, _OuterGrayscaleBlend, factor);
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 color = tex2D(_MainTex, IN.texcoord) * IN.color;

				if(color.a < 0.15)
				{
					discard;
				}

				fixed blend = GetGrayscaleBlend(IN.worldPos);
				if(blend > 0)
				{
					float luminosity = (0.299 * color.r) + (0.597 * color.g) + (0.114 * color.b);
					fixed3 gray = lerp(color, luminosity, 1);
	
					color.r = lerp(color.r, gray.r, blend);
					color.g = lerp(color.g, gray.g, blend);
					color.b = lerp(color.b, gray.b, blend);
				}

				fixed4 c;
				c.rgb = lerp(color.rgb, _Color.rgb, _ColorBlend) * color.a;
				c.a = color.a;

				return c;
			}

			ENDCG

		}

	}

}




