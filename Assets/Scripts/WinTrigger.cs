using UnityEngine;

public class DestroyLaser : MonoBehaviour
{
    [SerializeField] private SpawnLaser laserSpawner;
    [SerializeField] private UIController uiManager;

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
            // Stop laser spawning
            if (laserSpawner != null)
            {
                laserSpawner.StopSpawning();
            }

            // Disable all damage to the player
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.isInvincible = true;
            }

            // Display "YOU WIN!" on UI
            if (uiManager != null)
            {
                uiManager.ShowWinScreen();
            }
        }
    }
}
