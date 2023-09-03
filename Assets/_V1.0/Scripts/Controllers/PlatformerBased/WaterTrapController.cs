using UnityEngine;

public class WaterTrapController : MonoBehaviour
{
    [SerializeField] private bool isMoveRight = false;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float distance;
    [SerializeField] private Vector2 initialPosition;
    [SerializeField] private Vector2 targetPosition;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = rb.position;
        targetPosition = initialPosition - new Vector2(distance, 0);
    }

    private void Update()
    {
        targetPosition = initialPosition - new Vector2(distance, 0);
        Vector2 currentPosition = rb.position;
        if (!isMoveRight)
        {
            if (currentPosition.x < targetPosition.x)
            {
                isMoveRight = true;
                return;
            }
            rb.velocity = Vector2.left * moveSpeed;
        }
        else
        {
            if (currentPosition.x > initialPosition.x)
            {
                isMoveRight = false;
                return;
            }
            rb.velocity = Vector2.right * moveSpeed;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null) player.OnPlayerTakeDamage(10f);
        var stone = collision.gameObject.GetComponent<ItemController>();
        if (stone != null) stone.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        var stonePick = collision.gameObject.GetComponent<PickItem>();
        if (stonePick != null) stonePick.isPickable = false;
    }
}
