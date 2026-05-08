using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    private bool isCollected = false;

    public void TryCollect()
    {
        if (isCollected) return;

        isCollected = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.CollectItem();
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPickup();
        }

        Destroy(gameObject);
    }
}