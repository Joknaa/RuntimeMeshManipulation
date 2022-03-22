/*
 * Copyright (c) 2019 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
*/

using System.Collections.Generic;
using UnityEngine;

// When a class has this attribute, its Start will fire in both Play mode and Edit mode
[ExecuteInEditMode]
public class MeshStudy : MonoBehaviour {
    [HideInInspector] public Vector3[] vertices;

    [HideInInspector] public bool isCloned;

    // For Editor
    public float radius = 0.2f;
    public float pull = 0.3f;
    public float handleSize = 0.03f;
    public bool moveVertexPoint = true;
    public List<Vector3[]> allTriangleList;
    private Mesh clonedMesh;
    public List<int>[] connectedVertices;
    private MeshFilter meshFilter;
    private Mesh originalMesh;
    private int[] triangles;


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

    private void Start() {
        InitMesh();
    }

    /// <summary>
    ///     Makes a copy of the Mesh, so that we dont edit or overwrite the unity built-in meshes.
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
    ///     Pulls only one vertex pt in the mesh, results in broken mesh.
    /// </summary>
    /// <param name="index">selected vertex index</param>
    /// <param name="newPos"> new position of the selected vertex</param>
    private void PullOneVertex(int index, Vector3 newPos) {
        vertices[index] = newPos;
        clonedMesh.vertices = vertices;
        clonedMesh.RecalculateNormals();
    }

    /// <summary>
    ///     Pulls all vertices that are at the same location as the vertex at index, does not break mesh.
    /// </summary>
    /// <param name="index">selected vertex index</param>
    /// <param name="newPos"> new position of the selected vertex</param>
    /// >
    private void PullSimilarVertices(int index, Vector3 newPos) {
        var targetVertexPos = vertices[index];
        var relatedVertices = FindRelatedVertices(targetVertexPos, false);
        foreach (var i in relatedVertices) vertices[i] = newPos;

        clonedMesh.vertices = vertices;
        clonedMesh.RecalculateNormals();
    }

    /// <summary>
    ///     Finds the vertices that are at the same location as the vertex at index.
    /// </summary>
    /// <param name="targetPt"></param>
    /// <param name="findConnected"></param>
    /// <returns></returns>
    private List<int> FindRelatedVertices(Vector3 targetPt, bool findConnected) {
        var relatedVertices = new List<int>();

        var idx = 0;
        Vector3 pos;

        // loop through triangle array of indices
        for (var t = 0; t < triangles.Length; t++) {
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
                    if (t == 0) relatedVertices.Add(triangles[t + 1]);

                    // max 
                    // - prevent runnign out of count
                    if (t == triangles.Length - 1) relatedVertices.Add(triangles[t - 1]);

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