using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonChannel : MonoBehaviour {

    public Text label;
    public int channelId;
    //public CaptureData captureData;

    Button channelButton;

    private void Start()
    {
        channelButton = GetComponent<Button>();
        channelButton.onClick.AddListener(ChannelOnClick);
    }

    void ChannelOnClick()
    {
        ChannelsManager channelsManager = GameObject.Find("ChannelsManager").GetComponent<ChannelsManager>();
        channelsManager.LoadLightsMenu(channelId);
        channelsManager.HighlightAllChannelLights(channelId);
    }


}
