using UnityEngine;

public class EnableJumpingTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Gets the movement controller from player and enables jumping when triggered
            MovementController movement = other.GetComponent<MovementController>();
            if (movement != null)
            {
                movement.EnableJumping();
                print("Jumping is now enabled.");
            }
        }
    }
}
