using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private Image fillImage;
    [SerializeField] private Color highColor = Color.green;
    [SerializeField] private Color lowColor = Color.red;

    [Header("Smooth")]
    [SerializeField] private float smoothSpeed = 10f;

    private float targetValue;

    void Update()
    {
        if (player == null) return;

        targetValue = player.GetStaminaNormalized();
        fillImage.color = Color.Lerp(lowColor, highColor, targetValue);

        if (targetValue < 0.2f)
        {
            float pulse = Mathf.PingPong(Time.time * 5f, 1f);
            fillImage.color = Color.Lerp(Color.red, Color.white, pulse);
        }

        staminaSlider.value = Mathf.Lerp(
            staminaSlider.value,
            targetValue,
            Time.deltaTime * smoothSpeed
        );
    }
}