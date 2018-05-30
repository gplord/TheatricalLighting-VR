using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Sirenix.OdinInspector;

public class LightCues : MonoBehaviour {
    
    public Dictionary<int, float> cuesData = new Dictionary<int, float>();
    public Light lightComponent;
    private float targetIntensity = 0;

    private void Awake() {
        lightComponent = GetComponentInChildren<Light>();
    }
    
    void Start () {
        cuesData[0] = 0f;
	}

    public void NewIntensity(float newIntensity, float timeTransition) {
        StopAllCoroutines();
        StartCoroutine(ChangeIntensity(newIntensity, timeTransition));
    }

    public IEnumerator ChangeIntensity(float newIntensity, float timeTransition) {
        float elapsedTime = 0;
        float previousIntensity = lightComponent.intensity;

        while (elapsedTime < timeTransition)
        {
            lightComponent.intensity = Globals.AdjustLight(Mathf.Lerp(previousIntensity, newIntensity, (elapsedTime / timeTransition)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield break;

    }

}
