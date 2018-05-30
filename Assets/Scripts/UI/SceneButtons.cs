using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneButtons : MonoBehaviour {

	public Button lightsSetup;
	public Button kingAndI;

	// Use this for initialization
	void Start () {
		lightsSetup.onClick.AddListener(delegate {
			GoToLightsSetup();
		});
		kingAndI.onClick.AddListener(delegate {
			GoToKingAndI();	
		});
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void GoToLightsSetup() {
		SceneManager.LoadScene("RailSetup");
	}
	void GoToKingAndI() {
		SceneManager.LoadScene("KingAndI-Lit");
	}
}
