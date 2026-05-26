using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI")]
    public Slider healthSlider;

    private void Start()
    {
        LoadHealth();

        UpdateHealthUI();
    }

    // =========================
    // DAMAGE
    // =========================

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        currentHealth = Mathf.Clamp(
            currentHealth,
            0,
            maxHealth
        );

        // 🔥 save เลือด
        SaveHealth();

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
        }
    }

    // =========================
    // SAVE / LOAD
    // =========================

    void SaveHealth()
    {
        PlayerPrefs.SetInt(
            "PlayerHealth",
            currentHealth
        );
    }

    void LoadHealth()
    {
        // 🔥 ถ้ายังไม่เคย save
        if (PlayerPrefs.HasKey("PlayerHealth"))
        {
            currentHealth =
                PlayerPrefs.GetInt("PlayerHealth");
        }
        else
        {
            currentHealth = maxHealth;

            SaveHealth();
        }
    }

    // =========================
    // UI
    // =========================

    void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;

            healthSlider.value = currentHealth;
        }
    }

    // =========================
    // HEAL
    // =========================

    public void Heal(int amount)
    {
        currentHealth += amount;

        currentHealth = Mathf.Clamp(
            currentHealth,
            0,
            maxHealth
        );

        SaveHealth();

        UpdateHealthUI();
    }

    // =========================
    // RESET
    // =========================

    public void ResetHealth()
    {
        currentHealth = maxHealth;

        SaveHealth();

        UpdateHealthUI();
    }
}