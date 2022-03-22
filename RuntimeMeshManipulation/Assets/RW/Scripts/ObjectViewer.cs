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

using UnityEngine;

public class ObjectViewer : MonoBehaviour {
    public Camera mainCam;
    public Transform target;
    public bool isReadyForTransform;

    // rotate
    public float distance = 10f;
    public float xspeed = 250f;
    public float yspeed = 120f;
    public float xsign = 1f;
    private Vector3 prevPos;
    private float x;
    private float y;

    private void LateUpdate() {
        if (!isReadyForTransform) return;

        // Rotation
        var forward = mainCam.transform.TransformDirection(Vector3.up); // camera's transform
        var forward2 = target.transform.TransformDirection(Vector3.up); // target's transform

        if (Vector3.Dot(forward, forward2) < 0)
            xsign = -1;
        else
            xsign = 1;

        if (Input.GetMouseButton(0)) {
            if (prevPos != Vector3.zero && Input.mousePosition != prevPos) {
                x += xsign * (Input.mousePosition.x - prevPos.x) * xspeed * 0.02f;
                y -= (Input.mousePosition.y - prevPos.y) * yspeed * 0.02f;
                DoRotation(x, y);
            }

            prevPos = Input.mousePosition;
        }
        else {
            prevPos = Vector3.zero;
        }
    }

    // Use this for initialization
    public void Init() {
        // get distance
        distance = Vector3.Distance(mainCam.transform.position, target.transform.position);
        isReadyForTransform = true;
        Input.simulateMouseWithTouches = true;
    }

    private void DoRotation(float x, float y) {
        var rotation = Quaternion.Euler(y, x, 0);
        var position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.transform.position;
        mainCam.transform.rotation = rotation;
        mainCam.transform.position = position;
    }
}