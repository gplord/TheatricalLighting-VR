using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class CuesImport : MonoBehaviour {

    public CuesListData cuesListData;
    //string defaultCuesData = "SundayFinal.json";   // Json file containing raw JSON conversion of CSV output from spreadsheet
    //string defaultCuesData = "SundaySimplified.json";


    public string defaultCuesExportData = "Cues/TestCuesExport.json";
    string defaultCuesData = "Cues/TestCuesExport.json";

    public GameManager gameManager;

    private void Awake()
    {
    }

    private void Start()
    {
        //ImportData(defaultCuesData);
    }

    public void ImportData()
    {
        ImportData(defaultCuesExportData);
    }

    public void ImportData(string filename)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, filename);
        string cuesDataImport = File.ReadAllText(filePath);
        cuesListData = JsonConvert.DeserializeObject<CuesListData>(cuesDataImport);

        gameManager.cuesManager.cuesMasterList = new Dictionary<float, SortedDictionary<int, float>>();
        CreateMasterList();

    }

    void CreateMasterList() {

        Dictionary<float, SortedDictionary<int, float>> cuesMasterList = gameManager.cuesManager.cuesMasterList;

        cuesMasterList.Add(cuesListData.cues[0].cueId, new SortedDictionary<int, float>());
        SortedDictionary<int, float> firstCue = cuesMasterList[cuesListData.cues[0].cueId];

        foreach (KeyValuePair<int, Channel> entry in gameManager.captureImport.channels)
        {
            firstCue.Add(entry.Key, 0);
        }

        foreach (ChannelCueData channelCueData in cuesListData.cues[0].channelCues)
        {
            firstCue[channelCueData.channelID] = channelCueData.cueParameters[0].level;
        }
        int i = 0;
        foreach (CueData cueData in cuesListData.cues)
        {
            if (i > 0)
            {
                cuesMasterList.Add(cuesListData.cues[i].cueId, new SortedDictionary<int, float>());
                SortedDictionary<int, float> newCueDictionary = cuesMasterList[cuesListData.cues[i].cueId];
                foreach (KeyValuePair<int, float> entry in cuesMasterList[cuesListData.cues[i - 1].cueId])
                {
                    newCueDictionary.Add(entry.Key, entry.Value);
                    
                }

            }
            foreach (ChannelCueData channelCueData in cueData.channelCues)
            {
                cuesMasterList[cueData.cueId][channelCueData.channelID] = channelCueData.cueParameters[0].level;
            }
            i++;
        }
    }

    public void PrintMasterList()
    {
        Dictionary<float, SortedDictionary<int, float>> cuesMasterList = gameManager.cuesManager.cuesMasterList;
        foreach (KeyValuePair<float,SortedDictionary<int,float>> entry in cuesMasterList)
        {
            Debug.LogWarning("Cue " + entry.Key + ": --------------------");
            foreach (KeyValuePair<int,float> subentry in entry.Value)
            {
                Debug.Log("Cue " + entry.Key + "/Channel " + subentry.Key + ": " + subentry.Value);
            }
            Debug.Log("---------------------------------");
        }

    }

    public void SerializeCuesData(string filename) {

        string newCuesJson = JsonConvert.SerializeObject(cuesListData);
        if (!filename.EndsWith(".json")) filename = filename + ".json";
        string filepath = Path.Combine(Application.streamingAssetsPath, Globals.CuesPath + filename);
        File.WriteAllText(filepath, newCuesJson);

        Debug.Log(filepath + " : " + filename);

    }

    private void Update() { }

}
