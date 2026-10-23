using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerThrow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private Camera playerCamera; // Directs throw toward camera view
    [SerializeField] private Slider chargeBar;

    [Header("Throw Force Settings")]
    [SerializeField] private float minThrowForce = 8f;
    [SerializeField] private float maxThrowForce = 35f;
    [SerializeField] private float maxChargeTime = 1.5f; // Seconds to reach full power
    [SerializeField] private float throwUpwardForce = 1.5f;

    [Header("Aiming Settings")]
    [SerializeField] private float maxAimDistance = 100f;
    [SerializeField] private LayerMask aimTargetMask = ~0; // What objects count as targets

    [Header("Cooldown")]
    [SerializeField] private float cooldown = 0.3f;

    private float chargeTimer = 0f;
    private bool isCharging = false;
    private bool readyToThrow = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (chargeBar != null)
        {
            chargeBar.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current == null || !readyToThrow) return;

        // 1. On Click Press: Start Charging
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            isCharging = true;
            chargeTimer = 0f;

            if (chargeBar != null)
            {
                chargeBar.value = 0f;
                chargeBar.gameObject.SetActive(true);
            }
        }

        // 2. While Holding Click: Accumulate Charge
        if (isCharging && Mouse.current.leftButton.isPressed)
        {
            chargeTimer += Time.deltaTime;
            chargeTimer = Mathf.Clamp(chargeTimer, 0f, maxChargeTime);

            if (chargeBar != null)
            {
                chargeBar.value = chargeTimer / maxChargeTime;
            }
        }

        // 3. On Click Release: Fire
        if (isCharging && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if (chargeBar != null)
            {
                chargeBar.gameObject.SetActive(false);
            }

            ExecuteThrow();
        }
    }

    private void ExecuteThrow()
    {
        isCharging = false;
        readyToThrow = false;

        FindAnyObjectByType<ScoreManager>().RegisterThrow();

        // Calculate force based on how long button was held
        float chargeRatio = chargeTimer / maxChargeTime;
        float finalForce = Mathf.Lerp(minThrowForce, maxThrowForce, chargeRatio);

        // Calculate aim direction using camera center
        Vector3 targetPoint = GetCameraCenterTargetPoint();
        Vector3 throwDirection = (targetPoint - throwPoint.position).normalized;

        // Instantiate and apply physics force
        GameObject ball = Instantiate(ballPrefab, throwPoint.position, Quaternion.LookRotation(throwDirection));
        Rigidbody ballRb = ball.GetComponent<Rigidbody>();

        Vector3 forceToApply = (throwDirection * finalForce) + (Vector3.up * throwUpwardForce);
        ballRb.AddForce(forceToApply, ForceMode.Impulse);

        Invoke(nameof(ResetThrow), cooldown);
    }

    private Vector3 GetCameraCenterTargetPoint()
    {
        // Screen center point (0.5, 0.5 in Viewport Space)
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, maxAimDistance, aimTargetMask))
        {
            return hit.point; // Aim directly at what crosshair/center camera is pointing at
        }

        return ray.GetPoint(maxAimDistance); // Aim into empty space if nothing is hit
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }
}
