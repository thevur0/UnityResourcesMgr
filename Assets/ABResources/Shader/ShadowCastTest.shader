Shader "Unlit/ShadowCastTest"
{
    Properties
    {
				_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo", 2D) = "white" {}

		_Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

		_Glossiness("Smoothness", Range(0.0, 1.0)) = 0.5
		_GlossMapScale("Smoothness Scale", Range(0.0, 1.0)) = 1.0
		[Enum(Metallic Alpha,0,Albedo Alpha,1)] _SmoothnessTextureChannel("Smoothness texture channel", Float) = 0

		[Gamma] _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
		_MetallicGlossMap("Metallic", 2D) = "white" {}

		[ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
		[ToggleOff] _GlossyReflections("Glossy Reflections", Float) = 1.0

		_BumpScale("Scale", Float) = 1.0
		_BumpMap("Normal Map", 2D) = "bump" {}

		_Parallax("Height Scale", Range(0.005, 0.08)) = 0.02
		_ParallaxMap("Height Map", 2D) = "black" {}

		_OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
		_OcclusionMap("Occlusion", 2D) = "white" {}

		_EmissionColor("Color", Color) = (0,0,0)
		_EmissionMap("Emission", 2D) = "white" {}

		_DetailMask("Detail Mask", 2D) = "white" {}

		_DetailAlbedoMap("Detail Albedo x2", 2D) = "grey" {}
		_DetailNormalMapScale("Scale", Float) = 1.0
		_DetailNormalMap("Normal Map", 2D) = "bump" {}

		[Enum(UV0,0,UV1,1)] _UVSec("UV Set for secondary textures", Float) = 0


			// Blending state
			[HideInInspector] _Mode("__mode", Float) = 0.0
			[HideInInspector] _SrcBlend("__src", Float) = 1.0
			[HideInInspector] _DstBlend("__dst", Float) = 0.0
			[HideInInspector] _ZWrite("__zw", Float) = 1.0
    }
	
	SubShader
	{
		
		Tags { "RenderType" = "Opaque" }
		LOD 100
		Pass
		{
			Name "ForwardBase"
			Tags { "LightMode" = "ForwardBase" }
			CGPROGRAM
			#pragma vertex vertBase
			#pragma fragment fragBase

			#pragma multi_compile_fwdbase
			#pragma multi_compile_fog
			#pragma multi_compile_instancing
			#include "UnityStandardCore.cginc"
			#include "UnityStandardCoreForward.cginc"
			#include "UnityCG.cginc"

			//struct appdata
			//{
			//	float4 vertex : POSITION;
			//	float2 uv : TEXCOORD0;
			//};

			//struct v2f
			//{
			//	float2 uv : TEXCOORD0;
			//	UNITY_FOG_COORDS(1)
			//	float4 vertex : SV_POSITION;
			//};


			//v2f vert(appdata v)
			//{
			//	v2f o;
			//	o.vertex = UnityObjectToClipPos(v.vertex);
			//	o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			//	UNITY_TRANSFER_FOG(o,o.vertex);
			//	return o;
			//}

			//fixed4 frag(v2f i) : SV_Target
			//{
			//	// sample the texture
			//	fixed4 col = tex2D(_MainTex, i.uv);
			//	
			//	// apply fog
			//	UNITY_APPLY_FOG(i.fogCoord, col);
			//	return col;
			//}
			ENDCG
		}
		//Pass
		//{
		//	Name "ShadowCaster"
		//	Tags { "LightMode" = "ShadowCaster" }
		//	CGPROGRAM
		//	#include "UnityCG.cginc"
		//	#pragma vertex vert
		//	#pragma fragment frag
		//	//#pragma multi_complie_shadowcaster
		//	struct appdata
		//	{
		//		float4 vertex : POSITION;
		//	};

		//	struct v2f
		//	{
		//		V2F_SHADOW_CASTER;
		//	};

		//	v2f vert(appdata v)
		//	{
		//		v2f o;
		//		TRANSFER_SHADOW_CASTER(o);
		//		return o;
		//	}
		//	fixed4 frag(v2f i) : SV_Target
		//	{
		//		SHADOW_CASTER_FRAGMENT(i);
		//	}
		//	ENDCG
		//}
		
	}
		FallBack "Diffuse"
}
