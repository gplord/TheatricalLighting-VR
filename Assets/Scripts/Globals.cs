using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour {

    public static float LightBeamOutside = 0.35f;
    public static float LightBeamInside = 1f;

    public static float LightClampFactor = 0.15f;

    public static string FixturesPath = "Fixtures/";
    public static string CuesPath = "Cues/";

    public static bool ShowingHaze = true;

    public static float AdjustLight(float input)
    {
        return input * LightClampFactor;
    }

    public static float AdjustBeamValue(float beamType, float intensity)
    {
        return (beamType / 100f) * intensity;
    }

    public static float AdjustLightReverse(float input)
    {
        float result = (1 / LightClampFactor) * input;
        if (result > 100f)
        {
            result = 100f;
        }
        return result;
    }

}
