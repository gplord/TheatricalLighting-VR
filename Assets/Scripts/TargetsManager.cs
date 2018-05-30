using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TargetsManager : MonoBehaviour {

	public MeshRenderer[] renderers;

	public Button targetDisplay;

	// Use this for initialization
	void Start () {
		targetDisplay.onClick.AddListener ( delegate { TargetToggleCheck (); } );
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Q)) {
			TargetToggleCheck();
		}
	}

	void TargetToggleCheck() {
		foreach (MeshRenderer renderer in renderers) {
			renderer.enabled = !renderer.enabled;
		}
	}
}
