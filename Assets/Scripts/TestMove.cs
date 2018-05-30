using UnityEngine;
using System.Collections;

public class TestMove : MonoBehaviour {

	public bool reverseX = false;
	public bool reverseZ = false;

	private float minBoundX;
	private float minBoundZ;
	private float maxBoundX;
	private float maxBoundZ;

	// Use this for initialization
	void Start () {
		minBoundX = -40;
		minBoundZ = 30;
		maxBoundX = 40;
		maxBoundZ = 70;

		transform.position = new Vector3(Random.Range(minBoundX, maxBoundX), -19f, Random.Range(minBoundZ, maxBoundZ));
	}
	
	// Update is called once per frame
	void Update () {

		int dirX = 1;
		int dirZ = 1;
		if (reverseX) dirX = -1;
		if (reverseZ) dirZ = -1;

		transform.position = new Vector3(transform.position.x + 0.125f * dirX, transform.position.y, transform.position.z-0.125f * dirZ);

		if ((transform.position.x < minBoundX) || (transform.position.x > maxBoundX)) {
			reverseX = !reverseX;
		}
		if ((transform.position.z < minBoundZ) || (transform.position.z > maxBoundZ)) {
			reverseZ = !reverseZ;
		}
	}
}
