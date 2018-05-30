using System;
using UnityEngine;

namespace VLB
{
    public static class Utils
    {
        public static T NewWithComponent<T>(string name) where T : Component
        {
            return (new GameObject(name, typeof(T))).GetComponent<T>();
        }

        public static T GetOrAddComponent<T>(this GameObject self) where T : Component
        {
            var component = self.GetComponent<T>();
            if (component == null)
                component = self.AddComponent<T>();
            return component;
        }

        public static T GetOrAddComponent<T>(this MonoBehaviour self) where T : Component
        {
            return self.gameObject.GetOrAddComponent<T>();
        }

        /// <summary>
        /// true if the bit field or bit fields that are set in flags are also set in the current instance; otherwise, false.
        /// </summary>
        public static bool HasFlag(this Enum mask, Enum flags) // Same behavior than Enum.HasFlag is .NET 4
        {
#if DEBUG
            if (mask.GetType() != flags.GetType())
                throw new System.ArgumentException(string.Format("The argument type, '{0}', is not the same as the enum type '{1}'.", flags.GetType(), mask.GetType()));
#endif
            return ((int)(IConvertible)mask & (int)(IConvertible)flags) == (int)(IConvertible)flags;
        }
    }
}
