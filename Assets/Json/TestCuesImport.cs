using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class TestCuesImport : MonoBehaviour {
    
    public TestCuesData testCuesData;
    string defaultTestCuesData = "TestCues.json";   // Json file containing raw JSON conversion of CSV output from spreadsheet

    public GameManager gameManager;

    public void Start()
    {
        // Uncomment to process default json file 
        //ImportTestCuesData();
        //ImportData(defaultTestCuesData);
    }
    
    public void ImportData (string filename) {

        string filePath = Path.Combine(Application.streamingAssetsPath, filename);
        string testCuesDataImport = File.ReadAllText(filePath);
        testCuesData = JsonConvert.DeserializeObject<TestCuesData>(testCuesDataImport);

        int i = 1;

        float currentTargetId = 0f;

        string jsonWrite = string.Empty;

        bool firstLoopFinished = false;

        jsonWrite += "{\"Cues\": [";


        foreach (TestCue testCue in testCuesData.testCues) {

            if (testCue.targetId != currentTargetId)
            {
                if (firstLoopFinished)
                {
                    jsonWrite += "]},";
                }
                firstLoopFinished = true;

                currentTargetId = testCue.targetId;
                jsonWrite += "{\"CueID\": " + testCue.targetId + ",\"ChannelCues\": [";

            } else
            {
                jsonWrite += ",";
            }

            jsonWrite += "{\"ChannelID\": "+testCue.channel+",\"CueParameters\": [{\"ParameterType\": "+testCue.parameterType+",\"ParameterTypeAsText\": \""+testCue.parameterTypeAsText+"\",\"Level\": "+testCue.level+"}]}\n";

            if (testCue.targetId != currentTargetId)
            {
                currentTargetId = testCue.targetId;
            } else
            {

            }            
            i++;
        }
        jsonWrite += "]}]}";
        
        string filepath = Path.Combine(Application.streamingAssetsPath, "testoutput.json");
        File.WriteAllText(filepath, jsonWrite);

    }

}
