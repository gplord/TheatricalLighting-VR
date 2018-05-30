using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelCues : MonoBehaviour {

	public Button cuePrevious;
	public Button cueNext;

	public Text cueLabel;

	public Button cueSave;
	public Button cueDelete;
	public Button cueAdd;

	public int index;
	public int maxCueIndex;

    public GameManager gameManager;
    public JsonImport jsonImport;

	// Use this for initialization
	void Start() {

        maxCueIndex = jsonImport.maxCueIndex;

		//cuePrevious.onClick.AddListener(delegate { PrevCue(); });
		//cueNext.onClick.AddListener(delegate { NextCue(); });
		//cueSave.onClick.AddListener(delegate { SaveCue(); });
		//cueDelete.onClick.AddListener(delegate { DeleteCue(); });
        //cueAdd.onClick.AddListener(delegate { AddCue(); });

        // Fire a forced update, to enable these default values
        ValueChangeCheck();

        UpdateCueDisplay();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ValueChangeCheck() {
        

    }


    public void GoToCue(int index) {
        gameManager.GoToCue(index);
    }

    public void UpdateCueIndex(int newIndex) {
        index = newIndex;
        UpdateCueDisplay();
    }
    public void UpdateCueDisplay() {
        cueLabel.text = string.Format("Cue {0} of {1}", index + 1, maxCueIndex + 1);
    }

    public void NextCue() {
    	if (!(index+1 > maxCueIndex)) {
	    	GoToCue(index+1);
	    }
    }
    public void PrevCue() {
		if (!(index-1 < 0)) {
	    	GoToCue(index-1);
	    }
    }

    public void SaveCue() {
        gameManager.SaveCue();
    }

    public void DeleteCue() {
        gameManager.DeleteCue();
    }

    public void AddCue() {
        gameManager.AddCue();
    }

}
