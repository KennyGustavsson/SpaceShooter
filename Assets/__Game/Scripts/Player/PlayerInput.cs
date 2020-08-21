using UnityEngine;

namespace SS
{
    [RequireComponent(typeof(Player))]
    [RequireComponent(typeof(MovementController))]
    public class PlayerInput : MonoBehaviour
    {
        public InGameMenu menu;

        private MovementController _movement;
        private Player _player;

        private void Awake()
        {
            _movement = GetComponent<MovementController>();
            _player = GetComponent<Player>();
        }

        void Update()
        {
            _movement.vertical = Input.GetAxis("Vertical");
            _movement.rotation = Input.GetAxis("Horizontal");
            _movement.booster = Input.GetKey(KeyCode.Space);

            _player.primaryFire = Input.GetKey(KeyCode.Mouse0);
            _player.secondaryFire = Input.GetKey(KeyCode.Mouse1);

            if (Input.GetKeyDown(KeyCode.Escape)) menu.Pause();
        }
    }
}
