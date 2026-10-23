using UnityEngine;
using TMPro;

public class HiddenObjectManager : MonoBehaviour
{
    public static HiddenObjectManager Instance { get; private set; }

    [Header("UI & Counter")]
    [SerializeField] private TextMeshProUGUI counterText;

    private int collectedCount = 0;
    private int totalCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        totalCount = FindObjectsByType<HiddenObject>(FindObjectsSortMode.None).Length;
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Increment count of hidden objects collected and update UI
    public void AddCollectedObject()
    {
        collectedCount++;
        UpdateUI();
    }

    // Update UI to reflect current hidden object count
    private void UpdateUI()
    {
        if (counterText != null)
        {
            counterText.text = $"Hidden Objects: {collectedCount} / {totalCount}";
        }
    }
}
