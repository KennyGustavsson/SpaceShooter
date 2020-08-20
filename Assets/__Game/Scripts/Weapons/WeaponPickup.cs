using UnityEngine;

namespace SS
{
    public class WeaponPickup : MonoBehaviour
    {
        public enum weapon
        {
            Projectile,
            HoomingProjectile,
            Raygun
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
                        collision.transform.GetComponent<Player>().primaryID = (int)pickWeapon;
                        break;
                    case 1:
                        collision.transform.GetComponent<Player>().secondaryID = (int)pickWeapon;
                        break;
                }
                Destroy(gameObject);
            }
        }
    }
}
