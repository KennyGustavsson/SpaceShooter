using UnityEngine;

public class LevelConstraints : MonoBehaviour
{
    public float constrainValue = 500f;

    void Update()
    {
        // X
        if(transform.position.x > constrainValue) transform.position = new Vector3(constrainValue, transform.position.y, transform.position.z);
        if(transform.position.x < -constrainValue) transform.position = new Vector3(-constrainValue, transform.position.y, transform.position.z);

        // Y
        if(transform.position.y > constrainValue) transform.position = new Vector3(transform.position.x, constrainValue, transform.position.z);
        if(transform.position.y < -constrainValue) transform.position = new Vector3(transform.position.x, -constrainValue, transform.position.z);
    }
}