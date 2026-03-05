using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeWeapon : MonoBehaviour
{
   
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Animator animator;

    [Header("Light Attack")]
    [SerializeField] private int lightDamage = 1;
    [SerializeField] private float lightCooldown = 0.3f;
    [SerializeField] private float lightKnockback = 3f;

    [Header("Heavy Attack")]
    [SerializeField] private int heavyDamage = 3;
    [SerializeField] private float heavyCooldown = 0.8f;
    [SerializeField] private float heavyKnockback = 6f;

    private float nextLightTime;
    private float nextHeavyTime;

    private InputAction lightAction;
    private InputAction heavyAction;

    void Awake()
    {
        lightAction = new InputAction("Light", InputActionType.Button, "<Mouse>/leftButton");
        heavyAction = new InputAction("Heavy", InputActionType.Button, "<Mouse>/rightButton");
    }

    void OnEnable()
    {
        lightAction.Enable();
        heavyAction.Enable();
    }

    void OnDisable()
    {
        lightAction.Disable();
        heavyAction.Disable();
    }

    void Update()
    {
        if (lightAction.WasPressedThisFrame() && Time.time >= nextLightTime)
        {
            //animator.SetTrigger("LightAttack");
            nextLightTime = Time.time + lightCooldown;
            Debug.Log("Light Attack");
        }

        if (heavyAction.WasPressedThisFrame() && Time.time >= nextHeavyTime)
        {
            //animator.SetTrigger("HeavyAttack");
            nextHeavyTime = Time.time + heavyCooldown;
             Debug.Log("Heavy Attack");
        }
    }

   
    public void DealLightDamage()
    {
        DealDamage(lightDamage, lightKnockback);
    }

    
    public void DealHeavyDamage()
    {
        DealDamage(heavyDamage, heavyKnockback);
    }

    void DealDamage(int damage, float knockbackForce)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayer
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            // Dano
            enemy.SendMessage(
                "TakeDamage",
                damage,
                SendMessageOptions.DontRequireReceiver
            );

            // Direção do knockback
            Vector2 direction = 
                (enemy.transform.position - transform.position).normalized;

            // Aplica força
            enemy.SendMessage(
                "ApplyKnockback",
                direction * knockbackForce,
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