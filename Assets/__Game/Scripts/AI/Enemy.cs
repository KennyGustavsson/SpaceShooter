using System.Collections;
using UnityEngine;

namespace SS
{
    public class Enemy : MonoBehaviour
    {
        public float speed = 10;
        public float projectileSpeed = 30;
        public float fireRate = 1;

        private Transform _target;
        private bool _fireCooldown;

        void Awake()
        {
            _target = GameObject.Find("Player").transform;
        }

        private void OnEnable()
        {
            _fireCooldown = false;
        }

        private void FixedUpdate()
        {
            if (Vector3.Distance(_target.position, transform.position) >= 200) return;
            if (Vector3.Distance(_target.position, transform.position) <= 5)
            {
                FaceTarget();
                if(!_fireCooldown) Shot();
            }
            else
            {
                FaceTarget();
                if (!_fireCooldown) Shot();
                transform.position = Vector3.MoveTowards(transform.position, _target.position, speed * Time.fixedDeltaTime);
            }
        }

        private void Shot()
        {
            var obj = ObjectPool.ObjPool.GetPooledObject(4);
            obj.SetActive(false);
            obj.transform.position = transform.position + transform.up * 2;
            obj.transform.rotation = transform.rotation;
            obj.SetActive(true);
            obj.GetComponent<Rigidbody2D>().AddForce(transform.up * projectileSpeed, ForceMode2D.Impulse);
            StartCoroutine(Cooldown());
        }

        IEnumerator Cooldown()
        {
            _fireCooldown = true;
            yield return new WaitForSeconds(fireRate);
            _fireCooldown = false;
        }

        private void FaceTarget()
        {
            float posX = _target.position.x - transform.position.x;
            float posY = _target.position.y - transform.position.y;

            float angle = Mathf.Atan2(posX, posY) * Mathf.Rad2Deg;

            transform.localEulerAngles = new Vector3(0, 0, -angle);
        }
    }
}
