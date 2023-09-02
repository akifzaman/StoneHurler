using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public PlayerController PlayerController;
    public bool isInInitialPosition, isInTargetPosition;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float distance;
    [SerializeField] private Vector2 initialPosition;
    [SerializeField] private Vector2 targetPosition;
    [SerializeField] private Vector2 lockerMoveDirection;
    public GameObject Locker;
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
                lockerMoveDirection = moveDirection;
                lockerMoveDirection.y *= -1;
                rb.velocity = moveDirection * moveSpeed;
                Locker.GetComponent<Rigidbody2D>().velocity = lockerMoveDirection * moveSpeed;
            }
            else
            {
                rb.velocity = Vector2.zero;
                rb.position = targetPosition;
                Locker.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        else
        {
            if (currentPosition.y < initialPosition.y)
            {
                Vector2 moveDirection = (initialPosition - currentPosition).normalized;
                lockerMoveDirection = moveDirection;
                lockerMoveDirection.y *= -1;
                rb.velocity = moveDirection * moveSpeed;
                Locker.GetComponent<Rigidbody2D>().velocity = lockerMoveDirection * moveSpeed;
            }
            else
            {
                rb.velocity = Vector2.zero;
                rb.position = initialPosition;
                Locker.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
    }
}
