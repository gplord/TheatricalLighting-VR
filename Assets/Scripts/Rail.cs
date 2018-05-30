using UnityEngine;
using System.Collections;

public class Rail : SelectableObject {

	public BoxCollider collider;
	public Vector2 bounds;

    //public RailData railData;

    // Use this for initialization
    void Start () {
		bounds = GetBounds();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Vector2 GetBounds() {
		return new Vector2(collider.bounds.min.x, collider.bounds.max.x);
	}

}
