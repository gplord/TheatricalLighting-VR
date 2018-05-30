using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class JsonImport : MonoBehaviour {
    
    //public TextAsset sceneDataString;
    public SceneData sceneData;

    public GameObject stageModel;
    public GameObject houseModel;

    public GameObject pipePrefab;
    public GameObject pipePrefabVertical;
    public GameObject lightPrefab;

    public GameManager gameManager;

    public PanelCues panelCues;

    //List<GameObject> pipeList;

    int pipeCount = 1;
    int lightCount = 1;

    public int cueIndex = 0;
    public int maxCueIndex = 0;

    string pathResult = "";

    public string streamingFilename;
    public UnityEngine.UI.Text streamingTextTest;
    string streamResults;

    void Awake() {

        //pipeList = new List<GameObject>();

        sceneData = null;

		string sceneDataRead = File.ReadAllText("Assets/Resources/Cues.json");
		sceneData = JsonConvert.DeserializeObject<SceneData>(sceneDataRead);

		// Edit 5/31 -- the below was working for a build finding the streaming CuesStreaming.json file?
        //string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "CuesStreaming.json");
       	//string sceneDataRead = File.ReadAllText(filePath);
        //sceneData = JsonConvert.DeserializeObject<SceneData>(sceneDataRead);
		// ------------- End edit 5/31

        //string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "CuesStreaming.json");
        //StartCoroutine(Example(filePath));
        
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.sceneData = sceneData;

        // FIX
        //maxCueIndex = sceneData.cues.Count - 1;

        //StartCoroutine(GetStreamingText(streamingFilename));

    }

    IEnumerator Example(string filePath) {
        //Debug.Log(filePath);
        if (filePath.Contains("://")) {
            WWW www = new WWW(filePath);
            yield return www;
            pathResult = www.text;
        } else {
            pathResult = System.IO.File.ReadAllText(filePath);
        }
        sceneData = JsonConvert.DeserializeObject<SceneData>(pathResult);
    }

    IEnumerator GetStreamingText(string filename) {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, streamingFilename);

        if (filePath.Contains("://")) {
            WWW www = new WWW(filePath + "?p=" + Random.Range(1, 100000000).ToString());
            yield return www;
            streamResults = www.text;
            streamingTextTest.text = streamResults;
        } else
            streamResults = System.IO.File.ReadAllText(filePath);
            streamingTextTest.text = streamResults;
    }

    void Start() {

        GenerateStage();

        GenerateHouse();

        // Cue Index is automatically set to 0 to start -- override this here
        GenerateCue(cueIndex);

        //System.Diagnostics.Process p = new System.Diagnostics.Process();
        //p.StartInfo = new System.Diagnostics.ProcessStartInfo("explorer.exe");
        //p.Start();
        

    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.C)) {
            cueIndex++;
            if (cueIndex > maxCueIndex) {
                cueIndex = maxCueIndex;
            } else {
                GenerateCue(cueIndex);
            }
        }
        if (Input.GetKeyDown(KeyCode.Z)) {
            cueIndex--;
            if (cueIndex < 0) {
                cueIndex = 0;
            } else {
                GenerateCue(cueIndex);
            }
        }

        if (Input.GetKeyDown(KeyCode.Return)) {
            sceneData = JsonConvert.DeserializeObject<SceneData>(streamResults);
            GenerateHouse();
            GenerateStage();
            GenerateCue(0);
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter)) {

            string output = JsonConvert.SerializeObject(sceneData);
            string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, streamingFilename);
            File.WriteAllText(filePath, output);
        }

    }

    public void GenerateStage() {
        stageModel.transform.localScale = new Vector3(sceneData.stage.width, sceneData.stage.depth, sceneData.stage.height);
        Stage stage = stageModel.GetComponent<Stage>();
        stage.stageData = sceneData.stage;
        stage.CalculateRake(sceneData.stage.rake);
    }
    public void GenerateHouse() {
        houseModel.transform.localScale = new Vector3(sceneData.house.width, sceneData.house.depth, sceneData.house.height);
        houseModel.transform.position = sceneData.house.position;
        House house = houseModel.GetComponent<House>();
        house.houseData = sceneData.house;
    }

    public void GenerateCue(int index) {
        
        /*
        if (pipeList.Count > 0) {
            foreach(GameObject pipe in pipeList) {
                Destroy((GameObject)pipe);
            }
            pipeList.Clear();
        }
            

        foreach (PipeData i in sceneData.cues[index].pipes) {
            pipeCount++;
            if (!i.vertical) {
                InstantiatePipe(i);
            } else {
                InstantiatePipeVertical(i);
            }
        }

        panelCues.UpdateCueIndex(index);
        */

    }

    public void JsonExport() {

        string output = JsonConvert.SerializeObject(sceneData);

		string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "CuesStreaming.json");
		//File.WriteAllText(filePath, output);								// USED FOR PLATFORM BUILDS
        File.WriteAllText("Assets/Resources/Cues.json", output);  		// <---- USE THIS ONE OTHERWISE

        //string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "CuesStreaming.json");

        //if (filePath.Contains("://")) {
        //    WWW www = new WWW(filePath);
        //    yield return www;
        //    result = www.text;
        //} else {
        //    result = System.IO.File.ReadAllText(filePath);
        //}

        //File.WriteAllText(filePath, output);

    }

    // Deprecated -- removing this whole functionality, in favor of freely-placed 3D objects

    /*
    public void AddPipe(bool vertical) {

    	pipeCount++;

    	PipeData i = new PipeData();
    	i.center = false;
    	i.position = new Vector3(0,10,0);
    	i.width = 20;
    	i.lights = new List<LightData>();

    	sceneData.cues[cueIndex].pipes.Add(i);

		if (vertical) {
    		i.vertical = true;
    		InstantiatePipeVertical(i);
    	} else {
	    	i.vertical = false;
	    	InstantiatePipe(i);
    	}
    }

    public void InstantiatePipe(PipeData i)
    {
        GameObject newRail = (GameObject)Instantiate(pipePrefab, i.position, Quaternion.Euler(-90, 0, 0));
        pipeList.Add(newRail);

        newRail.GetComponent<Pipe>().pipeData = i;
        newRail.name = "Pipe " + pipeCount;

        Transform railModel = newRail.GetComponentInChildren<BoxCollider>().transform;
        railModel.localScale = new Vector3(i.width, railModel.localScale.y, railModel.localScale.z);
        //newRail.transform.localScale = new Vector3(i.width, newRail.transform.localScale.y, newRail.transform.localScale.z);

        // Center on stage (optional)
        if (i.center)
        {
            float xOffset = (sceneData.stage.width - i.width) / 2;
            newRail.transform.position = new Vector3(xOffset + stageModel.transform.position.x, newRail.transform.position.y, newRail.transform.position.z);
        }

        if (i.lights != null)
        {
            foreach (LightData j in i.lights)
            {

                InstantiateLightPrefab(newRail.GetComponent<Pipe>(), j);

            }
        }
    }

    public void InstantiatePipeVertical(PipeData i) {

		GameObject newRail = (GameObject)Instantiate(pipePrefabVertical, i.position, Quaternion.Euler(-90, 0, 0));
        pipeList.Add(newRail);

        newRail.GetComponent<Pipe>().pipeData = i;
        newRail.name = "Pipe " + pipeCount;
        pipeCount++;

        Transform railModel = newRail.GetComponentInChildren<BoxCollider>().transform;
        railModel.localScale = new Vector3(railModel.localScale.x, railModel.localScale.y, i.width);
        //newRail.transform.localScale = new Vector3(i.width, newRail.transform.localScale.y, newRail.transform.localScale.z);

        // Center on stage (optional)
        //if (i.center) {
        //    float xOffset = (sceneData.stage.width - i.width) / 2;
        //    newRail.transform.position = new Vector3(xOffset + stageModel.transform.position.x, newRail.transform.position.y, newRail.transform.position.z);
        //}

        if (i.lights != null) {
            foreach (LightData j in i.lights) {

				InstantiateLightPrefabVertical(newRail.GetComponent<Pipe>(), j);
                
            }
        }
    }

    public void InstantiateLightPrefab(Pipe pipe, LightData lightData) {

	    lightCount++;

	    GameObject newLight = (GameObject)Instantiate(lightPrefab, Vector3.zero, Quaternion.Euler(new Vector3(-90, 0, 0)));
	    StageLight stageLight = newLight.GetComponent<StageLight>();
	    newLight.name = "Light " + lightCount;

	    newLight.transform.SetParent(pipe.gameObject.transform);
	    newLight.transform.localPosition = new Vector3(lightData.position, 0, 0);

	    newLight.GetComponent<StageLight>().pipe = pipe;
		newLight.GetComponent<StageLight>().lightData = lightData;

	    // ADD DICTIONARY ASSOCIATING GAME OBJECTS WITH SCENE DATA -- KEY IS SCENE DATA NODE -- VALUE IS GAME OBJECT

	    //newLight.transform.rotation = Quaternion.Euler(180, 0, 0);
		stageLight.transform.localRotation = Quaternion.Euler(0, 0, lightData.rotation.z);
		stageLight.lightTransform.localRotation = Quaternion.Euler(lightData.rotation.x, 0, 0);

	    // OLD WITH COLOR PROPERTY
	    //newLight.GetComponentInChildren<Light>().color = new Color(j.color.r/255, j.color.g/255, j.color.b/255, 1);
		newLight.GetComponentInChildren<Light>().color = new Color(lightData.colorR / 255, lightData.colorG / 255, lightData.colorB / 255, 1);      // j.colorA is alpha, if we need it

	    // Turn 0-1 intensity into 0-8 Unity intensity
		newLight.GetComponentInChildren<Light>().intensity = lightData.intensity * 8;

    }

    public void InstantiateLightPrefabVertical(Pipe pipe, LightData lightData) {

	    lightCount++;
	    GameObject newLight = (GameObject)Instantiate(lightPrefab, Vector3.zero, Quaternion.Euler(new Vector3(-90, 0, 0)));
	    StageLight stageLight = newLight.GetComponent<StageLight>();
	    newLight.name = "Light " + lightCount;

	    newLight.transform.SetParent(pipe.gameObject.transform);
	    newLight.transform.localPosition = new Vector3(0, 0, lightData.position);

	    newLight.GetComponent<StageLight>().pipe = pipe;
	    newLight.GetComponent<StageLight>().lightData = lightData;

	    // ADD DICTIONARY ASSOCIATING GAME OBJECTS WITH SCENE DATA -- KEY IS SCENE DATA NODE -- VALUE IS GAME OBJECT

	    //newLight.transform.rotation = Quaternion.Euler(180, 0, 0);
	    stageLight.transform.localRotation = Quaternion.Euler(0, 0, lightData.rotation.z);
	    stageLight.lightTransform.localRotation = Quaternion.Euler(lightData.rotation.x, 0, 0);

	    // OLD WITH COLOR PROPERTY
	    //newLight.GetComponentInChildren<Light>().color = new Color(j.color.r/255, j.color.g/255, j.color.b/255, 1);
	    newLight.GetComponentInChildren<Light>().color = new Color(lightData.colorR / 255, lightData.colorG / 255, lightData.colorB / 255, 1);      // j.colorA is alpha, if we need it

	    // Turn 0-1 intensity into 0-8 Unity intensity
	    newLight.GetComponentInChildren<Light>().intensity = lightData.intensity * 8;

    }


	public void AddLightToPipe (Pipe pipe) {

		LightData newLightData = new LightData();
        newLightData.active = true;
        newLightData.colorR = 255;
        newLightData.colorG = 255;
        newLightData.colorB = 255;
        newLightData.colorA = 255;
        newLightData.gobo = "0";
        newLightData.intensity = 0.5f;
        newLightData.position = 0;
        newLightData.rotation = new Vector3(45, 0, 0);

        pipe.pipeData.lights.Add(newLightData);

        InstantiateLightPrefab(pipe, newLightData);

	}

	public void DeleteLight(StageLight light) {
		PipeData pipeData = light.pipe.pipeData;
		pipeData.lights.Remove(light.lightData);
		Destroy(light.gameObject);
	}

	public void DeletePipe(Pipe pipe) {
		sceneData.cues[cueIndex].pipes.Remove(pipe.pipeData);
		Destroy(pipe.gameObject);
	}
    */

}