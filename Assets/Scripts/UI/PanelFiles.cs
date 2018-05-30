using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PanelFiles : MonoBehaviour {

    public FileManager fileManager;

    public Text label;

    public GameObject errorMessage;
    public Text errorText;

    public InputField filenameInput;

    public Dropdown fileTypeDropdown;

    public Transform fileScrollList;

    public Button buttonCancel;
    public Button buttonLoadFixtures;
    public Button buttonSaveFixtures;
    public Button buttonLoadCues;
    public Button buttonSaveCues;

    public GameObject fileButtonPrefab;

    // Possible states for this window, used when configuring at load
    public enum FileMenuState
    {
        FixturesSave,
        FixturesLoad,
        CuesSave,
        CuesLoad
    }

    void Start() {
        //LoadFilesList(FileMenuState.FixturesLoad);
    }

    void LoadFilesList(FileMenuState state)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(Application.streamingAssetsPath);

        switch (state) {
            case FileMenuState.FixturesLoad:
            case FileMenuState.FixturesSave:
                dirInfo = new DirectoryInfo(Application.streamingAssetsPath + "/" + Globals.FixturesPath);
                break;
            case FileMenuState.CuesLoad:
            case FileMenuState.CuesSave:
                dirInfo = new DirectoryInfo(Application.streamingAssetsPath + "/" + Globals.CuesPath);
                break;
            default:
                break;
        }

        foreach (Transform child in fileScrollList)
        {
            Destroy(child.gameObject);
        }

        FileInfo[] dirFiles = dirInfo.GetFiles();
        int fileId = 0;
        foreach (FileInfo file in dirFiles)
        {
            if (fileManager.eligibleFileTypes.Contains(file.Extension))
            {
                GameObject fileButton = Instantiate(fileButtonPrefab, fileScrollList);
                UIButtonFile uiButtonFile = fileButton.GetComponent<UIButtonFile>();
                uiButtonFile.filename = file.Name;
                uiButtonFile.label.text = file.Name;
                uiButtonFile.fileId = fileId;
                uiButtonFile.fileManager = fileManager;
                
                fileId++;   // Currently unused, but available in case unique ids are required for files
            }
        }
    }
    
    public void GoToPanelState(FileMenuState state)
    {
        LoadFilesList(state);
        switch (state) {
            case FileMenuState.FixturesSave:
                label.text = "Save Fixtures File";
                buttonSaveFixtures.gameObject.SetActive(true);
                buttonLoadFixtures.gameObject.SetActive(false);
                buttonLoadCues.gameObject.SetActive(false);
                buttonSaveCues.gameObject.SetActive(false);
                fileTypeDropdown.gameObject.SetActive(true);
                break;
            case FileMenuState.FixturesLoad:
                label.text = "Load Fixtures File";
                buttonSaveFixtures.gameObject.SetActive(false);
                buttonLoadFixtures.gameObject.SetActive(true);
                buttonLoadCues.gameObject.SetActive(false);
                buttonSaveCues.gameObject.SetActive(false);
                fileTypeDropdown.gameObject.SetActive(false);
                break;
            case FileMenuState.CuesSave:
                label.text = "Save Cues File";
                buttonSaveFixtures.gameObject.SetActive(false);
                buttonLoadFixtures.gameObject.SetActive(false);
                buttonSaveCues.gameObject.SetActive(true);
                buttonLoadCues.gameObject.SetActive(false);
                fileTypeDropdown.gameObject.SetActive(true);
                break;
            case FileMenuState.CuesLoad:
                label.text = "Load Cues File";
                buttonSaveFixtures.gameObject.SetActive(false);
                buttonLoadFixtures.gameObject.SetActive(false);
                buttonSaveCues.gameObject.SetActive(false);
                buttonLoadCues.gameObject.SetActive(true);
                fileTypeDropdown.gameObject.SetActive(false);
                break;
            default:
                Debug.LogError("Error loading File menu panel state.");
                break;
        }
        fileManager.gameManager.StartCoroutine(fileManager.gameManager.ScrollFix(fileScrollList.parent.GetComponent<ScrollRect>()));    // Fix flickering scrollbar on dynamic content load
    }

    public void ShowErrorMessage(string message)
    {
        errorMessage.SetActive(true);
        errorText.text = message;
    }
    public void HideErrorMessage()
    {
        errorMessage.SetActive(false);
    }

    public void FilenameInputChanged()
    {
        fileManager.SelectFile(null);
        HideErrorMessage();
    }

}
