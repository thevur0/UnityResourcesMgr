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
			//#pragma shader_feature NORMALMAP_ON 
			#pragma multi_compile _ NORMALMAP_ON
			#pragma multi_compile _ TIME_ON
			//#define TIME_ON 1

			

			//#define SHADOWS_SHADOWMASK 1
			//#define SHADOWS_SCREEN 1

            #include "UnityCG.cginc"
			#include "AutoLight.cginc"
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 worldPos : TEXCOORD2;
				UNITY_LIGHTING_COORDS(3,4)
                float4 vertex : SV_POSITION;
				
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			fixed _MetallicPower;
			fixed _CutOff;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				//float fAtten = READ_SHADOW_COORDS(i);
				float fAtten = UnityComputeForwardShadows(i._ShadowCoord.xy,i.worldPos,READ_SHADOW_COORDS(i));

				col = float4(fAtten, fAtten, fAtten, 1);

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
