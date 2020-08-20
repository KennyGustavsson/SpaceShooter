using System;
using UnityEngine;

namespace SS {
    public class MovementController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _speed = 30;
        [SerializeField] private float _acceleration = 5;
        [SerializeField] private float _rotationSpeed = 75;
        [SerializeField] private float _boosterSpeed = 50;
        [Tooltip("Drag when deaccelerating")] [SerializeField] private float _drag = 1;

        [NonSerialized] public float vertical;
        [NonSerialized] public float rotation;
        [NonSerialized] public bool booster;
        [NonSerialized] public bool isBoosting;

        public Rigidbody2D rb;
        private float _initialDrag;
        private float _forwardSpeed;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            _initialDrag = rb.drag;
        }

        private void FixedUpdate()
        {
            //Boost
            if (Input.GetKey(KeyCode.Space) && vertical > 0)
            {
                isBoosting = true;
                transform.position += transform.up * _boosterSpeed * Time.fixedDeltaTime;
            }
            else isBoosting = false;

            //Rotate
            if (rotation != 0) Rotate();

            //Moveforward
            if (vertical > 0) MoveForward();
            else rb.drag = _drag;
        }

        private void MoveForward()
        {
            rb.drag = _initialDrag;
            _forwardSpeed = Mathf.MoveTowards(_forwardSpeed, _speed, _acceleration * Time.fixedDeltaTime);
            rb.AddForce(transform.up * _forwardSpeed);
        }

        private void Rotate()
        {
            rb.rotation += -rotation * _rotationSpeed * Time.fixedDeltaTime;
        }
    }
}
