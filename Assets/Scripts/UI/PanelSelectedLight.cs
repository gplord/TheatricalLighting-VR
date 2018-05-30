using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PanelSelectedLight : MonoBehaviour {

    public GameManager gameManager;

    //public LightMount lightFixture;
    public LightFixture lightFixture;

    private Transform lightTranform;

    public Text label;

    //public Dropdown lightRail;
    public Dropdown gelColorDropdown;

    //public Toggle lightEnabled;

    //public Slider lightRange;

    public Image colorPreview;

    public Slider lightColorR;
    public Slider lightColorG;
    public Slider lightColorB;

    public InputField lightColorRInput;
    public InputField lightColorGInput;
    public InputField lightColorBInput;

    //public Slider lightPosition;
    public InputField lightPosX;
    public InputField lightPosY;
    public InputField lightPosZ;

    public Slider lightPan;
    public Slider lightTilt;
    //public Slider lightAngle;

    public InputField lightPanInput;
    public InputField lightTiltInput;

    //public Text lightPositionText;
    //public Text lightPanText;
    //public Text lightTiltText;

    //public Text lightColorRText;
    //public Text lightColorGText;
    //public Text lightColorBText;
    
    public Slider lightIntensity;
    public InputField lightIntensityInput;

    public Dropdown lightGoboDropdown;
    private int lightGoboIndex;

    public Button editChannel;
    public Text channelNumber;

    public Button deleteLight;
    public Button showChannel;

    public GameObject errorPosition;

    GelColorListData gelColorsList;

    Color inputFieldColor = new Color(0,0,0,0.69f);
    Color inputErrorColor = new Color(0.88f, 0.24f, 0.24f, 1.0f);

    bool setupComplete = false;

    void Start() {
        ResetLightPanel();
    }

    public void ResetLightPanel()
    {

        Deselect();

        lightFixture = null;

        lightIntensity.value = 50;
        lightIntensityInput.text = "50";
        lightColorRInput.text = lightColorGInput.text = lightColorBInput.text = "127";
        
        lightPosX.text = lightPosY.text = lightPosZ.text = "0";
        lightColorR.value = lightColorG.value = lightColorB.value = 127;

        lightPan.value = -180;
        lightTilt.value = 0;
        lightPanInput.text = "-180";
        lightTiltInput.text = "0";

        channelNumber.text = "0";

        lightGoboDropdown.value = 0;
        lightGoboIndex = 0;               // No gobo selected
        
        gelColorDropdown.options.Clear();
        colorPreview.color = Color.white;

    }

    void Update() {
    }

    public void Setup() {
        
        //Debug.LogWarning("SETUP");

        lightPosX.GetComponent<Image>().color = inputFieldColor;
        lightPosY.GetComponent<Image>().color = inputFieldColor;
        lightPosZ.GetComponent<Image>().color = inputFieldColor;

        gelColorsList = gameManager.gelColorsList;
        gelColorDropdown.options.Clear();
        foreach (GelColorData gelColor in gelColorsList.gelColors) {
            gelColorDropdown.options.Add(new Dropdown.OptionData() { text = gelColor.gelColor + " " + gelColor.gelName });
        };

        // TODO: reflect filters here -- might need a new UI system, considering there can be multiple
        // Add/remove from list?
        //if (lightFixture.captureData.filters)

        lightTranform = lightFixture.lightTransform;  // Controls the pan/tilt

        label.text = string.Format("Selected Light: {0}", lightFixture.captureData.id);


        lightIntensity.value = Globals.AdjustLightReverse(lightFixture.spotlight.intensity);

        
        lightIntensityInput.text = lightIntensity.value.ToString();

        channelNumber.text = lightFixture.captureData.channel.ToString();

        //lightRange.value = lightFixture.spotlight.range;
        
        lightColorR.value = lightFixture.captureData.colorR; 
        lightColorG.value = lightFixture.captureData.colorG;  
        lightColorB.value = lightFixture.captureData.colorB;

        lightColorRInput.text = lightFixture.captureData.colorR.ToString();
        lightColorGInput.text = lightFixture.captureData.colorG.ToString();
        lightColorBInput.text = lightFixture.captureData.colorB.ToString();

        lightPosX.text = lightFixture.transform.position.x.ToString();
        lightPosY.text = lightFixture.transform.position.y.ToString();
        lightPosZ.text = lightFixture.transform.position.z.ToString();

        errorPosition.SetActive(false);

        colorPreview.color = new Color(lightColorR.value / 255.0f, lightColorG.value / 255.0f, lightColorB.value / 255.0f);

        lightPan.value = lightFixture.captureData.focusPan;
        lightTilt.value = lightFixture.captureData.focusTilt;

        lightPanInput.text = lightFixture.captureData.focusPan.ToString();
        lightTiltInput.text = lightFixture.captureData.focusTilt.ToString();
        //lightAngle.value = lightFixture.spotlight.spotAngle;

        // TODO: Replace "Cookie" with "Gobo" throughout
        lightGoboDropdown.options.Clear();
        gelColorDropdown.options.Add(new Dropdown.OptionData() { text = "" });
        int cookieIndex = 0;
        foreach (Texture cookie in gameManager.cookies) {
            if (lightFixture.captureData.gobos == cookie.name)
            {
                lightFixture.cookieIndex = cookieIndex;
            }
            lightGoboDropdown.options.Add(new Dropdown.OptionData() { text = cookie.name });
            cookieIndex++;
        }
        if (lightFixture.cookieIndex != 0) {
            lightGoboDropdown.value = lightFixture.cookieIndex;
            int TempInt = lightGoboDropdown.value;
            lightGoboDropdown.value = lightGoboDropdown.value + 1;
            lightGoboDropdown.value = TempInt;
        }

        lightColorR.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        lightColorG.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        lightColorB.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        lightIntensity.onValueChanged.AddListener(delegate { IntensityChangeCheck(); });
        lightIntensityInput.onEndEdit.AddListener(delegate { IntensityChangeTextCommit(); });

        lightPan.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        lightTilt.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        lightColorRInput.onValueChanged.AddListener(delegate { ValueChangeCheckText(); });
        lightColorGInput.onValueChanged.AddListener(delegate { ValueChangeCheckText(); });
        lightColorBInput.onValueChanged.AddListener(delegate { ValueChangeCheckText(); });

        lightPanInput.onValueChanged.AddListener(delegate { ValueChangeCheckText(); });
        lightTiltInput.onValueChanged.AddListener(delegate { ValueChangeCheckText(); });

        lightPosX.onValueChanged.AddListener(delegate { ValueChangeCheckText(); });
        lightPosY.onValueChanged.AddListener(delegate { ValueChangeCheckText(); });
        lightPosZ.onValueChanged.AddListener(delegate { ValueChangeCheckText(); });
        
        gelColorDropdown.onValueChanged.AddListener(delegate { GelColorDropdownChanged(); });
        lightGoboDropdown.onValueChanged.AddListener(delegate { GoboDropdownChanged(); });

        editChannel.onClick.AddListener(delegate { SelectChannel(lightFixture.captureData.channel); });

        showChannel.onClick.AddListener(delegate { HighlightChannel(lightFixture); });
        deleteLight.onClick.AddListener(delegate { DeleteLight(lightFixture); });

        setupComplete = true;
    }

    public void Deselect()
    {

        lightColorR.onValueChanged.RemoveAllListeners();
        lightColorG.onValueChanged.RemoveAllListeners();
        lightColorB.onValueChanged.RemoveAllListeners();

        lightIntensity.onValueChanged.RemoveAllListeners();

        lightColorRInput.onValueChanged.RemoveAllListeners();
        lightColorGInput.onValueChanged.RemoveAllListeners();
        lightColorBInput.onValueChanged.RemoveAllListeners();

        lightIntensityInput.onValueChanged.RemoveAllListeners();

        lightPan.onValueChanged.RemoveAllListeners();
        lightTilt.onValueChanged.RemoveAllListeners();

        lightPanInput.onValueChanged.RemoveAllListeners();
        lightTiltInput.onValueChanged.RemoveAllListeners();

        lightPosX.onValueChanged.RemoveAllListeners();
        lightPosY.onValueChanged.RemoveAllListeners();
        lightPosZ.onValueChanged.RemoveAllListeners();

        gelColorDropdown.onValueChanged.RemoveAllListeners();
        gelColorDropdown.ClearOptions();
        lightGoboDropdown.onValueChanged.RemoveAllListeners();
        lightGoboDropdown.ClearOptions();

        showChannel.onClick.RemoveAllListeners();
        deleteLight.onClick.RemoveAllListeners();
        
        gameManager.lightManager.UnhighlightAllLinked();

    }

    public void ValueChangeCheckText()
    {
        //lightIntensity.value = float.Parse(lightIntensityInput.text);

        //Debug.Log("ValueChangeCheckText: Colors: R:" + lightColorR.value + " / G:" + lightColorG.value + " / B:" + lightColorB.value);
        lightColorR.value = float.Parse(lightColorRInput.text);
        lightColorG.value = float.Parse(lightColorGInput.text);
        lightColorB.value = float.Parse(lightColorBInput.text);

        lightIntensity.value = float.Parse(lightIntensityInput.text);

        lightPan.value = float.Parse(lightPanInput.text);
        lightTilt.value = float.Parse(lightTiltInput.text);

        float[] parsedPos = new float[3];
        if (float.TryParse(lightPosX.text, out parsedPos[0]) && float.TryParse(lightPosY.text, out parsedPos[1]) && float.TryParse(lightPosZ.text, out parsedPos[2]))
        {
            lightFixture.transform.position = new Vector3(parsedPos[0], parsedPos[1], parsedPos[2]);

            // Update the spotlight's range, to make sure it's at least as powerful as its new distance from the origin
            lightFixture.spotlight.range = Vector3.Distance(lightFixture.transform.position, new Vector3(13f, 0, 13f));  // Adjust for center stage
            if (lightFixture.spotlight.range < 40f)     // Clamp minimum of 30f;
            {
                lightFixture.spotlight.range = 40f;
            }

            if (errorPosition.activeInHierarchy)
            {
                errorPosition.SetActive(false);
                lightPosX.GetComponent<Image>().color = inputFieldColor;
                lightPosY.GetComponent<Image>().color = inputFieldColor;
                lightPosZ.GetComponent<Image>().color = inputFieldColor;
            }

        } else {

            errorPosition.SetActive(true);
            lightPosX.GetComponent<Image>().color = inputErrorColor;
            lightPosY.GetComponent<Image>().color = inputErrorColor;
            lightPosZ.GetComponent<Image>().color = inputErrorColor;

        }

        lightFixture.captureData.posX = -lightFixture.transform.position.x;
        lightFixture.captureData.posY = lightFixture.transform.position.y;
        lightFixture.captureData.posZ = lightFixture.transform.position.z;

        lightFixture.SetEmissionColor(lightFixture.spotlight.intensity, colorPreview.color);
        
        //colorPreview.color = new Color(lightColorR.value / 255.0f, lightColorG.value / 255.0f, lightColorB.value / 255.0f);
        //lightFixture.spotlight.color = colorPreview.color;
        //lightFixture.GetComponentInChildren<VLB.BeamGeometry>().gameObject.GetComponent<Renderer>().material.SetColor("_Color", colorPreview.color);

    }

    public void GelColorDropdownChanged()
    { 
        
        int index = gelColorDropdown.value;
        List<GelColorData> gels = gelColorsList.gelColors;

        colorPreview.color = new Color(gels[index].colorR / 255f, gels[index].colorG / 255f, gels[index].colorB / 255f);
        lightColorR.value = gels[index].colorR;
        lightColorG.value = gels[index].colorG;
        lightColorB.value = gels[index].colorB;

    }
    
    public void GoboDropdownChanged()
    {
        lightFixture.spotlight.cookie = Resources.Load("Gobos/" + lightGoboDropdown.options[lightGoboDropdown.value].text) as Texture;
    }

    public void IntensityChangeTextCommit()
    {
        lightIntensity.value = float.Parse(lightIntensityInput.text);
        IntensityChangeCommit();

    }
    public void IntensityChangeCommit()
    {
        CuesManager cuesManager = gameManager.cuesManager;

        if (cuesManager.cuesListData != null) // In case we have not loaded any cues
        {
            bool foundCue = false;
            CueData cuesList = cuesManager.cuesListData.cues[cuesManager.currentCueIndex];
            foreach (ChannelCueData channelCue in cuesList.channelCues)
            {
                if (channelCue.channelID == lightFixture.captureData.channel)
                {
                    channelCue.cueParameters[0].level = lightIntensity.value;
                    foundCue = true;
                }
                if (foundCue) break;
            }
            if (!foundCue)
            {
                AddChannelCue(lightFixture.captureData.channel, lightIntensity.value, cuesList);
            }
        }
        // Save this value to cuesMasterList
        cuesManager.cuesMasterList[cuesManager.cuesListData.cues[cuesManager.currentCueIndex].cueId][lightFixture.captureData.channel] = lightIntensity.value;

        // If the option to affect all later cues is toggled on, save this value for future cues as well
        if (cuesManager.affectLaterCues.isOn)
        {
            int i = 0;
            foreach (CueData cue in cuesManager.cuesListData.cues)
            {
                if (i >= cuesManager.currentCueIndex)
                {
                    cuesManager.cuesMasterList[cuesManager.cuesListData.cues[i].cueId][lightFixture.captureData.channel] = lightIntensity.value;
                }
                i++;
            }
        }
        //Debug.LogWarning("CHANNEL CUE SAVED.");

    }

    public void IntensityChangeCheck() {
        
        lightIntensityInput.text = lightIntensity.value.ToString();
        lightFixture.spotlight.intensity = Globals.AdjustLight(lightIntensity.value);

        colorPreview.color = new Color(lightColorR.value / 255.0f, lightColorG.value / 255.0f, lightColorB.value / 255.0f);
        lightFixture.spotlight.color = colorPreview.color;
        lightFixture.SetEmissionColor(lightFixture.spotlight.intensity, colorPreview.color);
        lightFixture.GetComponentInChildren<VLB.BeamGeometry>().gameObject.GetComponent<Renderer>().material.SetColor("_Color", colorPreview.color);

        foreach (int id in gameManager.captureImport.channels[lightFixture.captureData.channel].lightIds)
        {
            LightFixture channelFixture = gameManager.lightManager.lightIds[id].GetComponent<LightFixture>();
            channelFixture.spotlight.intensity = Globals.AdjustLight(lightIntensity.value);
            channelFixture.SetEmissionColor(Globals.AdjustLight(lightIntensity.value), gameManager.lightManager.lightIds[id].GetComponent<LightFixture>().spotlight.color);
            if (lightFixture.spotlight.intensity > 0.05f)
            {
                if (Globals.ShowingHaze)
                {
                    VLB.VolumetricLightBeam beamComponent = gameManager.lightManager.lightIds[id].GetComponentInChildren<VLB.VolumetricLightBeam>();
                    beamComponent.enabled = true;
                    Material beamRenderer = channelFixture.GetComponentInChildren<VLB.BeamGeometry>().gameObject.GetComponent<Renderer>().material;
                    beamRenderer.SetFloat("_AlphaInside", Globals.AdjustBeamValue(Globals.LightBeamInside, lightIntensity.value));
                    beamRenderer.SetFloat("_AlphaOutside", Globals.AdjustBeamValue(Globals.LightBeamOutside, lightIntensity.value));
                }
            }
            else
            {
                gameManager.lightManager.lightIds[id].GetComponentInChildren<VLB.VolumetricLightBeam>().enabled = false;
            }
        }

    }

    public void ValueChangeCheck() {
        
        lightColorRInput.text = lightColorR.value.ToString();
        lightColorGInput.text = lightColorG.value.ToString();
        lightColorBInput.text = lightColorB.value.ToString();
        
        colorPreview.color = new Color(lightColorR.value / 255.0f, lightColorG.value / 255.0f, lightColorB.value / 255.0f);
        lightFixture.spotlight.color = colorPreview.color;
        lightFixture.GetComponentInChildren<VLB.BeamGeometry>().gameObject.GetComponent<Renderer>().material.SetColor("_Color", colorPreview.color);
        
        lightFixture.transform.rotation = Quaternion.Euler(lightTilt.value, lightPan.value, 0);    // Pan rotates along the local Z
        
        lightPanInput.text = lightPan.value.ToString("F0");
        lightTiltInput.text = lightTilt.value.ToString("F0");

        /*
        //lightFixture.spotlight.range = lightRange.value;
        //lightFixture.spotlight.spotAngle = lightAngle.value;

        //if (lightGoboDropdown.value == 0) {
        //    lightFixture.spotlight.cookie = null;
        //} else {
        //    lightFixture.spotlight.cookie = gameManager.cookies[lightGoboDropdown.value - 1];
        //    lightFixture.cookieIndex = lightGoboDropdown.value;
        //}
        
        */

        lightFixture.SetEmissionColor(lightFixture.spotlight.intensity, colorPreview.color);

        lightFixture.captureData.colorR = Mathf.Round(lightColorR.value);
        lightFixture.captureData.colorG = Mathf.Round(lightColorG.value);
        lightFixture.captureData.colorB = Mathf.Round(lightColorB.value);

        lightFixture.captureData.focusPan = lightPan.value;
        lightFixture.captureData.focusTilt = lightTilt.value;

        lightFixture.captureData.posX = -lightFixture.transform.position.x;
        lightFixture.captureData.posY = lightFixture.transform.position.y;
        lightFixture.captureData.posZ = lightFixture.transform.position.z;

    }

    void AddChannelCue(int lightFixtureChannelId, float lightIntensityValue, CueData cuesList)
    {
        ChannelCueData newChannelCue = new ChannelCueData
        {
            channelID = lightFixtureChannelId
        };
        CueParameterData newCueParameter = new CueParameterData
        {
            parameterType = 1,
            parameterTypeAsText = "Intens",
            level = lightIntensityValue
        };
        newChannelCue.cueParameters = new List<CueParameterData>();
        newChannelCue.cueParameters.Add(newCueParameter);
        cuesList.channelCues.Add(newChannelCue);
    }

    void SelectChannel(int channelId)
    {
        gameManager.channelsManager.OpenChannelsMenu();
        gameManager.channelsManager.LoadLightsMenu(channelId);
        gameManager.channelsManager.HighlightAllChannelLights(channelId);

        // Wait one frame, to let the window redraw before scrolling
        StartCoroutine(ScrollChannelsWindow(channelId));

    }

    public IEnumerator ScrollChannelsWindow(int channelId)
    {
        yield return new WaitForEndOfFrame();   // One frame delay for UI redraw
        float scrollPosition;
        scrollPosition = gameManager.captureImport.uiChannelsList.transform.Find("Channel " + channelId).GetComponent<RectTransform>().anchoredPosition.y;
        gameManager.captureImport.uiChannelsList.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -(scrollPosition + 10f));
        yield break;
    }

    void HighlightChannel(LightFixture lightFixture)
    {
        gameManager.lightManager.HighlightChannel(lightFixture.captureData.channel);
    }

    void GelDropdownChange() {
        GelColorData selectedGelColor = gelColorsList.gelColors[gelColorDropdown.value];
        lightColorR.value = selectedGelColor.colorR / 255;
        lightColorG.value = selectedGelColor.colorG / 255;
        lightColorB.value = selectedGelColor.colorB / 255;
        ValueChangeCheck();
    }
    
    public void DeleteLight(LightFixture lightFixture) {
        //gameManager.DeleteLight(lightFixture);
        gameManager.DeleteLight(lightFixture);
    }

}
