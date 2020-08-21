using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SS
{
    [RequireComponent(typeof(Player))]
    [RequireComponent(typeof(MovementController))]
    public class PlayerInput : MonoBehaviour
    {
        public InGameMenu menu;

        private PlayerController _controller;
        private MovementController _movement;
        private Player _player;

        private void Awake()
        {
            _controller = new PlayerController();

            _movement = GetComponent<MovementController>();
            _player = GetComponent<Player>();

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
            _controller.Gameplay.Fire2.performed += StopFire2;

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
        }

        private void OnDisable()
        {
            // Disale
            _controller.Gameplay.Boost.Disable();
            _controller.Gameplay.Vertical.Disable();
            _controller.Gameplay.Horizontal.Disable();
            _controller.Gameplay.Fire1.Disable();
            _controller.Gameplay.Fire2.Disable();
        }

        // Booster
        private void StartBoost(InputAction.CallbackContext context)
        {
            Debug.Log("Booster");
            _movement.booster = true;
        }

        private void StopBoost(InputAction.CallbackContext context)
        {
            Debug.Log("BoosterStop");
            _movement.booster = false;
        }

        // Fire 1
        private void StartFire1(InputAction.CallbackContext context)
        {
            Debug.Log("Prifire");
            _player.primaryFire = true;
        }

        private void StopFire1(InputAction.CallbackContext context)
        {
            Debug.Log("PriFireStop");
            _player.primaryFire = false;
        }

        // Fire 2
        private void StartFire2(InputAction.CallbackContext context)
        {
            Debug.Log("SecFire");
            _player.secondaryFire = true;
        }

        private void StopFire2(InputAction.CallbackContext context)
        {
            Debug.Log("SecFireStop");
            _player.secondaryFire = false;
        }

        // Pause
        private void PauseHandle(InputAction.CallbackContext context)
        {
            Debug.Log("Pause");
            menu.Pause();
        }
    }
}
