using System.Collections.Generic;
using UnityEngine;

public class ResetObjectsTrigger : MonoBehaviour
{
    private GameObject[] cans;
    private List<Vector3> startPositions = new List<Vector3>();
    private List<Quaternion> startRotations = new List<Quaternion>();

    private void Start()
    {
        cans = GameObject.FindGameObjectsWithTag("Can");

        // Store the starting position and rotation for each can
        foreach (GameObject can in cans)
        {
            if (can != null)
            {
                startPositions.Add(can.transform.position);
                startRotations.Add(can.transform.rotation);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ResetCans();
        }
    }

    public void ResetCans()
    {
        for (int i = 0; i < cans.Length; i++)
        {
            if (cans[i] == null) continue;

            // 2. Clear physics movement so they don't keep moving
            Rigidbody rb = cans[i].GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero; // Use rb.velocity on Unity 2022 or older
                rb.angularVelocity = Vector3.zero;
            }

            // 1. Move position and rotation back to start
            cans[i].transform.position = startPositions[i];
            cans[i].transform.rotation = startRotations[i];
        }
    }
}