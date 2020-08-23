using UnityEngine;

namespace SS
{
    public class WeaponPickup : MonoBehaviour
    {
        public enum Weapon
        {
            Projectile,
            HoomingMissle,
            Raygun,
            Missle
        }
        public Weapon pickWeapon;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == 8)
            {
                switch ((int)pickWeapon)
                {
                    case 0:
                        collision.transform.GetComponent<PlayerWeapn>().primaryID = (int)pickWeapon;                        
                        break;
                    case 1:
                        collision.transform.GetComponent<PlayerWeapn>().secondaryID = (int)pickWeapon;
                        break;
                    case 2:
                        collision.transform.GetComponent<PlayerWeapn>().primaryID = (int)pickWeapon;
                        break;
                    case 3:
                        collision.transform.GetComponent<PlayerWeapn>().secondaryID = (int)pickWeapon;
                        break;
                }
                SoundManager.Instance.PlayAudioAtLocation(4, transform.position);
                Destroy(gameObject);
            }
        }
    }
}