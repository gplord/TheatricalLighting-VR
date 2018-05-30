using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightShutters : MonoBehaviour {

    public SkinnedMeshRenderer skinnedMeshRenderer;
    //Mesh mesh;
    int blendShapeCount;

    public float adjustMin;
    
    [Range(0f, 100f)]
    public float topShutter;
    [Range(0f, 100f)]
    public float rightShutter;
    [Range(0f, 100f)]
    public float bottomShutter;
    [Range(0f, 100f)]
    public float leftShutter;

    // Use this for initialization
    void Awake() {
        //skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        //mesh = skinnedMeshRenderer.GetComponent<SkinnedMeshRenderer>().sharedMesh;
    }

    private void Start() {
        //blendShapeCount =  mesh.blendShapeCount;
	}
	
	// Update is called once per frame
	void Update () {
        skinnedMeshRenderer.SetBlendShapeWeight((int)ShutterBlendShapes.Top, AdjustShutter(topShutter));
        skinnedMeshRenderer.SetBlendShapeWeight((int)ShutterBlendShapes.Right, AdjustShutter(rightShutter));
        skinnedMeshRenderer.SetBlendShapeWeight((int)ShutterBlendShapes.Bottom, AdjustShutter(bottomShutter));
        skinnedMeshRenderer.SetBlendShapeWeight((int)ShutterBlendShapes.Left, AdjustShutter(leftShutter));
    }

    float AdjustShutter(float amount)
    {
        return AdjustedValue(amount, 100f, 0f, 100f, adjustMin);
    }

    float AdjustedValue (float OldValue, float OldMax, float OldMin, float NewMax, float NewMin)
    {
        float OldRange = OldMax - OldMin;
        float NewRange = NewMax - NewMin;
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;
        return NewValue;
    }

}

public enum ShutterBlendShapes
{
    Top,
    Right,
    Bottom,
    Left
}
