﻿using System.Collections;
using UnityEngine;

namespace SS {
    [RequireComponent(typeof(Rigidbody2D))]
    public class HoomingMissle : MonoBehaviour
    {
        public int damage = 50;
        public float damageRadius = 3f;
        public float hoomingRadius = 20f;
        public float rocketSpeed = 5f;
        public float rocketAcceleration = 20f;
        public float lifeSpan = 5f;

        private Rigidbody2D _rb;
        private Transform _target;
        private bool _exploded;
        private float _distance;
        private float forwardSpeed;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            _exploded = false;
            StopCoroutine(Timer());

            Collider2D[] hitColliders;
            hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), hoomingRadius);

            _distance = (transform.position - hitColliders[0].transform.position).sqrMagnitude;

            for(int i = 0; i < hitColliders.Length; i++)
            {
                if ((transform.position - hitColliders[i].transform.position).sqrMagnitude > _distance)
                {
                    if(hitColliders[i].gameObject.layer == 9)
                    {
                        _distance = (hitColliders[i].transform.position - transform.position).sqrMagnitude;
                        _target = hitColliders[i].transform;
                    }                    
                }
            }

            StartCoroutine(Timer());
        }

        private void FixedUpdate()
        {
            if (_exploded) return;
            if(_target != null) FaceTarget();

             forwardSpeed = Mathf.MoveTowards(forwardSpeed, rocketSpeed, rocketAcceleration * Time.fixedDeltaTime);
            _rb.AddForce(transform.up * forwardSpeed);
        }

        private void FaceTarget()
        {   
            float posX = _target.position.x - transform.position.x;
            float posY = _target.position.y - transform.position.y;

            float angle = Mathf.Atan2(posX, posY) * Mathf.Rad2Deg;

            transform.localEulerAngles =  new Vector3(0, 0, -angle);                              
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == 8 || collision.gameObject.layer == 10) return;
            Explode();
        }

        private IEnumerator Timer()
        {
            yield return new WaitForSeconds(lifeSpan);
            if(!_exploded) Explode();
        }

        public void Explode()
        {
            _exploded = true;

            // Area of effect Damage
            Collider2D[] hitColliders;
            hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), damageRadius);
           
            for (int i = 0; i < hitColliders.Length; i++)
            {
                // Enemy Layer
                if (hitColliders[i].gameObject.layer == 9)
                {
                    hitColliders[i].GetComponent<Health>().HealthChange(-damage);
                }
            }

            // Explosion animation
            var obj = ObjectPool.ObjPool.GetPooledObject(20);
            obj.SetActive(false);
            obj.transform.position = transform.position + transform.up;
            obj.transform.rotation = transform.rotation;
            obj.SetActive(true);

            SoundManager.Instance.PlayAudioAtLocation(3, transform.position);

            gameObject.SetActive(false);
        }
    }
}
