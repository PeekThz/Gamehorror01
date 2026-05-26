using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTransition : MonoBehaviour
{
    [Header("Scene")]
    public string targetSceneName;

    [Header("Spawn")]
    public string targetSpawnID;

    [Header("UI")]
    public GameObject enterButton;

    private bool playerInRange = false;

    void Start()
    {
        if (enterButton != null)
        {
            enterButton.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (enterButton != null)
            {
                enterButton.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (enterButton != null)
            {
                enterButton.SetActive(false);
            }
        }
    }

    public void EnterDoor()
    {
        if (!playerInRange) return;

        // 🔥 จำ spawn ปลายทาง
        PlayerPrefs.SetString(
            "SpawnPoint",
            targetSpawnID
        );

        // 🔥 โหลด Scene
        SceneManager.LoadScene(targetSceneName);
    }
}