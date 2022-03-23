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

public class Curve {
    public bool drawCurve;
    public Vector3[] points = new Vector3[3];

    public Curve(Vector3 p0, Vector3 p1, Vector3 p2, bool tdraw) {
        drawCurve = tdraw;
        points = new[] {p0, p1, p2};

        // Draw curve
        if (drawCurve) {
            var steps = 10;
            var start = GetPoint(0f);
            for (var i = 0; i <= steps; i++) {
                var t = i / (float) steps;
                var end = GetPoint(t);
                Debug.DrawLine(start, end, Color.cyan, 10, true);
                start = end;
            }
        }
    }

    public Vector3 GetPoint(float t) {
        t = Mathf.Clamp01(t);
        var oneMinusT = 1f - t;
        return oneMinusT * oneMinusT * points[0] + 2f * oneMinusT * t * points[1]
                                                 + oneMinusT * oneMinusT * points[2];
    }
}