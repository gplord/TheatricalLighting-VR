using UnityEngine;
using System.Collections;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;

[JsonObject("TargetsList")]
public class TargetsListData
{
    [JsonProperty("Target")]
    public List<TargetData> targets { get; set; }
}

[JsonObject("Target")]
public class TargetData
{

    [JsonProperty("Target_Type")]
    public int targetType;

    [JsonProperty("Target_Type_As_Text")]
    public string targetTypeAsText;

}