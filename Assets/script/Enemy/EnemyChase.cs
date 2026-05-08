using UnityEngine;
using System.Collections;

public class EnemyChase : MonoBehaviour
{
    public Transform player;
    public Transform[] spawnPoints;
    public float moveSpeed = 2.5f;

    [Header("Leave")]
    public Transform leavePoint;
    public float stopDistance = 0.05f;
    public float waitBeforeLeave = 1.5f;

    [Header("References")]
    public GameObject damageZone;

    private bool isSpawned = false;
    private bool isChasing = false;
    private bool isLeaving = false;
    public float spawnDistance = 4f; // ระยะห่างจาก player

    [Header("Fade Out")]
    public float fadeDuration = 1f;
    public GameObject visualRoot;

    private void Start()
    {
        isSpawned = false;
        isChasing = false;
        isLeaving = false;

        if (damageZone != null)
            damageZone.SetActive(false);

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!isSpawned) return;

        if (isLeaving)
        {
            Debug.Log("isLeaving: " + isLeaving);
            MoveToLeavePoint();
            return;
        }

        if (isChasing && player != null)
        {
            Vector2 targetPos = player.position;
            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            FaceTarget(targetPos.x);
        }
    }

    private void MoveToLeavePoint()
    {
        if (leavePoint == null)
        {
            HideEnemy();
            return;
        }

        float distanceToLeave = Vector2.Distance(transform.position, leavePoint.position);
        Vector2 targetPos = leavePoint.position;
        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        FaceTarget(targetPos.x);
        Debug.Log(Vector2.Distance(transform.position, targetPos));
        if (Vector2.Distance(transform.position, targetPos) > distanceToLeave + 0.1f)
        {
            HideEnemy();
        }
    }

    private void FaceTarget(float targetX)
    {
        Vector3 scale = transform.localScale;

        if (targetX > transform.position.x)
            scale.x = Mathf.Abs(scale.x);
        else if (targetX < transform.position.x)
            scale.x = -Mathf.Abs(scale.x);

        transform.localScale = scale;
    }

    public void SpawnAndChase(Vector3 spawnPosition)
    {
        transform.position = spawnPosition; // 👈 เพิ่ม

        isSpawned = true;
        isChasing = true;
        isLeaving = false;

        gameObject.SetActive(true);

        if (damageZone != null)
            damageZone.SetActive(true);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMonster();
        }
    }

    public void SpawnAndChase()
    {
        SpawnAndChase(transform.position);
    }

    public void PlayerHidden()
    {
        if (!isSpawned) return;

        Debug.Log("ENEMY LEAVING"); // 👈 เช็ค

        StartCoroutine(LeaveAfterDelay());
    }
    private IEnumerator LeaveAfterDelay()
    {
        isChasing = false;

        if (damageZone != null)
            damageZone.SetActive(false);

        yield return new WaitForSeconds(waitBeforeLeave);

        isLeaving = true;
    }

    private void HideEnemy()
    {
        Debug.Log("HIDE ENEMY CALLED");

        isSpawned = false;
        isChasing = false;
        isLeaving = false;

        if (damageZone != null)
            damageZone.SetActive(false);

        gameObject.SetActive(false);

        Debug.Log("ACTIVE: " + gameObject.activeSelf); // 👈 เพิ่ม
    }

    public void SpawnNearPlayer()
    {
        if (player == null) return;

        Vector3 playerPos = player.position;

        // สุ่มซ้ายหรือขวา
        int dir = Random.value > 0.5f ? 1 : -1;

        // วางตำแหน่งด้านข้าง
        Vector3 spawnPos = new Vector3(
            playerPos.x + (spawnDistance * dir),
            playerPos.y,
            playerPos.z
        );

        SpawnAndChase(spawnPos);
    }


    public void SpawnAtRandomPoint()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned!");
            return;
        }

        int index = Random.Range(0, spawnPoints.Length);
        SpawnAndChase(spawnPoints[index].position);
    }

    // IEnumerator FadeOutAndDisable()
    // {
    //     float time = 0f;

    //     Vector3 startScale = visualRoot.localScale;
    //     Vector3 targetScale = startScale * 0.3f;

    //     AudioSource audioSrc = GetComponent<AudioSource>();

    //     while (time < fadeDuration)
    //     {
    //         time += Time.deltaTime;
    //         float t = time / fadeDuration;

    //         // 🔹 scale ค่อย ๆ หด

    //         visualRoot.localScale = Vector3.Lerp(startScale, targetScale, t);

    //         // 🔹 เสียงค่อย ๆ เบา
    //         if (audioSrc != null)
    //         {
    //             audioSrc.volume = Mathf.Lerp(1f, 0f, t);
    //         }

    //         yield return null;
    //     }

    //     // 🔁 reset scale (สำคัญมาก ไม่งั้น spawn ครั้งต่อไปจะตัวเล็ก)
    //     visualRoot.localScale = Vector3.one;

    //     if (audioSrc != null)
    //     {
    //         audioSrc.volume = 1f;
    //     }

    //     // ปิดระบบทั้งหมด
    //     isSpawned = false;
    //     isChasing = false;
    //     isLeaving = false;

    //     if (damageZone != null)
    //         damageZone.SetActive(false);

    //     gameObject.SetActive(false);
    // }
}