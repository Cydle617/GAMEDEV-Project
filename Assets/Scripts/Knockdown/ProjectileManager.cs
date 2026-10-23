using UnityEngine;

public class ItemProjectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Automatically destroy after specified seconds
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
