// The following comment prevents Unity from auto upgrading the shader. Please keep it to keep backward compatibility
// UNITY_SHADER_NO_UPGRADE

#ifndef _VOLUMETRIC_LIGHT_BEAM_SHARED_INCLUDED_
#define _VOLUMETRIC_LIGHT_BEAM_SHARED_INCLUDED_

#include "UnityCG.cginc"


//#define DEBUG_SHOW_DEPTH 1
//#define DEBUG_SHOW_NOISE3D 1

#if DEBUG_SHOW_DEPTH && !VLB_DEPTH_BLEND
#define VLB_DEPTH_BLEND 1
#endif

#if DEBUG_SHOW_NOISE3D && !VLB_NOISE_3D
#define VLB_NOISE_3D 1
#endif


#if UNITY_VERSION < 540
#define matWorldToObject _World2Object
#define matObjectToWorld _Object2World
inline float4 UnityObjectToClipPos(in float3 pos) { return mul(UNITY_MATRIX_MVP, float4(pos, 1.0)); }
inline float3 UnityObjectToViewPos(in float3 pos) { return mul(UNITY_MATRIX_MV, float4(pos, 1.0)).xyz; }
#else
#define matWorldToObject unity_WorldToObject
#define matObjectToWorld unity_ObjectToWorld
#endif

inline float3 UnityWorldToObjectPos(in float3 pos) { return mul(matWorldToObject, float4(pos, 1.0)).xyz; }
inline float3 UnityObjectToWorldPos(in float3 pos) { return mul(matObjectToWorld, float4(pos, 1.0)).xyz; }

#if VLB_DEPTH_BLEND
sampler2D_float _CameraDepthTexture;

inline float SampleSceneZ(float4 uv)
{
    return LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, uv));
}

float4 DepthFade_VS_ComputeProjPos(float4 vertex_in, float4 vertex_out)
{
    float4 projPos = ComputeScreenPos(vertex_out);
    projPos.z = -UnityObjectToViewPos(vertex_in).z; // = COMPUTE_EYEDEPTH
    return projPos;
}

float DepthFade_PS_BlendDistance(float4 projPos, float distance)
{
    float sceneZ = max(0, SampleSceneZ(UNITY_PROJ_COORD(projPos)) - _ProjectionParams.g);
    float partZ = max(0, projPos.z - _ProjectionParams.g);
    return saturate((sceneZ - partZ) / distance);
}
#endif

inline float lerpClamped(float a, float b, float t)
{
    return lerp(a, b, saturate(t));
}
inline float invLerp(float a, float b, float t)
{
    return (t - a) / (b - a);
}
inline float invLerpClamped(float a, float b, float t)
{
    return saturate(invLerp(a, b, t));
}


struct v2f
{
    float4 posClipSpace : SV_POSITION;
    float4 posObjectSpace : TEXCOORD0;
    float4 posWorldSpace : TEXCOORD1;
    float4 posViewSpaceAndIsCap : TEXCOORD2;
    UNITY_FOG_COORDS(3)
#if VLB_DEPTH_BLEND
    float4 projPos : TEXCOORD4;
#endif
#if VLB_NOISE_3D
    float4 uvgrab : TEXCOORD5;
#endif
};


uniform half4 _Color;
uniform half _AlphaInside;
uniform half _AlphaOutside;
uniform float2 _ConeSlopeCosSin; // between -1 and +1
uniform float2 _ConeRadius; // x = start radius ; y = end radius
uniform float _AttenuationLerpLinearQuad;
uniform float _DistanceFadeStart;
uniform float _DistanceFadeEnd;
#if VLB_DEPTH_BLEND
uniform float _DepthBlendDistance;
uniform float _DepthBlendStart;
#endif
uniform float _DistanceCamClipping;

uniform float _FresnelPow;

uniform float _GlareFrontal;
uniform float _GlareBehind;

#if VLB_NOISE_3D
uniform sampler3D _VLB_NoiseTex3D;
uniform float4 _VLB_NoiseGlobal;
uniform float4 _NoiseLocal;
uniform float3 _NoiseParam;
#endif

#define CONST_PI 3.14159

v2f vert(appdata_base v)
{
    v2f o;

    // compute the proper cone shape
    float4 vertex = v.vertex;
    vertex.xy *= lerp(_ConeRadius.x, _ConeRadius.y, vertex.z);
    vertex.z *= _DistanceFadeEnd;

    o.posClipSpace = UnityObjectToClipPos(vertex);
    o.posWorldSpace = mul(matObjectToWorld, vertex);

    o.posObjectSpace = vertex;

#if VLB_DEPTH_BLEND
    o.projPos = DepthFade_VS_ComputeProjPos(vertex, o.posClipSpace);
#endif

    float3 posViewSpace = UnityObjectToViewPos(vertex);
    float isCap = v.texcoord.x > 0.5;
    o.posViewSpaceAndIsCap = float4(posViewSpace, isCap);

#if VLB_NOISE_3D
#if UNITY_UV_STARTS_AT_TOP
    float scaleY = -1.0;
#else
    float scaleY = 1.0;
#endif
    o.uvgrab.xy = (float2(o.posClipSpace.x, o.posClipSpace.y * scaleY) + o.posClipSpace.w) * 0.5;
    o.uvgrab.zw = o.posClipSpace.zw;
#endif
    UNITY_TRANSFER_FOG(o, o.posClipSpace);
    return o;
}

