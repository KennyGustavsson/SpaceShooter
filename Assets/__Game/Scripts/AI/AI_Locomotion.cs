using SS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Locomotion : MonoBehaviour {

    private Vector3 previousPosition;
    private Vector3 position;
    private Rigidbody2D selfRigidBody;
    private Collider2D selfCollider;

    [Header("Random spawn locations")]
    [SerializeField] private float minX = -32f;
    [SerializeField] private float maxX = 30f;
    [SerializeField] private float minY = -11f;
    [SerializeField] private float maxY = 30f;

    [Header("Raycasting")]
    [SerializeField] private LayerMask planetLayer = (LayerMask)0 >> 7 & 0 >> 9;
    [SerializeField] private float rotateTargetRaycastDirection = 90;
    [SerializeField] private float directionMulitplier = 1f;

    [Header("Target")]
    [SerializeField] private Vector3 finalTarget;
    [ReadOnly, SerializeField] private Vector3 moveToTarget;
    [ReadOnly, SerializeField] private Vector3 headingTo;

    [ReadOnly, SerializeField] private float distanceToTarget;
    [SerializeField] private float distanceThreshold = 1f;

    [Header("Direction")]
    [ReadOnly, SerializeField] private Vector3 direction;

    [Header("Movement")]
    [SerializeField] private float minSpeed = 2f;
    [SerializeField] private float maxSpeed = 5f;
    [ReadOnly, SerializeField] private float targetSpeed = 5f;
    [ReadOnly, SerializeField] private float speed;
    [SerializeField] private float speedDrag = 0.1f;
    private float smoothSpeedVelocity;
    [Space]
    [ReadOnly, SerializeField] private Vector3 targetVelocity;
    [ReadOnly, SerializeField] private Vector3 velocity;
    [SerializeField] private float velocityDrag = 5;
    private Vector3 smoothVelocityVelocity;
    [Space]
    [ReadOnly, SerializeField] private Quaternion targetRotation;

    [SerializeField] private float turnDrag = 1f;
    private float turnSmoothVelocity;

    [Header("Player Interaction")]
    private Vector3 playerPosition;
    [SerializeField] private float searchForPlayerRadius = 20f;
    [ReadOnly, SerializeField] private bool isPlayerInSight;
    [SerializeField] private LayerMask playerRaycastMask = 8;
    [SerializeField] private float speedWhenShootingAtPlayer = 0.5f;
    [SerializeField] private float distanceToShotAtPlayer = 25f;
    [SerializeField] private float projectileSpeed = 30f;
    [SerializeField] private bool canShoot;
    [SerializeField] private float shootingCooldownTime = 0.75f;
    private float shootingCooldownTimer;



    [Header("Misc")]
    [ReadOnly, SerializeField] private float magnitude;
    [Space]
    [SerializeField] private Vector3 addForce;

    void Start() {
        selfRigidBody = GetComponent<Rigidbody2D>();
        selfCollider = GetComponent<Collider2D>();
        SetRandomDestination();
        shootingCooldownTimer = shootingCooldownTime;
    }

    private void SetRandomDestination() {
        if (moveToTarget != finalTarget) {
            moveToTarget = finalTarget;
            return;
        }

        Vector3 oldTarget = finalTarget;
        finalTarget = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        if (Vector3.Distance(oldTarget, finalTarget) < 10f) {
            finalTarget = oldTarget;
            SetRandomDestination();
        }
        if (Physics2D.CircleCast(finalTarget, 3f, Vector3.zero).collider != null) {
            finalTarget = oldTarget;
            SetRandomDestination();
        }
        moveToTarget = finalTarget;
    }

    // Update is called once per frame
    void FixedUpdate() {
        float deltaTime = Time.fixedDeltaTime;
        position = transform.position;

        CheckForPlayer();

        if (canShoot == false) {
            shootingCooldownTimer -= deltaTime;
            if (shootingCooldownTimer <= 0f) {
                canShoot = true;
                shootingCooldownTimer = shootingCooldownTime;
            }
        }

        direction = CheckForCollision();

        //Debug to shot where to go
        //Vector3 rotatedDirection = Quaternion.AngleAxis(rotateTargetRaycastDirection, Vector3.forward) * direction;
        Debug.DrawLine(moveToTarget - direction / 2, moveToTarget + direction / 2, Color.blue);


        CalculatetSpeed();
        CalculateRotation(deltaTime);
        CalculateVelocity(deltaTime);

        Move();

        if (distanceToTarget < distanceThreshold) {
            SetRandomDestination();
        }

        previousPosition = transform.position;
    }

    private void CalculatetSpeed() {
        if (isPlayerInSight == true) {
            targetSpeed = 0.5f;
        }
        else {
            targetSpeed = Mathf.Lerp(minSpeed, maxSpeed, distanceToTarget / 15f);
        }
        speed = Mathf.SmoothDamp(speed, targetSpeed, ref smoothSpeedVelocity, speedDrag);
    }

    private void CalculateRotation(float deltaTime) {
        if (isPlayerInSight == true) {
            targetRotation = Quaternion.LookRotation(transform.forward, playerPosition - position);
        }
        else {
            targetRotation = Quaternion.LookRotation(transform.forward, headingTo);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, deltaTime * turnDrag);
    }

    private void CalculateVelocity(float deltaTime) {
        targetVelocity = direction * speed * deltaTime;
        velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref smoothVelocityVelocity, velocityDrag);
    }

    private void Move() {
        magnitude = Vector3.Magnitude(velocity);
        magnitude = Mathf.Clamp(magnitude, 0f, 15f);

        transform.Translate(new Vector3(0f, magnitude, 0f), Space.Self);
    }

    private Vector3 CheckForCollision() {
        Vector3 headingToTarget = finalTarget;
        if (isPlayerInSight == true) {
            headingToTarget = playerPosition;
        }

        headingTo = headingToTarget - transform.position;
        distanceToTarget = headingTo.magnitude;
        Vector3 avoidanceDirection = headingTo / distanceToTarget;
        RaycastHit2D raycastHit = Physics2D.Raycast(position + (avoidanceDirection * 3), avoidanceDirection, distanceToTarget, planetLayer);
        if (raycastHit.collider == null) {
            Debug.DrawRay(position, avoidanceDirection + avoidanceDirection + avoidanceDirection, Color.green);
            moveToTarget = finalTarget;
            return avoidanceDirection;
        }

        headingToTarget = moveToTarget;
        // Gets a vector that points from the player's position to the target's.
        headingTo = headingToTarget - transform.position;
        distanceToTarget = headingTo.magnitude;
        avoidanceDirection = headingTo / distanceToTarget;

        //raycastHit = Physics2D.Raycast(position + avoidanceDirection + avoidanceDirection, transform.forward, distanceToTarget > 7f ? 7f : distanceToTarget, planetLayer);
        raycastHit = Physics2D.Raycast(position + avoidanceDirection * 2, avoidanceDirection, distanceToTarget > 15f ? 15f : distanceToTarget, planetLayer);
        Collider2D collider = raycastHit.collider;
        Debug.DrawRay(position + transform.up * 2, transform.up, Color.yellow);
        //If we didn't hit a collider, we're fine, keep going.
        if (collider == null) {
            //Debug.DrawRay(position + avoidanceDirection + avoidanceDirection, avoidanceDirection + avoidanceDirection + avoidanceDirection, Color.green);
            Debug.DrawRay(position + avoidanceDirection * 2, transform.forward, Color.green);
        }
        //We hit something. Trying to avoid collision!
        else if (collider != selfCollider) {
            Debug.DrawRay(position, avoidanceDirection * 3, Color.red);

            //Printing out where we hit something, and making a white debug line.
            Vector3 rotatedDirection = Quaternion.AngleAxis(rotateTargetRaycastDirection, Vector3.forward) * direction * directionMulitplier;
            Vector3 halfRotatedDirection = rotatedDirection / 2;
            Debug.DrawLine((Vector3)raycastHit.point - halfRotatedDirection, (Vector3)raycastHit.point + halfRotatedDirection, Color.white, 0.1f);

            //Let's raycast among the white line we just made and see if we can find a way around the object.
            int raycastCount = 9;
            bool canAvoidCollision = false;
            List<Vector3> midPointBetweenTwoVectorsHigh = new List<Vector3>();
            List<Vector3> midPointBetweenTwoVectorsLow = new List<Vector3>();

            //This for-loop will begin from the middle and check towards the end of the white line if we can find a way
            for (int i = 0; i < raycastCount / 2 + 1; i++) {
                RaycastHit2D raycastHitHigh;
                RaycastHit2D raycastHitLow;

                //Finds the two end vectors on the white line
                Vector3 highPoint = (Vector3)raycastHit.point + halfRotatedDirection;
                Vector3 lowPoint = (Vector3)raycastHit.point - halfRotatedDirection;

                //Checks the higher end of the white line
                int highIntervall = (raycastCount / 2) + 1 + i;
                Vector3 highPointToCastFrom = Vector3.Lerp(lowPoint, highPoint, (float)highIntervall / raycastCount);

                headingTo = headingToTarget - highPointToCastFrom;
                float distance = headingTo.magnitude;
                Vector3 highDirection = headingTo / distance;

                raycastHitHigh = Physics2D.Raycast(highPointToCastFrom, highDirection, distance, planetLayer);

                if (raycastHitHigh.collider != null) {
                    Debug.DrawRay(highPointToCastFrom, highDirection, Color.grey, 0.1f);
                }
                else {
                    if (canAvoidCollision == false) {
                        Debug.DrawRay(highPointToCastFrom, highDirection, Color.green);
                        midPointBetweenTwoVectorsHigh.Add(highPointToCastFrom);
                        moveToTarget = highPointToCastFrom;
                        headingTo = transform.position - highPointToCastFrom;
                        if (midPointBetweenTwoVectorsHigh.Count == 2) {
                            Vector3 midPoint = (midPointBetweenTwoVectorsHigh[0] + midPointBetweenTwoVectorsHigh[1]) / 2;
                            headingTo = transform.position - midPoint;
                            highDirection = headingTo / distance;
                            moveToTarget = (midPointBetweenTwoVectorsHigh[0] + midPointBetweenTwoVectorsHigh[1]) / 2;
                            canAvoidCollision = true;
                            return avoidanceDirection;
                        }
                        avoidanceDirection = highDirection;
                    }
                    else {
                        Debug.DrawRay(highPointToCastFrom, highDirection, Color.magenta, 0.1f);
                    }
                }

                //Checks the lower end of the white line
                int minusIntervall = (raycastCount / 2) - i;
                Vector3 lowPointToCastFrom = Vector3.Lerp(lowPoint, highPoint, (float)minusIntervall / raycastCount);

                headingTo = headingToTarget - lowPointToCastFrom;
                distance = headingTo.magnitude;
                Vector3 lowDirection = headingTo / distance;

                raycastHitLow = Physics2D.Raycast(lowPointToCastFrom, lowDirection, distance, planetLayer);
                if (raycastHitLow.collider != null) {
                    Debug.DrawRay(lowPointToCastFrom, lowDirection, Color.grey, 0.1f);
                }
                else {
                    if (canAvoidCollision == false) {
                        Debug.DrawRay(lowPointToCastFrom, lowDirection, Color.green);
                        midPointBetweenTwoVectorsLow.Add(highPointToCastFrom);
                        moveToTarget = lowPointToCastFrom;
                        headingTo = transform.position - lowPointToCastFrom;
                        if (midPointBetweenTwoVectorsLow.Count == 2) {
                            Vector3 midPoint = (midPointBetweenTwoVectorsLow[0] + midPointBetweenTwoVectorsLow[1]) / 2;
                            headingTo = transform.position - midPoint;
                            highDirection = headingTo / distance;
                            moveToTarget = (midPointBetweenTwoVectorsLow[0] + midPointBetweenTwoVectorsLow[1]) / 2;
                            canAvoidCollision = true;
                            return avoidanceDirection;
                        }
                        avoidanceDirection = highDirection;
                    }
                    else {
                        Debug.DrawRay(lowPointToCastFrom, lowDirection, Color.magenta, 0.1f);
                    }
                }
            }
            if (canAvoidCollision == false) {
                SetRandomDestination();
            }
        }
        return avoidanceDirection;
    }

    private void CheckForPlayer() {
        float distanceToPlayer = 0f;
        RaycastHit2D raycastHit = Physics2D.CircleCast(position, searchForPlayerRadius, Vector3.forward, distanceToPlayer, playerRaycastMask);
        if (raycastHit.collider != null) {
            isPlayerInSight = true;
            playerPosition = raycastHit.point;
            if (Physics2D.Raycast(position, direction, distanceToShotAtPlayer, playerRaycastMask).collider != null) {
                Debug.DrawRay(position, direction * 3, Color.magenta);
                if (canShoot == true) {
                    ShootAtPlayer();
                    canShoot = false;
                }
            }
            return;
        }
        isPlayerInSight = false;
    }

    private void ShootAtPlayer() {
        GameObject objecPoolProjectile = ObjectPool.ObjPool.GetPooledObject(4);
        objecPoolProjectile.SetActive(false);
        objecPoolProjectile.transform.position = transform.position + transform.up * 2;
        objecPoolProjectile.transform.rotation = transform.rotation;
        objecPoolProjectile.SetActive(true);
        objecPoolProjectile.GetComponent<Rigidbody2D>().AddForce(transform.up * projectileSpeed, ForceMode2D.Impulse);
    }

    //IEnumerator Cooldown() {
    //    _fireCooldown = true;
    //    yield return new WaitForSeconds(fireRate);
    //    _fireCooldown = false;
    //}

    private void AddForce() {
        if (Input.GetKey(KeyCode.DownArrow)) {
            addForce.y += -1f;
        }
        if (Input.GetKey(KeyCode.UpArrow)) {
            addForce.y += 1f;
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            addForce.x += -1f;
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            addForce.x += 1f;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(position, searchForPlayerRadius);

    }

}
