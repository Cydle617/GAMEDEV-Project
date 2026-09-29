using UnityEngine;

public class ResetTrigger : MonoBehaviour
{
    private GameObject[] platforms;
    private Vector3[] startPositions;
    private Quaternion[] startRotations;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Finds all platforms that has a "Platform" tag and saves their
        // initial positions and rotations for resetting later using vector3
        // and quaternion (4D vector) arrays
        platforms = GameObject.FindGameObjectsWithTag("Platform");
        startPositions = new Vector3[platforms.Length];
        startRotations = new Quaternion[platforms.Length];

        for (int i = 0; i < platforms.Length; i++)
        {
            // Saves the platforms' initial positions and rotations
            startPositions[i] = platforms[i].transform.position;
            startRotations[i] = platforms[i].transform.rotation;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // When triggered, this resets the position and rotation for each platform
            for (int i = 0; i < platforms.Length; i++)
            {
                platforms[i].transform.position = startPositions[i];
                platforms[i].transform.rotation = startRotations[i];
            }
        }
    }
}