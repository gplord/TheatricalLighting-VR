using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelHouseStage : MonoBehaviour {

    // TODO: Delete this whole script -- legacy

    public GameManager gameManager;

    public Transform model;
    public House house;

    public Text name;

    public InputField inputWidth;
    public InputField inputDepth;
    public InputField inputHeight;
    
    public InputField inputPosX;
    public InputField inputPosY;
    public InputField inputPosZ;
    
    void Start() {

        inputWidth.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        inputDepth.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        inputHeight.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        inputPosX.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        inputPosY.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        inputPosZ.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        // Fire a forced update, to enable these default values
        ValueChangeCheck();

    }

    void Update() {

    }

    public void Setup() {

        inputWidth.text = house.houseData.width.ToString();
        inputDepth.text = house.houseData.depth.ToString();
        inputHeight.text = house.houseData.height.ToString();

        inputPosX.text = house.houseData.position.x.ToString();
        inputPosY.text = house.houseData.position.y.ToString();
        inputPosZ.text = house.houseData.position.z.ToString();

    }


    public void ValueChangeCheck() {

        float tempWidth = float.Parse(inputWidth.text.ToString());
        float tempDepth = float.Parse(inputDepth.text.ToString());
        float tempHeight = float.Parse(inputHeight.text.ToString());

        model.localScale = new Vector3(tempWidth, tempDepth, tempHeight);
        
        model.position = new Vector3(float.Parse(inputPosX.text), float.Parse(inputPosY.text), float.Parse(inputPosZ.text));

        house.houseData.width = model.localScale.x;
        house.houseData.depth = model.localScale.y;
        house.houseData.height = model.localScale.z;

        house.houseData.position = model.position;

    }

    public void CloseWindow() {
        this.gameObject.SetActive(false);
    }

    public void TargetSelectCheck() {

        //gameManager.selectingTarget = true;

    }

}
