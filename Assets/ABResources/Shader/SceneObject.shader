Shader "Unlit/SceneObject"
{
    Properties
    {
		_Color("Color",Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}

		[HideInInspector][Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull",float) = 2.0
		[HideInInspector][Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("SrcBlend",float) = 1.0
		[HideInInspector][Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("DstBlend",float) = 0
		[HideInInspector][Enum(False,0,True,1)] _ZClip("ZClip", float) = 1.0
		[HideInInspector][Enum(Off,0,On,1)] _ZWrite("ZWrite", float) = 1
		[HideInInspector][Enum(Less,2,Equal,3,LEqual,4,Greater ,5, NotEqual,6, GEqual,7, Always,8)] _ZTest("ZTest", float) = 4

		_CutOff("Cut Off", Range(0,1)) = 0.5

		_MetallicPower("MetallicPower",Range(0,2)) = 1
		[Toggle(NORMALMAP_ON)] _NORMALMAP("NormalMap On", int) = 0
		_NormalMap("NormalMap", 2D) = "black" {}
    }
    SubShader
    {
        Tags { "RenderType"="Geometry" }
        LOD 100
		Cull [_Cull]
		Blend[_SrcBlend][_DstBlend]
		ZWrite[_ZWrite]
		ZClip[_ZClip]
		ZTest[_ZTest]

        Pass
        {
			Name "Forward"
			Tags{ "LightMode" = "ForwardBase" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

			//#ifndef UNITY_PASS_FORWARDBASE
			//	#define UNITY_PASS_FORWARDBASE
			//#endif

			//#pragma multi_compile_fwdbase_fullshadows
            // make fog work
            #pragma multi_compile_fog

			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile _ SHADOWS_SHADOWMASK
			#pragma multi_compile _ SHADOWS_SCREEN
#define SHADOWS_SCREEN 1
			//#pragma multi_compile _ NORMALMAP_ON
			//#pragma multi_compile _ TIME_ON


            #include "UnityCG.cginc"
			#include "AutoLight.cginc"
			#include "Common.cginc"
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float2 uv1: TEXCOORD1;
				float3 normal: NORMAL;
				float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
				float2 lightmapUV : TEXCOORD1;
				float4 worldPos : TEXCOORD2;
				UNITY_FOG_COORDS(9)
				unityShadowCoord4 _ShadowCoord:TEXCOORD3;

				float3 worldNormal: NORMAL;
                float4 pos : SV_POSITION;
				float4 vertexColor:COLOR;
				
				
            };
			
            sampler2D _MainTex;
            float4 _MainTex_ST;
			fixed _MetallicPower;
			fixed _CutOff;
			
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldPos = CalcWorldPos(v.vertex);
				o.worldNormal = CalcWorldNormal(v.normal);
				o.vertexColor = v.color;

#if  defined(LIGHTMAP_ON)
				o.lightmapUV = CalcLightmapUV(v.uv1.xy);
#endif
				TRANSFER_SHADOW(o);//o._ShadowCoord = mul(unity_WorldToShadow[0], mul(unity_ObjectToWorld, v.vertex));
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);

				//float fAtten = UnityComputeForwardShadows(i._ShadowCoord.xy,i.worldPos,READ_SHADOW_COORDS(i));

				float4 lightmap = 0;
				float shadowmask = 0;
				float shadowmap = 0;

#if  defined(LIGHTMAP_ON)
				lightmap = GetLightmap(i.lightmapUV);
				shadowmask = GetShadowMask(i.lightmapUV);
				shadowmap = shadowmask;
#else
				float4 screenpos = CalcScreenPos(i.pos);
				shadowmap = unitySampleShadow(i._ShadowCoord);
#endif
				return col* shadowmap*GetLight();
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }

		Pass
		{
			Name "ShadowCaster"
			Tags { "LightMode" = "ShadowCaster" }
			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				V2F_SHADOW_CASTER;
			};

			v2f vert(appdata v)
			{
				v2f o;
				TRANSFER_SHADOW_CASTER(o);
				return o;
			}
			fixed4 frag(v2f i) : SV_Target
			{
				SHADOW_CASTER_FRAGMENT(i);
			}
			ENDCG
		}
    }
}
