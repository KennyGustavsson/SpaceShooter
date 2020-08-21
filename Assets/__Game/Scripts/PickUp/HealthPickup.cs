using UnityEngine;

namespace SS
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class HealthPickup : MonoBehaviour
    {
        public int health;

        private void Awake()
        {
            GetComponent<CircleCollider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == 8) 
            {
                collision.GetComponent<Health>().HealthChange(health);
                SoundManager.Instance.PlayAudioAtLocation(4, transform.position);
                Destroy(gameObject);
            }
        }
    }
}
