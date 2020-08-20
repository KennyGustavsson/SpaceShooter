using UnityEngine;

public class GravityAffect : MonoBehaviour
{
    public PlanetGravity[] planets;

    public Rigidbody2D rb;
    
    private void FixedUpdate()
    {
        Vector2 currentGravity = Vector2.zero;
        foreach (PlanetGravity planet in planets)
        {
            currentGravity += planet.GetGravity(transform.position, rb.mass);
        }
        rb.AddForce(currentGravity);
    }
}
