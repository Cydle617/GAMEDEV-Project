using UnityEngine;

public class ObjectTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject textObject;
    /*
    [SerializeField]
    private bool hideOnExit = false;
    */

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
            // Enables and/or shows the textObject when the player stands on the trigger area
            textObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        /* 
        if (other.CompareTag("Player") && hideOnExit)
        {
            textObject.SetActive(false);
        }
        */
    }
}