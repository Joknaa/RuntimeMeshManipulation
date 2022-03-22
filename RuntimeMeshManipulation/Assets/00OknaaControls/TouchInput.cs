using System;
using JetBrains.Annotations;
using UnityEngine;
using static UnityEngine.TouchPhase;

public static class TouchInput {
    [CanBeNull] public static event Action<Vector3?> touchDownEvent;
    public static event Action touchUpEvent;

    public static void CheckInput() {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);
        if (isTouchingScreen(touch)) {
            touchDownEvent?.Invoke(touch.position);
        }
        else {
            touchUpEvent?.Invoke();
        }
    }


    private static bool isTouchingScreen(Touch touch) {
        return touch.phase == Began || touch.phase == Moved || touch.phase == Stationary;
    }
}