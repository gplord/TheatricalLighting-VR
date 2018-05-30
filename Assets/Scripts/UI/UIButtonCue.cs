using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonCue : MonoBehaviour {

    public Text label;
    //public int cueId;
    public float cueId;
    public int cueIndex;
    
    Button cueButton;
    public CuesManager cuesManager;

    private void Start()
    {
        cueButton = GetComponent<Button>();
        cueButton.onClick.AddListener(CueOnClick);
    }

    void CueOnClick()
    {
        cuesManager.GetComponent<CuesManager>().GoToCue(cueIndex);
    }

}
