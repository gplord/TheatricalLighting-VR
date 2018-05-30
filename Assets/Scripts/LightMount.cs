using UnityEngine;
using System.Collections;

public class LightMount : SelectableObject {

	public Pipe rail;
	public Transform mountRig;
	public Transform lightRig;
	public Light light;

	public Vector3 lookPosition;
	public Transform target;
	public bool followTarget;

	//private bool highlighted = false;

	public int cookieIndex;	// Workaround to store previous cookie index
	public float tiltValue = -65; // Workaround to store tilt angle, until all conversions/rotations are finalized
    public float panValue = 0;

    public LightData lightData;

	// Use this for initialization
	void Awake () {
	}

	// Update is called once per frame
	void Update () {

		if (followTarget) {
			if (target != null) {
				SetMountRotationTarget(target);
				SetLightRotationTarget(target);
			}
		}

	}

	public void SetMountRotationDegrees (float deg) {
		mountRig.eulerAngles = new Vector3(270f, deg, 0);
	}
	public void SetMountRotationTarget (Transform target) {
		if (target != null) {
			lookPosition = target.position;
		}
		mountRig.LookAt(lookPosition);
		mountRig.eulerAngles = new Vector3(270f, mountRig.eulerAngles.y, 0);
	}

	public void SetLightRotationDegrees (float deg) {
		lightRig.eulerAngles = new Vector3(deg, 0, 0);
	}
	public void SetLightRotationTarget (Transform target) {
		if (target != null) {
			lookPosition = target.position;
		}
		lightRig.LookAt(lookPosition);
		// Quick fix for model rig angles -- can be fixed later in the model/armature
		lightRig.Rotate(new Vector3(-90f,0,0));
	}

	public override void Highlight (bool onOff) {

		if (onOff) {
//		if (!highlighted) {

			Renderer[] renderers = GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer in renderers) {
				foreach (Material mat in renderer.materials) {
					mat.EnableKeyword ("_EMISSION");
					mat.SetColor ("_EmissionColor", new Color(0,0.35f,0.75f));
				}
			}
			highlighted = true;

		} else {

			Renderer[] renderers = GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer in renderers) {
				foreach (Material mat in renderer.materials) {
					mat.DisableKeyword ("_EMISSION");
				}
			}
			highlighted = false;
		}

	}

	public string GetTargetName() {
		if (target != null) {
			return target.gameObject.name;
		} else {
			return "(None)";
		}
	}

}
