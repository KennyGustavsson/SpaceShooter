using UnityEngine;

namespace SS {
    public class Projectile : MonoBehaviour
    {
        public int damage = 20;
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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == 8 || collision.gameObject.layer == 10) return;

            if (collision.gameObject.layer == 9)
            {
                collision.transform.GetComponent<Health>().HealthChange(-damage);
            }

            gameObject.SetActive(false);
        }
    }
}
