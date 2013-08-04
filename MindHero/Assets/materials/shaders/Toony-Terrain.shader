Shader "Toon/Toony Terrain" {
	Properties {
		[HideInInspector] _Control ("Control (RGBA)", 2D) = "red" {}
		[HideInInspector] _Splat3 ("Layer 3 (A)", 2D) = "white" {}
		[HideInInspector] _Splat2 ("Layer 2 (B)", 2D) = "white" {}
		[HideInInspector] _Splat1 ("Layer 1 (G)", 2D) = "white" {}
		[HideInInspector] _Splat0 ("Layer 0 (R)", 2D) = "white" {}
		// used in fallback on old cards & base map
		[HideInInspector] _MainTex ("BaseMap (RGB)", 2D) = "white" {}
		[HideInInspector] _Color ("Main Color", Color) = (1,1,1,1)
		_Ramp ("Toon Ramp", 2D) = "white" { }
	}
	
	SubShader {
		Tags
		{
				"SplatCount" = "4"
				"Queue" = "Geometry-100"
				"RenderType" = "Opaque"
		}

		CGPROGRAM
		#pragma surface surf ToonRamp

		sampler2D _Ramp;

		#pragma lighting ToonRamp exclude_path:prepass
		inline half4 LightingToonRamp (SurfaceOutput s, half3 lightDir, half atten)
		{
			#ifndef USING_DIRECTIONAL_LIGHT
			lightDir = normalize(lightDir);
			#endif
	
			half diffuse = dot (s.Normal, lightDir) * .5 + .5;
			half3 ramp = tex2D (_Ramp, float2(diffuse, diffuse)).rgb;
	
			half4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * (ramp * atten * 2);
			c.a = 0;
			return c;
		}

		struct Input 
		{
			float2 uv_Control : TEXCOORD0;
			float2 uv_Splat0 : TEXCOORD1;
			float2 uv_Splat1 : TEXCOORD2;
			float2 uv_Splat2 : TEXCOORD3;
			float2 uv_Splat3 : TEXCOORD4;
		};

		sampler2D _Control;
		sampler2D _Splat0,_Splat1,_Splat2,_Splat3;

		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed4 splat_control = tex2D (_Control, IN.uv_Control);
			fixed3 col;
			col  = splat_control.r * tex2D (_Splat0, IN.uv_Splat0).rgb;
			col += splat_control.g * tex2D (_Splat1, IN.uv_Splat1).rgb;
			col += splat_control.b * tex2D (_Splat2, IN.uv_Splat2).rgb;
			col += splat_control.a * tex2D (_Splat3, IN.uv_Splat3).rgb;
			o.Albedo = col;
			o.Alpha = 0.0;
		}
		ENDCG  
	}

	Fallback "Diffuse"
}
