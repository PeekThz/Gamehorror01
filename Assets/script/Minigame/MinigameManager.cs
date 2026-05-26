using System.Collections;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance;

    [Header("UI")]
    public GameObject minigameUI;
    public TimingBar timingBar;
    public ResultSystem resultSystem;

    [Header("Player")]
    public PlayerHide playerHide;

    [Header("Enemy")]
    public EnemyChase enemy;

    [Header("Loop")]
    public float minDelay = 4f;
    public float maxDelay = 7f;

    [Header("Minigame Timer")]
    public float minigameTimeLimit = 5f;

    private bool isPlaying = false;
    private bool loopRunning = false;

    private int difficultyLevel = 0;

    private HideLocker currentLocker;

    private Coroutine timerCoroutine;

    void Awake()
    {
        Instance = this;
    }

    // =========================
    // START GAME
    // =========================

    public void StartGame()
    {
        if (isPlaying) return;

        Debug.Log("START MINIGAME");

        minigameUI.SetActive(true);

        timingBar.StartBar();

        isPlaying = true;

        // ⏱ เริ่มจับเวลา
        timerCoroutine = StartCoroutine(MinigameTimer());
    }

    void StartGameWithDifficulty()
    {
        difficultyLevel++;

        // ⚡ เพิ่มความยากทีละนิด
        timingBar.SetDifficulty(difficultyLevel);

        // ⏱ เวลาน้อยลงเรื่อย ๆ
        minigameTimeLimit = Mathf.Clamp(
            5f - (difficultyLevel * 0.3f),
            2f,
            5f
        );

        StartGame();
    }

    // =========================
    // PLAYER TAP
    // =========================

    public void OnTap()
    {
        if (!isPlaying) return;

        // ⛔ หยุด timer
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }

        bool success = timingBar.CheckZone();

        if (success)
        {
            resultSystem.Success();

            // 👁️ ศัตรูหยุดไล่
            if (enemy != null)
            {
                enemy.PlayerHidden();
            }

            EndGame();
        }
        else
        {
            FailMinigame();
        }
    }

    // =========================
    // FAIL
    // =========================

    void FailMinigame()
    {
        resultSystem.Fail();

        // ❌ เด้งออกจากตู้
        playerHide.HideOrExit();

        // 💀 ศัตรูโผล่
        if (enemy != null)
        {
            enemy.SpawnAtRandomPoint();
        }

        // 🚫 ล็อกตู้
        if (currentLocker != null)
        {
            currentLocker.LockLocker();
        }

        EndGame();
    }

    // =========================
    // END GAME
    // =========================

    void EndGame()
    {
        isPlaying = false;

        minigameUI.SetActive(false);
    }

    // =========================
    // MINIGAME TIMER
    // =========================

    IEnumerator MinigameTimer()
    {
        float timer = minigameTimeLimit;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;

            yield return null;
        }

        // ⏰ หมดเวลา
        if (isPlaying)
        {
            Debug.Log("TIME OUT");

            FailMinigame();
        }
    }

    // =========================
    // LOOP SYSTEM
    // =========================

    IEnumerator MinigameLoop()
    {
        loopRunning = true;

        while (playerHide.isHidden)
        {
            float wait = Random.Range(minDelay, maxDelay);

            // 🔥 ยิ่งอยู่นานยิ่งถี่ขึ้น
            wait -= difficultyLevel * 0.08f;

            wait = Mathf.Clamp(wait, 1.5f, maxDelay);

            yield return new WaitForSeconds(wait);

            // ถ้ามีเกมอยู่แล้ว ข้าม
            if (isPlaying) continue;

            StartGameWithDifficulty();
        }

        loopRunning = false;
    }

    public IEnumerator StartLoop()
    {
        if (!loopRunning)
        {
            yield return StartCoroutine(MinigameLoop());
        }
    }

    public void StopLoop()
    {
        StopAllCoroutines();

        loopRunning = false;

        difficultyLevel = 0;

        isPlaying = false;

        minigameUI.SetActive(false);
    }

    // =========================
    // LOCKER
    // =========================

    public void SetCurrentLocker(HideLocker locker)
    {
        currentLocker = locker;
    }

    // =========================
    // HELPER
    // =========================

    public bool IsPlaying()
    {
        return isPlaying;
    }
}