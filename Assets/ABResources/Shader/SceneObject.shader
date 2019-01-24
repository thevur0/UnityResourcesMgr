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
			#pragma multi_compile _ DIRECTIONAL
			#pragma multi_compile _ LIGHTPROBE_SH
			//#pragma multi_compile _ NORMALMAP_ON
			//#pragma multi_compile _ TIME_ON
#define SHADOWS_SCREEN
            #include "UnityCG.cginc"
			#include "AutoLight.cginc"
			#include "Common.cginc"
			#include "UnityPBSLighting.cginc"
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
				float4 lightmapUV : TEXCOORD1;
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
			


			UnityGI UnityGICalc(v2f i)
			{
				UnityLight light;
				light.color = _LightColor0.rgb;
				light.dir = _WorldSpaceCameraPos.xyz;

				UnityGIInput d;
				d.light = light;
				d.worldPos = i.worldPos;
				d.worldViewDir = UnityWorldSpaceViewDir(i.worldPos);
#if defined(LIGHTMAP_ON)
				d.ambient = 0;
				d.lightmapUV = i.lightmapUV;
#else
				d.ambient = i.lightmapUV.rgb;
				d.lightmapUV = 0;
#endif

				d.atten = UnityComputeForwardShadows(d.lightmapUV, i.worldPos, (READ_SHADOW_COORDS(i)));


				d.boxMax[0] = unity_SpecCube0_BoxMax;
				d.boxMax[1] = unity_SpecCube1_BoxMax;
				d.boxMin[0] = unity_SpecCube0_BoxMin;
				d.boxMin[1] = unity_SpecCube1_BoxMin;
				d.probePosition[0] = unity_SpecCube0_ProbePosition;
				d.probePosition[1] = unity_SpecCube1_ProbePosition;
				d.probeHDR[0] = unity_SpecCube0_HDR;
				d.probeHDR[1] = unity_SpecCube1_HDR;

				Unity_GlossyEnvironmentData g;
				g.roughness = 1;
				g.reflUVW = 1;

				UnityGI gi = UnityGlobalIllumination(d, 1, i.worldNormal, g);
				return gi;
			}




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
				fixed4 diffuseColor = tex2D(_MainTex, i.uv);
			//return diffuseColor;


				UnityGI gi = UnityGICalc(i);
				return float4((gi.light.color + gi.indirect.diffuse) * diffuseColor,1);
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
