using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    public SoundSystem soundSystem;

    [SerializeField] private Animator animator;

    private float aniMovmentX;
    private float aniMovmentY;

    float user_Vertical_Input;
    float user_Horizontal_Input;

    private bool user_can_Move = true;
    private bool coroutine_Running = false;

    [HideInInspector] public bool int_NPC;
    [HideInInspector] public bool not_In_Screen;
    [HideInInspector] public bool player_Next_to_NPC;

    public float stepping_Distens;
    public float movement_Speed;
    public float movement_Cooldown = 1f;

    [Space]

    private RaycastHit2D hit_up;
    private RaycastHit2D hit_down;
    private RaycastHit2D hit_left;
    private RaycastHit2D hit_right;

    public RaycastHit2D hit;

    private Vector2 facingDirection = Vector2.down;

    private Vector3 newPosition;

    public bool there_is_Monstors;

    private void Awake()
    {
        player_Next_to_NPC = false;
        animator = GetComponent<Animator>(); // FIX: was never assigned, caused NullReferenceException
    }

    void Update()
    {
        PlayerInput();

        if (not_In_Screen) return;

        if (user_can_Move)
            PlayerMovement();
        else
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movement_Speed);

        hit = Physics2D.Raycast(transform.position, facingDirection, stepping_Distens);
        Debug.DrawRay(transform.position, facingDirection * stepping_Distens, Color.green);
    }

    void PlayerInput()
    {
        Vector2 moveInput = new Vector2(
            (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed ? 1f : 0f) -
            (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed ? 1f : 0f),
            (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed ? 1f : 0f) -
            (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed ? 1f : 0f)
        );

        user_Horizontal_Input = moveInput.x;
        user_Vertical_Input = moveInput.y;
    }

    void WallDetector()
    {
        hit_up = Physics2D.Raycast(transform.position, Vector2.up, stepping_Distens);
        hit_down = Physics2D.Raycast(transform.position, Vector2.down, stepping_Distens);
        hit_left = Physics2D.Raycast(transform.position, Vector2.left, stepping_Distens);
        hit_right = Physics2D.Raycast(transform.position, Vector2.right, stepping_Distens);

        Debug.DrawRay(transform.position, Vector2.up * stepping_Distens, Color.red);
        Debug.DrawRay(transform.position, Vector2.down * stepping_Distens, Color.red);
        Debug.DrawRay(transform.position, Vector2.left * stepping_Distens, Color.red);
        Debug.DrawRay(transform.position, Vector2.right * stepping_Distens, Color.red);
    }

    void PlayerMovement()
    {
        WallDetector();

        newPosition = transform.position;

        if (user_Horizontal_Input > 0 && (hit_right.collider == null || !hit_right.collider.CompareTag("wall")))
        {
            newPosition.x += stepping_Distens;
            facingDirection = Vector2.right;
            animatorValues(0, -1); // blend tree: walk right = X:-1, Y:0
        }
        // LEFT movement checks hit_right — should be hit_left
        else if (user_Horizontal_Input < 0 && (hit_left.collider == null || !hit_left.collider.CompareTag("wall")))
        {
            newPosition.x -= stepping_Distens;
            facingDirection = Vector2.left;
            animatorValues(0, 1); // blend tree: walk left = X:1, Y:0
        }
        else if (user_Vertical_Input > 0 && (hit_up.collider == null || !hit_up.collider.CompareTag("wall")))
        {
            newPosition.y += stepping_Distens;
            facingDirection = Vector2.up;
            animatorValues(-1, 0); // blend tree: walk up = X:0, Y:-1
        }
        else if (user_Vertical_Input < 0 && (hit_down.collider == null || !hit_down.collider.CompareTag("wall")))
        {
            newPosition.y -= stepping_Distens;
            facingDirection = Vector2.down;
            animatorValues(1, 0); // blend tree: walk down = X:0, Y:1
        }

        if (newPosition != transform.position && !coroutine_Running)
        {
            soundSystem.activateWalkingSound = true;
            StartCoroutine(MovementCooldown());
        }
    }

    void animatorValues(int y, int x)
    {
        animator.SetFloat("X", x);
        animator.SetFloat("Y", y);
    }

    IEnumerator MovementCooldown()
    {
        coroutine_Running = true;
        user_can_Move = false;

        yield return new WaitForSeconds(movement_Cooldown);

        user_can_Move = true;
        coroutine_Running = false;
        animator.SetFloat("Y", 0);
        animator.SetFloat("X", 0);
    }
}