using UnityEngine;

public class SimpleLaser : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private float damageCooldown = 0.2f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector3 moveDirection = Vector3.forward;

    [Header("Lifetime Settings")]
    [SerializeField] private float maxDistance = 100f;

    private float damageTimer = 0f;
    private Vector3 startPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // Start ready to damage instantly on first touch
        damageTimer = damageCooldown;
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Move continuously in world space along specified direction
        transform.Translate(moveDirection.normalized * moveSpeed * Time.deltaTime, Space.World);

        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            damageTimer += Time.deltaTime;

            if (damageTimer >= damageCooldown)
            {
                PlayerHealth health = other.GetComponent<PlayerHealth>();
                if (health != null)
                {
                    health.TakeDamage(damageAmount);
                }

                // Reset timer to start the cooldown
                damageTimer = 0f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Reset timer so entering another laser (or re-entering) deals instant damage again
            damageTimer = damageCooldown;
        }
    }
}