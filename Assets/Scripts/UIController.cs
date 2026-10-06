using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text winText; // UI text for "You Win!"
    [SerializeField] private PlayerHealth playerHealth;

    private float elapsedTime = 0f;
    private bool isGameWon = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (winText != null)
        {
            winText.gameObject.SetActive(false); // Hide text at start
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameWon)
        {
            elapsedTime += Time.deltaTime;
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            int minutes = Mathf.FloorToInt(elapsedTime / 60);

            if (timerText != null)
            {
                timerText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
            }
        }

        // 2. Update Health Display
        if (playerHealth != null && healthText != null)
        {
            healthText.text = "Health: " + playerHealth.currentHealth;
        }
    }

    public void ShowWinScreen()
    {
        isGameWon = true; // Freezes the timer in Update()

        // Calculate final completion time
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        string finalTimeFormatted = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (winText != null)
        {
            winText.text = string.Format("YOU WIN!\nFinal Time: {0}", finalTimeFormatted);
            winText.gameObject.SetActive(true);
        }
    }
}
