using UnityEngine;

namespace SS {
    public class Projectile : MonoBehaviour
    {
        public int damage = 10;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 0)
            {
                other.transform.GetComponent<Player>().HealthChange(-damage);
            }

            gameObject.SetActive(false);
        }
    }
}
