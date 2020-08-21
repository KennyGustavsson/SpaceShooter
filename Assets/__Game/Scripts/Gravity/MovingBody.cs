using System;
using UnityEngine;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.Mathematics;
using UnityEngine.Serialization;
[ExecuteInEditMode]
public class MovingBody : MonoBehaviour
{
    public Vector2[] futurePoints;

    private int amountOfPoints;
    public bool SetUp { get; private set; }

    public float distanceFromCenter;
    private float timeCounter;
    public float rotationSpeed;
    [SerializeField] private int planetMass;
    
    private void Awake()
    {
        amountOfPoints = PlanetManager.SimulationPoints;
        futurePoints = new Vector2[amountOfPoints];
        timeCounter = 0;
    }

    private void FixedUpdate()
    {
        timeCounter += Time.fixedDeltaTime;
        float newX = Mathf.Cos(timeCounter * rotationSpeed / distanceFromCenter) * distanceFromCenter;
        float newY = -Mathf.Sin(timeCounter * rotationSpeed / distanceFromCenter) * distanceFromCenter;
        
        transform.localPosition = new Vector2(newX, newY);
        
        for (int i = 0; i < amountOfPoints; i++)
        {
            float nextX = transform.parent.position.x + Mathf.Cos((timeCounter + Time.fixedDeltaTime * i) * rotationSpeed/ distanceFromCenter) * distanceFromCenter;
            float nextY = transform.parent.position.y - Mathf.Sin((timeCounter + Time.fixedDeltaTime * i) * rotationSpeed/ distanceFromCenter) * distanceFromCenter;
            futurePoints[i] = new Vector2(nextX, nextY);
        }
    }

    public Vector2 GetGravity(Vector3 objectPosition, float objectMass, int stepsAhead)
    {
        if (stepsAhead > amountOfPoints)
        {
            return Vector2.zero;
        }
        //F = G*mass1*mass2/r^2, Newtons theory is inaccurate but close enough for a video game
        //We will ignore G for this

        //finding r using pythagoras
        float r = Vector2.Distance(objectPosition, futurePoints[stepsAhead]);
        //r^2
        r *= r;

        float mass = objectMass * planetMass;
        float gravityStrength = mass / r;


        Vector2 gravityDirection = futurePoints[stepsAhead] - new Vector2(objectPosition.x, objectPosition.y);
        return gravityDirection.normalized * gravityStrength;
    }

    public void SetUpInEditor()
    {
        float startX = transform.parent.position.x + Mathf.Cos((0) * rotationSpeed/ distanceFromCenter) * distanceFromCenter;
        float startY = transform.parent.position.y - Mathf.Sin((0) * rotationSpeed/ distanceFromCenter) * distanceFromCenter;
        int pointsTotal = PlanetManager.SimulationPoints;
        Vector2[] points = new Vector2[pointsTotal];
        
        points[0] = new Vector2(startX, startY);
        float timeCounter = 0 + Time.fixedDeltaTime;
        for (int i = 0; i < pointsTotal; i++)
        {
            float nextX = transform.parent.position.x + Mathf.Cos((timeCounter + Time.fixedDeltaTime * i) * rotationSpeed/ distanceFromCenter) * distanceFromCenter;
            float nextY = transform.parent.position.y - Mathf.Sin((timeCounter + Time.fixedDeltaTime * i) * rotationSpeed/ distanceFromCenter) * distanceFromCenter;
            points[i] = new Vector2(nextX, nextY);
        }
        
        futurePoints = points;
        SetUp = true;
    }
}
