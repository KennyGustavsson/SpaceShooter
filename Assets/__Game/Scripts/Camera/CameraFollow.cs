using UnityEditor;
using UnityEngine;

namespace SS {
    public class CameraFollow : MonoBehaviour {
        [Header("Camera Follow")]
        public Transform target;
        public float dampTime = 0.41f;
        public Vector3 offset = new Vector3(0, 0.36f, 0);

        [Header("Camera Speed Effect")]
        public float sizeMultiplier = 1.3f;
        public float time = 10;

        private Vector3 _velocity = Vector3.zero;
        private Vector3 _cameraMove;

        private Camera _camera;
        private MovementController _movCtrl;
        private float _initialSize;

        private void Awake() {
            _movCtrl = target.GetComponent<MovementController>();
            _camera = GetComponent<Camera>();
            _initialSize = _camera.orthographicSize;
        }

        private void FixedUpdate() {
            _cameraMove = Vector3.SmoothDamp(transform.position, target.position, ref _velocity, dampTime);

            if (target.rotation.z > 0.5 || target.rotation.z < -0.5)
                transform.position = new Vector3(_cameraMove.x, _cameraMove.y, -10) + -offset;
            else
                transform.position = new Vector3(_cameraMove.x, _cameraMove.y, -10) + offset;

            if (_movCtrl != null && _movCtrl.isBoosting)
                _camera.orthographicSize = Mathf.MoveTowards(_camera.orthographicSize, _initialSize * sizeMultiplier, time * Time.fixedDeltaTime);
            else
                _camera.orthographicSize = Mathf.MoveTowards(_camera.orthographicSize, _initialSize, (time / 3) * Time.fixedDeltaTime);
        }
    }
}
