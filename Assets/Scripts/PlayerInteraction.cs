using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionRange = 2.5f;
    [SerializeField] private LayerMask hiddenObjectLayer;

    [Header("UI Prompt")]
    [SerializeField] private TextMeshProUGUI promptText;

    private HiddenObject currentTarget;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        CheckForInteractable();

        // Collect object when E is pressed
        if (currentTarget != null && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            currentTarget.Collect();
            ClearTarget();
        }
    }

    // Checks for interactable hidden objects within the specified interaction range and updates the UI prompt accordingly
    private void CheckForInteractable()
    {
        // Finds all colliders within radius on the hidden object layer
        Collider[] candidates = Physics.OverlapSphere(transform.position, interactionRange, hiddenObjectLayer);

        HiddenObject closestRevealedObj = null;
        float closestDistance = float.MaxValue;

        // Iterate through candidates to find the closest revealed hidden object
        foreach (Collider col in candidates)
        {
            if (col.TryGetComponent<HiddenObject>(out HiddenObject hiddenObj))
            {
                // Only interact if flashlight is actively revealing the item
                if (hiddenObj.IsRevealed)
                {
                    float distance = Vector3.Distance(transform.position, col.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestRevealedObj = hiddenObj;
                    }
                }
            }
        }

        // Display prompt if a valid target is within radius
        if (closestRevealedObj != null)
        {
            currentTarget = closestRevealedObj;
            if (promptText != null)
            {
                promptText.text = "Press [E] to Interact";
                promptText.gameObject.SetActive(true);
            }
        }
        else
        {
            ClearTarget();
        }
    }

    // Clears the current target and hides the interaction prompt
    private void ClearTarget()
    {
        currentTarget = null;
        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }
    }

    // Visualizes the interaction range in the Scene view for easier debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
