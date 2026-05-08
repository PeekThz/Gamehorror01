using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damage = 20;
    public float attackCooldown = 1f;

    private float lastAttackTime;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                PlayerHealth health = other.GetComponent<PlayerHealth>();
                if (health != null)
                {
                    health.TakeDamage(damage);
                    lastAttackTime = Time.time;
                }
            }
        }
    }
}