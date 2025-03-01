
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public PlayerSO playerData;

    public Joystick joystick;  // Assign in the Inspector
    public float moveSpeed = 5f;
    private Rigidbody rb;
    private string currentPlayerName;
    private float currentPlayerHealth;
    private float currentPlayerArmor;


    private void Start()
    {
        references();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent Rigidbody from rotating due to physics
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        // Apply velocity to Rigidbody
        rb.linearVelocity = moveDirection * moveSpeed;

        // Rotate the player to face movement direction
        if (moveDirection.magnitude >= 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            rb.MoveRotation(targetRotation);
        }
    }

    void references()
    {
        currentPlayerName = playerData.playerName;
        currentPlayerHealth = playerData.playerHealth;
        currentPlayerArmor = playerData.PlayerArmor;

    }


}

// rebuild check point