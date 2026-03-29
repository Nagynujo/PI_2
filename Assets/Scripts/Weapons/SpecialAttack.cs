using UnityEngine;
using UnityEngine.InputSystem;

public class SpecialAttack : MonoBehaviour
{
    enum SpecialState { Idle, Playing, Success, Fail }

    private SpecialState state = SpecialState.Idle;

    [Header("Custo / Cost")]
    [SerializeField] private int cost = 30;

    [Header("Cooldown geral")]
    [SerializeField] private float cooldown = 3f;

    [Header("Timer por tecla")]
    [Tooltip("Tempo em segundos para apertar cada tecla. Configure o mesmo valor em SpecialAttackUI.")]
    [SerializeField] private float timePerKey = 2f;

    [Header("Sequencia")]
    [SerializeField] private Key[] sequence = { Key.A, Key.D, Key.A };

    // Estado interno
    private int   currentIndex;
    private float nextTime;
    private float keyTimer;

    // Referencias
    private InputAction  specialAction;
    private PlayerPoints points;
    private PlayerBehavior player;

    void Awake()
    {
        specialAction = new InputAction("Special", InputActionType.Button, "<Keyboard>/f");
        points = GetComponent<PlayerPoints>();
        player = GetComponent<PlayerBehavior>();
    }

    void OnEnable()  => specialAction.Enable();
    void OnDisable() => specialAction.Disable();

    void Update()
    {
        switch (state)
        {
            case SpecialState.Idle:    CheckStart();    break;
            case SpecialState.Playing: CheckSequence(); break;
        }
    }

    void CheckStart()
    {
        if (!specialAction.WasPressedThisFrame()) return;
        if (Time.time < nextTime)                 return;
        if (!points.UsePoints(cost))              return;
        StartSolo();
    }

    void StartSolo()
    {
        Debug.Log("SOLO START");
        state        = SpecialState.Playing;
        currentIndex = 0;
        player.isLocked = true;
        SpecialAttackUI.Instance?.ShowSolo(sequence.Length);
        ShowCurrentKey();
    }

    void CheckSequence()
    {
        keyTimer -= Time.deltaTime;
        if (keyTimer <= 0f)
        {
            Debug.Log("TEMPO ESGOTADO: " + sequence[currentIndex]);
            Fail();
            return;
        }

        Key expected = sequence[currentIndex];

        if (Keyboard.current[expected].wasPressedThisFrame)
        {
            Debug.Log("Correta: " + expected);
            SpecialAttackUI.Instance?.KeyPressed();
            currentIndex++;

            if (currentIndex >= sequence.Length)
                Success();
            else
                ShowCurrentKey();
        }
        else if (AnyWrongKey())
        {
            Fail();
        }
    }

    void ShowCurrentKey()
    {
        keyTimer = timePerKey;
        string keyName = KeyToDisplay(sequence[currentIndex]);
        SpecialAttackUI.Instance?.ShowKey(keyName, currentIndex);
    }

    bool AnyWrongKey()
    {
        foreach (Key k in System.Enum.GetValues(typeof(Key)))
            if (Keyboard.current[k].wasPressedThisFrame)
                if (currentIndex < sequence.Length && k != sequence[currentIndex])
                    return true;
        return false;
    }

    void Success()
    {
        Debug.Log("SOLO SUCCESS");
        state = SpecialState.Success;
        SpecialAttackUI.Instance?.ShowSuccess();
        DoSpecialEffect();
        EndSolo();
    }

    void Fail()
    {
        Debug.Log("SOLO FAIL");
        state = SpecialState.Fail;
        SpecialAttackUI.Instance?.ShowFail();
        EndSolo();
    }

    void EndSolo()
    {
        player.isLocked = false;
        nextTime        = Time.time + cooldown;
        state           = SpecialState.Idle;
    }

    void DoSpecialEffect()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 3f);
        foreach (Collider2D e in enemies)
            e.SendMessage("TakeDamage", 999, SendMessageOptions.DontRequireReceiver);
    }

    static string KeyToDisplay(Key key) => key switch
    {
        Key.A => "A", Key.B => "B", Key.C => "C", Key.D => "D",
        Key.E => "E", Key.F => "F", Key.G => "G", Key.H => "H",
        Key.I => "I", Key.J => "J", Key.K => "K", Key.L => "L",
        Key.M => "M", Key.N => "N", Key.O => "O", Key.P => "P",
        Key.Q => "Q", Key.R => "R", Key.S => "S", Key.T => "T",
        Key.U => "U", Key.V => "V", Key.W => "W", Key.X => "X",
        Key.Y => "Y", Key.Z => "Z",
        _ => key.ToString()
    };
}