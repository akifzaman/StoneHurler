using System.Collections;
using UnityEngine;

//Don't override the enemy prefab from the hierarchy without unpackcing
public class EnemyController : MonoBehaviour
{
    private float bloodSplashDuration = 0.2f;
    [SerializeField] private Enemy enemy = new Enemy();
    [SerializeField] private bool isFollowingPlayer = false;
    public bool movingRight;
    [SerializeField] private GameObject detectionIcon;
    [SerializeField] private GameObject bloodSplashAnimation;
    private Vector3 leftPoint;
    private Vector3 rightPoint;
    private Transform playerTransform;
    [SerializeField] private bool isGrounded;
    [SerializeField] private LayerMask _groundMask;

    private void Awake()
    {
        enemy.Health = 10f;
        enemy.Damage = 10f;
        enemy.MoveSpeed = 20f;
        enemy.JumpSpeed = 20f;
        enemy.DetectionRange = 120f;
        //enemy.MovingDistance should be set from the editor based on the platform size it is currently on
    }
    private void Start()
    {
        movingRight = true;
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
            if (isFollowingPlayer && IsGrounded()) FollowPlayer();
            else
            {
                detectionIcon.gameObject.SetActive(false);
                MoveBetweenPoints();
            }
        }
        else
        {
            detectionIcon.gameObject.SetActive(false);
            MoveBetweenPoints();
        }
    }
    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 10f, _groundMask);
        return hit.collider != null;
    }
    private void FollowPlayer()
    {
        detectionIcon.gameObject.SetActive(true);
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
        if (distanceToTarget < 1f)
        {
            movingRight = !movingRight;
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
        }
    }
    public void OnEnemyTakeDamage(float value)
    {
        ActivateEnemyRecovery();
        enemy.Health -= value;
        if (enemy.Health <= 0)
        {
            GameManager.Instance.UpdateScore();
            bloodSplashAnimation.gameObject.SetActive(true);
            var go = Instantiate(bloodSplashAnimation, transform.position, Quaternion.identity);
            Destroy(go, bloodSplashDuration);
            Destroy(gameObject);
        }
    }
    public void ActivateEnemyRecovery()
    {
        enemy.MoveSpeed = 0f;
        if (movingRight) transform.position -= new Vector3(4f, 0f, 0f);
        else transform.position += new Vector3(4f, 0f, 0f);
        StartCoroutine(RestoreSpeed());
    }
    IEnumerator RestoreSpeed()
    {
        yield return new WaitForSeconds(0.4f);
        enemy.MoveSpeed = 20f;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("HangingPlatform")) OnEnemyTakeDamage(10f);
    }
}
