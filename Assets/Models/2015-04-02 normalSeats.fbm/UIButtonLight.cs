using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonLight : MonoBehaviour {
    
    public Text label;
    public int lightId;
    //public CaptureData captureData;

    public GameManager gameManager;

    Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(LightOnClick);
    }

    void LightOnClick()
    {
        LightFixture lightFixture = gameManager.lightManager.lightIds[lightId].GetComponent<LightFixture>();
        gameManager.SelectLight(lightFixture);
        gameManager.lightManager.HighlightChannel(lightFixture.captureData.channel);
    }

}
