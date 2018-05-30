using UnityEngine;
using System.Collections;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;

[JsonObject("CuesList")]
public class CuesListData
{
    [JsonProperty("Cues")]
    public List<CueData> cues { get; set; }
}

[JsonObject("Cues")]
public class CueData
{
    [JsonProperty("CueID")]
    public float cueId;

    [JsonProperty("ChannelCues")]
    public List<ChannelCueData> channelCues { get; set; }
}

[JsonObject("ChannelCue")]
public class ChannelCueData
{
    [JsonProperty("ChannelID")]
    public int channelID;

    [JsonProperty("CueParameters")]
    public List<CueParameterData> cueParameters { get; set; }
}

[JsonObject("CueParameters")]
public class CueParameterData
{
    [JsonProperty("ParameterType")]
    public int parameterType;

    [JsonProperty("ParameterTypeAsText")]
    public string parameterTypeAsText;

    [JsonProperty("Level")]
    public float level;
}

/*
[JsonObject("CuesList")]
public class CuesListData {
    [JsonProperty("Cues")]
    public List<NewCueData> cues { get; set; }
}

//[JsonObject("Cues")]
//public class CueData
//{
//    [JsonProperty("CueID")]
//    public float cueId;     // Float because naming conventions allows adding interstitial cues between existing integer cues

//    public float duration;

//}

[JsonObject("Cues")]
public class NewCueData {

    [JsonProperty("CueId")]
    public int cueId;

    [JsonProperty("CueChanges")]
    public List<CueChangeData> cueChanges { get; set; }

}

[JsonObject("CueChanges")]
public class CueChangeData {
    [JsonProperty("channelID")]
    public int channelID;

    [JsonProperty("Intensity")]
    public float intensity;
}
*/