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
using UnityEngine;

/// <summary>
///     When any GameObject with the Mesh Study component attached to it is visible in the Scene view, this class will handle drawing it
/// </summary>
[CustomEditor(typeof(MeshStudy))]
public class MeshInspector : Editor {
    private Quaternion handleRotation;
    private Transform handleTransform;
    private MeshStudy mesh;
    private string triangleIdx;

    /// <summary>
    ///     OnSceneGUI is an event method that Unity calls every time it renders the Scene view in the editor
    /// </summary>
    private void OnSceneGUI() {
        mesh = target as MeshStudy;
        EditMesh();
    }

    /// <summary>
    ///     Creating a custom handle for each vertex of the mesh to allow the user to manipulate the mesh in real time.
    /// </summary>
    private void EditMesh() {
        handleTransform = mesh.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;
        for (var i = 0; i < mesh.vertices.Length; i++) ShowPoint(i);
    }

    private void ShowPoint(int index) {
        if (mesh.moveVertexPoint) {
            //draw dot
            var point = handleTransform.TransformPoint(mesh.vertices[index]);
            Handles.color = Color.blue;
            point = Handles.FreeMoveHandle(point, handleRotation, mesh.handleSize, Vector3.zero, Handles.DotHandleCap);

            //drag
            if (GUI.changed) mesh.DoAction(index, handleTransform.InverseTransformPoint(point));
        }
    }


    /// <summary>
    ///     OnInspectorGUI lets you customize the Inspector for your object with extra GUI elements and logic
    /// </summary>
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        mesh = target as MeshStudy;

        //draw reset button
        if (GUILayout.Button("Reset")) mesh.Reset();

        // For testing Reset function
        if (mesh.isCloned)
            if (GUILayout.Button("Test Edit"))
                mesh.EditMesh();
    }
}