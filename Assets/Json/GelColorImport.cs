using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class GelColorImport : MonoBehaviour {

    public TextAsset gelColorJson;
    public GelColorListData gelColorsList;
    public GameManager gameManager;

    public Dictionary<string, GelColorData> gelColorsDictionary;

    void Awake () {

        gelColorsDictionary = new Dictionary<string, GelColorData>();
        
    }
	
	void Update () {

	}

    public void ImportColors()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "GelColors.json");
        string gelColorDataRead = File.ReadAllText(filePath);
        gelColorsList = JsonConvert.DeserializeObject<GelColorListData>(gelColorDataRead);

        foreach (GelColorData gelColor in gelColorsList.gelColors)
        {
            gelColorsDictionary.Add(gelColor.gelColor, gelColor);
        }

        gameManager.gelColorsList = gelColorsList;
        gameManager.gelColorsDictionary = gelColorsDictionary;

        //Debug.Log("Imported " + gelColorsList.gelColors.Count + " Gel Colors from " + filePath);

        //gameManager.CaptureImport();
    }
}
