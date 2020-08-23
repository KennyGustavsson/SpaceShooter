using UnityEngine;

namespace SS
{
    public class Spawn : MonoBehaviour
    {
        private Animator _anim;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _anim.Play("SpawnEffect");
        }
    }
}

