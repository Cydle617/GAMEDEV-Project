using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
public class HazardDetector : MonoBehaviour
{
    public enum BehaviorMode { AngleSequence, WaypointPatrol }
    private enum DetectionShape { Sphere, Cube, Capsule }

    [Header("Behavior Mode")]
    [SerializeField] private BehaviorMode behaviorMode = BehaviorMode.AngleSequence;

    [Header("Angle Sequence Settings")]
    [SerializeField] private float[] targetAngles = new float[] { 0f, 90f, 180f, -90f };
    [SerializeField] private float turnSpeed = 120f; // Degrees per second

    [Header("Patrol Waypoints")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float waypointTolerance = 0.3f;

    [Header("Timing")]
    [SerializeField] private float waitTimeAtWaypoint = 1.0f;

    [Header("Laser Settings")]
    [SerializeField] private float laserRange = 10f;
    [SerializeField] private Transform laserEmitter;
    [SerializeField] private LayerMask detectionLayers; // Include Obstacles AND Player layer
    [SerializeField] private LayerMask playerLayer;

    [Header("Proximity Detection Shape")]
    [SerializeField] private DetectionShape shapeType = DetectionShape.Sphere;
    [SerializeField] private float proximityRadius = 1.5f;
    [SerializeField] private float capsuleHeight = 3.0f;
    [SerializeField] private Vector3 boxHalfExtents = new Vector3(1f, 1f, 1f);

    private LineRenderer lineRenderer;
    private NavMeshAgent agent;
    private Quaternion startRotation;

    private int currentAngleIndex = 0;
    private int currentWaypointIndex = 0;
    private bool movingForward = true;
    private float waitTimer = 0f;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        agent = GetComponent<NavMeshAgent>();

        if (laserEmitter == null) laserEmitter = transform;

        lineRenderer.startWidth = 0.08f;
        lineRenderer.endWidth = 0.08f;
        lineRenderer.positionCount = 2;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        startRotation = transform.rotation;

        if (behaviorMode == BehaviorMode.WaypointPatrol && agent != null && agent.isOnNavMesh)
        {
            if (waypoints != null && waypoints.Length > 0)
            {
                agent.SetDestination(waypoints[0].position);
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        switch (behaviorMode)
        {
            case BehaviorMode.AngleSequence:
                HandleAngleSequence();
                break;

            case BehaviorMode.WaypointPatrol:
                PatrolWaypoints();
                break;
        }

        UpdateLaserAndDetect();
    }

    // Handles the rotation of the hazard between specified angles, waiting at each angle for a set duration
    private void HandleAngleSequence()
    {
        if (targetAngles == null || targetAngles.Length == 0) return;

        Quaternion targetRotation = startRotation * Quaternion.Euler(0f, targetAngles[currentAngleIndex], 0f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.5f)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTimeAtWaypoint)
            {
                currentAngleIndex = (currentAngleIndex + 1) % targetAngles.Length;
                waitTimer = 0f;
            }
        }
    }

