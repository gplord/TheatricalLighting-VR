using UnityEngine;
using System.Collections;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;

[JsonObject("ParametersList")]
public class ParametersListData
{
    [JsonProperty("Parameter")]
    public List<ParameterData> parameters { get; set; }
}

[JsonObject("Parameter")]
public class ParameterData
{

    [JsonProperty("Parameter_Type")]
    public int parameterType;

    [JsonProperty("Parameter_Type_As_Text")]
    public string parameterTypeAsText;

    [JsonProperty("Level")]
    public float level;

}