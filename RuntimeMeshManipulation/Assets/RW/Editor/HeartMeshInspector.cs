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

using UnityEditor;
<<<<<<< HEAD
using UnityEngine;

[CustomEditor(typeof(HeartMesh))]
public class HeartMeshInspector : Editor {
=======
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(HeartMesh))]
public class HeartMeshInspector : Editor
{
    private HeartMesh mesh;
    private Transform handleTransform;
>>>>>>> parent of 0858a8d (Added Mesh handlers, and ability to control the effect force, radius, and animation duration)
    private Quaternion handleRotation;
    private Transform handleTransform;
    private HeartMesh mesh;

<<<<<<< HEAD
    private void OnSceneGUI() {
=======
    void OnSceneGUI()
    {
>>>>>>> parent of 0858a8d (Added Mesh handlers, and ability to control the effect force, radius, and animation duration)
        mesh = target as HeartMesh;
        handleTransform = mesh.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;

<<<<<<< HEAD
        // Show Handles on Mesh
        if (mesh.isEditMode) {
            if (mesh.originalVertices == null || mesh.normals.Length == 0) mesh.Init();

            for (var i = 0; i < mesh.originalVertices.Length; i++) ShowHandle(i);
=======
        // ShowHandles on Mesh
        if (mesh.isEditMode)
        {
            if (mesh.originalVertices == null || mesh.normals.Length == 0)
            {
                mesh.Init();
            }
            for (int i = 0; i < mesh.originalVertices.Length; i++)
            {
                ShowHandle(i);
            }
>>>>>>> parent of 0858a8d (Added Mesh handlers, and ability to control the effect force, radius, and animation duration)
        }

        // Show/ Hide Transform Tool
        if (mesh.showTransformHandle)
        {
            Tools.current = Tool.Move;
        }
        else
        {
            Tools.current = Tool.None;
        }
    }

<<<<<<< HEAD
    /// <summary>
    ///     Creating a custom handle for each vertex of the mesh to allow the user to manipulate the mesh in real time.
    /// </summary>
    private void ShowHandle(int index) {
        var point = handleTransform.TransformPoint(mesh.originalVertices[index]);

        // Show the handle only if the vertex is not selected
        if (mesh.selectedIndices.Contains(index)) return;
        Handles.color = Color.blue;
        if (Handles.Button(point, handleRotation, mesh.pickSize, mesh.pickSize, Handles.DotHandleCap)) mesh.selectedIndices.Add(index);
=======
    void ShowHandle(int index)
    {
        Vector3 point = handleTransform.TransformPoint(mesh.originalVertices[index]);

        // unselected vertex
        if (!mesh.selectedIndices.Contains(index))
        {
        }
>>>>>>> parent of 0858a8d (Added Mesh handlers, and ability to control the effect force, radius, and animation duration)
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        mesh = target as HeartMesh;

        if (mesh.isEditMode || mesh.isMeshReady)
<<<<<<< HEAD
            if (GUILayout.Button("Show Normals")) {
                var verts = mesh.modifiedVertices.Length == 0 ? mesh.originalVertices : mesh.modifiedVertices;
                var normals = mesh.normals;
                Debug.Log(normals.Length);
                for (var i = 0; i < verts.Length; i++)
=======
        {
            if (GUILayout.Button("Show Normals"))
            {
                Vector3[] verts = mesh.modifiedVertices.Length == 0 ? mesh.originalVertices : mesh.modifiedVertices;
                Vector3[] normals = mesh.normals; Debug.Log(normals.Length);
                for (int i = 0; i < verts.Length; i++)
                {
>>>>>>> parent of 0858a8d (Added Mesh handlers, and ability to control the effect force, radius, and animation duration)
                    Debug.DrawLine(handleTransform.TransformPoint(verts[i]), handleTransform.TransformPoint(normals[i]), Color.green, 4.0f, true);
            }
<<<<<<< HEAD

        // This adds a custom Reset button in the Inspector
        if (GUILayout.Button("Clear Selected Vertices")) mesh.ClearAllData();

        if (!mesh.isEditMode && mesh.isMeshReady) {
            var path = "Assets/RW/Prefabs/CustomHeart.prefab";

            if (GUILayout.Button("Save Mesh")) { // Save the mesh as a prefab
                mesh.isMeshReady = false;
                var prefabToInstantiate = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
                var referencePrefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
                var gameObj = (GameObject) PrefabUtility.InstantiatePrefab(prefabToInstantiate);
                var prefabMesh = (Mesh) AssetDatabase.LoadAssetAtPath(path, typeof(Mesh));
                if (!prefabMesh) {
                    prefabMesh = new Mesh();
                    AssetDatabase.AddObjectToAsset(prefabMesh, path);
                }
                else {
                    prefabMesh.Clear();
                }

                prefabMesh = mesh.SaveMesh();
                AssetDatabase.AddObjectToAsset(prefabMesh, path);
                gameObj.GetComponentInChildren<MeshFilter>().mesh = prefabMesh;
                PrefabUtility.SaveAsPrefabAsset(gameObj, path);
                DestroyImmediate(gameObj);
            }
        }
    }
}
=======
        }
    }


}
>>>>>>> parent of 0858a8d (Added Mesh handlers, and ability to control the effect force, radius, and animation duration)
