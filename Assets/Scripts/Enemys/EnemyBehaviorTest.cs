using UnityEngine;

public class EnemyBehaviorTest : MonoBehaviour
{
        [SerializeField] private int maxHealth = 5;
    private int currentHealth;

    
    [SerializeField] private float knockbackDuration = 0.2f;

    
    [SerializeField] private int pointsGiven = 10;

    private Rigidbody2D rb;
    private Animator animator;

    private bool isDead;
    private float knockbackTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;
    }

    void Update()
    {
        if (knockbackTimer > 0)
        {
            knockbackTimer -= Time.deltaTime;
        }
    }

   
    public void TakeDamage(int damage)
    {
        Debug.Log("ENEMY TOOK DAMAGE");

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    
    public void ApplyKnockback(Vector2 force)
    {
        if (isDead) return;

        rb.linearVelocity = Vector2.zero;

        force.y = 2f;

        rb.AddForce(force, ForceMode2D.Impulse);
    }

    
    void Die()
    {
        if (isDead) return;

        isDead = true;

        Debug.Log(name + " morreu");

        GivePoints();

        // animação opcional
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        Destroy(gameObject, 0.2f);
    }

    
    void GivePoints()
    {
        PlayerPoints player = FindObjectOfType<PlayerPoints>();

        if (player != null)
        {
            player.AddPoints(pointsGiven);
        }
    }
}