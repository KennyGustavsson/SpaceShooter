using UnityEngine;

namespace SS
{
    public class WeaponPickup : MonoBehaviour
    {
        public enum weapon
        {
            Projectile,
            HoomingProjectile,
            Raygun,
            Missle
        }
        public weapon pickWeapon;

        public enum weaponSlot
        {
            Primary,
            Secondary
        }
        public weaponSlot pickSlot;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == 8)
            {
                switch ((int)pickSlot)
                {
                    case 0:
                        collision.transform.GetComponent<PlayerWeapn>().primaryID = (int)pickWeapon;                        
                        break;
                    case 1:
                        collision.transform.GetComponent<PlayerWeapn>().secondaryID = (int)pickWeapon;
                        break;
                }
                SoundManager.Instance.PlayAudioAtLocation(4, transform.position);
                Destroy(gameObject);
            }
        }
    }
}
