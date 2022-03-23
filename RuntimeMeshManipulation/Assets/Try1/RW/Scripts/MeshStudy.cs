using System.Collections.Generic;
using System;
using UnityEngine;

// When a class has this attribute, its Start will fire in both Play mode and Edit mode
[ExecuteInEditMode]
public class MeshStudy : MonoBehaviour {
    Mesh originalMesh;
    Mesh clonedMesh;
    MeshFilter meshFilter;
    int[] triangles;
    [HideInInspector] public Vector3[] vertices;

    [HideInInspector] public bool isCloned = false;

    // For Editor
    public float radius = 0.2f;
    public float pull = 0.3f;
    public float handleSize = 0.03f;
    public List<int>[] connectedVertices;
    public List<Vector3[]> allTriangleList;
    public bool moveVertexPoint = true;

    void Start() {
        InitMesh();
    }

    /// <summary>
    /// Makes a copy of the Mesh, so that we dont edit or overwrite the unity built-in meshes.
    /// </summary>
    public void InitMesh() {
        meshFilter = GetComponent<MeshFilter>();
        originalMesh = meshFilter.sharedMesh;
        clonedMesh = new Mesh();
        clonedMesh.name = "clone";
        clonedMesh.vertices = originalMesh.vertices;
        clonedMesh.triangles = originalMesh.triangles;
        clonedMesh.normals = originalMesh.normals;
        clonedMesh.uv = originalMesh.uv;
        meshFilter.mesh = clonedMesh;
        vertices = clonedMesh.vertices;
        triangles = clonedMesh.triangles;
        isCloned = true;
        Debug.Log("Init & Cloned 2");
    }


    public void Reset() {
        if (clonedMesh == null || originalMesh == null) return;
        clonedMesh.vertices = originalMesh.vertices;
        clonedMesh.triangles = originalMesh.triangles;
        clonedMesh.normals = originalMesh.normals;
        clonedMesh.uv = originalMesh.uv;
        meshFilter.mesh = clonedMesh;
        vertices = clonedMesh.vertices;
        triangles = clonedMesh.triangles;
    }

    public void GetConnectedVertices() {
        connectedVertices = new List<int>[vertices.Length];
    }

    public void DoAction(int index, Vector3 localPos) {
        // specify methods here
        //PullOneVertex(index, localPos);
        PullSimilarVertices(index, localPos);
    }


    public void BuildTriangleList() { }
    public void ShowTriangle(int idx) { }

    /// <summary>
    /// Pulls only one vertex pt in the mesh, results in broken mesh.
    /// </summary>
    /// <param name="index">selected vertex index</param>
    /// <param name="newPos"> new position of the selected vertex</param>
    private void PullOneVertex(int index, Vector3 newPos) {
        vertices[index] = newPos;
        clonedMesh.vertices = vertices;
        clonedMesh.RecalculateNormals();
    }

    /// <summary>
    /// Pulls all vertices that are at the same location as the vertex at index, does not break mesh.
    /// </summary>
    /// <param name="index">selected vertex index</param>
    /// <param name="newPos"> new position of the selected vertex</param>>
    private void PullSimilarVertices(int index, Vector3 newPos) {
        Vector3 targetVertexPos = vertices[index];
        List<int> relatedVertices = FindRelatedVertices(targetVertexPos, false);
        foreach (int i in relatedVertices) {
            vertices[i] = newPos;
        }

        clonedMesh.vertices = vertices;
        clonedMesh.RecalculateNormals();
    }

    /// <summary>
    /// Finds the vertices that are at the same location as the vertex at index.
    /// </summary>
    /// <param name="targetPt"></param>
    /// <param name="findConnected"></param>
    /// <returns></returns>
    private List<int> FindRelatedVertices(Vector3 targetPt, bool findConnected) {
        List<int> relatedVertices = new List<int>();

        int idx = 0;
        Vector3 pos;
        // loop through triangle array of indices
        for (int t = 0; t < triangles.Length; t++) {
            // current idx return from tris
            idx = triangles[t];
            // current pos of the vertex
            pos = vertices[idx];
            // if current pos is same as targetPt
            if (pos == targetPt) {
                // add to list
                relatedVertices.Add(idx);
                // if find connected vertices
                if (findConnected) {
                    // min
                    // - prevent running out of count
                    if (t == 0) {
                        relatedVertices.Add(triangles[t + 1]);
                    }

                    // max 
                    // - prevent runnign out of count
                    if (t == triangles.Length - 1) {
                        relatedVertices.Add(triangles[t - 1]);
                    }

                    // between 1 ~ max-1 
                    // - add idx from triangles before t and after t 
                    if (t > 0 && t < triangles.Length - 1) {
                        relatedVertices.Add(triangles[t - 1]);
                        relatedVertices.Add(triangles[t + 1]);
                    }
                }
            }
        }

// return compiled list of int
        return relatedVertices;
    }

    // To test Reset function
    public void EditMesh() {
        vertices[2] = new Vector3(2, 3, 4);
        vertices[3] = new Vector3(1, 2, 4);
        clonedMesh.vertices = vertices;
        clonedMesh.RecalculateNormals();
    }
}