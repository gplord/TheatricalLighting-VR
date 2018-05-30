using UnityEngine;
using System.Collections;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;

[JsonObject("TestCuesList")]
public class TestCuesData
{
    [JsonProperty("TestCues")]
    public List<TestCue> testCues { get; set; }
}

[JsonObject("TestCues")]
public class TestCue
{

    [JsonProperty("TARGET_TYPE")]
    public int targetType { get; set; }

    [JsonProperty("TARGET_TYPE_AS_TEXT")]
    public string targetTypeAsText { get; set; }

    [JsonProperty("TARGET_LIST_NUMBER")]
    public int targetListNumber { get; set; }

    [JsonProperty("TARGET_ID")]
    public float targetId { get; set; }

    [JsonProperty("TARGET_PART_NUMBER")]
    public string targetPartNumber { get; set; }

    [JsonProperty("CHANNEL")]
    public int channel { get; set; }

    [JsonProperty("PARAMETER_TYPE")]
    public int parameterType { get; set; }

    [JsonProperty("PARAMETER_TYPE_AS_TEXT")]
    public string parameterTypeAsText { get; set; }

    [JsonProperty("LEVEL")]
    public float level { get; set; }

}
