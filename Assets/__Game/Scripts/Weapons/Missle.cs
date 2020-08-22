using System.Collections;
using UnityEngine;

namespace SS
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Missle : MonoBehaviour
    {
        public int damage = 100;
        public float damageRadius = 3f;
        public float rocketSpeed = 5f;
        public float rocketAcceleration = 20f;
        public float lifeSpan = 5f;

        private Rigidbody2D _rb;
        private bool _exploded;
        private float forwardSpeed;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            _exploded = false;
            StopCoroutine(Timer());
            StartCoroutine(Timer());
        }

        private void FixedUpdate()
        {
            if (_exploded) return;

            forwardSpeed = Mathf.MoveTowards(forwardSpeed, rocketSpeed, rocketAcceleration * Time.fixedDeltaTime);
            _rb.AddForce(transform.up * forwardSpeed);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer != 8) Explode();
        }

        private IEnumerator Timer()
        {
            yield return new WaitForSeconds(lifeSpan);
            if (!_exploded) Explode();
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
