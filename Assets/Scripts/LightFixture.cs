using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VLB;

public class LightFixture : SelectableObject {
    public Transform lightTransform;
    public Light spotlight;
    public CaptureData captureData;

    public int cookieIndex;

    Renderer beamRenderer;

    GameManager gameManager;
        
    // Use this for initialization
    public override void Awake ()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        base.Awake();
	}

    private void Start()
    {
    }

    // Update is called once per frame
    void Update () {
    }

    public void NewIntensity(float newIntensity, float timeTransition)
    {
        //SetColor();
        StopAllCoroutines();
        StartCoroutine(ChangeIntensity(newIntensity, timeTransition));
    }

    public void SetColor()
    {
        Color newColor = new Color(captureData.colorR / 255.0f, captureData.colorG / 255.0f, captureData.colorB / 255.0f);
        spotlight.color = newColor;
        GetComponentInChildren<VLB.BeamGeometry>().gameObject.GetComponent<Renderer>().material.SetColor("_Color", newColor);
        GetComponentInChildren<VLB.BeamGeometry>().gameObject.GetComponent<Renderer>().material.SetFloat("_AlphaInside", Globals.LightBeamInside);
        GetComponentInChildren<VLB.BeamGeometry>().gameObject.GetComponent<Renderer>().material.SetFloat("_AlphaOutside", Globals.LightBeamOutside);
    }

    public IEnumerator ChangeIntensity(float newIntensity, float timeTransition)
    {
        float elapsedTime = 0;
        float previousIntensity = spotlight.intensity;//Globals.AdjustLight(spotlight.intensity);

        beamRenderer = GetComponentInChildren<VLB.BeamGeometry>().gameObject.GetComponent<Renderer>();
        SetEmissionColor(newIntensity, spotlight.color);

        float previousBeamIntensityInside = beamRenderer.material.GetFloat("_AlphaInside");
        float previousBeamIntensityOutside = beamRenderer.material.GetFloat("_AlphaOutside");
        float newBeamIntensityInside = 0f;
        float newBeamIntensityOutside = 0f;
        if (newIntensity > 0f)
        {
            newBeamIntensityInside = Globals.AdjustBeamValue(Globals.LightBeamInside, newIntensity);
            newBeamIntensityOutside = Globals.AdjustBeamValue(Globals.LightBeamOutside, newIntensity);
        }

        if (gameManager.showHaze == false)
        {
            newBeamIntensityInside = 0f;
            newBeamIntensityOutside = 0f;
        }

        while (elapsedTime < timeTransition)
        {
            spotlight.intensity = Mathf.Lerp(previousIntensity, Globals.AdjustLight(newIntensity), (elapsedTime / timeTransition));
            beamRenderer.material.SetFloat("_AlphaInside", Mathf.Lerp(previousBeamIntensityInside, newBeamIntensityInside, (elapsedTime / timeTransition)));
            beamRenderer.material.SetFloat("_AlphaOutside", Mathf.Lerp(previousBeamIntensityOutside, newBeamIntensityOutside, (elapsedTime / timeTransition)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        spotlight.intensity = Globals.AdjustLight(newIntensity);
        beamRenderer.material.SetFloat("_AlphaInside", newBeamIntensityInside);
        beamRenderer.material.SetFloat("_AlphaOutside", newBeamIntensityOutside);

        yield break;

    }

    public IEnumerator ChangeBeamIntensity(float newIntensity, float timeTransition)
    {
        float elapsedTime = 0;
        yield break;
    }

}
