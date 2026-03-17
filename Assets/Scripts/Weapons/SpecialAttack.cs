using UnityEngine;
using UnityEngine.InputSystem;

public class SpecialAttack : MonoBehaviour
{
    enum SpecialState
    {
        Idle,
        Playing,
        Success,
        Fail,
        Cooldown
    }

    private SpecialState state = SpecialState.Idle;

    
    [SerializeField] private int cost = 30;

    
    [SerializeField] private float cooldown = 3f;

    private float nextTime;

   
    [SerializeField] private Key[] sequence =
    {
        Key.A,
        Key.D,
        Key.A
    };

    private int currentIndex;

    private InputAction specialAction;

    private PlayerPoints points;
    private PlayerBehavior player;

    void Awake()
    {
        specialAction = new InputAction(
            "Special",
            InputActionType.Button,
            "<Keyboard>/f"
        );

        points = GetComponent<PlayerPoints>();
        player = GetComponent<PlayerBehavior>();
    }

    void OnEnable() => specialAction.Enable();
    void OnDisable() => specialAction.Disable();

    void Update()
    {
        switch (state)
        {
            case SpecialState.Idle:
                CheckStart();
                break;

            case SpecialState.Playing:
                CheckSequence();
                break;
        }
    }

    

    void CheckStart()
    {
        if (!specialAction.WasPressedThisFrame())
            return;

        if (Time.time < nextTime)
            return;

        if (!points.UsePoints(cost))
            return;

        StartSolo();
    }

    void StartSolo()
    {
        Debug.Log("SOLO START");

        state = SpecialState.Playing;
        currentIndex = 0;

        player.isLocked = true;
    }

    

    void CheckSequence()
    {
        if (currentIndex >= sequence.Length)
        {
            Success();
            return;
        }

        Key expected = sequence[currentIndex];

        if (Keyboard.current[expected].wasPressedThisFrame)
        {
            Debug.Log("Correct " + expected);

            currentIndex++;
        }
        else if (AnyWrongKey())
        {
            Fail();
        }
    }

    bool AnyWrongKey()
    {
        foreach (Key k in System.Enum.GetValues(typeof(Key)))
        {
            if (Keyboard.current[k].wasPressedThisFrame)
            {
                if (currentIndex < sequence.Length &&
                    k != sequence[currentIndex])
                {
                    return true;
                }
            }
        }

        return false;
    }

   

    void Success()
    {
        Debug.Log("SOLO SUCCESS");

        state = SpecialState.Success;

        DoSpecialEffect();

        EndSolo();
    }

   

    void Fail()
    {
        Debug.Log("SOLO FAIL");

        state = SpecialState.Fail;

        EndSolo();
    }

   

    void EndSolo()
    {
        player.isLocked = false;

        nextTime = Time.time + cooldown;

        state = SpecialState.Idle;
    }

   

    void DoSpecialEffect()
    {
        Collider2D[] enemies =
            Physics2D.OverlapCircleAll(
                transform.position,
                3f
            );

        foreach (Collider2D e in enemies)
        {
            e.SendMessage(
                "TakeDamage",
                999,
                SendMessageOptions.DontRequireReceiver
            );
        }
    }
}