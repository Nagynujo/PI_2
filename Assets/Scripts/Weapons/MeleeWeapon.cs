using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeWeapon : MonoBehaviour
{
   
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 2.0f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Animator animator;
    

    private PlayerPoints playerPoints;

    [Header("Light Attack")]
    [SerializeField] private int lightDamage = 1;
    [SerializeField] private float lightCooldown = 0.3f;
    [SerializeField] private float lightKnockback = 5f;

    [Header("Heavy Attack")]
    [SerializeField] private int heavyDamage = 3;
    [SerializeField] private float heavyCooldown = 0.8f;
    [SerializeField] private float heavyKnockback = 10f;


    private float nextLightTime;
    private float nextHeavyTime;

    private InputAction lightAction;
    private InputAction heavyAction;

    void Awake()
    {
        lightAction = new InputAction("Light", InputActionType.Button, "<Mouse>/leftButton");
        heavyAction = new InputAction("Heavy", InputActionType.Button, "<Mouse>/rightButton");
    }

    void Start()
    {
        playerPoints = FindObjectOfType<PlayerPoints>();
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
        nextLightTime = Time.time + lightCooldown;
        Debug.Log("Light Attack");

        DealLightDamage(); 
    }

    if (heavyAction.WasPressedThisFrame() && Time.time >= nextHeavyTime)
    {
        nextHeavyTime = Time.time + heavyCooldown;
        Debug.Log("Heavy Attack");

        DealHeavyDamage(); 
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
            
            enemy.SendMessage(
                "TakeDamage",
                damage,
                SendMessageOptions.DontRequireReceiver
            );

            
            Vector2 direction = 
                (enemy.transform.position - transform.position).normalized;

           
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