    // Handles the patrol behavior between waypoints, including waiting at each waypoint for a specified duration
    private void PatrolWaypoints()
    {
        if (waypoints == null || waypoints.Length == 0 || agent == null || !agent.isOnNavMesh) return;
        if (agent.pathPending) return;

        if (agent.remainingDistance <= agent.stoppingDistance + waypointTolerance)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTimeAtWaypoint)
            {
                AdvanceToNextWaypoint();
                waitTimer = 0f;
            }
        }
    }

    // Advances to the next waypoint in the patrol route, reversing direction if at the end of the list
    private void AdvanceToNextWaypoint()
    {
        if (waypoints.Length <= 1) return;

        if (movingForward)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = waypoints.Length - 2;
                movingForward = false;
            }
        }
        else
        {
            currentWaypointIndex--;
            if (currentWaypointIndex < 0)
            {
                currentWaypointIndex = 1;
                movingForward = true;
            }
        }

        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    // Updates the laser's position and checks for player detection using both raycasting and proximity volume checks
    private void UpdateLaserAndDetect()
    {
        Vector3 startPos = laserEmitter.position;
        Vector3 rayDirection = laserEmitter.forward;
        Vector3 endPos = startPos + rayDirection * laserRange;

        if (Physics.Raycast(startPos, rayDirection, out RaycastHit hit, laserRange, detectionLayers))
        {
            endPos = hit.point;

            if (((1 << hit.collider.gameObject.layer) & playerLayer) != 0)
            {
                OnPlayerDetected();
            }
        }

        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);

        CheckProximityVolume();
    }

    // Checks for player presence within the defined proximity volume based on the selected shape
    private void CheckProximityVolume()
    {
        Collider[] hits = null;

        switch (shapeType)
        {
            case DetectionShape.Sphere:
                hits = Physics.OverlapSphere(transform.position, proximityRadius, playerLayer);
                break;

            case DetectionShape.Cube:
                hits = Physics.OverlapBox(transform.position, boxHalfExtents, transform.rotation, playerLayer);
                break;

            case DetectionShape.Capsule:
                float capOffset = Mathf.Max(0f, (capsuleHeight / 2f) - proximityRadius);
                Vector3 p1 = transform.position + transform.up * capOffset;
                Vector3 p2 = transform.position - transform.up * capOffset;
                hits = Physics.OverlapCapsule(p1, p2, proximityRadius, playerLayer);
                break;
        }

        if (hits != null && hits.Length > 0)
        {
            Vector3 targetPoint = hits[0].bounds.center;
            Vector3 startPoint = laserEmitter != null ? laserEmitter.position : transform.position;
            Vector3 dirToPlayer = (targetPoint - startPoint).normalized;
            float dist = Vector3.Distance(startPoint, targetPoint);

            if (Physics.Raycast(startPoint, dirToPlayer, out RaycastHit lineOfSightHit, dist, detectionLayers))
            {
                if (((1 << lineOfSightHit.collider.gameObject.layer) & playerLayer) != 0)
                {
                    OnPlayerDetected();
                }
            }
        }
    }

    // Handles the event when the player is detected by the hazard, triggering a respawn at the last checkpoint
    private void OnPlayerDetected()
    {
        if (CheckpointManager.Instance != null)
        {
            CheckpointManager.Instance.RespawnPlayer();
        }
    }

    // Used for debugging and visualization in the editor, does not impact gameplay
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 startPos = laserEmitter != null ? laserEmitter.position : transform.position;
        Gizmos.DrawRay(startPos, transform.forward * laserRange);

        switch (shapeType)
        {
            case DetectionShape.Sphere:
                Gizmos.DrawWireSphere(transform.position, proximityRadius);
                break;

            case DetectionShape.Cube:
                Matrix4x4 originalMatrix = Gizmos.matrix;
                Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
                Gizmos.DrawWireCube(Vector3.zero, boxHalfExtents * 2f);
                Gizmos.matrix = originalMatrix;
                break;

            case DetectionShape.Capsule:
                float capOffset = Mathf.Max(0f, (capsuleHeight / 2f) - proximityRadius);
                Vector3 top = transform.position + transform.up * capOffset;
                Vector3 bottom = transform.position - transform.up * capOffset;

                Gizmos.DrawWireSphere(top, proximityRadius);
                Gizmos.DrawWireSphere(bottom, proximityRadius);

                Gizmos.DrawLine(top + transform.forward * proximityRadius, bottom + transform.forward * proximityRadius);
                Gizmos.DrawLine(top - transform.forward * proximityRadius, bottom - transform.forward * proximityRadius);
                Gizmos.DrawLine(top + transform.right * proximityRadius, bottom + transform.right * proximityRadius);
                Gizmos.DrawLine(top - transform.right * proximityRadius, bottom - transform.right * proximityRadius);
                break;
        }
    }
}