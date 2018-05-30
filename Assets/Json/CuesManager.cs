using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class CuesManager : MonoBehaviour {

    public Text cueDisplay;
    public Text cueDisplay2;
    public int currentCueIndex = 0;

    public CaptureImport captureImport;
    public float maxIntensity = 5f;

    public GameObject cuesMenuClosed;
    public GameObject cuesMenuOpen;

    public GameObject uiButtonCue;
    public Transform uiCuesList;

    public Toggle affectLaterCues;

    public CuesImport cuesImport;

    public CuesListData cuesListData;
    public TextAsset cuesListJson;

    public GameManager gameManager;
    public float lightAdjustmentFactor = 3f;

    public Dictionary<float, SortedDictionary<int, float>> cuesMasterList = new Dictionary<float, SortedDictionary<int, float>>();

    void Awake()
    {
        cuesImport = GetComponent<CuesImport>();
    }

    void Start() {
        CloseCuesMenu();
    }

    void Update() {
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftCommand)) && Input.GetKeyDown(KeyCode.LeftArrow)) {
            GoToCue(PreviousCue());
        }
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftCommand)) && Input.GetKeyDown(KeyCode.RightArrow)) {
            GoToCue(NextCue());
        }
    }

    int NextCue() {
        return currentCueIndex + 1;
    }
    int PreviousCue() {
        if (currentCueIndex > 0) {
            return currentCueIndex - 1;
        } else {
            return currentCueIndex;
        }
    }
    int CurrentCue()
    {
        return currentCueIndex;
    }

    public void OpenCuesMenu()
    {
        cuesMenuClosed.SetActive(false);
        cuesMenuOpen.SetActive(true);
    }
    public void CloseCuesMenu()
    {
        cuesMenuOpen.SetActive(false);
        cuesMenuClosed.SetActive(true);
    }

    public void GoToNext() {
        if (cuesListData != null)
        {
            if (cuesListData.cues.Count - 1 > currentCueIndex)
                GoToCue(NextCue());
        }
    }
    public void GoToPrevious() {
        if (cuesListData != null)
        {
            GoToCue(PreviousCue());
        }
    }

    public float NormalizeLight(float lightFromCapture) {
        return (lightFromCapture / (100 / maxIntensity));
    }

    public float IntensityLevel(float baseLevel)
    {
        float adjustedLevel = baseLevel / lightAdjustmentFactor;
        return adjustedLevel;
    }

    public void ReloadCue()
    {
        GoToCue(CurrentCue());
    }

    public void GoToCue(int cueIndex) {

        currentCueIndex = cueIndex;
        cueDisplay.text = cueDisplay2.text = cuesListData.cues[currentCueIndex].cueId.ToString();

        // Reference the master list of cue values, which compiles previous values in a 2D dictionary, enabling moving backwards through previous cues
        foreach (KeyValuePair<int, float> entry in cuesMasterList[cuesListData.cues[currentCueIndex].cueId])
        {
            if (gameManager.captureImport.channels.ContainsKey(entry.Key))
            {
                foreach (int id in gameManager.captureImport.channels[entry.Key].lightIds)
                {
                    gameManager.lightManager.lightIds[id].GetComponent<LightFixture>().NewIntensity(entry.Value, 0.75f);
                    if (entry.Value > 0.05f)
                    {
                        gameManager.lightManager.lightIds[id].GetComponentInChildren<VLB.VolumetricLightBeam>().enabled = true;
                    }
                    else
                    {
                        //gameManager.lightManager.lightIds[id].GetComponentInChildren<VLB.VolumetricLightBeam>().enabled = false;
                    }
                }
            }
        }

    }

    public void ImportCues()
    {
        
        cuesImport.ImportData();
        cuesListData = cuesImport.cuesListData;

        LoadCuesMenu();
        GoToCue(0);

    }
    public void ImportCues(string filename)
    {
        cuesImport.ImportData(filename);
        cuesListData = cuesImport.cuesListData;

        LoadCuesMenu();
        GoToCue(0);

    }

    public void AddCue()
    {
        CueData newCueData = new CueData();
        newCueData.channelCues = new List<ChannelCueData>();
        newCueData.cueId = cuesListData.cues[cuesListData.cues.Count - 1].cueId + 1f;
        cuesListData.cues.Add(newCueData);

        // Add new cue to cuesMasterList, and copy values from previous cue
        cuesMasterList.Add(newCueData.cueId, new SortedDictionary<int, float>());
        foreach (KeyValuePair<int, float> entry in cuesMasterList[newCueData.cueId - 1])
        {
            cuesMasterList[newCueData.cueId][entry.Key] = entry.Value;
        }

        LoadCuesMenu();

    }

    public void InsertCue() { 

        CueData newCueData = new CueData();
        newCueData.channelCues = new List<ChannelCueData>();

        float currentCueId = cuesListData.cues[currentCueIndex].cueId;

        if (cuesListData.cues.Count > currentCueIndex + 1) {    // There is a next index
            
            float newCueId = 0f;
            float attempt = 0.5f;
            if ((currentCueId + attempt) < Mathf.Floor(cuesListData.cues[currentCueIndex + 1].cueId))
            {
                newCueId = (currentCueId + attempt);
            }
            else
            {
                attempt = 0.1f;
                if ((currentCueId + attempt) < cuesListData.cues[currentCueIndex + 1].cueId)
                {
                    newCueId = Mathf.Round((currentCueId + attempt) * 10f) / 10f;
                }
                else
                {
                    attempt = 0.01f;
                    if ((currentCueId + attempt) < cuesListData.cues[currentCueIndex + 1].cueId)
                    {
                        newCueId = Mathf.Round((currentCueId + attempt) * 100f) / 100f;
                    }
                    else
                    {
                        //Debug.Log("No cue name found: use 0 as placeholder.");
                    }
                }
            }

            // Insert the new cue
            newCueData.cueId = newCueId;
            cuesListData.cues.Insert(currentCueIndex+1, newCueData);

            // Insert new cue into cuesMasterList, and copy values from previous (current) cue
            cuesMasterList.Add(newCueId, new SortedDictionary<int, float>());
            foreach (KeyValuePair<int, float> entry in cuesMasterList[currentCueId])
            {
                cuesMasterList[newCueId][entry.Key] = entry.Value;
            }

            LoadCuesMenu();

        } else {

            AddCue();

        }

    }

    public void LoadCuesMenu() {

        foreach (Transform child in uiCuesList)
        {
            GameObject.Destroy(child.gameObject);
        }
        //foreach (NewCueData cueData in cuesListData.cues)
        int i = 0;
        foreach (CueData cueData in cuesListData.cues)
        {
            GameObject newButton = (GameObject)Instantiate(uiButtonCue, uiCuesList);

            UIButtonCue uiButtonCueComponent = newButton.GetComponent<UIButtonCue>();

            uiButtonCueComponent.label.text = string.Format("Cue {0}", cueData.cueId.ToString());
            uiButtonCueComponent.cueId = cueData.cueId;
            uiButtonCueComponent.cuesManager = this;
            uiButtonCueComponent.cueIndex = i;

            i++;

        }

        gameManager.StartCoroutine(gameManager.ScrollFix(uiCuesList.parent.GetComponent<ScrollRect>()));    // Fix flickering scrollbar on dynamic content load
    }

    public void UnsetAllCues()
    {
        foreach (Transform child in uiCuesList)
        {
            GameObject.Destroy(child.gameObject);
        }
        cuesListData = new CuesListData();
        cuesListData.cues = new List<CueData>();

        currentCueIndex = 0;
        cueDisplay.text = cueDisplay2.text = currentCueIndex.ToString();
    }

    public void LoadCuesFile(string filename)
    {
        UnsetAllCues();
        ImportCues(filename);
    }

}
