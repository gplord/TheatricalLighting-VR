using UnityEngine;
using System.Collections;

public class Pipe : SelectableObject {

    // Deprecated -- removing Pipes in favor of freely-placed 3D Objects
    /*
    public bool vertical = false;
    public bool center = false;
	public BoxCollider boxCollider;
	public Vector2 bounds;

    public PipeData pipeData;
    public Transform pipeModel;

    // Use this for initialization
    void Start () {
		bounds = GetBounds();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Vector2 GetBounds() {
        //Debug.Log("Bounds: " + boxCollider.bounds.min.x + " / " + boxCollider.bounds.max.x);
        //return new Vector2(boxCollider.bounds.min.x, boxCollider.bounds.max.x);
        //return new Vector2(0, pipeData.width);
        boxCollider = GetComponentInChildren<BoxCollider>();
        return new Vector2(boxCollider.bounds.min.x, boxCollider.bounds.max.x);
	}
    */

}
