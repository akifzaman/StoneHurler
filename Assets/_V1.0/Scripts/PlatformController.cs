//using UnityEngine;

//public class PlatformController : MonoBehaviour
//{
//    public PlayerController PlayerController;
//    [SerializeField] private float moveSpeed = 1.5f;
//    //[SerializeField] private bool isOnPlatform = false;
//    [SerializeField] private Vector2 initialPosition;
//    [SerializeField] private Vector2 currentPosition;
//    [SerializeField] private Vector2 targetPosition;
//    private float distance = 2f;
//    private Rigidbody2D rb;
//    private void Start()
//    {
//        //isOnPlatform = true;
//        initialPosition = transform.position;
//        currentPosition = initialPosition;
//        targetPosition = initialPosition - new Vector2(0, distance);
//    }
//    private void Update()
//    {
//        currentPosition = transform.position;
//        if(PlayerController.isOnPlatform && currentPosition.y > targetPosition.y) 
//            transform.Translate(targetPosition * moveSpeed * Time.deltaTime);
//        else if(PlayerController.isOnPlatform && currentPosition.y < targetPosition.y) 
//            transform.Translate(targetPosition * 0 * Time.deltaTime);
//        else if(!PlayerController.isOnPlatform && currentPosition.y < initialPosition.y) 
//            transform.Translate(targetPosition * -moveSpeed * Time.deltaTime);
//        else if(!PlayerController.isOnPlatform && currentPosition.y > initialPosition.y) 
//            transform.Translate(targetPosition * 0 * Time.deltaTime);
//    }
//}
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public PlayerController PlayerController;
    public bool isInInitialPosition, isInTargetPosition;
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float distance = 2f;
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
                rb.position = targetPosition; // Ensure platform reaches target
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
                rb.position = initialPosition; // Ensure platform returns to initial position
            }
        }
    }
}
