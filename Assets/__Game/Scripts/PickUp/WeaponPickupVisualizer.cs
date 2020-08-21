using UnityEngine;

namespace SS
{
    public class WeaponPickupVisualizer : MonoBehaviour
    {
        public Sprite[] sprites;
        public float rotationSpeed = 5f;

        private void Awake()
        {
            switch ((int)GetComponentInParent<WeaponPickup>().pickWeapon)
            {
                case 0:
                    GetComponent<SpriteRenderer>().sprite = sprites[0];
                    transform.localScale = new Vector3(1, 3, 1);
                    break;
                case 1:
                    GetComponent<SpriteRenderer>().sprite = sprites[1];
                    transform.localScale = new Vector3(0.3f, 0.3f, 1);
                    break;
                case 2:
                    GetComponent<SpriteRenderer>().sprite = sprites[2];
                    transform.localScale = new Vector3(1, 1, 1);
                    break;
                case 3:
                    GetComponent<SpriteRenderer>().sprite = sprites[3];
                    transform.localScale = new Vector3(0.3f, 0.3f, 1);
                    break;
            }
        }

        private void FixedUpdate()
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.fixedDeltaTime);
        }
    }
}
