using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChannelsManager : MonoBehaviour {

    public GameObject channelsMenuOpen;
    public GameObject channelsMenuClosed;

    public GameObject uiButtonLight;
    public Transform uiLightsList;

    public Text selectedLightLabel;
    public Dropdown selectedLightChannelDropdown;

    public CaptureImport captureImport;

    public GameManager gameManager;

	void Start () {
        CloseChannelsMenu();
        ChannelDeselectLight();
	}

	void Update () {
    }

    public void OpenChannelsMenu() {
        channelsMenuClosed.SetActive(false);
        channelsMenuOpen.SetActive(true);
    }

    public void CloseChannelsMenu()
    {
        channelsMenuOpen.SetActive(false);
        channelsMenuClosed.SetActive(true);
    }

    public void HighlightAllChannelLights(int channelId)
    {
        gameManager.lightManager.UnhighlightAllLinked();
        //gameManager.DeselectAll();
        foreach (int id in gameManager.captureImport.channels[channelId].lightIds)
        {
            gameManager.lightManager.HighlightLinked(id);
        }
    }

    public void LoadLightsMenu(int channelIndex)
    {

        foreach(Transform child in uiLightsList)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (int id in captureImport.channels[channelIndex].lightIds)
        {
            GameObject newButton = (GameObject) Instantiate(uiButtonLight, uiLightsList);
            UIButtonLight uiButtonLightComponent = newButton.GetComponent<UIButtonLight>();
            uiButtonLightComponent.label.text = string.Format("Light {0}", id.ToString());
            uiButtonLightComponent.lightId = id;
            uiButtonLightComponent.gameManager = gameManager;
        }
    }

    public void ChannelSelectLight(LightFixture lightFixture)
    {
        selectedLightLabel.text = lightFixture.captureData.id.ToString();
        selectedLightChannelDropdown.gameObject.SetActive(true);
        selectedLightChannelDropdown.ClearOptions();

        selectedLightChannelDropdown.onValueChanged.RemoveAllListeners();
        
        int dropdownIndex = 0;
        int i = 0;
        foreach (KeyValuePair<int, Channel> entry in gameManager.captureImport.channels)
        {
            if (entry.Key == lightFixture.captureData.channel) {
                dropdownIndex = i;
            }
            selectedLightChannelDropdown.options.Add(new Dropdown.OptionData() { text = string.Format("Channel {0}", entry.Key) });
            i++;
        }
        selectedLightChannelDropdown.value = dropdownIndex;

        int TempInt = selectedLightChannelDropdown.value;
        selectedLightChannelDropdown.value = selectedLightChannelDropdown.value + 1;
        selectedLightChannelDropdown.value = TempInt;

        selectedLightChannelDropdown.onValueChanged.AddListener( delegate { MoveSelectedLightToChannel(); });
    }

    public void MoveSelectedLightToChannel()
    {

        selectedLightChannelDropdown.onValueChanged.RemoveAllListeners();

        int dropdownIndex = selectedLightChannelDropdown.value;
        string[] label = selectedLightChannelDropdown.options[dropdownIndex].text.Split(null);
        int channelId = int.Parse(label[1]);

        if (gameManager.selectedObject != null)
        {
            LightFixture selectedLight = gameManager.selectedObject.GetComponent<LightFixture>();
            gameManager.captureImport.channels[selectedLight.captureData.channel].lightIds.Remove(selectedLight.captureData.id);
            selectedLight.captureData.channel = channelId;
            gameManager.lightMenuOpen.GetComponent<PanelSelectedLight>().channelNumber.text = selectedLight.captureData.channel.ToString();
            gameManager.captureImport.channels[channelId].lightIds.Add(selectedLight.captureData.id);
            //ChannelSelectLight(selectedLight);
        }
    }

    public void ChannelDeselectLight()
    {
        selectedLightLabel.text = "None";
        selectedLightChannelDropdown.gameObject.SetActive(false);
    }

    public void UnsetAllChannelsData()
    {
        foreach (Transform child in uiLightsList)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

}
