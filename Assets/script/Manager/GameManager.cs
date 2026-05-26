using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Collect")]
    public int totalCollectibles = 5;
    public int collectedCount = 0;
    public TextMeshProUGUI collectText;

    [Header("Panels")]
    public GameObject winPanel;
    public GameObject gameOverPanel;

    [Header("Enemy")]
    public EnemyChase enemy;

    private bool gameEnded = false;

    private void Awake()
    {
        // ✅ Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // 🔥 โหลดค่า
        LoadData();

        UpdateCollectUI();

        if (winPanel != null)
            winPanel.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    // =========================
    // COLLECT
    // =========================

    public void CollectItem()
    {
        if (gameEnded) return;

        collectedCount++;

        // 🔥 save
        SaveData();

        UpdateCollectUI();

        if (enemy != null)
        {
            enemy.SpawnAndChase();
        }

        if (collectedCount >= totalCollectibles)
        {
            WinGame();
        }
    }

    void UpdateCollectUI()
    {
        if (collectText != null)
        {
            collectText.text =
                collectedCount + " / " + totalCollectibles;
        }
    }

    // =========================
    // WIN
    // =========================

    public void WinGame()
    {
        if (gameEnded) return;

        gameEnded = true;

        if (winPanel != null)
            winPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    // =========================
    // GAME OVER
    // =========================

    public void GameOver()
    {
        if (gameEnded) return;

        gameEnded = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    // =========================
    // RESTART
    // =========================

    public void RestartGame()
    {
        Time.timeScale = 1f;

        // 🔥 ล้างค่า spawn ตอนรีเกม
        PlayerPrefs.DeleteKey("SpawnPoint");
        PlayerPrefs.DeleteKey("CollectedCount");
        PlayerPrefs.DeleteKey("PlayerHealth");

        PlayerPrefs.DeleteKey("Item_1");
        PlayerPrefs.DeleteKey("Item_2");
        PlayerPrefs.DeleteKey("Item_3");
        PlayerPrefs.DeleteKey("Item_4");
        PlayerPrefs.DeleteKey("Item_5");

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    // =========================
    // QUIT
    // =========================

    public void QuitGame()
    {
        Debug.Log("Quit Game");

        Application.Quit();
    }
    void SaveData()
    {
        PlayerPrefs.SetInt(
            "CollectedCount",
            collectedCount
        );
    }

    void LoadData()
    {
        collectedCount =
            PlayerPrefs.GetInt("CollectedCount", 0);
    }
}