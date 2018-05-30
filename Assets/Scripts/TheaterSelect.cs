using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TheaterSelect : MonoBehaviour {

    Dropdown dropdown;
    public GameObject[] theaterModels;

    public GameObject activeModel;

	// Use this for initialization
	void Start () {
        dropdown = GetComponent<Dropdown>();
        dropdown.onValueChanged.AddListener(delegate { SelectTheater(); });
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void SelectTheater()
    {
        int index = dropdown.value;
        Destroy(activeModel);
        activeModel = (GameObject) Instantiate(theaterModels[index]);
    }

}
