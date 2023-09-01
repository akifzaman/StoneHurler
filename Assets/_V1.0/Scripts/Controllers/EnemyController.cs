using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Enemy enemy = new Enemy();
    [SerializeField] private bool isFollowingPlayer = false;
    [SerializeField] private bool movingRight = true;
    private Vector3 leftPoint;
    private Vector3 rightPoint;
    private Transform playerTransform;

    private void Awake()
    {
        enemy.Health = 10f;
        enemy.Damage = 10f;
        enemy.MoveSpeed = 20f;
        enemy.JumpSpeed = 20f;
        enemy.DetectionRange = 200f;
        enemy.MovingDistance = 20f;
    }
    private void Start()
    {
        playerTransform = FindObjectOfType<PlayerController>()?.transform;
        leftPoint = transform.position - Vector3.right * enemy.MovingDistance;
        rightPoint = transform.position + Vector3.right * enemy.MovingDistance;
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            float distanceToPlayerX = Mathf.Abs(transform.position.x - playerTransform.position.x);
            float distanceToPlayerY = Mathf.Abs(transform.position.y - playerTransform.position.y);
            if (distanceToPlayerX <= enemy.DetectionRange && distanceToPlayerY >= 0f && distanceToPlayerY <= 5f) isFollowingPlayer = true;
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
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, enemy.MoveSpeed * Time.deltaTime);
    }

    private void MoveBetweenPoints()
    {
        Vector3 targetPoint = movingRight ? rightPoint : leftPoint;
        Vector3 movementDirection = (targetPoint - transform.position).normalized;
        transform.position += movementDirection * enemy.MoveSpeed * Time.deltaTime;
        float distanceToTarget = Vector3.Distance(transform.position, targetPoint);
        if (distanceToTarget < 0.1f)
        {
            movingRight = !movingRight;
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
        }
    }
    public void OnEnemyTakeDamage()
    {
        enemy.Health -= 5f;
        if (enemy.Health <= 0)
        {
            GameManager.Instance.UpdateScore();
            Destroy(gameObject);
        }
    }
}
