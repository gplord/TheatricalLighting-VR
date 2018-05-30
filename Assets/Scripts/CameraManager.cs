using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour {

    public Camera activeCamera;

    public List<Camera> cameras = new List<Camera>();

    private int cameraIndex;
    public int perspectiveSize = 5;
    public int orthographicSize = 25;

    public float cameraMoveSpeed = 0.75f;

    public Dropdown cameraDropdown;

    public bool blockedByInputField = false;

	// Use this for initialization
	void Awake () {
        cameraIndex = 0;
        activeCamera = cameras[cameraIndex];
        
        cameraDropdown.onValueChanged.AddListener(delegate { ChooseCamera(cameraDropdown); });

    }

    private void Start()
    {
        RedrawCameraDropdowns();
        ChooseCamera(0);
    }

    // Update is called once per frame
    void Update () {

        if (!blockedByInputField)
        {
            if (!Input.GetKey(KeyCode.LeftControl)) // Reserves Ctrl+Left/Ctrl+Right to be cues hotkeys
            {
                activeCamera.transform.Translate(Input.GetAxisRaw("Horizontal") * cameraMoveSpeed, 0, 0);   // Move camera left/right (X-axis)

                if (!activeCamera.orthographic) // Move camera back/forward (Z-axis), unless in orthographic mode
                {
                    activeCamera.transform.Translate(0, 0, Input.GetAxisRaw("Vertical") * cameraMoveSpeed);
                }
                else
                {
                    activeCamera.orthographicSize += -Input.GetAxisRaw("Vertical") * 0.5f;  // In orthographic mode, size up/down ortho scale (zoom)
                }
            }

            if (Input.GetKey(KeyCode.E))    // Move camera up/down (Y-Axis)
            {
                activeCamera.transform.Translate(0, cameraMoveSpeed, 0);
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                activeCamera.transform.Translate(0, -cameraMoveSpeed, 0);
            }

            if (Input.GetKeyDown(KeyCode.X))
            {  // Switch between perspective and orthographic camera modes
                activeCamera.orthographic = !activeCamera.orthographic;
                activeCamera.orthographicSize = orthographicSize;
            }

            // TODO: Replace camera cycling hotkeys
            if (Input.GetKeyDown(KeyCode.LeftBracket))
            {
                CycleCamera(-1);
            }
            if (Input.GetKeyDown(KeyCode.RightBracket))
            {
                CycleCamera(1);
            }

        }

        if (Input.GetMouseButton(1)) {  // Hold right-click to free-rotate the camera

            activeCamera.transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"), 0, 0));
            activeCamera.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0), Space.World);
            
        }
        
    }

    // Select a particular camera by index
    public void ChooseCamera(int index) {

        activeCamera.gameObject.SetActive(false);

        activeCamera = cameras[index];
        cameraIndex = index;

        activeCamera.gameObject.SetActive(true);
        cameraDropdown.value = cameraIndex;

    }

    public void ChooseCamera(Dropdown dropdown)
    {
        ChooseCamera(dropdown.value);
    }


    // Cycle through all cameras, accepts forward/backward directions (1/-1)
    public void CycleCamera(int dir) {

        cameraIndex += dir;
        
        if (cameraIndex < 0) {
            cameraIndex = cameras.Count - 1;
        } else if (cameraIndex >= cameras.Count) {
            cameraIndex = 0;
        }

        ChooseCamera(cameraIndex);

    }

    public void AddCamera()
    {
        GameObject newCamera = Instantiate(activeCamera.gameObject);
        cameras.Add(newCamera.GetComponent<Camera>());

        RedrawCameraDropdowns();

        CycleCamera(1);
    }

    public void RedrawCameraDropdowns()
    {
        cameraDropdown.options.Clear();
        int i = 1;
        foreach (Camera camera in cameras)
        {
            cameraDropdown.options.Add(new Dropdown.OptionData() { text = "Camera " + i });
            i++;
        };
    }

}
