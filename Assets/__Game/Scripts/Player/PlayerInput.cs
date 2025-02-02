﻿using UnityEngine;
using UnityEngine.InputSystem;

namespace SS
{
    [RequireComponent(typeof(PlayerWeapn))]
    [RequireComponent(typeof(MovementController))]
    public class PlayerInput : MonoBehaviour
    {
        public InGameMenu menu;

        private PlayerController _controller;
        private MovementController _movement;
        private PlayerWeapn _player;

        private void Awake()
        {
            _controller = new PlayerController();

            _movement = GetComponent<MovementController>();
            _player = GetComponent<PlayerWeapn>();

            // Movement
            _controller.Gameplay.Vertical.performed += ctx => _movement.vertical = ctx.ReadValue<float>();
            _controller.Gameplay.Vertical.canceled += ctx => _movement.vertical = 0;

            _controller.Gameplay.Horizontal.performed += ctx => _movement.rotation = ctx.ReadValue<float>();
            _controller.Gameplay.Horizontal.canceled += ctx => _movement.rotation = 0;

            // Booster
            _controller.Gameplay.Boost.performed += StartBoost;
            _controller.Gameplay.Boost.canceled += StopBoost;

            //Fire
            _controller.Gameplay.Fire1.performed += StartFire1;
            _controller.Gameplay.Fire1.canceled += StopFire1;

            _controller.Gameplay.Fire2.performed += StartFire2;
            _controller.Gameplay.Fire2.canceled += StopFire2;

            // Pause
            _controller.Gameplay.Pause.performed += PauseHandle;
        }

        private void OnEnable()
        {
            // Enable
            _controller.Gameplay.Boost.Enable();
            _controller.Gameplay.Vertical.Enable();
            _controller.Gameplay.Horizontal.Enable();
            _controller.Gameplay.Fire1.Enable();
            _controller.Gameplay.Fire2.Enable();
            _controller.Gameplay.Pause.Enable();
        }

        private void OnDisable()
        {
            // Disale
            _controller.Gameplay.Boost.Disable();
            _controller.Gameplay.Vertical.Disable();
            _controller.Gameplay.Horizontal.Disable();
            _controller.Gameplay.Fire1.Disable();
            _controller.Gameplay.Fire2.Disable();
            _controller.Gameplay.Pause.Disable();
        }

        // Booster
        private void StartBoost(InputAction.CallbackContext context)
        {
            _movement.isBoosting = true;
        }

        private void StopBoost(InputAction.CallbackContext context)
        {
            _movement.isBoosting = false;
        }

        // Fire 1
        private void StartFire1(InputAction.CallbackContext context)
        {
            _player.primaryFire = true;
        }

        private void StopFire1(InputAction.CallbackContext context)
        {
            _player.primaryFire = false;
        }

        // Fire 2
        private void StartFire2(InputAction.CallbackContext context)
        {
            _player.secondaryFire = true;
        }

        private void StopFire2(InputAction.CallbackContext context)
        {
            _player.secondaryFire = false;
        }

        // Pause
        private void PauseHandle(InputAction.CallbackContext context)
        {
            menu.Pause();
        }
    }
}