Shader "Custom/WeightMask" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_MaskTex ("Mask (RGB)", 2D) = "white" {}
		_AlphaCut("Alpha Cuttoff", Range(0, 1)) = 0.5
	}
	SubShader 
	{
		Lighting off
		ZWrite off
		Tags 
		{ 
			"Queue"="Transparent" 
			"RenderType"="Transparent"
			"IgnoreProjector"="true"
		}
		
		Pass 
		{
			Blend SrcAlpha OneMinusSrcAlpha
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct v2f
			{
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
			};
			
			sampler2D _MainTex;
			sampler2D _MaskTex;
			fixed _AlphaCut;

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_UV(0);
				o.uv2 = TRANSFORM_UV(1);

				return o;
			}
			
			half4 frag(v2f i) : COLOR
			{
				half4 col = tex2D(_MainTex, i.uv);
				half ma  = tex2D(_MaskTex, i.uv2).a;
				
				col.a = ma < _AlphaCut ? col.a : 0;
				
				return col;
			}
		
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
