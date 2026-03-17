using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{
    private Rigidbody2D rb;

    private InputAction moveAction;
    private InputAction jumpAction;

    private float horizontalInput;
    private bool isGrounded;

    [SerializeField] int velocity = 4;
    [SerializeField] float jumpForce = 300f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;

    public bool isLocked;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        
        moveAction = new InputAction(
            "Move",
            InputActionType.Value,
            "<Keyboard>/leftArrow"
        );
        moveAction.AddCompositeBinding("2DVector")
            .With("Left", "<Keyboard>/a")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/d")
            .With("Right", "<Keyboard>/rightArrow");

        // Pulo
        jumpAction = new InputAction(
            "Jump",
            InputActionType.Button,
            "<Keyboard>/space"
        );
    }

    void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
    }

    void Update()
    {
       
        horizontalInput = moveAction.ReadValue<Vector2>().x;

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            0.2f,
            groundLayer
        );

        if (jumpAction.WasPressedThisFrame() && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce);
        }
    }

    void FixedUpdate()
    {
       if (!isLocked)
        {
            rb.linearVelocity = new Vector2(
            horizontalInput * velocity,
            rb.linearVelocity.y
            );
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }
}