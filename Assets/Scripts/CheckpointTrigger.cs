using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CheckpointTrigger : MonoBehaviour
{
    private bool isActivated = false;

    // Called when another collider enters the trigger collider attached to this GameObject
    private void OnTriggerEnter(Collider other)
    {
        if (!isActivated && (other.CompareTag("Player") || other.transform.root.CompareTag("Player")))
        {
            isActivated = true;

            // Reads the world position and rotation of the capsule that touched the trigger
            Transform capsuleTransform = other.transform;

            if (CheckpointManager.Instance != null)
            {
                CheckpointManager.Instance.SetCheckpoint(capsuleTransform.position, capsuleTransform.rotation);
            }
        }
    }
}