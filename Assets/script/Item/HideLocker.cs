using System.Collections;
using UnityEngine;

public class HideLocker : MonoBehaviour
{
    public Transform hidePoint;
    public PlayerHide playerHide;
    public bool isLocked = false;
    public float lockTime = 5f;

    [Header("UI")]
    public GameObject hideButton; // ปุ่ม HIDE

    private bool playerInRange = false;

    private void Reset()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.isTrigger = true;
    }

    private void Start()
    {
        if (hideButton != null)
            hideButton.SetActive(false); // ซ่อนตอนเริ่ม
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (hideButton != null)
                hideButton.SetActive(true); // โชว์ปุ่ม
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            // if (hideButton != null)
            // hideButton.SetActive(false); // ซ่อนปุ่ม
        }
    }

    // 👇 เรียกจากปุ่ม UI
    public void OnHidePressed()
    {
        // ❌ ถ้าตู้โดนล็อก → ใช้ไม่ได้
        if (isLocked) return;

        if (playerHide.isHidden)
        {
            playerHide.HideOrExit();
            return;
        }

        if (!playerInRange) return;

        playerHide.HideOrExit();

        MinigameManager.Instance.SetCurrentLocker(this);
        MinigameManager.Instance.StartGame();
        StartCoroutine(MinigameManager.Instance.StartLoop());
    }
    public void LockLocker()
    {
        if (!gameObject.activeInHierarchy) return;
        StartCoroutine(LockRoutine());
    }

    IEnumerator LockRoutine()
    {
        isLocked = true;

        if (hideButton != null)
            hideButton.SetActive(false);

        yield return new WaitForSeconds(lockTime);

        isLocked = false;

        if (playerInRange && hideButton != null)
            hideButton.SetActive(true);
    }
}