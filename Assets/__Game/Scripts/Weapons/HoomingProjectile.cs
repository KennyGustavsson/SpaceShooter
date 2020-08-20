using System.Collections;
using UnityEngine;

namespace SS {
    [RequireComponent(typeof(Rigidbody2D))]
    public class HoomingProjectile : MonoBehaviour
    {
        public float hoomingRadius = 20f;
        public float rocketSpeed = 5f;
        public float rocketAcceleration = 20f;
        public float lifeSpan = 5f;

        private Rigidbody2D _rb;
        public Transform _target;
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
                if ((transform.position - hitColliders[i].transform.position).sqrMagnitude < _distance)
                {
                    if(hitColliders[i].gameObject.layer == 9)
                    {
                        _distance = (hitColliders[i].transform.position - transform.position).sqrMagnitude;
                        _target = hitColliders[i].transform;
                    }
                    else _target = null;
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

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0, 0, .1f);
            Gizmos.DrawSphere(transform.position, hoomingRadius);

            Gizmos.color = Color.yellow;
            if (_target != null)  Gizmos.DrawLine(transform.position, _target.position);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.up);
        }
#endif

        private void FaceTarget()
        {
            float posX = _target.position.x - transform.position.x;
            float posY = _target.position.y - transform.position.y;

            float angle = Mathf.Atan2(posX, posY) * Mathf.Rad2Deg;

            transform.localEulerAngles =  new Vector3(0, 0, -angle);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.layer != 8) Explode();
        }

        private IEnumerator Timer()
        {
            yield return new WaitForSeconds(lifeSpan);
            if(!_exploded) Explode();
        }

        public void Explode()
        {
            _exploded = true;
            gameObject.SetActive(false);
        }
    }
}
