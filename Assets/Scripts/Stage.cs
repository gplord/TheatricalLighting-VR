using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour {

    public StageData stageData;

    private List<int> vertexIndices = new List<int>();
    private float initialZ;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public float CalculateRake(float rake) {

        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        Mesh mesh = meshFilter.mesh;
        
        int vertexIndex = 0;
        float highestY = 9999;
        float highestZ = -9999;

        foreach (Vector3 vertex in mesh.vertices) {
            if (vertex.y < highestY) highestY = vertex.y;
            if (vertex.z > highestZ) highestZ = vertex.z;
        }
        foreach (Vector3 vertex in mesh.vertices) {
            if ((vertex.y == highestY) && (vertex.z == highestZ)) vertexIndices.Add(vertexIndex);
            vertexIndex++;
        }
        Vector3[] vertices = mesh.vertices;
        foreach (int index in vertexIndices) {
            vertices[index] = new Vector3(mesh.vertices[index].x, mesh.vertices[index].y, 1 + (stageData.rake / stageData.height));
        }
        mesh.vertices = vertices;

        float slopeRad = rake * Mathf.PI / 180;
        float triangleBase = stageData.depth;

        float missingAngle = 180 - (90 + rake);
        float missingAngleRad = missingAngle * Mathf.PI / 180;
        float sideC = (triangleBase * Mathf.Sin(slopeRad)) / Mathf.Sin(missingAngleRad);

        return (triangleBase * Mathf.Sin(slopeRad)) / Mathf.Sin(missingAngleRad);

    }


}
