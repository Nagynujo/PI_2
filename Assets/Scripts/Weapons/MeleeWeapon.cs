using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeWeapon : MonoBehaviour
{
  
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float attackCooldown = 0.4f;

   
    [SerializeField] private LayerMask enemyLayer;

    
    [SerializeField] private bool showCooldownDebug = true;

    private float nextAttackTime;
    private bool wasOnCooldown;

    void Update()
    {
        bool isOnCooldown = Time.time < nextAttackTime;

        // Debug limpo
        if (showCooldownDebug && isOnCooldown != wasOnCooldown)
        {
            Debug.Log(isOnCooldown ? "Cooldown iniciado" : "Ataque pronto!");
            wasOnCooldown = isOnCooldown;
        }

        // Mouse esquerdo
        if (Mouse.current.leftButton.wasPressedThisFrame && !isOnCooldown)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayer
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.SendMessage(
                "TakeDamage",
                attackDamage,
                SendMessageOptions.DontRequireReceiver
            );
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}