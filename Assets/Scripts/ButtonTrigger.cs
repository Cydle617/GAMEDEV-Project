using System.Collections;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject obj;
    [SerializeField]
    Vector3 targetOffset = new Vector3(0, 0f, 0);
    [SerializeField] 
    private float duration = 1.0f;

    private Vector3 startPos;
    private Vector3 targetPos;
    private Coroutine moveCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = obj.transform.position;
        targetPos = startPos + targetOffset;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartMove(targetPos);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartMove(startPos);
        }
    }

    private void StartMove(Vector3 destination)
    {
        // Stop active movement to prevent overlapping transitions if the trigger is rapidly toggled
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = StartCoroutine(MoveToPosition(destination));
    }

    private IEnumerator MoveToPosition(Vector3 destination)
    {
        Vector3 initialPos = obj.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Smoothly interpolate between the current position and target position
            obj.transform.position = Vector3.Lerp(initialPos, destination, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Snap to destination to ensure precision at the end of movement
        obj.transform.position = destination;
    }
}