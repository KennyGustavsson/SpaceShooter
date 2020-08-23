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

        [NonSerialized]public Rigidbody2D rb;

        private Animator _animator;
        private float _initialDrag;
        private float _forwardSpeed;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            _animator = transform.GetChild(0).GetComponent<Animator>();
            _initialDrag = rb.drag;
        }

        private void FixedUpdate()
        {
            // Boost
            if (vertical > 0 && isBoosting)
            {
                transform.position += transform.up * _boosterSpeed * Time.fixedDeltaTime;
            }

            // Rotate
            if (rotation != 0) Rotate();

            // Moveforward
            if (vertical > 0) MoveForward();
            else rb.drag = _drag;

            // Animation
            if (rotation < 0)
            {
                _animator.SetBool("TurnRight", true);
                _animator.SetBool("TurnLeft", false);
            }
            else if (rotation > 0)
            {
                _animator.SetBool("TurnRight", false);
                _animator.SetBool("TurnLeft", true);
            }
            else
            {
                _animator.SetBool("TurnRight", false);
                _animator.SetBool("TurnLeft", false);
            }
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