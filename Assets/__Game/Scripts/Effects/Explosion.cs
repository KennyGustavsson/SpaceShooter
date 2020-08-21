using UnityEngine;

public class Explosion : MonoBehaviour
{
    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _anim.Play("Explosion");
    }
}
