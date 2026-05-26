using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string gameSceneName = "SampleScene";

    // ▶ เริ่มเกม
    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // ❌ ออกจากเกม
    public void QuitGame()
    {
        Application.Quit();

        Debug.Log("QUIT GAME");
    }
}