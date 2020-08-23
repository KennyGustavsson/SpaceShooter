using UnityEngine;

public class SpawningManager : MonoBehaviour
{
    public float maxDistance;

    private Transform _player;

    private void Awake()
    {
        _player = GameObject.Find("Player").transform;
    }

    private void DistanceCheck(Transform transform)
    {
        if(Vector3.Distance(_player.position, transform.position) > maxDistance)
        {

        }
    }

    private void Respawn()
    {

    }
}
