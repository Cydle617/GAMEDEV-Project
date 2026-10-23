using UnityEngine;

public class HealingTrigger : MonoBehaviour
{
    [SerializeField] private int healAmount = 25;
    [SerializeField] private bool destroyOnUse = true;

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
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.Heal(healAmount);

                if (destroyOnUse)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}