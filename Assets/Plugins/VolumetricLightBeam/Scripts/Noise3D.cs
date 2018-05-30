using UnityEngine;

#pragma warning disable 0429, 0162 // Unreachable expression code detected (because of Noise3D.isSupported on mobile)

namespace VLB
{
    public static class Noise3D
    {
#if UNITY_IPHONE || UNITY_IOS || UNITY_ANDROID
        public const bool isSupported = false;  // no sampler3D support on mobile
        public static bool isProperlyLoaded { get { return true; } }
#else
        public const bool isSupported = true;
        public static bool isProperlyLoaded { get { return ms_NoiseTexture != null; } }
#endif

        static Texture3D ms_NoiseTexture = null;
        const HideFlags kHideFlags = HideFlags.HideAndDontSave; // hide the noise texture


#if UNITY_EDITOR
        public static void _EditorForceReloadData()
        {
            if (ms_NoiseTexture)
            {
                Object.DestroyImmediate(ms_NoiseTexture);
                ms_NoiseTexture = null;
            }
            LoadIfNeeded();
        }
#endif

        public static void LoadIfNeeded()
        {
            if (!isSupported) return;

            if (ms_NoiseTexture == null)
            {
                ms_NoiseTexture = LoadTexture3D(Config.Instance.noise3DData, Config.Instance.noise3DSize);
                if(ms_NoiseTexture)
                    ms_NoiseTexture.hideFlags = kHideFlags;
            }

            Shader.SetGlobalTexture("_VLB_NoiseTex3D", ms_NoiseTexture);
            Shader.SetGlobalVector("_VLB_NoiseGlobal", Config.Instance.globalNoiseParam);
        }

        static Texture3D LoadTexture3D(TextAsset textData, int size)
        {
            if (textData == null)
            {
                Debug.LogErrorFormat("Fail to open Noise 3D Data");
                return null;
            }

            var bytes = textData.bytes;
            Debug.Assert(bytes != null);

            int dataLen = Mathf.Max(0, size * size * size);
            if (bytes.Length != dataLen)
            {
                Debug.LogErrorFormat("Noise 3D Data file has not the proper size {0}x{0}x{0}", size);
                return null;
            }

            var tex = new Texture3D(size, size, size, TextureFormat.Alpha8, false);

            var colors = new Color[dataLen];
            for (int i = 0; i < dataLen; ++i)
                colors[i] = new Color32(0, 0, 0, bytes[i]);

            tex.SetPixels(colors);
            tex.Apply();
            return tex;
        }
    }
}