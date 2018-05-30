using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VLB;

public class LightManager : MonoBehaviour {

    public Dictionary<int, GameObject> lightIds;
    public bool increasedVisibility = false;

    public Slider ambientLightSlider;

    public List<int> highlightedLights;

    public InputField lightAdjustFactorInput;

    public GameManager gameManager;

    public GameObject lightMenuClosed;
    public GameObject lightMenuOpen;

    void Awake()
    {
        highlightedLights = new List<int>();
        if (lightIds == null) lightIds = new Dictionary<int, GameObject>();
    }
    
    void Start () {
        ambientLightSlider.onValueChanged.AddListener(delegate { AmbientLightChange(); });
        lightAdjustFactorInput.text = Globals.LightClampFactor.ToString();
        lightAdjustFactorInput.onEndEdit.AddListener(delegate { LightAdjustInputChange(); });
	}
    
    void Update() {
    }

    public void AddToLights(int index, GameObject lightGameObject)
    {
        if (lightIds.ContainsKey(index)) {
            //Debug.LogWarning("Channel " + index + " already exists; skipping.");
        } else {
            lightIds.Add(index, lightGameObject);
        }
    }

    void AmbientLightChange()
    {
        RenderSettings.ambientLight = new Color(ambientLightSlider.value, ambientLightSlider.value, ambientLightSlider.value);
    }
    void LightAdjustInputChange()
    {
        Globals.LightClampFactor = float.Parse(lightAdjustFactorInput.text);
        gameManager.cuesManager.ReloadCue();
    }

    public void HighlightChannel(int channelId)
    {
        foreach (int id in gameManager.captureImport.channels[channelId].lightIds)
        {
            HighlightLinked(id);
        }
    }

    public void HighlightLinked (int id)
    {
        lightIds[id].GetComponent<LightFixture>().HighlightLinked(true);
        highlightedLights.Add(id);
    }
    public void UnhighlightAllLinked()
    {
        foreach (int id in highlightedLights)
        {
            lightIds[id].GetComponent<LightFixture>().HighlightLinked(false);
        }
        highlightedLights.Clear();
    }

    public void UnsetAllLightData()
    {
        lightIds = new Dictionary<int, GameObject>();
        increasedVisibility = false;
    }

}
