using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public SelectableObject selectedObject;
	public Canvas canvas;
    
	public GameObject lightPanel;
    public GameObject pipePanel;
    public GameObject housePanel;
    public GameObject stagePanel;

    public GameObject lightMenuClosed;
    public GameObject lightMenuOpen;
    public GameObject lightMenuNoLightSelected;

    public GameObject spaceMenuClosed;
    public GameObject spaceMenuOpen;

    public GameObject[] cameras;
	public Dropdown cameraSelector;

	public Texture[] cookies;
	public Pipe[] rails;

	public SceneData sceneData;

	public JsonImport jsonImport;
    public CaptureImport captureImport;

    public GelColorListData gelColorsList;
    public Dictionary<string, GelColorData> gelColorsDictionary;

    public LayerMask layerMask;

    public LightManager lightManager;
    public CuesManager cuesManager;
    public ChannelsManager channelsManager;
    public CameraManager cameraManager;
    public GelColorImport gelColorImport;

    private string workingFilename;

    public bool showHaze = true;
    public Toggle showHazeToggle;

    private void Awake()
    {
        gelColorsDictionary = new Dictionary<string, GelColorData>();
        layerMask = LayerMask.GetMask("Lights");
        //Debug.Log(LayerMask.NameToLayer("UI"));
    }

    // Use this for initialization
    void Start () {
		selectedObject = null;

		cameraSelector.onValueChanged.AddListener ( delegate { CameraChangeCheck (); } );
		cameraSelector.value = 2;
		CameraChangeCheck();

        CloseLightMenu();
        CloseSpaceMenu();

        LoadDefaultScene();
    }

    void LoadDefaultScene()
    {

        gelColorImport.ImportColors();
        captureImport.ImportData();
        cuesManager.ImportCues();

        Debug.Log("Scene loaded.");

    }

    void UnloadScene()
    {
        // Keep original GelColorImport data, as it will not have changed during runtime

        // Undo the original CuesManager import
        cuesManager.UnsetAllCues();
        
        //lightManager.UnsetAllLightData();     // TODO: Restructure Light UI materials under LightManager
        lightMenuOpen.GetComponent<PanelSelectedLight>().ResetLightPanel();
        lightManager.UnsetAllLightData();
        CloseLightMenu();
        channelsManager.UnsetAllChannelsData();
        
        // Undo the original CaptureImport data
        captureImport.UnsetAllCaptureData();

    }

    public void LoadNewScene(string filename)
    {

        UnloadScene();

        captureImport.ImportData(Globals.FixturesPath + filename);
        cuesManager.ImportCues();
        Debug.Log("Scene loaded.");

        workingFilename = filename;

    }

    public void SaveScene()
    {
        // Save based on workingFilename
    }

    public void SaveSceneAs(string filename)
    {

        captureImport.SerializeLights(filename);

        // Save based on new filename
        workingFilename = filename;
    }


    // TODO: Deprecated function
    //public void CaptureImport()
    //{
    //    // Sequence: Called from GelColorImport class, when the gels are processed
    //    // (This is because we need the gel colors before we can assign color values to the imported lights
    //    captureImport.ImportData();
    //    cuesManager.ImportCues();
    //}
    
    void Update () {

        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    cuesManager.ImportCues();
        //}
        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    cuesManager.UnsetAllCues();
        //}
        
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            cuesManager.cuesImport.SerializeCuesData(cuesManager.cuesImport.defaultCuesExportData);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            if (lightManager.increasedVisibility)
            {
                foreach (KeyValuePair<int, GameObject> entry in lightManager.lightIds)
                {
                    entry.Value.GetComponent<LightFixture>().IncreaseVisibility(false);
                }
                lightManager.increasedVisibility = false;
            } else {
                foreach (KeyValuePair<int, GameObject> entry in lightManager.lightIds)
                {
                    entry.Value.GetComponent<LightFixture>().IncreaseVisibility(true);
                }
                lightManager.increasedVisibility = true;
            }
        }
        
        if (Input.GetButtonDown("Fire1")) { // More flexible than explicitly getting MouseButton 0, for VR, etc.

            if (!EventSystem.current.IsPointerOverGameObject()) { 
                RaycastHit hit;
			    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(ray, out hit, 200f, layerMask))
                {

                    //Debug.Log("Clicked: " + hit.transform.parent.parent.parent.parent.name);
                    if (hit.transform.tag == "Light")
                    {
                        if (hit.transform.GetComponent<LightFixture>() != null)
                        {
                            LightFixture lightFixture = hit.transform.GetComponent<LightFixture>();
                            SelectLight(lightFixture);
                        }
                    }
                } else {
                    DeselectAll();
                }
			} else {

				if (EventSystem.current.IsPointerOverGameObject()) {

				} else {
                    
                    DeselectAll();

                }
			}
		}			
	}

    public void ShowHaze()
    {
        if (showHazeToggle.isOn)
        {
            showHaze = true;
            Globals.ShowingHaze = true;
        } else
        {
            showHaze = false;
            Globals.ShowingHaze = false;
        }
        cuesManager.ReloadCue();
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void OpenLightMenu()
    {
        lightMenuClosed.SetActive(false);
        if (selectedObject == null)
        {
            lightMenuNoLightSelected.SetActive(true);
            lightMenuOpen.SetActive(false);
        } else
        {
            lightMenuNoLightSelected.SetActive(false);
            lightMenuOpen.SetActive(true);
        }
    }
    public void CloseLightMenu()
    {
        lightMenuClosed.SetActive(true);
        lightMenuOpen.SetActive(false);
        lightMenuNoLightSelected.SetActive(false);
        lightMenuOpen.GetComponent<PanelSelectedLight>().Deselect();
    }

    public void OpenSpaceMenu()
    {
        spaceMenuClosed.SetActive(false);
        spaceMenuOpen.SetActive(true);
    }
    public void CloseSpaceMenu()
    {
        spaceMenuClosed.SetActive(true);
        spaceMenuOpen.SetActive(false);
    }

    public void SelectLight(LightFixture lightFixture)
    {
        DeselectAll();
        selectedObject = lightFixture;
        lightFixture.Highlight(true);
        channelsManager.ChannelSelectLight(lightFixture);
        CreateLightPanel(lightFixture);


    }

	public void DeselectAll() {

        //Debug.LogWarning("DESELECT ALL");
        lightMenuOpen.GetComponent<PanelSelectedLight>().Deselect();

		if (selectedObject != null) {
			selectedObject.Highlight(false);
            selectedObject = null;
		}
		if (lightMenuOpen.activeInHierarchy) {
            CloseLightMenu();
        }
        lightManager.UnhighlightAllLinked();

        channelsManager.ChannelDeselectLight();

    }

    public void CreateLightPanel(LightFixture lightFixture) { 

        OpenLightMenu();

		PanelSelectedLight panelLight = lightMenuOpen.GetComponent<PanelSelectedLight>();

        panelLight.lightFixture = lightFixture;
		panelLight.gameManager = this;
		panelLight.Setup();

	}

    public void OpenHousePanel() {
        housePanel.SetActive(true);
        housePanel.GetComponent<PanelHouseStage>().Setup();
    }
    public void OpenStagePanel() {
        stagePanel.SetActive(true);
        stagePanel.GetComponent<PanelStage>().Setup();
    }

    public void CameraChangeCheck () {
		for (int i = 0; i < cameras.Length; i++) {
			if (cameraSelector.value == i) {
				cameras[i].SetActive(true);
			} else {
				cameras[i].SetActive(false);
			}
		}
	}

    public IEnumerator ScrollFix(ScrollRect scrollRect)      // Waits one frame for UI to redraw, then returns scrollbar to AutoHide, solving flicker
    {
        scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.Permanent;
        yield return new WaitForSeconds(0.25f);
        scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
        yield break;
    }
    
    public void DeleteLight(LightFixture lightFixture)
    {
        if (captureImport.lightFixtures.Contains(lightFixture))
        {
            captureImport.lightFixtures.Remove(lightFixture);
        }
        captureImport.captureData.importLights.Remove(lightFixture.captureData.importLight);
        int channelId = lightFixture.captureData.channel;
        captureImport.channels[channelId].lightIds.Remove(lightFixture.captureData.id);

        int id = lightFixture.captureData.id;
        DeselectAll();
        lightManager.lightIds.Remove(id);
        Destroy(lightFixture.gameObject);        
    }

    public void SaveCue() { }
    public void DeleteCue() { }
    public void AddCue() { }
    public void GoToCue(int index) {
        jsonImport.GenerateCue(index);
    }

}
