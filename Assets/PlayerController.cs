using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Player player = new Player();
    public bool isOnPlatform;
    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        Move();
        Jump();
        isOnPlatform = IsOnPlatform();
    }
    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 moveDirection = new Vector2(horizontalInput, 0);
        rb.velocity = new Vector2(moveDirection.x * player.MoveSpeed, rb.velocity.y);
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space)) rb.velocity = new Vector2(rb.velocity.x, player.JumpSpeed);
    }
    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, LayerMask.GetMask("Ground"));
        return hit.collider != null;
    }
    private bool IsOnPlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 5f, LayerMask.GetMask("WeightPlatform"));
        return hit.collider != null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("WeightPlatform")) transform.SetParent(collision.transform);
        if (collision.gameObject.CompareTag("Item"))
        {
            IPickable iPickable = collision.gameObject.GetComponent<IPickable>();
            if (iPickable != null)
            {
                iPickable.Pick();
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision) => transform.SetParent(null);  
    
}
