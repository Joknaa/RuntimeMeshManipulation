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

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeartMesh : MonoBehaviour {
    Mesh originalMesh;
    Mesh clonedMesh;
    MeshFilter meshFilter;
    MeshCollider meshCollider;

    [HideInInspector] public int targetIndex;
    [HideInInspector] public Vector3 targetVertex;
    [HideInInspector] public Vector3[] originalVertices;
    [HideInInspector] public Vector3[] modifiedVertices;
    [HideInInspector] public Vector3[] normals;
    [HideInInspector] public bool isMeshReady = false;
    
    public bool isEditMode = true;
    public bool showTransformHandle = true;
    public List<int> selectedIndices = new List<int>();
    public float pickSize = 0.01f;

    #region Moving a vertex should have some influence on the vertices around it to maintain a smooth shape. These variables control that effect

    /// <summary>
    /// Radius of area affected by the targeted vertex
    /// </summary>
    public float radiusOfEffect = 0.3f;

    /// <summary>
    /// The strength of the pull effect
    /// </summary>
    public float pullValue = 0.3f;

    /// <summary>
    /// How long the animation will run for
    /// </summary>
    public float duration = 1.2f;

    /// <summary>
    /// Current index of the selectedIndices list
    /// </summary>
    int currentIndex = 0;

    bool isAnimate = false;
    float startTime = 0f;
    float runTime = 0f;

    #endregion


    void Start() {
        Init();
    }

    /// <summary>
    /// Makes a copy of the Mesh, so that we dont edit or overwrite the unity built-in meshes.
    /// </summary>
    public void Init() {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        isMeshReady = false;
        currentIndex = 0;

        if (isEditMode) {
            originalMesh = meshFilter.sharedMesh;
            clonedMesh = new Mesh();
            clonedMesh.name = "clone";
            clonedMesh.vertices = originalMesh.vertices;
            clonedMesh.triangles = originalMesh.triangles;
            clonedMesh.normals = originalMesh.normals;
            meshFilter.mesh = clonedMesh;

            originalVertices = clonedMesh.vertices;
            normals = clonedMesh.normals;
            Debug.Log("Init & Cloned");
        }
        else {
            originalMesh = meshFilter.mesh;
            originalVertices = originalMesh.vertices;
            normals = originalMesh.normals;
            modifiedVertices = new Vector3[originalVertices.Length];
            for (int i = 0; i < originalVertices.Length; i++) {
                modifiedVertices[i] = originalVertices[i];
            }

            StartDisplacement();
        }
    }

    /// <summary>
    /// StartDisplacement is what actually moves the vertices. It only runs when isEditMode is false.
    /// </summary>
    public void StartDisplacement() {
        targetVertex = originalVertices[selectedIndices[currentIndex]];
        startTime = Time.time;
        isAnimate = true;
    }

    protected void FixedUpdate() {
        if (!isAnimate) return; // no animation ? no need to go further.

        runTime = Time.time - startTime; // time since animation started

        if (runTime < duration) { // if animation is still running, move the selected vertex.
            Vector3 targetVertexPos = meshFilter.transform.InverseTransformPoint(targetVertex);
            DisplaceVertices(targetVertexPos, pullValue, radiusOfEffect);
        }
        else { // if animation is done, move to the next vertex.
            currentIndex++;
            if (currentIndex < selectedIndices.Count) { // if there are still vertices to move, start the animation again.
                StartDisplacement();
            }
            else { // if there are no more vertices to move, stop the animation, and make a copy of the current mesh.
                originalMesh = GetComponent<MeshFilter>().mesh;
                isAnimate = false;
                isMeshReady = true;
            }
        }
    }

    void DisplaceVertices(Vector3 targetVertexPos, float force, float radius) {
        Vector3 currentVertexPos = Vector3.zero;
        float sqrRadius = radius * radius;

        for (int i = 0; i < modifiedVertices.Length; i++) { // Loop through all vertices in the mesh
            currentVertexPos = modifiedVertices[i];
            float sqrMagnitude = (currentVertexPos - targetVertexPos).sqrMagnitude; // get the distance between the current vertex and the targeted vertex (squared)
            if (sqrMagnitude > sqrRadius) continue; // If this vertex is outside the area of effect, do nothing and continues to the next vertex.
            
            float distance = Mathf.Sqrt(sqrMagnitude);
            float falloff = GaussFalloff(distance, radius); // Using this method, we can make the effect more or less smooth.
            Vector3 translate = (currentVertexPos * force) * falloff; // Displacement vector
            translate.z = 0f;
            Quaternion rotation = Quaternion.Euler(translate); // Displacement direction (This makes the vertex move "outward", making it seem to puff out from the center)
            Matrix4x4 m = Matrix4x4.TRS(translate, rotation, Vector3.one);
            modifiedVertices[i] = m.MultiplyPoint3x4(currentVertexPos);
        }

        originalMesh.vertices = modifiedVertices; // Apply the changes to the mesh
        originalMesh.RecalculateNormals();
        meshCollider.sharedMesh = originalMesh;
    }

    /// <summary>
    /// This clears the values selected by the user.
    /// </summary>
    public void ClearAllData() {
        selectedIndices = new List<int>();
        targetIndex = 0;
        targetVertex = Vector3.zero;
    }

    public Mesh SaveMesh() {
        Mesh nMesh = new Mesh();
        nMesh.name = "HeartMesh";
        nMesh.vertices = originalMesh.vertices;
        nMesh.triangles = originalMesh.triangles;
        nMesh.normals = originalMesh.normals;
        return nMesh;

    }

    #region HELPER FUNCTIONS

    static float LinearFalloff(float dist, float inRadius) {
        return Mathf.Clamp01(0.5f + (dist / inRadius) * 0.5f);
    }

    /// <summary>
    /// Gaussian functions create a smooth bell curve.
    /// </summary>
    /// <param name="dist"></param>
    /// <param name="inRadius"></param>
    /// <returns></returns>
    static float GaussFalloff(float dist, float inRadius) {
        return Mathf.Clamp01(Mathf.Pow(360, -Mathf.Pow(dist / inRadius, 2.5f) - 0.01f));
    }

    static float NeedleFalloff(float dist, float inRadius) {
        return -(dist * dist) / (inRadius * inRadius) + 1.0f;
    }

    #endregion
}