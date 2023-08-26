using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isOnPlatform;
    public float playerSpeed = 2.0f;
    public float jumpForce = 5.0f; // Adjust the jump force as needed
    private Rigidbody2D rb;

    private void Start() => rb = GetComponent<Rigidbody2D>();
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
        rb.velocity = new Vector2(moveDirection.x * playerSpeed, rb.velocity.y);
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space)) rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.2f, LayerMask.GetMask("Ground"));
        return hit.collider != null;
    }
    private bool IsOnPlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, LayerMask.GetMask("WeightPlatform"));
        return hit.collider != null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("WeightPlatform")) transform.SetParent(collision.transform);
    }
    private void OnCollisionExit2D(Collision2D collision) => transform.SetParent(null);  
    
}
