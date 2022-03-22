using System;
using JetBrains.Annotations;
using UnityEngine;
using static UnityEngine.TouchPhase;

namespace FireFighter3D {
    public class FF_InputController : MonoBehaviour {
        [CanBeNull] public event Action<Vector3?> touchDownEvent;
        
        public event Action touchUpEvent;

        private void Start() {
            TouchInput.touchDownEvent += OnTouchDown;
            TouchInput.touchUpEvent += OnTouchUp;
        }
        
        private void Update() {
            TouchInput.CheckInput();
        }

        private void OnTouchUp() {
            touchUpEvent?.Invoke();
        }

        private void OnTouchDown(Vector3? obj) {
            touchDownEvent?.Invoke(obj);
        }
    }
}