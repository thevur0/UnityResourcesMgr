#ifndef SHADER_COMMON
#define SHADER_COMMON

#include "UnityCG.cginc"
#include "AutoLight.cginc"
#include "UnityLightingCommon.cginc"


float2 CalcLightmapUV(float2 uv1)
{
	return  uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
}

float3 CalcWorldNormal(float3 normal)
{
	return UnityObjectToWorldNormal(normal);
}

float4 CalcScreenPos(float4 pos)
{
	return ComputeScreenPos(pos);
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

UNITY_DECLARE_SHADOWMAP(_ShadowMapTexture);
float4 GetShadowMap(float4 worldPos)
{
	return UNITY_SAMPLE_SHADOW(_ShadowMapTexture, worldPos);
}

UNITY_DECLARE_SCREENSPACE_SHADOWMAP(_ScreenShadowMapTexture);
float4 GetScreenShadowMap(float4 shadowcoord)
{
	return UNITY_SAMPLE_SCREEN_SHADOW(_ScreenShadowMapTexture, shadowcoord);
}

#endif