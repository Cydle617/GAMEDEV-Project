using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Light))]
public class FlashlightController : MonoBehaviour
{
    [Header("Flashlight Setup")]
    [SerializeField] private Light flashlight;

    [Header("Detection Layers")]
    [SerializeField] private LayerMask hiddenObjectLayer;
    [SerializeField] private LayerMask obstacleLayer;

    // Update is called once per frame
    private void Update()
    {
        // Toggle light when F is pressed using New Input System
        if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
        {
            flashlight.enabled = !flashlight.enabled;
        }

        if (flashlight.enabled)
        {
            ScanForHiddenObjects();
        }
    }

    // Scans for hidden objects within the flashlight's range and angle, revealing them if they are not obstructed by obstacles
    private void ScanForHiddenObjects()
    {
        Collider[] candidates = Physics.OverlapSphere(transform.position, flashlight.range, hiddenObjectLayer);

        foreach (Collider col in candidates)
        {
            if (col.TryGetComponent<HiddenObject>(out HiddenObject hiddenObj))
            {
                Vector3 dirToTarget = (col.transform.position - transform.position).normalized;
                float distanceToTarget = Vector3.Distance(transform.position, col.transform.position);

                float angle = Vector3.Angle(transform.forward, dirToTarget);
                if (angle <= flashlight.spotAngle / 2f)
                {
                    if (!Physics.Raycast(transform.position, dirToTarget, distanceToTarget, obstacleLayer))
                    {
                        hiddenObj.Reveal();
                    }
                }
            }
        }
    }
}
