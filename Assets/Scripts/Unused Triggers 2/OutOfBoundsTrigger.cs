using UnityEngine;

public class OutOfBoundsTrigger : MonoBehaviour
{
    [SerializeField] 
    private Transform spawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Resets the jump state back to disabled
            MovementController movement = other.GetComponent<MovementController>();
            if (movement != null)
            {
                movement.DisableJumping();
            }

            // Teleport the player to the spawn point object
            CharacterController cc = other.GetComponent<CharacterController>();
            other.transform.position = spawnPoint.position;
        }
    }
}
