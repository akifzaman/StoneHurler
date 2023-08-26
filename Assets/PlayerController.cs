////using UnityEngine;
////public class PlayerController : MonoBehaviour
////{
////    public Player player = new Player();
////    public bool isGrounded;
////    public bool isOnPlatform;
////    private void Start()
////    {
////        player.Speed = 1.5f;
////        player.Health = 10.0f;
////        player.Damage = 2.0f;
////        player.Mass = 1.0f;
////    }
////    private void Update()
////    {
////        transform.Translate(Vector2.down * player.Speed * Time.deltaTime);
////    }
////    private void OnCollisionEnter2D(Collision2D collision)
////    {
////        if (collision.gameObject.CompareTag("WeightPlatform"))
////        {
////            Debug.Log("Collided");
////            isOnPlatform = true;
////        }
////    }
////    private void OnCollisionExit2D(Collision2D collision)
////    {
////        if (collision.gameObject.CompareTag("WeightPlatform"))
////        {
////            isOnPlatform = false;
////        }
////    }
////}
//using UnityEngine;

//public class PlayerController : MonoBehaviour
//{
//    public bool isOnPlatform;
//    public float playerSpeed = 2.0f; // Adjust the speed as needed
//    private Rigidbody2D rb;

//    private void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//    }

//    private void Update()
//    {  
//        // Move the player horizontally
//        float horizontalInput = Input.GetAxis("Horizontal");
//        float verticalInput = Input.GetAxis("Vertical");
//        Vector2 moveDirection = new Vector2(horizontalInput, verticalInput);
//        rb.velocity = new Vector2(moveDirection.x * playerSpeed, rb.velocity.y);
//    }

//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag("WeightPlatform"))
//        {
//            Debug.Log("Collided");
//            isOnPlatform = true;
//        }
//    }

//    private void OnCollisionExit2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag("WeightPlatform"))
//        {
//            isOnPlatform = false;
//        }
//    }
//}
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isOnPlatform;
    public float playerSpeed = 2.0f;
    public float jumpForce = 5.0f; // Adjust the jump force as needed
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Move the player horizontally
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 moveDirection = new Vector2(horizontalInput, 0);
        rb.velocity = new Vector2(moveDirection.x * playerSpeed, rb.velocity.y);

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private bool IsGrounded()
    {
        // Perform a raycast to check if the player is grounded
        // You need to adjust the position and length of the raycast based on your player's position
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f);
        return hit.collider != null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("WeightPlatform"))
        {
            Debug.Log("Collided");
            isOnPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("WeightPlatform"))
        {
            isOnPlatform = false;
        }
    }
}
