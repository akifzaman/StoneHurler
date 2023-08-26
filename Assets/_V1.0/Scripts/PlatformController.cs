using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public PlayerController PlayerController;
    public bool isInInitialPosition, isInTargetPosition;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float distance;
    [SerializeField] private Vector2 initialPosition;
    [SerializeField] private Vector2 targetPosition;
    private Rigidbody2D rb;

    private void Start()
    {
        distance = PlayerController.player.Mass;
        rb = GetComponent<Rigidbody2D>();
        initialPosition = rb.position;
        targetPosition = initialPosition - new Vector2(0, distance);
    }

    private void Update()
    {
        distance = PlayerController.player.Mass;
        targetPosition = initialPosition - new Vector2(0, distance);
        Vector2 currentPosition = rb.position;

        if (PlayerController.isOnPlatform)
        {
            if (currentPosition.y > targetPosition.y)
            {
                Vector2 moveDirection = (targetPosition - currentPosition).normalized;
                rb.velocity = moveDirection * moveSpeed;
            }
            else
            {
                rb.velocity = Vector2.zero;
                rb.position = targetPosition;
            }
        }
        else
        {
            if (currentPosition.y < initialPosition.y)
            {
                Vector2 moveDirection = (initialPosition - currentPosition).normalized;
                rb.velocity = moveDirection * moveSpeed;
            }
            else
            {
                rb.velocity = Vector2.zero;
                rb.position = initialPosition;
            }
        }
    }
}
