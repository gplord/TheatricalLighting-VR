using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor;
using System.IO;

public class TestLoadStream : MonoBehaviour {
    
    private string gameDataProjectFilePath = "/StreamingAssets/testdata.txt";

    public Text textField;

    // Use this for initialization
    void Start () {
        LoadStreamingData();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    private void LoadStreamingData ()
    {
        string filePath = Application.dataPath + gameDataProjectFilePath;

        if (File.Exists (filePath))
        {
            string dataAsString = File.ReadAllText(filePath);
            Debug.LogWarning(dataAsString);
            textField.text = dataAsString;
        } else
        {
            Debug.LogError("Not working");
        }
    }

}
