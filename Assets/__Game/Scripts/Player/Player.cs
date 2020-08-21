using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

namespace SS
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(MovementController))]
    public class Player : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] private int health = 100;

        [Header("Current Fire Mode")]
        public float projectileSpeed = 5;
        public int primaryID = 0;
        public float primaryFireRate = 0.3f;
        public int secondaryID = 1;
        public float secondaryFireRate = 0.6f;

        [NonSerialized] public bool primaryFire;
        [NonSerialized] public bool secondaryFire;

        [Header("UI")]
        public Text healthDisplay;


        private Rigidbody2D _rb;
        private bool fireCooldown;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            healthDisplay.text = $"Health {health}";
        }

        private void Update()
        {
            print(secondaryFire);
        }

        private void FixedUpdate()
        {
            if (secondaryFire && !fireCooldown) Fire2();
            if (primaryFire && !fireCooldown) Fire1();
        }

        private void Fire1()
        {
            var obj = ObjectPool.ObjPool.GetPooledObject(primaryID);
            obj.SetActive(false);
            obj.transform.position = transform.position + transform.up;
            obj.transform.rotation = transform.rotation;
            obj.SetActive(true);
            obj.GetComponent<Rigidbody2D>().AddForce(transform.up * projectileSpeed, ForceMode2D.Impulse);
            StartCoroutine(PrimaryFireCoolDown());
        }

        private void Fire2()
        {
            var obj = ObjectPool.ObjPool.GetPooledObject(secondaryID);
            obj.SetActive(false);
            obj.transform.position = transform.position + transform.up;
            obj.transform.rotation = transform.rotation;
            obj.SetActive(true);
            obj.GetComponent<Rigidbody2D>().AddForce(transform.up * projectileSpeed);
            StartCoroutine(SecondaryFireCoolDown());
        }

        IEnumerator PrimaryFireCoolDown()
        {
            fireCooldown = true;
            yield return new WaitForSeconds(primaryFireRate);
            fireCooldown = false;
        }

        IEnumerator SecondaryFireCoolDown()
        {
            fireCooldown = true;
            yield return new WaitForSeconds(secondaryFireRate);
            fireCooldown = false;
        }

        public void HealthChange(int value)
        {
            health += value;
            if (health <= 0)
            {
                // GameOver;
                health = 0;
                Debug.Log("GameOver");
            }
            else if (health > 100) health = 100;

            healthDisplay.text = $"Health {health}";
        }
    }
}
