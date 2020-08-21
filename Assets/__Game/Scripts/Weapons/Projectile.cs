using UnityEngine;

namespace SS {
    public class Projectile : MonoBehaviour
    {
        public int damage = 10;
        private bool initialized;

        private void OnEnable()
        {
            if (!initialized) return;
            SoundManager.Instance.PlayAudioAtLocation(0, transform.position);
        }

        private void OnDisable()
        {
            initialized = true;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == 8) return;

            if (collision.gameObject.layer == 9)
            {
                // Damage
            }

            gameObject.SetActive(false);
        }
    }
}
