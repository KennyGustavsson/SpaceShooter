using UnityEngine;

[ExecuteInEditMode]
public class PlanetGravity : MonoBehaviour
{
    [SerializeField] int planetMass;

    //Get vector pointing towards center of planet with a magnitude of gravityStrength
    public Vector2 GetGravity(Vector3 objectPosition, float objectMass)
    {
        //F = G*mass1*mass2/r^2, Newtons theory is inaccurate but close enough for a video game
        //We will ignore G for this

        //finding r using pythagoras
        float r = Vector2.Distance(objectPosition, transform.position);
        //r^2
        r *= r;

        float mass = objectMass * planetMass;
        float gravityStrength = mass / r;


        Vector2 gravityDirection = transform.position - objectPosition;
        return gravityDirection.normalized * gravityStrength;
    }
}