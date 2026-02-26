using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{
    private float horizontalInput;
    private Rigidbody2D rb;

    [SerializeField] int velocity = 4;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
        {
            rb.AddForce(Vector2.up * 300);
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * velocity, rb.linearVelocity.y);
    }
}
