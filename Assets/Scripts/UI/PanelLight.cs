using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelLight : MonoBehaviour {

    /*
    public GameManager gameManager;

    //public LightMount stageLight;
    public StageLight stageLight;

    private Transform lightTranform;

    public Text name;

    public Dropdown lightRail;
    public Dropdown gelColorDropdown;

    public Toggle lightEnabled;
    public Slider lightStrength;
    public Slider lightRange;

    public Slider lightColorR;
    public Slider lightColorG;
    public Slider lightColorB;

    public Slider lightPosition;
    public Slider lightPan;
    public Slider lightTilt;
    public Slider lightAngle;
    
    public Text lightPositionText;
    public Text lightPanText;
    public Text lightTiltText;

    public Text lightStrengthText;
    public Text lightColorRText;
    public Text lightColorGText;
    public Text lightColorBText;
    
    public Dropdown lightCookie;
    private int lightCookieIndex;   // Quick workaround, to store the previous dropdown index

    public Button deleteLight;

    GelColorListData gelColorsList;

    void Start() {

        gelColorsList = gameManager.gelColorsList;

        lightRail.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        gelColorDropdown.onValueChanged.AddListener(delegate { GelDropdownChange(); });

        lightEnabled.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        lightStrength.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        lightRange.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        lightColorR.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        lightColorG.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        lightColorB.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        lightPosition.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        lightPan.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        lightTilt.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        lightAngle.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        lightCookie.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        deleteLight.onClick.AddListener( delegate { DeleteLight(); });

        // Default starting values
        lightCookieIndex = 0;               // No cookies selected
                                            // TODO: No target selected

        // Fire a forced update, to enable these default values
        ValueChangeCheck();

    }

    void Update() {

    }

    public void Setup() {

        gelColorsList = gameManager.gelColorsList;
        foreach (GelColorData gelColor in gelColorsList.gelColors) {
            gelColorDropdown.options.Add(new Dropdown.OptionData() { text = gelColor.gelColor + " " + gelColor.gelName });
        };

        lightTranform = stageLight.lightTransform;  // Controls the pan/tilt

        name.text = stageLight.gameObject.name;
        lightStrength.value = stageLight.spotlight.intensity;
        lightRange.value = stageLight.spotlight.range;
        lightColorR.value = stageLight.spotlight.color.r;
        lightColorG.value = stageLight.spotlight.color.g;
        lightColorB.value = stageLight.spotlight.color.b;

        //Vector2 railBounds = stageLight.pipe.GetBounds();
        
        lightPosition.minValue = 0;

        
        //lightPosition.maxValue = stageLight.pipe.pipeData.width;
        //if (stageLight.pipe.pipeData.vertical) {
        //    lightPosition.value = stageLight.transform.localPosition.z;
        //} else {
        //    lightPosition.value = stageLight.transform.localPosition.x;
        //}
        

        lightPan.value = stageLight.lightData.rotation.z;
        lightTilt.value = stageLight.lightData.rotation.x;
        //lightTilt.value = stageLight.lightTransform.localRotation.eulerAngles.x;

        lightAngle.value = stageLight.spotlight.spotAngle;

        foreach (Texture cookie in gameManager.cookies) {
            lightCookie.options.Add(new Dropdown.OptionData() { text = cookie.name });
        }
        if (stageLight.cookieIndex != 0) {
            lightCookie.value = stageLight.cookieIndex;
            ValueChangeCheck();
        }

    }

    public void ValueChangeCheck() {

        stageLight.spotlight.enabled = lightEnabled.isOn;
        stageLight.spotlight.intensity = lightStrength.value;
        stageLight.spotlight.range = lightRange.value;
        stageLight.spotlight.color = new Color(lightColorR.value, lightColorG.value, lightColorB.value);
        stageLight.spotlight.spotAngle = lightAngle.value;

        lightStrengthText.text = (lightStrength.value / 8).ToString("F2");
        lightColorRText.text = (lightColorR.value * 255).ToString("F0");
        lightColorGText.text = (lightColorG.value * 255).ToString("F0");
        lightColorBText.text = (lightColorB.value * 255).ToString("F0");

        //stageLight.transform.localPosition = new Vector3(lightPosition.value * stageLight.pipe.transform.localScale.x, stageLight.transform.localPosition.y, stageLight.transform.localPosition.z);
        
        //if (stageLight.pipe.pipeData.vertical) {
        //    stageLight.transform.localPosition = new Vector3(stageLight.transform.localPosition.x, stageLight.transform.localPosition.y, lightPosition.value);
        //} else {
        //    stageLight.transform.localPosition = new Vector3(lightPosition.value, stageLight.transform.localPosition.y, stageLight.transform.localPosition.z);
        //}
        //lightPositionText.text = lightPosition.value.ToString("F1");
        
        
        stageLight.transform.localRotation = Quaternion.Euler(0, 0, lightPan.value);
        stageLight.lightTransform.localRotation = Quaternion.Euler(lightTilt.value, 0, 0);
        lightPanText.text = lightPan.value.ToString("F0");
        lightTiltText.text = lightTilt.value.ToString("F0");

        if (lightCookie.value == 0) {
            stageLight.spotlight.cookie = null;
        } else {
            stageLight.spotlight.cookie = gameManager.cookies[lightCookie.value - 1];
            stageLight.cookieIndex = lightCookie.value;
        }
        
        // Update sceneData object for serialization
        stageLight.lightData.active = lightEnabled.isOn;
        stageLight.lightData.position = lightPosition.value;
        //stageLight.lightData.position = lightPosition.value / stageLight.pipe.transform.localScale.x;
        
        stageLight.lightData.intensity = (float) System.Math.Round(lightStrength.value / 8, 2);
        stageLight.lightData.colorR = Mathf.Round(lightColorR.value * 255);
        stageLight.lightData.colorG = Mathf.Round(lightColorG.value * 255);
        stageLight.lightData.colorB = Mathf.Round(lightColorB.value * 255);

        stageLight.lightData.rotation = new Vector3(lightTilt.value, 0, lightPan.value);

    }

    void GelDropdownChange() {
        GelColorData selectedGelColor = gelColorsList.gelColors[gelColorDropdown.value];
        lightColorR.value = selectedGelColor.colorR / 255;
        lightColorG.value = selectedGelColor.colorG / 255;
        lightColorB.value = selectedGelColor.colorB / 255;
        ValueChangeCheck();
    }

    public void TargetSelectCheck() {

        gameManager.selectingTarget = true;

    }

    public void DeleteLight() {
    	gameManager.DeleteLight(stageLight);
    }
    */

}
