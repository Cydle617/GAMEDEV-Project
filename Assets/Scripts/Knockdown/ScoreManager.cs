using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public enum GameMode { Static, Moving }

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gameTimerText;
    [SerializeField] private TextMeshProUGUI throwCountText;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Triggers & Buttons to Hide")]
    [Tooltip("Drag all start triggers or physical buttons here")]
    [SerializeField] private GameObject[] objectsToHide;

    [Header("Script References")]
    [SerializeField] private PlayerThrow playerThrow;
    [SerializeField] private ResetObjectsTrigger resetObjectsTrigger;

    [Header("Score Settings")]
    [SerializeField] private int targetScore = 20;
    [SerializeField] private float autoRestartDelay = 10f;

    private int currentScore = 0;
    private int projectilesThrown = 0;
    private float elapsedTime = 0f;
    private HashSet<GameObject> scoredCans = new HashSet<GameObject>();
    private bool isGameStarted = false;
    private bool hasWon = false;
    private GameMode currentMode = GameMode.Static;

    private void Start()
    {
        if (winPanel != null) winPanel.SetActive(false);
        if (playerThrow == null) playerThrow = FindAnyObjectByType<PlayerThrow>();
        if (resetObjectsTrigger == null) resetObjectsTrigger = FindAnyObjectByType<ResetObjectsTrigger>();     

        PrepareGameForStart();
    }

    private void Update()
    {
        // Increment timer continuously while game is active
        if (isGameStarted && !hasWon)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    public void PrepareGameForStart()
    {
        isGameStarted = false;
        hasWon = false;

        // Reset tracking variables
        elapsedTime = 0f;
        projectilesThrown = 0;

        if (winPanel != null) winPanel.SetActive(false);
        if (playerThrow != null) playerThrow.enabled = false;
        if (resetObjectsTrigger != null) resetObjectsTrigger.ResetCans();

        ToggleTriggers(true);
        ToggleTargetMovement(false);

        UpdateUI();
        UpdateTimerUI();
        UpdateThrowUI();
    }

    // Called by Button / Trigger A
    public void StartStaticMode()
    {
        StartGame(GameMode.Static);
    }

    // Called by Button / Trigger B
    public void StartMovingMode()
    {
        StartGame(GameMode.Moving);
    }

    public void StartGame(GameMode mode)
    {
        StopAllCoroutines();

        currentMode = mode;
        isGameStarted = true;
        hasWon = false;

        // Reset stats for the new round
        elapsedTime = 0f;
        projectilesThrown = 0;
        currentScore = 0;
        scoredCans.Clear();

        if (winPanel != null) winPanel.SetActive(false);
        if (playerThrow != null) playerThrow.enabled = true;
        if (resetObjectsTrigger != null) resetObjectsTrigger.ResetCans();

        ToggleTriggers(false);
        // Enable or disable target movement based on selected mode
        ToggleTargetMovement(mode == GameMode.Moving);

        UpdateUI();
        UpdateTimerUI();
        UpdateThrowUI();
    }

    public void RegisterThrow()
    {
        if (!isGameStarted || hasWon) return;

        projectilesThrown++;
        UpdateThrowUI();
    }

    private void ToggleTriggers(bool show)
    {
        if (objectsToHide == null) return;

        foreach (GameObject obj in objectsToHide)
        {
            if (obj != null) obj.SetActive(show);
        }
    }

    private void ToggleTargetMovement(bool enable)
    {
        MovingTarget[] movingTargets = FindObjectsByType<MovingTarget>(FindObjectsSortMode.None);
        foreach (MovingTarget target in movingTargets)
        {
            target.SetMovementState(enable);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isGameStarted || hasWon) return;

        if (other.CompareTag("Can") && !scoredCans.Contains(other.gameObject))
        {
            scoredCans.Add(other.gameObject);
            currentScore += 1;
            UpdateUI();

            if (currentScore >= targetScore) WinGame();
        }
    }

    private void WinGame()
    {
        hasWon = true;
        isGameStarted = false;

        if (winPanel != null) winPanel.SetActive(true);
        if (playerThrow != null) playerThrow.enabled = false;

        ToggleTargetMovement(false);
        StartCoroutine(AutoRestartRoutine());
    }

    private IEnumerator AutoRestartRoutine()
    {
        float timeRemaining = autoRestartDelay;

        while (timeRemaining > 0)
        {
            if (timerText != null)
            {
                timerText.text = $"Restarting in {Mathf.CeilToInt(timeRemaining)}s...";
            }

            yield return new WaitForSeconds(1f);
            timeRemaining -= 1f;
        }

        PrepareGameForStart();
    }

    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = isGameStarted
                ? $"[{currentMode} Mode]\nScore: {currentScore} / {targetScore}"
                : "Step on a trigger to choose \nyour gamemode!";
        }
    }

    private void UpdateTimerUI()
    {
        if (gameTimerText != null)
        {
            int minutes = Mathf.FloorToInt(elapsedTime / 60F);
            int seconds = Mathf.FloorToInt(elapsedTime % 60F);
            gameTimerText.text = $"Time: {minutes:00}:{seconds:00}";
        }
    }

    private void UpdateThrowUI()
    {
        if (throwCountText != null)
        {
            throwCountText.text = $"Throws: {projectilesThrown}";
        }
    }
}
