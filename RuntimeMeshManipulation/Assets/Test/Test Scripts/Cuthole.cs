using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Cuthole : MonoBehaviour {
    public float radius;
    private MeshFilter meshFilter;
    
    // Start is called before the first frame update
    void Start() {
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
        if (Input.GetMouseButton(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.SphereCast(ray, radius, out hit)) {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Erasable")) {

                    Vector3 impactPoint = hit.point;
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
    }

    private void OnCollisionStay(Collision collision) {
        collision.transform.position += Mathf.Epsilon * Vector3.up;
    }
}