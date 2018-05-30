using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class UIInputFieldBlock : MonoBehaviour, ISelectHandler {

    GameManager gameManager;
    CameraManager cameraManager;

    InputField inputField;

	// Use this for initialization
	void Start () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        cameraManager = gameManager.cameraManager;
        inputField = GetComponent<InputField>();
        
        inputField.onEndEdit.AddListener(delegate { InputBlock(false); });
	}
    
    public void OnSelect(BaseEventData eventData)
    {
        InputBlock(true);
    }

    void InputBlock(bool onOff)
    {
        cameraManager.blockedByInputField = onOff;
    }

}
