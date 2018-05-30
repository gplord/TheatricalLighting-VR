using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelRail : MonoBehaviour {

    // Deprecated -- removing Pipes in favor of freely-placed 3D Objects
    /*
    public GameManager gameManager;

    public Pipe pipe;

    public Text name;
    public Toggle railEnabled;

    public InputField posX;
    public InputField posY;
    public InputField posZ;

    public InputField width;
    public Toggle center;

    public Dropdown type;

    public Button addLight;
    public Button deleteRail;

    public GameObject lightsList;
    public GameObject buttonLightPrefab;

    
    void Start() {

        posX.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        posY.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        posZ.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        width.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        center.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        type.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

		addLight.onClick.AddListener( delegate { AddLight(); });
        deleteRail.onClick.AddListener( delegate { DeleteRail(); });
 
        // Fire a forced update, to enable these default values
        ValueChangeCheck();

    }

    void Update() {

    }

    public void Setup() {

        name.text = pipe.gameObject.name;

        posX.text = pipe.transform.position.x.ToString();
        posY.text = pipe.transform.position.y.ToString();
        posZ.text = pipe.transform.position.z.ToString();

        if (pipe.pipeData.vertical) {
            width.text = pipe.pipeModel.localScale.z.ToString();
        } else {
            width.text = pipe.pipeModel.localScale.x.ToString();
        }
        center.isOn = pipe.pipeData.center;

        // TODO: Need type (horiz/vert) here

        int j = 0;

        foreach(LightData i in pipe.pipeData.lights) {

            GameObject button = (GameObject) Instantiate(buttonLightPrefab) as GameObject;
            button.transform.SetParent(lightsList.transform);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            ButtonLight buttonLight = button.GetComponent<ButtonLight>();
            //buttonLight. = lightMount;
            j++;
        }

    }


    public void ValueChangeCheck() {

        pipe.transform.position = new Vector3(float.Parse(posX.text), float.Parse(posY.text), float.Parse(posZ.text));
        //rail.transform.localScale = new Vector3(float.Parse(width.text), rail.transform.localScale.y, rail.transform.localScale.z);

        if (pipe.pipeData.vertical) {
            pipe.pipeModel.localScale = new Vector3(1, 1, float.Parse(width.text));
        } else {
            pipe.pipeModel.localScale = new Vector3(float.Parse(width.text), 1, 1);
        }

        pipe.pipeData.position = new Vector3(float.Parse(posX.text), float.Parse(posY.text), float.Parse(posZ.text));
        pipe.pipeData.center = center.isOn;
        pipe.pipeData.width = float.Parse(width.text);

        // REMEMBER TO USE THE NEGATIVE VALUE OF EACH PIPE'S Y -- UNITY FLIPS THESE

    }

    public void DeleteRail() {
    	gameManager.DeletePipe(pipe);
    }

    public void AddLight() {
    	gameManager.AddLightToPipe(pipe);
    }

    public void TargetSelectCheck() {

        gameManager.selectingTarget = true;

    }
    */

}
