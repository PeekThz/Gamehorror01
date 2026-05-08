using UnityEngine;

public class PlayerHide : MonoBehaviour
{
    [Header("References")]
    public GameObject playerVisual;
    public Rigidbody2D rb;
    public PlayerMovement movement;
    public Collider2D playerCollider;

    [Header("State")]
    public bool isHidden = false;

    private HideLocker nearbyLocker;
    private HideLocker currentLocker;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (movement == null) movement = GetComponent<PlayerMovement>();
        if (playerCollider == null) playerCollider = GetComponent<Collider2D>();
    }

    public void HideOrExit()
    {
        if (!isHidden)
        {
            TryHide();
        }
        else
        {
            ExitHide();
        }
    }

    private void TryHide()
    {
        if (nearbyLocker == null || nearbyLocker.hidePoint == null) return;

        isHidden = true;
        currentLocker = nearbyLocker;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        if (movement != null)
        {
            movement.MoveButtonUp();
            movement.AimButtonUp();
            movement.enabled = false;
        }

        transform.position = nearbyLocker.hidePoint.position;

        if (playerCollider != null)
            playerCollider.enabled = false;

        if (playerVisual != null)
            playerVisual.SetActive(false);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayHide();
        }

        EnemyChase enemy = FindFirstObjectByType<EnemyChase>();
        if (enemy != null)
        {
            enemy.PlayerHidden();
        }
    }

    private void ExitHide()
    {
        isHidden = false;
        MinigameManager.Instance.StopLoop();

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        if (movement != null)
            movement.enabled = true;

        if (playerCollider != null)
            playerCollider.enabled = true;

        if (playerVisual != null)
            playerVisual.SetActive(true);

        currentLocker = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HideLocker locker = other.GetComponent<HideLocker>();
        if (locker != null)
        {
            nearbyLocker = locker;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        HideLocker locker = other.GetComponent<HideLocker>();
        if (locker != null && nearbyLocker == locker)
        {
            nearbyLocker = null;
        }
    }
}