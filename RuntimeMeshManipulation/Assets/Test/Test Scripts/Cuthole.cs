using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Cuthole : MonoBehaviour {
    public float radius;
    private MeshFilter meshFilter;
    private Camera _camera;

    // Start is called before the first frame update
    void Start() {
        _camera = Camera.main;
        meshFilter = GetComponent<MeshFilter>();
    }

    void deleteTri(int index) {
        Destroy(this.gameObject.GetComponent<MeshCollider>());
        Mesh mesh = transform.GetComponent<MeshFilter>().mesh;
        int[] oldTriangles = mesh.triangles;
        int[] newTriangles = new int[mesh.triangles.Length - 3];

        int i = 0;
        int j = 0;

        while (j < mesh.triangles.Length) {
            if (j != index * 3) {
                newTriangles[i++] = oldTriangles[j++];
                newTriangles[i++] = oldTriangles[j++];
                newTriangles[i++] = oldTriangles[j++];
            }
            else {
                j += 3;
            }
        }

        meshFilter.mesh.triangles = newTriangles;
        this.gameObject.AddComponent<MeshCollider>();
    }

    void Update() {
        if (!Input.GetMouseButton(0)) return;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit)) {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Erasable")) {
                Vector3 top = new Vector3(hit.point.x, hit.point.y + radius, hit.point.z);
                Vector3 bottom = new Vector3(hit.point.x, hit.point.y - radius, hit.point.z);
                Vector3 left = new Vector3(hit.point.x - radius, hit.point.y, hit.point.z);
                Vector3 right = new Vector3(hit.point.x + radius, hit.point.y, hit.point.z);
                Vector3 topLeft = new Vector3(hit.point.x - radius, hit.point.y + radius, hit.point.z);
                Vector3 topRight = new Vector3(hit.point.x + radius, hit.point.y + radius, hit.point.z);
                Vector3 bottomLeft = new Vector3(hit.point.x - radius, hit.point.y - radius, hit.point.z);
                Vector3 bottomRight = new Vector3(hit.point.x + radius, hit.point.y - radius, hit.point.z);
                    
                Debug.DrawLine(hit.point, top, Color.red);
                Debug.DrawLine(hit.point, bottom, Color.red);
                Debug.DrawLine(hit.point, left, Color.red);
                Debug.DrawLine(hit.point, right, Color.red);
                Debug.DrawLine(hit.point, topLeft, Color.red);
                Debug.DrawLine(hit.point, topRight, Color.red);
                Debug.DrawLine(hit.point, bottomLeft, Color.red);
                Debug.DrawLine(hit.point, bottomRight, Color.red);
                    
                DeleteAdjacentTriangles(top);
                DeleteAdjacentTriangles(bottom);
                DeleteAdjacentTriangles(left);
                DeleteAdjacentTriangles(right);
                DeleteAdjacentTriangles(topLeft);
                DeleteAdjacentTriangles(topRight);
                DeleteAdjacentTriangles(bottomLeft);
                DeleteAdjacentTriangles(bottomRight);

                deleteTri(hit.triangleIndex);
            }
        }
        /*
            if (Physics.Raycast(ray, out hit, 1000.0f)) {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Erasabe")) {
                    deleteTri(hit.triangleIndex);
                }
            }*/
    }

    private void DeleteAdjacentTriangles(Vector3 position) {
        Ray ray = _camera.ScreenPointToRay(position);
        if (!Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit)) return;
        if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Erasable")) {
            deleteTri(hit.triangleIndex);
        }
    }

    private void OnCollisionStay(Collision collision) {
        collision.transform.position += Mathf.Epsilon * Vector3.up;
    }
}