using UnityEngine;

public class ThorneController : MonoBehaviour
{
    [SerializeField] private bool isMoveDown = false;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float distance;
    [SerializeField] private Vector2 initialPosition;
    [SerializeField] private Vector2 targetPosition;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = rb.position;
        targetPosition = initialPosition - new Vector2(0, distance);
    }

    private void Update()
    {
        targetPosition = initialPosition - new Vector2(0, distance);
        Vector2 currentPosition = rb.position;
        if (isMoveDown)
        {
            if (currentPosition.y < targetPosition.y)
            {
                isMoveDown = false;
                return;
            }
            rb.velocity = Vector2.down * moveSpeed;
        }
        else
        {
            if (currentPosition.y > initialPosition.y)
            {
                isMoveDown = true;
                return;
            }
            rb.velocity = Vector2.up * moveSpeed;
        }
    }
}
