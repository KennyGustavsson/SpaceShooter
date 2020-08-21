using UnityEngine;

public class SkyboxCam : MonoBehaviour
{
    [SerializeField, Range(0.000001f, 1)]
    float slowDown = 0.05f;
    Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        transform.rotation = Quaternion.Euler(-player.position.y * slowDown, -player.position.x * slowDown, 0);
    }
}
