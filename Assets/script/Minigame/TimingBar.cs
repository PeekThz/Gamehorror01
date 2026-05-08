using UnityEngine;

public class TimingBar : MonoBehaviour
{
    public RectTransform cursor;
    public RectTransform successZone;

    public float speed = 800f;

    private bool movingRight = true;
    private bool isRunning = false;

    public float leftLimit = -300f;
    public float rightLimit = 300f;

    public void StartBar()
    {
        isRunning = true;
        cursor.anchoredPosition = new Vector2(leftLimit, 0);

        RandomizeZone();
    }

    void Update()
    {
        if (!isRunning) return;

        float move = speed * Time.deltaTime;

        if (movingRight)
            cursor.anchoredPosition += Vector2.right * move;
        else
            cursor.anchoredPosition += Vector2.left * move;

        if (cursor.anchoredPosition.x > rightLimit)
            movingRight = false;

        if (cursor.anchoredPosition.x < leftLimit)
            movingRight = true;
    }

    void RandomizeZone()
    {
        float randomX = Random.Range(-200f, 200f);
        successZone.anchoredPosition = new Vector2(randomX, 0);
    }

    public bool CheckZone()
    {
        float cursorX = cursor.anchoredPosition.x;
        float zoneX = successZone.anchoredPosition.x;

        float halfWidth = successZone.rect.width / 2f;

        return Mathf.Abs(cursorX - zoneX) <= halfWidth;
    }

    public void SetDifficulty(int level)
    {
        // ⚡ ความเร็วเพิ่ม
        speed = 600f + (level * 100f);

        // 🎯 โซนเล็กลง
        float newWidth = Mathf.Clamp(120f - (level * 10f), 40f, 120f);
        successZone.sizeDelta = new Vector2(newWidth, successZone.sizeDelta.y);
    }
}