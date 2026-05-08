using System.Collections;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance;

    public GameObject minigameUI;
    public TimingBar timingBar;
    public ResultSystem resultSystem;
    public PlayerHide playerHide;

    private bool isPlaying = false;
    public float minDelay = 2f;
    public float maxDelay = 5f;

    private int difficultyLevel = 0;
    private bool loopRunning = false;
    private HideLocker currentLocker;
    public EnemyChase enemy;

    void Awake()
    {
        Instance = this;
    }

    public void StartGame()
    {
        Debug.Log("START MINIGAME"); // 👈
        if (isPlaying) return;
        minigameUI.SetActive(true);
        timingBar.StartBar();
        isPlaying = true;
    }

    void StartGameWithDifficulty()
    {
        difficultyLevel++;

        timingBar.SetDifficulty(difficultyLevel);

        StartGame();
    }

    public void OnTap()
    {
        if (!isPlaying) return;

        bool success = timingBar.CheckZone();

        if (success)
        {
            resultSystem.Success();

            // 👇 เพิ่มอันนี้
            if (enemy != null)
            {
                enemy.PlayerHidden();
            }
        }
        else
        {
            resultSystem.Fail();

            // ❌ เด้งออกจากตู้
            playerHide.HideOrExit();

            // 💀 เรียกศัตรูมาที่ตู้
            if (enemy != null && currentLocker != null)
            {
                enemy.SpawnAtRandomPoint();
            }

            // 🚫 ล็อกตู้
            if (currentLocker != null)
                currentLocker.LockLocker();
        }
        EndGame();
    }

    void EndGame()
    {
        isPlaying = false;
        minigameUI.SetActive(false);
    }
    IEnumerator MinigameLoop()
    {
        loopRunning = true;

        while (playerHide.isHidden)
        {
            float wait = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(wait);

            // ❗ ถ้ามีเกมอยู่แล้ว → ข้าม
            if (isPlaying) continue;

            StartGameWithDifficulty();
        }

        loopRunning = false;
    }

    public IEnumerator StartLoop()
    {
        if (!loopRunning)
            yield return StartCoroutine(MinigameLoop());
    }

    public void StopLoop()
    {
        StopAllCoroutines();
        loopRunning = false;
        difficultyLevel = 0;
    }
    public void SetCurrentLocker(HideLocker locker)
    {
        currentLocker = locker;
    }
    public GameObject enemyPrefab;

    void SpawnEnemyAlert()
    {
        if (enemyPrefab == null) return;

        Vector3 spawnPos = playerHide.transform.position + Vector3.right * 3f;

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}