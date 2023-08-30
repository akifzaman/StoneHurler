using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float detectionRange;
    [SerializeField] private bool isFollowingPlayer = false;
    [SerializeField] private float movingDistance = 20;
    private bool movingRight = true;
    private Vector3 leftPoint;
    private Vector3 rightPoint;
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = FindObjectOfType<PlayerController>()?.transform;
        leftPoint = transform.position - Vector3.right * movingDistance;
        rightPoint = transform.position + Vector3.right * movingDistance;
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            float distanceToPlayerX = Mathf.Abs(transform.position.x - playerTransform.position.x);
            float distanceToPlayerY = Mathf.Abs(transform.position.y - playerTransform.position.y);
            if (distanceToPlayerX <= detectionRange && distanceToPlayerY > -5f && distanceToPlayerY < 1f) isFollowingPlayer = true;
            else isFollowingPlayer = false;
            if (isFollowingPlayer) FollowPlayer();
            else MoveBetweenPoints();     
        }
        else MoveBetweenPoints();
    }

    private void FollowPlayer()
    {
        Vector2 targetPosition = new Vector2(playerTransform.position.x, transform.position.y);
        if (playerTransform.position.x < transform.position.x) movingRight = false;
        else movingRight = true;
        if (!movingRight) 
        { 
            Vector3 newScale = transform.localScale;
            newScale.x = -3;
            transform.localScale = newScale;
        }
        else
        {
            Vector3 newScale = transform.localScale;
            newScale.x = 3;
            transform.localScale = newScale;
        }
        
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private void MoveBetweenPoints()
    {
        Vector3 targetPoint = movingRight ? rightPoint : leftPoint;
        Vector3 movementDirection = (targetPoint - transform.position).normalized;
        transform.position += movementDirection * moveSpeed * Time.deltaTime;
        float distanceToTarget = Vector3.Distance(transform.position, targetPoint);
        if (distanceToTarget < 0.1f)
        {
            movingRight = !movingRight;
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Item") && collision.gameObject.GetComponent<Rigidbody2D>().velocity.x != 0)
        {
            GameManager.Instance.Score += 10;
            GameManager.Instance.UpdateScore();
            Destroy(gameObject);
        }
    }
}
