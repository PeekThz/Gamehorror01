using UnityEngine;
public class PlayerRoot : MonoBehaviour
{
    [Header("Stamina")]
    public float maxStamina = 100f;
    public float currentStamina;

    public float drainRate = 20f;
    public float regenRate = 10f;

    public bool IsExhausted => currentStamina <= 0;

    void Start()
    {
        currentStamina = maxStamina;
    }

    public void UseStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }

    public void RecoverStamina(float amount)
    {
        currentStamina += amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }
}