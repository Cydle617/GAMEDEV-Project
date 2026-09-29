using System.Collections;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject obj;
    [SerializeField] 
    private Vector3 movementPerSecond = new Vector3(0f, 0f, 0f);  // Movement vector
    [SerializeField] 
    private Vector3 rotationPerSecond = new Vector3(0f, 0f, 0f);  // Rotation vector

    private Vector3 startPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = obj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Moves and rotates every frame the player is standing on the trigger
            obj.transform.Translate(movementPerSecond * Time.deltaTime);
            obj.transform.Rotate(rotationPerSecond * Time.deltaTime);
        }
    }
}