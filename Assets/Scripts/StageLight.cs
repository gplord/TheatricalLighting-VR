using UnityEngine;
using System.Collections;

public class StageLight : SelectableObject {

	public Pipe pipe;
	public Transform lightTransform;
	public Light spotlight;

	//public Vector3 lookPosition;
	//public Transform target;
	//public bool followTarget;
    
	public int cookieIndex;
    public float tiltValue = 0;
    public float panValue = 0;

    public LightData lightData;
    
	void Awake () {
	}
    
	void Update () {

		//if (followTarget) {
		//	if (target != null) {
		//		SetMountRotationTarget(target);
		//		SetLightRotationTarget(target);
		//	}
		//}

	}

	//public void SetLightRotationDegrees (float deg) {
	//	lightRig.eulerAngles = new Vector3(deg, 0, 0);
	//}
	//public void SetLightRotationTarget (Transform target) {
	//	if (target != null) {
	//		lookPosition = target.position;
	//	}
	//	lightRig.LookAt(lookPosition);
	//	// Quick fix for model rig angles -- can be fixed later in the model/armature
	//	lightRig.Rotate(new Vector3(-90f,0,0));
	//}

	
	//public string GetTargetName() {
	//	if (target != null) {
	//		return target.gameObject.name;
	//	} else {
	//		return "(None)";
	//	}
	//}

}
