using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GenerateMeshForWarping : MonoBehaviour {
    public float minDistance;
    public Transform trackedObject;

    public MeshFilter filter;

    public void Remesh() {
        filter.mesh = GenerateMeshWithHoles();
    }

    Mesh mesh;
    Vector3[] vertices;
    Vector3[] normals;
    Vector2[] uvs;
    int[] triangles;
    bool[] trianglesDisabled;
    List<int>[] trisWithVertex;

    Vector3[] origvertices;
    Vector3[] orignormals;
    Vector2[] origuvs;
    int[] origtriangles;

    void Start() {
        mesh = new Mesh();
        filter = GetComponent<MeshFilter>();
        orignormals = filter.mesh.normals;
        origvertices = filter.mesh.vertices;
        origuvs = filter.mesh.uv;
        origtriangles = filter.mesh.triangles;

        vertices = new Vector3[origvertices.Length];
        normals = new Vector3[orignormals.Length];
        uvs = new Vector2[origuvs.Length];
        triangles = new int[origtriangles.Length];
        trianglesDisabled = new bool[origtriangles.Length];

        orignormals.CopyTo(normals, 0);
        origvertices.CopyTo(vertices, 0);
        origtriangles.CopyTo(triangles, 0);
        origuvs.CopyTo(uvs, 0);

        trisWithVertex = new List<int>[origvertices.Length];
        for (int i = 0; i < origvertices.Length; ++i) {
            trisWithVertex[i] = origtriangles.IndexOf(i);
        }

        filter.mesh = GenerateMeshWithHoles();
    }

    Mesh GenerateMeshWithHoles() {
        Vector3 trackPos = trackedObject.position;
        for (int i = 0; i < origvertices.Length; ++i) {
            Vector3 v = new Vector3(origvertices[i].x * transform.localScale.x, origvertices[i].y * transform.localScale.y, origvertices[i].z * transform.localScale.z);
            if ((v + transform.position - trackPos).magnitude < minDistance) {
                for (int j = 0; j < trisWithVertex[i].Count; ++j) {
                    int value = trisWithVertex[i][j];
                    int remainder = value % 3;
                    trianglesDisabled[value - remainder] = true;
                    trianglesDisabled[value - remainder + 1] = true;
                    trianglesDisabled[value - remainder + 2] = true;
                }
            }
        }

        triangles = origtriangles;
        triangles = triangles.RemoveAllSpecifiedIndicesFromArray(trianglesDisabled).ToArray();

        mesh.SetVertices(vertices.ToList<Vector3>());
        mesh.SetNormals(normals.ToList());
        mesh.SetUVs(0, uvs.ToList());
        mesh.SetTriangles(triangles, 0);
        for (int i = 0; i < trianglesDisabled.Length; ++i)
            trianglesDisabled[i] = false;
        return mesh;
    }

    Mesh GenerateMeshWithFakeHoles() {
        Vector3 trackPos = trackedObject.position;
        for (int i = 0; i < origvertices.Length; ++i) {
            if ((origvertices[i] + transform.position - trackPos).magnitude < minDistance) {
                normals[i] = -orignormals[i];
            }
            else {
                normals[i] = orignormals[i];
            }
        }

        mesh.SetVertices(vertices.ToList<Vector3>());
        mesh.SetNormals(normals.ToList());
        mesh.SetUVs(0, uvs.ToList());
        mesh.SetTriangles(triangles, 0);
        return mesh;
    }

    void Update() {
        Remesh();
    }
}