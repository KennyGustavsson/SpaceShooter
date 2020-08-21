using System;
using UnityEngine;

[ExecuteInEditMode]

public class VisualizeOrbit : MonoBehaviour
{
    public bool FakeOrbit;
    public MovingBody mb;
    private int previewLineMaxPoints;
    public Rigidbody2D rb;
    public LineRenderer lr;
    public GravityAffect gA;

    private void Awake()
    {
        previewLineMaxPoints = PlanetManager.SimulationPoints;
    }

    private void Update()
    {
        if (!Application.isPlaying && !FakeOrbit)
        {
            DrawOrbit();
        }
        else if (!Application.isPlaying && FakeOrbit)
        {
            DrawFakeOrbit();
        }
    }

    void DrawOrbit()
    {
        PlanetGravity[] planets = FindObjectsOfType<PlanetGravity>();
        MovingBody[] bodies = FindObjectsOfType<MovingBody>();

        Vector2[] segments = new Vector2[previewLineMaxPoints];

        Vector2 currentVelocity = new Vector2(0f, gA.startForce) * Time.fixedDeltaTime;

        segments[0] = transform.position;

        for (int i = 1; i < previewLineMaxPoints; i++)
        {

            Vector2 gravity = Vector2.zero;
            if (planets.Length > 0)
                foreach (PlanetGravity planet in planets)
                {
                    if (planet.gameObject != this.gameObject)
                        gravity += planet.GetGravity(segments[i - 1], rb.mass);
                }

            if (bodies.Length > 0)
            {
                foreach (MovingBody body in bodies)
                {
                    if (body.gameObject != this.gameObject)
                    {
                        if(!body.SetUp)
                            body.SetUpInEditor();
                        gravity += body.GetGravity(segments[i - 1], rb.mass, i-1);
                    }
                }
            }

            currentVelocity += gravity * (Time.fixedDeltaTime * gA.slowDown);

            segments[i] = segments[i - 1] + currentVelocity;

        }

        lr.positionCount = previewLineMaxPoints;
        for (int j = 0; j < previewLineMaxPoints; j++)
        {
            lr.SetPosition(j, segments[j]);
        }
    }

    void DrawFakeOrbit()
    {
        float startX = transform.parent.position.x + Mathf.Cos((0) * mb.rotationSpeed/ mb.distanceFromCenter) * mb.distanceFromCenter;
        float startY = transform.parent.position.y - Mathf.Sin((0) * mb.rotationSpeed/ mb.distanceFromCenter) * mb.distanceFromCenter;
        transform.position = new Vector2(startX, startY);

        int pointsTotal = PlanetManager.SimulationPoints;
        lr.positionCount = pointsTotal;
        for (int i = 0; i < pointsTotal-1; i++)
        {
            float step = 2*Mathf.PI / (pointsTotal-1);
            float nextX = transform.parent.position.x +
                          Mathf.Cos((step * i)) * mb.distanceFromCenter;
            float nextY = transform.parent.position.y -
                          Mathf.Sin((step * i)) * mb.distanceFromCenter;
            lr.SetPosition(i, new Vector2(nextX, nextY));
        }
        lr.SetPosition(pointsTotal-1, transform.position);
    }
}
