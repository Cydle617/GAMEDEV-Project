using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    [Header("Player References")]
    [SerializeField] private Transform playerRoot;
    [SerializeField] private Transform playerCapsule;

    private Vector3 savedCapsulePosition;
    private Quaternion savedCapsuleRotation;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (playerCapsule != null)
        {
            SetCheckpoint(playerCapsule.position, playerCapsule.rotation);
        }
    }

    // Sets the checkpoint position and rotation based on the player's capsule transform when they enter a checkpoint trigger
    public void SetCheckpoint(Vector3 capsulePos, Quaternion capsuleRot)
    {
        savedCapsulePosition = capsulePos;
        savedCapsuleRotation = capsuleRot;
        // Debug.Log($"Checkpoint saved at Capsule position: {savedCapsulePosition}");
    }

    // Respawns the player at the last saved checkpoint, adjusting the root and capsule transforms accordingly
    public void RespawnPlayer()
    {
        // Disable CharacterController to allow position override
        CharacterController cc = playerRoot.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        // Calculate root rotation so the capsule faces savedCapsuleRotation
        Quaternion capsuleLocalRotation = playerCapsule.localRotation;
        playerRoot.rotation = savedCapsuleRotation * Quaternion.Inverse(capsuleLocalRotation);

        // Calculate capsule offset in world space after rotation
        Vector3 capsuleWorldOffset = playerRoot.rotation * playerCapsule.localPosition;

        // Move playerRoot so playerCapsule lands EXACTLY at savedCapsulePosition
        playerRoot.position = savedCapsulePosition - capsuleWorldOffset;

        // Sync physics and re-enable controller
        Physics.SyncTransforms();
        if (cc != null) cc.enabled = true;

        // Debug.Log($"Entire family teleported. Capsule is now at: {playerCapsule.position}");
    }
}