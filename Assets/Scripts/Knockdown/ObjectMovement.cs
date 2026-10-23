using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    [Header("Assign Moving Platform")]
    [SerializeField] private Transform platform;
    [SerializeField] private GameObject scoreZone;

    private Vector3 lastPlatformPos;
    private Vector3 startOffset;
    private Quaternion startRotation;
    private Rigidbody rb;
    private bool isFollowing = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (platform != null)
        {
            lastPlatformPos = platform.position;
            // Record starting offset relative to the platform
            startOffset = transform.position - platform.position;
            startRotation = transform.rotation;
        }
    }

    private void LateUpdate()
    {
        if (!isFollowing || platform == null) return;

        // 1. Calculate how much the platform moved this frame
        Vector3 platformDelta = platform.position - lastPlatformPos;

        // 2. Add that exact movement to the can's position
        transform.position += platformDelta;

        // 3. Update saved platform position for next frame
        lastPlatformPos = platform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Disable tracking as soon as a ball or another falling can hits it
        isFollowing = false;
    }
}
