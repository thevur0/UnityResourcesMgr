#ifndef SHADER_COMMON
#define SHADER_COMMON

#include "UnityCG.cginc"
#include "AutoLight.cginc"
#include "UnityLightingCommon.cginc"


float4 CalcLightmapUV(float2 uv1)
{
	return  float4(uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw,0,0);
}

float3 CalcWorldNormal(float3 normal)
{
	return UnityObjectToWorldNormal(normal);
}

float4 CalcScreenPos(float4 pos)
{
	return ComputeScreenPos(pos);
}
float4 CalcWorldPos(float4 pos)
{
	return mul(unity_ObjectToWorld, pos);
}

float4 GetLight()
{
	return _LightColor0;
}

float4 GetLightmap(float2 lightmapuv)
{
	return UNITY_SAMPLE_TEX2D(unity_Lightmap, lightmapuv);
}

float4 GetShadowMask(float2 lightmapuv)
{
	return UNITY_SAMPLE_TEX2D(unity_ShadowMask, lightmapuv);
}

UNITY_DECLARE_DEPTH_TEXTURE(_DepthTex);
float4 GetDepthMap(float2 uv)
{
	return SAMPLE_RAW_DEPTH_TEXTURE(_DepthTex, uv);
}

#if defined(SHADOWS_SCREEN)
UNITY_DECLARE_SCREENSPACE_SHADOWMAP(_ScreenSpaceShadowMapTexture);
float4 GetShadowMap(float4 shadowcoord)
{
	return UNITY_SAMPLE_SCREEN_SHADOW(_ScreenSpaceShadowMapTexture, shadowcoord);
}
#else
UNITY_DECLARE_SHADOWMAP(_ShadowMapTexture);
float4 GetShadowMap(float4 shadowcoord)
{
	fixed shadow = UNITY_SAMPLE_SHADOW(_ShadowMapTexture, shadowcoord);
	return shadow;
}

#endif

#endif