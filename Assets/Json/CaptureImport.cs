using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class CaptureImport : MonoBehaviour {

    public GameObject lightsParent;
    public GameObject lightPrefab;
    public CaptureExportData captureData;
    public string defaultCaptureData = "Fixtures/CaptureExport.json";

    public SortedDictionary<int, Channel> channels;
    public List<LightCues> lightsWithCues;
    public List<LightFixture> lightFixtures;
    
    public Transform uiChannelsList;
    public GameObject uiButtonChannel;

    public LightManager lightManager;

    public GameManager gameManager;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SerializeLights();
        }
    }

    public void ImportData()
    {
        ImportData(defaultCaptureData);
    }

    public void ImportData (string filename) {

        string filePath = Path.Combine(Application.streamingAssetsPath, filename);
        string captureDataImport = File.ReadAllText(filePath);
        captureData = JsonConvert.DeserializeObject<CaptureExportData>(captureDataImport);

        int i = 1;

        channels = new SortedDictionary<int, Channel>();

        // Fixing instantiation order
        if (lightManager.lightIds == null) lightManager.lightIds = new Dictionary<int, GameObject>();

        foreach (ImportLight importLight in captureData.importLights) {
            importLight.id = i;
            Vector3 importPos = new Vector3(-importLight.posX, importLight.posY, importLight.posZ);
            GameObject newLight = (GameObject)Instantiate(lightPrefab, importPos, Quaternion.identity);
            newLight.name = "Light " + i;
            newLight.transform.parent = lightsParent.transform;
            newLight.transform.rotation = Quaternion.Euler(importLight.focusTilt, importLight.focusPan, 0f);

            Light lightComponent = newLight.GetComponentInChildren<Light>();

            CaptureData data = newLight.GetComponent<CaptureData>();
            data.id = importLight.id;
            data.fixture = importLight.fixture;
            data.optics = importLight.optics;
            data.wattage = importLight.wattage;
            data.unit = importLight.unit;
            data.circuit = importLight.circuit;
            data.channel = importLight.channel;
            data.patch = importLight.patch;
            data.dmxmode = importLight.dmxmode;
            data.dmxchannels = importLight.dmxchannels;
            data.layer = importLight.layer;
            data.focus = importLight.focus;
            data.filters = importLight.filters;
            data.gobos = importLight.gobos;
            data.accessories = importLight.accessories;
            data.note = importLight.note;
            data.weight = importLight.weight;
            data.posX = importLight.posX;
            data.posY = importLight.posY;
            data.posZ = importLight.posZ;
            data.focusPan = importLight.focusPan;
            if (data.focusPan < 0) data.focusPan += 360;    // Keep Pan in the positive coordinate system, without needing to affect Capture export data
            data.focusTilt = importLight.focusTilt;

            data.importLight = importLight;

            // Process filters, or use previously stored RGB
            if ((importLight.colorR > 0) || (importLight.colorG > 0) || (importLight.colorB > 0)) {     // If we have previously saved color data (overriding filter values)

                data.colorR = importLight.colorR;
                data.colorG = importLight.colorG;
                data.colorB = importLight.colorB;

                lightComponent.color = new Color(importLight.colorR / 255f, importLight.colorG / 255f, importLight.colorB / 255f);

            } else {                                                                                    // If not, process filters array, if there is one
                string[] filters = data.filters.Split(',');
                //Debug.Log("Light #" + data.id + " has " + filters.Length + " filters.");
                if (filters.Length > 1)
                {
                    List<float> colorsR = new List<float>();
                    List<float> colorsG = new List<float>();
                    List<float> colorsB = new List<float>();
                    foreach (string filter in filters)
                    {
                        string trimmedFilter = filter.Trim();
                        if (gameManager.gelColorsDictionary.ContainsKey(trimmedFilter))
                        {
                            colorsR.Add(gameManager.gelColorsDictionary[trimmedFilter].colorR);
                            colorsG.Add(gameManager.gelColorsDictionary[trimmedFilter].colorG);
                            colorsB.Add(gameManager.gelColorsDictionary[trimmedFilter].colorB);
                            //Debug.Log("Result: [" + filter + "]");
                        }
                    }

                    float tempColorR = colorsR[0];
                    float tempColorG = colorsG[0];
                    float tempColorB = colorsB[0];
                    for (int j = 1; j < colorsR.Count; j++)
                    {
                        tempColorR = (tempColorR * colorsR[j]) / 255f;
                        tempColorG = (tempColorG * colorsG[j]) / 255f;
                        tempColorB = (tempColorB * colorsB[j]) / 255f;
                    }

                    //Debug.Log("Final Color Values// R:" + tempColorR + " / G:" + tempColorG + " / B:" + tempColorB);
                    data.colorR = Mathf.Round(tempColorR);
                    data.colorG = Mathf.Round(tempColorG);
                    data.colorB = Mathf.Round(tempColorB);
                    lightComponent.color = new Color(tempColorR / 255f, tempColorG / 255f, tempColorB / 255f);

                }
                else
                {
                    if (filters[0] == "")
                    {
                        lightComponent.color = Color.white;
                        data.colorR = 255;
                        data.colorG = 255;
                        data.colorB = 255;
                    }
                    else
                    {
                        if (gameManager.gelColorsDictionary.ContainsKey(filters[0]))
                        {
                            GelColorData gel = gameManager.gelColorsDictionary[filters[0]];
                            lightComponent.color = new Color(gel.colorR / 255f, gel.colorG / 255f, gel.colorB / 255f);
                            data.colorR = gel.colorR;
                            data.colorG = gel.colorG;
                            data.colorB = gel.colorB;
                        }
                        else
                        {
                            lightComponent.color = Color.white;
                            data.colorR = 255;
                            data.colorG = 255;
                            data.colorB = 255;
                        }
                    }
                }

                // Process gobos
                if (data.gobos != null) { 
                    string[] gobos = data.gobos.Split(',');
                    //Debug.Log("Light #" + data.id + " has " + gobos.Length + " gobos.");
                    if (gobos.Length > 1) {

                        data.gobos = gobos[0];  // Cannot stack Cookies in Unity, so use only the first
                        // If we do need to combine these, a texture combination script exists in TextureTest, although these will likely not be processed as cookies properly

                    }

                    lightComponent.cookie = Resources.Load("Gobos/" + data.gobos) as Texture;

                }

            }
                        
            lightManager.AddToLights(data.id, newLight);

            //LightCues lightCues = newLight.GetComponent<LightCues>();
            //lightsWithCues.Add(lightCues);

            // Replacing LightsWithCues with this, as the old cues paradigm is getting replaced
            LightFixture lightFixture = newLight.GetComponent<LightFixture>();
            lightFixture.SetColor();
            lightFixtures.Add(lightFixture);

            AddToChannel(importLight.channel, importLight.id);

            // TODO: Rethink the light component's range element -- for now, adjust range at load time, and readjust when moving it (see PanelLightSelected)
            // Set range to distance from origin, which seems like a reasonable universal value
            lightFixture.spotlight.range = Vector3.Distance(lightFixture.transform.position, new Vector3(13f, 0, 13f));  // Adjust for center stage
            if (lightFixture.spotlight.range < 40f) lightFixture.spotlight.range = 40f;

            i++;
        }
        
        foreach (KeyValuePair<int, Channel> entry in channels) {
            GameObject newButton = (GameObject)Instantiate(uiButtonChannel, uiChannelsList);
            newButton.name = "Channel " + entry.Key.ToString();
            newButton.GetComponent<UIButtonChannel>().label.text = "Channel " + entry.Key.ToString();
            newButton.GetComponent<UIButtonChannel>().channelId = entry.Key;

        }

    }

    public void AddToChannel(int index, int lightId) {
        
        if (channels.ContainsKey(index)) {

            //Debug.LogWarning("Channel " + index + " already exists; skipping.");

        } else {

            //Debug.LogWarning("Created channel " + index);
            channels.Add(index, new Channel());
            channels[index].channelId = index;
            channels[index].lightIds = new List<int>();

        }

        channels[index].lightIds.Add(lightId);

    }

    public void UnsetAllCaptureData()
    {
        captureData = new CaptureExportData();
        foreach (Transform child in lightManager.gameObject.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in uiChannelsList)
        {
            Destroy(child.gameObject);
        }
        lightsWithCues = new List<LightCues>();
        lightFixtures = new List<LightFixture>();
        channels = new SortedDictionary<int, Channel>();
    }

    public void SerializeLights()
    {
        // Fix me by uncommenting this
        //foreach (LightCues lightWithCues in lightsWithCues)
        //{
        //    CaptureData captureData = lightWithCues.GetComponent<CaptureData>();
        //    captureData.UpdateImportLight();
        //}
        int i = 1;
        foreach (LightFixture lightFixture in lightFixtures)
        {
            CaptureData captureData = lightFixture.GetComponent<CaptureData>();
            captureData.UpdateImportLight(i);
            i++;
        }

        string newJson = JsonConvert.SerializeObject(captureData);

        // TODO: Replace this with the working filename, assuming this has already been loaded or saved
        string filepath = Path.Combine(Application.streamingAssetsPath, "outputTest.json");
        File.WriteAllText(filepath, newJson);

    }

    public void SerializeLights(string filename)
    {
        int i = 1;
        foreach (LightFixture lightFixture in lightFixtures)
        {
            CaptureData captureData = lightFixture.GetComponent<CaptureData>();
            captureData.UpdateImportLight(i);
            i++;
        }

        string newJson = JsonConvert.SerializeObject(captureData);

        if (!filename.EndsWith(".json")) filename = filename + ".json";

        string filepath = Path.Combine(Application.streamingAssetsPath, Globals.FixturesPath + filename);
        File.WriteAllText(filepath, newJson);

    }

}
