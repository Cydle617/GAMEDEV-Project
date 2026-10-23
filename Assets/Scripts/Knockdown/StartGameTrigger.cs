using UnityEngine;

public class StartGameTrigger : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private ScoreManager.GameMode modeToStart = ScoreManager.GameMode.Static;
    [SerializeField] private string playerTag = "Player";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (scoreManager == null)
        {
            scoreManager = FindAnyObjectByType<ScoreManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (scoreManager != null)
            {
                scoreManager.StartGame(modeToStart);
            }
        }
    }
}
