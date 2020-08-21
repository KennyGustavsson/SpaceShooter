using System.Collections;
using UnityEngine;

namespace SS
{
    [RequireComponent(typeof(LineRenderer))]
    public class Raygun : MonoBehaviour
    {
        public float length = 100;

        private LineRenderer _ln;
        private Vector3 _rot;

        private void Awake()
        {
            _ln = GetComponent<LineRenderer>();
            _ln.SetPosition(0, transform.position);
            _ln.SetPosition(1, transform.position);
        }

        private void OnEnable()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up, transform.up);
            if (hit.collider != null)
            {
                _ln.SetPosition(0, transform.position);
                _ln.SetPosition(1, hit.point);

                //Damage thing
            }
            else
            {
                _ln.SetPosition(0, transform.position);
                _ln.SetPosition(1, transform.up * length);
            }
            StartCoroutine(timer());
        }

        private IEnumerator timer()
        {
            yield return new WaitForSeconds(0.1f);
            _ln.SetPosition(0, new Vector3(0, 0, 0));
            _ln.SetPosition(1, new Vector3(0, 0, 0));
        }
    }
}
