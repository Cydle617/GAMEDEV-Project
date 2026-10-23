using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class HiddenObject : MonoBehaviour
{
    private Renderer objRenderer;
    public bool IsRevealed { get; private set; }

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        objRenderer = GetComponent<Renderer>();
        objRenderer.enabled = false;
    }

    // LateUpdate is called after all Update functions have been called, ensuring the object is only visible if revealed by the flashlight in the current frame
    private void LateUpdate()
    {
        objRenderer.enabled = IsRevealed;
        IsRevealed = false; // Resets every frame unless active flashlight beam hits it
    }

    // Reveals the hidden object, making it visible in the scene
    public void Reveal()
    {
        IsRevealed = true;
    }

    // Collects the hidden object, updating the manager and destroying the object in the scene
    public void Collect()
    {
        if (HiddenObjectManager.Instance != null)
        {
            HiddenObjectManager.Instance.AddCollectedObject();
        }

        Destroy(gameObject);
    }
}
