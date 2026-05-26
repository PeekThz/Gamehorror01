using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public string itemID;

    private bool isCollected = false;

    void Start()
    {
        // 🔥 ถ้าเคยเก็บแล้ว
        if (PlayerPrefs.GetInt(itemID, 0) == 1)
        {
            gameObject.SetActive(false);
        }
    }

    public void TryCollect()
    {
        if (isCollected) return;

        isCollected = true;

        // 🔥 save ว่าเก็บแล้ว
        PlayerPrefs.SetInt(itemID, 1);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.CollectItem();
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPickup();
        }

        gameObject.SetActive(false);
    }
}