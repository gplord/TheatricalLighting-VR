using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonFile : MonoBehaviour {

    public Text label;
    public string filename;
    public int fileId;
    
    public FileManager fileManager;
    Button button;
    
    void Awake () {
        button = GetComponent<Button>();
        button.onClick.AddListener(FileOnClick);
    }

    void FileOnClick() {
        
        fileManager.SelectFile(filename);

    }
}
