using UnityEngine;

namespace VLB
{
    public static class Consts
    {
        public static bool ProceduralObjectsVisibleInEditor = true;
        public static HideFlags ProceduralObjectsHideFlags { get { return ProceduralObjectsVisibleInEditor ? (HideFlags.NotEditable | HideFlags.DontSave) : (HideFlags.HideAndDontSave); } }

        public const float Alpha = 1f;
        public const float SpotAngle = 35f;
        public const float ConeRadiusStart = 0.1f;
        public const int GeomSides = 18;
        public const bool GeomCap = false;

        public const VolumetricLightBeam.AttenuationEquation AttenuationEquation = VolumetricLightBeam.AttenuationEquation.Quadratic;
        public const float AttenuationCustomBlending = 0.5f;
        public const float FadeStart = 0f;
        public const float FadeEnd = 3f;
        public const float FadeMinThreshold = 0.01f;

        public const float DepthBlendDistance = 2f;
        public const float CameraClippingDistance = 0.5f;

        public const float FresnelPowMaxValue = 10f;
        public const float FresnelPow = 8f;

        public const float GlareFrontal = 0.5f;
        public const float GlareBehind = 0.5f;

        public const float NoiseIntensityDefault = 0.5f;

        public const float NoiseScaleMin = 0.01f;
        public const float NoiseScaleMax = 2f;
        public const float NoiseScaleDefault = 0.5f;

        public static Vector3 NoiseVelocityDefault = new Vector3(0.07f, 0.18f, 0.05f);
    }
}