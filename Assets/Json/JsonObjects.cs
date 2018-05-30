using UnityEngine;
using System.Collections;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;

[JsonObject("Scene")]
public class SceneData {
    [JsonProperty("Stage")]
    public StageData stage { get; set; }
    [JsonProperty("House")]
    public HouseData house { get; set; }
//    [JsonProperty("Cues")]
//    public List<CueData> cues { get; set; }
    [JsonProperty("active")]
    public bool active { get; set; }
}
[JsonObject("Stage")]
public class StageData {
    [JsonProperty("width")]
    public float width { get; set; }
    [JsonProperty("depth")]
    public float depth { get; set; }
    [JsonProperty("height")]
    public float height { get; set; }
    [JsonProperty("rake")]
    public float rake { get; set; }

}
[JsonObject("House")]
public class HouseData {
    [JsonProperty("width")]
    public float width { get; set; }
    [JsonProperty("depth")]
    public float depth { get; set; }
    [JsonProperty("height")]
    public float height { get; set; }
    [JsonProperty("position")]
    public Vector3 position { get; set; }
}

//[JsonObject("Cues")]
//public class CueData {
//    [JsonProperty("pipes")]
//    public List<PipeData> pipes { get; set; }
//}

//[JsonObject("Pipe")]
//public class PipeData {
//    [JsonProperty("width")]
//    public float width { get; set; }
//    [JsonProperty("position")]
//    public Vector3 position { get; set; }
//    [JsonProperty("lights")]
//    public List<LightData> lights { get; set; }
//    [JsonProperty("center")]
//    public bool center { get; set; }
//    [JsonProperty("vertical")]
//    public bool vertical { get; set; }
//}

[JsonObject("Light")]
public class LightData {
    [JsonProperty("active")]
    public bool active { get; set; }
    [JsonProperty("position")]
    public float position { get; set; }
    [JsonProperty("intensity")]
    public float intensity { get; set; }
    [JsonProperty("gobo")]
    public string gobo { get; set; }
    [JsonProperty("rotation")]
    public Vector3 rotation { get; set; }
    //    [JsonProperty("color")]
    //	public Color color { get; set; }
    [JsonProperty("colorR")]
    public float colorR { get; set; }
    [JsonProperty("colorG")]
    public float colorG { get; set; }
    [JsonProperty("colorB")]
    public float colorB { get; set; }
    [JsonProperty("colorA")]
    public float colorA { get; set; }
}

[JsonObject("GelColorsList")]
public class GelColorListData {
    [JsonProperty("GelColors")]
    public List<GelColorData> gelColors { get; set; }
}

[JsonObject("GelColor")]
public class GelColorData {
    [JsonProperty("GelColor")]
    public string gelColor { get; set; }
    [JsonProperty("GelName")]
    public string gelName { get; set; }
    [JsonProperty("R")]
    public float colorR { get; set; }
    [JsonProperty("G")]
    public float colorG { get; set; }
    [JsonProperty("B")]
    public float colorB { get; set; }
}

//
//using UnityEngine;
//using System.Collections;
//using Newtonsoft.Json.Serialization;
//using Newtonsoft.Json;
//using System.Collections.Generic;
//
//[JsonObject("Scene")]
//public class SceneData {
//    [JsonProperty("Stage")]
//    public StageData stage { get; set; }
//    [JsonProperty("House")]
//    public HouseData house { get; set; }
//    [JsonProperty("rails")]
//    public RailData[] rails { get; set; }
//    [JsonProperty("active")]
//    public bool active { get; set; }
//}
//[JsonObject("Stage")]
//public class StageData {
//    [JsonProperty("width")]
//    public float width { get; set; }
//    [JsonProperty("depth")]
//    public float depth { get; set; }
//    [JsonProperty("height")]
//    public float height { get; set; }
//
//}
//[JsonObject("House")]
//public class HouseData {
//    [JsonProperty("width")]
//    public float width { get; set; }
//    [JsonProperty("depth")]
//    public float depth { get; set; }
//    [JsonProperty("height")]
//    public float height { get; set; }
//    [JsonProperty("position")]
//    public Vector3 position { get; set; }
//}
//[JsonObject("Rail")]
//public class RailData {
//    [JsonProperty("width")]
//    public float width { get; set; }
//    [JsonProperty("position")]
//    public Vector3 position { get; set; }
//    [JsonProperty("lights")]
//    public LightData[] lights { get; set; }
//}
//[JsonObject("Light")]
//public class LightData {
//    [JsonProperty("active")]
//    public bool active { get; set; }
//    [JsonProperty("position")]
//    public float position { get; set; }
//    [JsonProperty("intensity")]
//    public float intensity { get; set; }
//    [JsonProperty("gobo")]
//    public string gobo { get; set; }
//    [JsonProperty("rotation")]
//    public Vector3 rotation { get; set; }
////    [JsonProperty("color")]
////	public Color color { get; set; }
//    [JsonProperty("colorR")]
//	public float colorR { get; set; }
//    [JsonProperty("colorG")]
//	public float colorG { get; set; }
//    [JsonProperty("colorB")]
//	public float colorB { get; set; }
//    [JsonProperty("colorA")]
//    public float colorA { get; set; }
//}