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
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        UpdateCollectUI();

        if (winPanel != null)
            winPanel.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void CollectItem()
    {
        if (gameEnded) return;

        collectedCount++;
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

    private void UpdateCollectUI()
    {
        if (collectText != null)
        {
            collectText.text = collectedCount + " / " + totalCollectibles;
        }
    }

    public void WinGame()
    {
        if (gameEnded) return;
        gameEnded = true;

        if (winPanel != null)
            winPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void GameOver()
    {
        if (gameEnded) return;
        gameEnded = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void QuitGame()
{
    Debug.Log("Quit Game");

    Application.Quit();
}

}