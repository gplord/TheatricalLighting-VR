using UnityEngine;
using System.Collections;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;


[JsonObject("CaptureObjects")]
public class CaptureExportData {
    [JsonProperty("ImportLights")]
    public List<ImportLight> importLights { get; set; }
}

[JsonObject("ImportLights")]
public class ImportLight {

    public int id;

    [JsonProperty("Fixture")]
    public string fixture { get; set; }
    [JsonProperty("Optics")]
    public string optics { get; set; }
    [JsonProperty("Wattage")]
    public string wattage { get; set; }
    [JsonProperty("Unit")]
    public string unit { get; set; }
    [JsonProperty("Circuit")]
    public string circuit { get; set; }
    [JsonProperty("Channel")]
    public int channel { get; set; }
    [JsonProperty("Patch")]
    public string patch { get; set; }
    [JsonProperty("DMX Mode")]
    public string dmxmode { get; set; }
    [JsonProperty("DMX Channels")]
    public string dmxchannels { get; set; }
    [JsonProperty("Layer")]
    public string layer { get; set; }
    [JsonProperty("Focus")]
    public string focus { get; set; }
    [JsonProperty("Filters")]
    public string filters { get; set; }
    [JsonProperty("Gobos")]
    public string gobos { get; set; }       
    [JsonProperty("Accessories")]
    public string accessories { get; set; }
    [JsonProperty("Note")]
    public string note { get; set; }
    [JsonProperty("Weight")]
    public string weight { get; set; }
    [JsonProperty("Position X")]
    public float posX { get; set; }
    [JsonProperty("Position Y")]
    public float posY { get; set; }
    [JsonProperty("Position Z")]
    public float posZ { get; set; }
    [JsonProperty("Focus Pan")]
    public float focusPan { get; set; }
    [JsonProperty("Focus Tilt")]
    public float focusTilt { get; set; }

    public float colorR;
    public float colorG;
    public float colorB;

}
