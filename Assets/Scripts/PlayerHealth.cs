using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 50f;
    public float currentHealth;
    public bool isInvincible = false; // Flag to disable incoming damage

    [Header("Damage Settings")]
    public float damageCooldown = 1.0f; // Time in seconds before taking damage again
    private float nextDamageTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float amount)
    {
        if (isInvincible) return;

        // If the current game time hasn't reached the next allowed damage time, skip!
        if (Time.time < nextDamageTime) return;

        currentHealth -= amount;
        Debug.Log("Player Health: " + currentHealth);

        // Set the timestamp for when the player can be hurt again
        nextDamageTime = Time.time + damageCooldown;

        if (currentHealth <= 0)
        {
            ResetLevel();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}