float GetNoise3DFactor(float3 wpos)
{
#if VLB_NOISE_3D
    float intensity = _NoiseParam.x;
    float3 velocity = lerp(_NoiseLocal.xyz, _VLB_NoiseGlobal.xyz, _NoiseParam.y);
    float scale = lerp(_NoiseLocal.w, _VLB_NoiseGlobal.w, _NoiseParam.z);
	float noise = tex3D(_VLB_NoiseTex3D, frac(wpos * scale + (_Time.y * velocity))).a;
    return lerp(1, noise, intensity);
#else
    return 1;
#endif
}

// Camera forward vector, in world space
inline float3 GetCameraForwardVectorWS()
{
    float4x4 mat = UNITY_MATRIX_V;
    return (-mat[2].xyz);
}

half4 fragShared(v2f i, float outsideBeam)
{
#if DEBUG_SHOW_DEPTH
    return SampleSceneZ(UNITY_PROJ_COORD(i.projPos)) * _ProjectionParams.w;
#endif

#if DEBUG_SHOW_NOISE3D
    return GetNoise3DFactor(i.posWorldSpace);
#endif
    half cameraIsOrtho = unity_OrthoParams.w; // w is 1.0 when camera is orthographic, 0.0 when perspective
    float3 posViewSpace = i.posViewSpaceAndIsCap.xyz;
    float isCap = i.posViewSpaceAndIsCap.w;
    float pixDistFromSource = length(i.posObjectSpace.z);

    // Camera Position in Object Space
    float3 camPosObjectSpace = UnityWorldToObjectPos(_WorldSpaceCameraPos);

    // Vector Camera to current Pixel, in object space and normalized
    float3 vecCamToPixOSN = normalize(i.posObjectSpace.xyz - camPosObjectSpace);

    // Deal with ortho camera:
    // With ortho camera, we don't want to change the fresnel according to camera position.
    // So instead of computing the proper vector "Camera to Pixel", we take account of the "Camera Forward" vector (which is not dependant on the pixel position)
    float3 vecCamForwardOSN = UnityWorldToObjectDir(GetCameraForwardVectorWS());
    vecCamToPixOSN = lerp(vecCamToPixOSN, vecCamForwardOSN, cameraIsOrtho);


    // 0 = camera is placed in front of the source ; 1 = camera is placed behind the source
    float isCamBehind = step(camPosObjectSpace.z, 0);

    // compute the apex distance and position from the truncated radius (simple intercept)
    float ratioRadius = _ConeRadius.x / _ConeRadius.y;
    float apexDist = (_DistanceFadeEnd * ratioRadius) / (1 - ratioRadius);
    float3 apexPosObjectSpace = float3(0,0,-apexDist);

    // Compute a factor to know how far inside the beam cone the camera is
    float2 triangle2D = normalize(float2(length(camPosObjectSpace.xy), camPosObjectSpace.z + apexDist));
    const float maxRadiansDiff = 0.1;
    float isInsideCone = clamp((abs(_ConeSlopeCosSin.y) - abs(triangle2D.x)) / maxRadiansDiff, -1, 1);
    isInsideCone = lerp(isInsideCone, -1, isCamBehind);
    // isInsideCone (-1 : +1)
    // < 0 = the camera is exactly on the cone geometry 
    // > 0 = the camera is inside the cone

    // Compute normal
    float2 cosSinFlat = normalize(i.posObjectSpace.xy);
    float3 normalObjectSpace = normalize(float3(cosSinFlat.x * _ConeSlopeCosSin.x, cosSinFlat.y * _ConeSlopeCosSin.x, -_ConeSlopeCosSin.y));
    normalObjectSpace *= (outsideBeam * 2 - 1); // = outsideBeam ? 1 : -1;
    normalObjectSpace = lerp(normalObjectSpace, float3(0, 0, -1), isCap);

    // compute Boost factor
    float insideBoostDistance = lerp(0, _DistanceFadeEnd, _GlareFrontal);
    float boostFactor = 1 - smoothstep(0, 0 + insideBoostDistance + 0.001, pixDistFromSource); // 0 = no boost ; 1 = max boost
    boostFactor = lerp(boostFactor, 0, outsideBeam); // no boost for outside pass
    boostFactor = lerp(0, boostFactor, saturate(isInsideCone)); // no boost for outside pass
    boostFactor = lerp(boostFactor, 1, isCap); // cap is always at max boost
    
    // Attenuation
    float distFromSourceNormalized = invLerpClamped(_DistanceFadeStart, _DistanceFadeEnd, pixDistFromSource);
    // Almost simple linear attenuation between Fade Start and Fade End: Use smoothstep for a better fall to zero rendering
    float attLinear = smoothstep(0, 1, 1 - distFromSourceNormalized);
    // Unity's custom quadratic attenuation https://forum.unity.com/threads/light-attentuation-equation.16006/
    float attQuad = 1.0 / (1.0 + 25.0 * distFromSourceNormalized * distFromSourceNormalized);
    const float kAttQuadStartToFallToZero = 0.8;
    attQuad *= saturate(smoothstep(1.0, kAttQuadStartToFallToZero, distFromSourceNormalized)); // Near the light's range (fade end) we fade to 0 (because quadratic formula never falls to 0)
    float attenuation = lerp(attLinear, attQuad, _AttenuationLerpLinearQuad);

    // Noise factor
    float noise3DFactor = GetNoise3DFactor(i.posWorldSpace);
 //   noise3DFactor = lerpClamped(noise3DFactor, 1, attenuation * 0.1);
    
    // depth blend factor
#if VLB_DEPTH_BLEND
    float depthBlendDist = _DepthBlendDistance * invLerpClamped(0, _DepthBlendStart, pixDistFromSource);
    float depthBlendFactor = DepthFade_PS_BlendDistance(i.projPos, depthBlendDist);
    depthBlendFactor = lerp(depthBlendFactor, 1, step(_DepthBlendDistance, 0));
    depthBlendFactor = lerp(depthBlendFactor, 1, cameraIsOrtho); // disable depth BlendState factor with ortho camera (temporary fix)
#else
    float depthBlendFactor = 1;
#endif

    // fade when too close factor
    float distCamClipping = lerp(_DistanceCamClipping, 0, boostFactor); // do not fade according to camera when we are in boost zone, to keep boost effect
    float camFadeDistStart = _ProjectionParams.y; // cam near place
    float camFadeDistEnd = camFadeDistStart + distCamClipping;
    float distCamToPixWS = abs(posViewSpace.z); // only check Z axis (instead of length(posViewSpace.xyz)) to have smoother transition with near plane (which is not curved)
    float fadeWhenTooClose = smoothstep(0, 1, invLerpClamped(camFadeDistStart, camFadeDistEnd, distCamToPixWS));
    fadeWhenTooClose = lerp(fadeWhenTooClose, 1, cameraIsOrtho); // fading according to camera eye position doesn't make sense with ortho camera

    float viewDirDotPlusZ = dot(vecCamToPixOSN, float3(0, 0, 1));
    float factorNearAxisZ = abs(viewDirDotPlusZ);

    // disable noise 3D when looking from behind or from inside because it makes the cone shape too much visible
    noise3DFactor = lerp(noise3DFactor, 1, pow(factorNearAxisZ, 10));

    // fresnel
    float fresnel = 0;
    {
        // real fresnel factor
        float fresnelReal = dot(normalObjectSpace, -vecCamToPixOSN);

        // compute a fresnel factor to support long beams by projecting the viewDir vector
        // on the virtual plane formed by the normal and tangent
        float3 tangentPlaneNormal = normalize(i.posObjectSpace.xyz - apexPosObjectSpace);
        float distToPlane = dot(-vecCamToPixOSN, tangentPlaneNormal);
        float3 vec2D = normalize(-vecCamToPixOSN - distToPlane * tangentPlaneNormal);
        float fresnelProjOnTangentPlane = dot(normalObjectSpace, vec2D);

        // blend between the 2 fresnels
        fresnel = lerp(fresnelProjOnTangentPlane, fresnelReal, factorNearAxisZ);
    }

    float fresnelPow = _FresnelPow;

    // Lerp the fresnel pow to the glare factor according to how far we are from the axis Z
    const float kMaxGlarePow = 1.5;
    float glareFactor = kMaxGlarePow * (1 - lerp(_GlareFrontal, _GlareBehind, outsideBeam));
    fresnelPow = lerpClamped(fresnelPow, min(fresnelPow, glareFactor), factorNearAxisZ);

    // Pow the fresnel
    fresnel = saturate(fresnel);
    fresnel = smoothstep(0, 1, fresnel);
    fresnel = saturate(pow(fresnel, fresnelPow));

    // Treat Cap a special way
    fresnel = lerp(fresnel, outsideBeam, isCap);
    outsideBeam = lerp(outsideBeam, 1 - outsideBeam, isCap);

    // Boost distance inside
    float boostFresnel = lerpClamped(fresnel, 1 + 0.001, boostFactor);
    fresnel = lerp(boostFresnel, fresnel, outsideBeam); // no boosted fresnel if outside

    // smooth switch between inside and outside geometry depending of View Direction
    float lookingInsideFactor = saturate(-viewDirDotPlusZ);
    lookingInsideFactor = lerp(lookingInsideFactor, 1 - lookingInsideFactor, outsideBeam);

    float intensity = 1
        * attenuation
        * fadeWhenTooClose
        * depthBlendFactor
        * fresnel
        * lookingInsideFactor
        * noise3DFactor
    ;

    half4 col = _Color * intensity;
    col.rgb *= _Color.a;
    col.rgb *= lerp(_AlphaInside, _AlphaOutside, outsideBeam);

    UNITY_APPLY_FOG_COLOR(i.fogCoord, col, fixed4(0, 0, 0, 0)); // since we use this shader with Additive blending, fog color should be treating as black
    return col;

}



#endif
