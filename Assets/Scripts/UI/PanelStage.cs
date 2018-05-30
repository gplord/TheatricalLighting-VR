using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelStage : MonoBehaviour {

    // TODO: Condense this into remaining needed functionality
    
    public GameManager gameManager;

    public Transform model;
    public Stage stage;

    public Text name;

    public InputField inputWidth;
    public InputField inputDepth;
    public InputField inputHeight;
    public InputField inputRake;
    
    public InputField inputPosX;
    public InputField inputPosY;
    public InputField inputPosZ;
    
    void Start() {

        inputWidth.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        inputDepth.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        inputHeight.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        inputRake.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        // Fire a forced update, to enable these default values
        ValueChangeCheck();

    }

    void Update() {

    }

    public void Setup() {

        inputWidth.text = stage.stageData.width.ToString();
        inputDepth.text = stage.stageData.depth.ToString();
        inputHeight.text = stage.stageData.height.ToString();
        inputRake.text = stage.stageData.rake.ToString();

        //inputPosX.text = stage.stageData.width.ToString();
        //inputPosY.text = stage.stageData.width.ToString();
        //inputPosZ.text = stage.stageData.width.ToString();

    }
    
    public void ValueChangeCheck() {

        //model.transform.localScale = new Vector3(float.Parse(inputWidth.text), float.Parse(inputHeight.text), float.Parse(inputDepth.text));

        stage.stageData.rake = float.Parse(inputRake.text);
        stage.CalculateRake(stage.stageData.rake);



        model.transform.localScale = new Vector3(float.Parse(inputWidth.text), float.Parse(inputDepth.text), float.Parse(inputHeight.text));

        //float tempPosX = float.Parse(inputPosX.text);
        //float tempPosY = float.Parse(inputPosY.text);
        //float tempPosZ = float.Parse(inputPosZ.text);

        //model.position = new Vector3(tempPosX, tempPosY, tempPosZ);

        //float tempWidth = float.Parse(inputWidth.text);
        //float tempDepth = float.Parse(inputDepth.text);
        //float tempHeight = float.Parse(inputHeight.text);

        //Debug.LogError(tempWidth);

        ////model.localScale = new Vector3(float.Parse(inputWidth.text), float.Parse(inputDepth.text), float.Parse(inputHeight.text));
        ////model.position = new Vector3(float.Parse(inputPosX.text), float.Parse(inputPosY.text), float.Parse(inputPosZ.text));

        ////model.localScale = new Vector3(tempWidth, tempDepth, tempHeight);

   
    }

    public void CloseWindow() {
        this.gameObject.SetActive(false);
    }

    public void TargetSelectCheck() {

        //gameManager.selectingTarget = true;

    } 

}
