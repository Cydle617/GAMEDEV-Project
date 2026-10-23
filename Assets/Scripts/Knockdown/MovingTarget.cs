using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    [Header("Horizontal Movement")]
    [SerializeField] private Vector3 horizontalAxis = Vector3.right; // Direction for side-to-side/forward-back
    [SerializeField] private float horizontalDistance = 3f;          // Side-to-side distance

    [Header("Vertical Movement (Height)")]
    [SerializeField] private float heightDistance = 2f;              // Up/down height amplitude (Y axis)

    [Header("Speed & Timing")]
    [SerializeField] private float moveSpeed = 2f;                  // General movement speed
    [SerializeField] private float verticalSpeedMultiplier = 1f;    // Set higher/lower for different bobbing rhythm

    private Vector3 startPosition;
    private Rigidbody rb;
    private bool isMoving = false;

    private void Awake()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!isMoving) return;

        // Calculate side-to-side offset
        float horizontalOffset = Mathf.Sin(Time.time * moveSpeed) * horizontalDistance;
        Vector3 horizontalDisplacement = horizontalAxis.normalized * horizontalOffset;

        // Calculate height (Y-axis) offset
        float heightOffset = Mathf.Sin(Time.time * moveSpeed * verticalSpeedMultiplier) * heightDistance;
        Vector3 verticalDisplacement = Vector3.up * heightOffset;

        // Combine starting position with horizontal and vertical offsets
        Vector3 targetPos = startPosition + horizontalDisplacement + verticalDisplacement;

        if (rb != null && !rb.isKinematic)
        {
            rb.MovePosition(targetPos);
        }
        else
        {
            transform.position = targetPos;
        }
    }

    public void SetMovementState(bool enableMovement)
    {
        isMoving = enableMovement;

        // Reset back to starting position when stopping movement
        if (!enableMovement)
        {
            transform.position = startPosition;
            if (rb != null)
            {
                rb.position = startPosition;
                rb.linearVelocity = Vector3.zero; // Use rb.velocity on Unity 2022 or older
                rb.angularVelocity = Vector3.zero;
            }
        }
    }

    public void UpdateStartPosition(Vector3 newPos)
    {
        startPosition = newPos;
    }
}
