using UnityEngine;
using System;
using System.Collections;

namespace SS
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(MovementController))]
    [RequireComponent(typeof(Health))]
    public class PlayerWeapn : MonoBehaviour
    {
        [Header("Current Fire Mode")]
        public float projectileSpeed = 5;
        public int primaryID = 0;
        public float primaryFireRate = 0.3f;
        public int secondaryID = 1;
        public float secondaryFireRate = 0.6f;

        [NonSerialized] public bool primaryFire;
        [NonSerialized] public bool secondaryFire;

        private bool fireCooldown;

        private void FixedUpdate()
        {
            if (secondaryFire && !fireCooldown) Fire2();
            if (primaryFire && !fireCooldown) Fire1();
        }

        private void Fire1()
        {
            var obj = ObjectPool.ObjPool.GetPooledObject(primaryID);
            obj.SetActive(false);
            obj.transform.position = transform.position + transform.up * 2;
            obj.transform.rotation = transform.rotation;
            obj.SetActive(true);
            obj.GetComponent<Rigidbody2D>().AddForce(transform.up * projectileSpeed, ForceMode2D.Impulse);
            StartCoroutine(PrimaryFireCoolDown());
        }

        private void Fire2()
        {
            var obj = ObjectPool.ObjPool.GetPooledObject(secondaryID);
            obj.SetActive(false);
            obj.transform.position = transform.position + transform.up * 2;
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
    }
}