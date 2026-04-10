using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    float user_Vertical_Input;
    float user_Horizontal_Input;
    bool user_interact_Input;

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

    private Vector3 newPosition;

    [Space]

    public bool there_is_Monstors;

    private void Awake()
    {
        player_Next_to_NPC = false;
    }

    void Update()
    {
        PlayerInput();

        if (not_In_Screen) return;

        if (user_can_Move)
            PlayerMovement();
        else
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movement_Speed);
    }

    void PlayerInput()
    {
        // New Input System equivalents
        Vector2 moveInput = new Vector2(
            (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed ? 1f : 0f) -
            (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed ? 1f : 0f),
            (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed ? 1f : 0f) -
            (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed ? 1f : 0f)
        );

        // Player input
        user_Horizontal_Input = moveInput.x;
        user_Vertical_Input = moveInput.y;
        user_interact_Input = Keyboard.current.eKey.isPressed;
    }

    // Checks surroundings and determines which directions the player can move
    public void WallDetector()
    {
        // Cast rays in all four directions
        hit_up = Physics2D.Raycast(transform.position, Vector2.up, stepping_Distens);
        hit_down = Physics2D.Raycast(transform.position, Vector2.down, stepping_Distens);
        hit_left = Physics2D.Raycast(transform.position, Vector2.left, stepping_Distens);
        hit_right = Physics2D.Raycast(transform.position, Vector2.right, stepping_Distens);

        // Debug visualisation
        Debug.DrawRay(transform.position, Vector2.up * stepping_Distens, Color.red);
        Debug.DrawRay(transform.position, Vector2.down * stepping_Distens, Color.red);
        Debug.DrawRay(transform.position, Vector2.left * stepping_Distens, Color.red);
        Debug.DrawRay(transform.position, Vector2.right * stepping_Distens, Color.red);
    }

    bool IsBlocked()
    {
        return hit.collider != null &&
               (hit.collider.CompareTag("wall") || hit.collider.CompareTag("npc"));
    }

    void PlayerMovement()
    {
        WallDetector();

        newPosition = transform.position;

        // Move in the input direction if not blocked
        if (user_Horizontal_Input > 0 && !IsBlocked()) newPosition.x += stepping_Distens;
        else if (user_Horizontal_Input < 0 && !IsBlocked()) newPosition.x -= stepping_Distens;
        else if (user_Vertical_Input > 0 && !IsBlocked()) newPosition.y += stepping_Distens;
        else if (user_Vertical_Input < 0 && !IsBlocked()) newPosition.y -= stepping_Distens;

        if (newPosition != transform.position && !coroutine_Running)
            StartCoroutine(MovementCooldown());

        hit = hit_up.collider.tag;

    }

    // Movement cooldown — prevents the player from moving until the cooldown expires
    IEnumerator MovementCooldown()
    {
        coroutine_Running = true;
        user_can_Move = false;

        yield return new WaitForSeconds(movement_Cooldown);

        user_can_Move = true;
        coroutine_Running = false;
    }
}