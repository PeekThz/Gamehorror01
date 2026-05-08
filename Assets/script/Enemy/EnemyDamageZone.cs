using UnityEngine;

public class EnemyDamageZone : MonoBehaviour
{
    public int damage = 10;
    public float damageCooldown = 1f;

    private float lastDamageTime;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (Time.time < lastDamageTime + damageCooldown) return;

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            lastDamageTime = Time.time;
        }
    }
}