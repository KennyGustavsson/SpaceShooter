using System.Collections;
using UnityEngine;

namespace SS
{
    [RequireComponent(typeof(LineRenderer))]
    public class Raygun : MonoBehaviour
    {
        public int damage = 20;
        public float length = 100;

        private LineRenderer _ln;
        private bool _initialized;
        private Rigidbody2D _rb;

        private void Awake()
        {
            _ln = GetComponent<LineRenderer>();
            _ln.SetPosition(0, transform.position);
            _ln.SetPosition(1, transform.position);
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            _rb.velocity = Vector2.zero;

            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position + transform.up, transform.up, length);
            if (hit.Length > 0)
            {
                for(int i = 0; i < hit.Length; i++)
                {
                    if(hit[i].transform.gameObject.layer == 9) hit[i].collider.GetComponent<Health>().HealthChange(-damage);
                }
            }

            _ln.SetPosition(0, transform.position);
            _ln.SetPosition(1, transform.position + transform.up * length);

            if (_initialized) SoundManager.Instance.PlayAudioAtLocation(1, transform.position);
            StartCoroutine(timer());
        }

        private void OnDisable()
        {
            _initialized = true;
        }

        private IEnumerator timer()
        {
            yield return new WaitForSeconds(0.1f);
            _ln.SetPosition(0, new Vector3(0, 0, 0));
            _ln.SetPosition(1, new Vector3(0, 0, 0));
        }
    }
}