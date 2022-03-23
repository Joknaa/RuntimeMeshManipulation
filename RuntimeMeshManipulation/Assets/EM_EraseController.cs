using FireFighter3D;
using UnityEngine;

namespace EscapeMasters {
    public class EM_EraseController : MonoBehaviour {
        private Camera _camera;
        private FF_InputController inputController;

        private void Start() {
            _camera = Camera.main;
            inputController = GameObject.FindGameObjectWithTag("GameController").GetComponent<FF_InputController>();
            inputController.touchDownEvent += OnTouchDown;
            inputController.touchUpEvent += OnTouchUp;
        }

        private void OnTouchUp() { }

        private void OnTouchDown(Vector3? touchPosition) {
            var ray = _camera.ScreenPointToRay(touchPosition.Value);

            Debug.DrawRay(ray.origin, ray.direction * 3, Color.yellow);

            if (Physics.Raycast(ray, out var hit, 100))
                Debug.Log("Hit: " + hit.transform.name);
            else
                Debug.Log(hit.distance);

            /*if (touchPosition == null) return;
            var touchPosition2D = (Vector2) touchPosition;
            var ray = _camera.ScreenPointToRay(touchPosition2D);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit)) return;
            Debug.Log("Hit: " + hit.transform.name);*/
        }
    }
}