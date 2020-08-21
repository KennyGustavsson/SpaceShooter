using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(LineRenderer))]

[ExecuteInEditMode]

public class GravityAffect : MonoBehaviour
{
    public LineRenderer lr;
    public Rigidbody2D rb;
    private bool collisionCourse = false;

    [Range(0,10)]
    public float startForce;
    public float slowDown;

    [Tooltip("How far the preview line will reach")]
    int previewLineMaxPoints;
    [SerializeField] private float segScale;

    private void Awake()
    {
        lr.positionCount = previewLineMaxPoints = PlanetManager.SimulationPoints;
        rb.AddForce(transform.up * startForce * rb.mass, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        Vector2 currentGravity = Vector2.zero;
        if(PlanetManager.singleton != null && PlanetManager.singleton.planetList.Count > 0)
            foreach (PlanetGravity planet in PlanetManager.singleton.planetList)
            {
                if(planet.gameObject != this.gameObject)
                    currentGravity += planet.GetGravity(transform.position, rb.mass);
            }

        if (PlanetManager.singleton != null && PlanetManager.singleton.movingList.Count > 0)
        {
            foreach (MovingBody movingBody in PlanetManager.singleton.movingList)
            {
                if (movingBody.gameObject != this.gameObject)
                {
                    currentGravity += movingBody.GetGravity(transform.position, rb.mass, 0);
                }
            }
        }
        rb.AddForce(currentGravity * slowDown * rb.mass, ForceMode2D.Impulse);
        VisualizeTrajectory();
    }

    void VisualizeTrajectory()
    {
        collisionCourse = false;
        
        Vector2[] segments = new Vector2[previewLineMaxPoints];

        Vector2 currentVelocity = rb.velocity * Time.fixedDeltaTime;

        segments[0] = transform.position;

        for (int i = 1; i < previewLineMaxPoints; i++)
        { 
            Vector2 gravity = Vector2.zero;
            if (PlanetManager.singleton != null && PlanetManager.singleton.planetList.Count > 0)
            {
                foreach (var planet in PlanetManager.singleton.planetList)
                {
                    if (planet.gameObject != this.gameObject)
                        gravity += planet.GetGravity(segments[i - 1], rb.mass);
                }
            }

            if (PlanetManager.singleton != null && PlanetManager.singleton.movingList.Count > 0)
            {
                foreach (MovingBody movingBody in PlanetManager.singleton.movingList)
                {
                    if (movingBody.gameObject != this.gameObject)
                    {
                        gravity += movingBody.GetGravity(segments[i-1], rb.mass, i);
                    }
                }
            }

            currentVelocity += gravity * Time.fixedDeltaTime * slowDown;

            segments[i] = segments[i - 1] + currentVelocity;
            RaycastHit2D hit = Physics2D.Raycast(segments[i-1], currentVelocity, currentVelocity.magnitude);
            if(hit.collider != null)
            {
                if (hit.collider.transform.tag == "MovingBody" || hit.collider.transform.tag == "Planet")
                {
                    lr.startColor = Color.red;
                    lr.endColor = Color.red;
                    collisionCourse = true;
                }
            }
        }

        if (collisionCourse == false)
        {
            lr.startColor = Color.green;
            lr.endColor = Color.green;
        }
        

        lr.positionCount = previewLineMaxPoints;
        for (int j = 0; j < previewLineMaxPoints; j++)
        {
            lr.SetPosition(j, segments[j]);
        }
    }
}
