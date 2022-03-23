using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HeartMesh))]
public class HeartMeshInspector : Editor {
    private HeartMesh mesh;
    private Transform handleTransform;
    private Quaternion handleRotation;

    void OnSceneGUI() {
        mesh = target as HeartMesh;
        handleTransform = mesh.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;

        // Show Handles on Mesh
        if (mesh.isEditMode) {
            if (mesh.originalVertices == null || mesh.normals.Length == 0) {
                mesh.Init();
            }

            for (int i = 0; i < mesh.originalVertices.Length; i++) {
                ShowHandle(i);
            }
        }

        // Show/Hide Transform Tool
        Tools.current = mesh.showTransformHandle ? Tool.Move : Tool.None;
    }

    /// <summary>
    /// Creating a custom handle for each vertex of the mesh to allow the user to manipulate the mesh in real time.
    /// </summary>
    void ShowHandle(int index) {
        Vector3 point = handleTransform.TransformPoint(mesh.originalVertices[index]);

        // Show the handle only if the vertex is not selected
        if (mesh.selectedIndices.Contains(index)) return;
        Handles.color = Color.blue;
        if (Handles.Button(point, handleRotation, mesh.pickSize, mesh.pickSize, Handles.DotHandleCap)) {
            mesh.selectedIndices.Add(index);
        }
    }

    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        mesh = target as HeartMesh;

        if (mesh.isEditMode || mesh.isMeshReady) {
            if (GUILayout.Button("Show Normals")) {
                Vector3[] verts = mesh.modifiedVertices.Length == 0 ? mesh.originalVertices : mesh.modifiedVertices;
                Vector3[] normals = mesh.normals;
                Debug.Log(normals.Length);
                for (int i = 0; i < verts.Length; i++) {
                    Debug.DrawLine(handleTransform.TransformPoint(verts[i]), handleTransform.TransformPoint(normals[i]), Color.green, 4.0f, true);
                }
            }
        }

        // This adds a custom Reset button in the Inspector
        if (GUILayout.Button("Clear Selected Vertices")) {
            mesh.ClearAllData();
        }

        if (!mesh.isEditMode && mesh.isMeshReady) {
            string path = "Assets/RW/Prefabs/CustomHeart.prefab";

            if (GUILayout.Button("Save Mesh")) { // Save the mesh as a prefab
                mesh.isMeshReady = false;
                Object prefabToInstantiate = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
                Object referencePrefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
                GameObject gameObj = (GameObject) PrefabUtility.InstantiatePrefab(prefabToInstantiate);
                Mesh prefabMesh = (Mesh) AssetDatabase.LoadAssetAtPath(path, typeof(Mesh));
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