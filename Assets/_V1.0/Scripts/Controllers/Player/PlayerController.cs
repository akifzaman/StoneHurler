using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class PlayerController : MonoBehaviour
{
    #region Animation Keys
    private static readonly int jumpKey = Animator.StringToHash("IsJumping");
    private static readonly int doubleJumpKey = Animator.StringToHash("IsDoubleJumping");
    private static readonly int moveKey = Animator.StringToHash("Speed");
    private static readonly int onDamageKey = Animator.StringToHash("IsTakeDamage");
    #endregion

    #region Variables
    //[SerializeField] private AudioSource _source;
    [SerializeField] private Animator _anim;
    [SerializeField] private float verticalSpeed = 30f;
    [SerializeField] private float accelaration = 0.098f;
   
    private Movement inputActions;
    private Rigidbody2D playerRb;
    private Vector2 force;
    [SerializeField] private float pushForce = 1.5f;
    [SerializeField] private float velocityDampning = 0.12f;
    [SerializeField] private float bloodSplashDuration = 0.2f;

    [SerializeField] private LayerMask _groundMask;

    private float lastGrounded = 0;
    private float lastGroundExit = 0;
    [SerializeField] private float coyoteTimeValue;
    private float gravityScale;
    #endregion
    public Player player = new Player();
    public bool isOnPlatform;
    public GameObject bloodSplashAnimation;
    private Vector3 velocity = Vector3.zero;
    private Vector2 currentDirection = Vector2.zero;
    private Vector2 prevDirection = Vector2.zero;
    [SerializeField] private float modifiedVelocity;
    [SerializeField] private int counter = 0;
    [SerializeField] private bool isDoubleJumpAllowed = true;
    [SerializeField] private float momentBeforeGroundHit = 0f;
    [SerializeField] private float momentOfJump = 0f;

    private void Awake()
    {
        inputActions = new Movement();
        inputActions.player.Jump.performed += Jump;
        inputActions.player.Jump.canceled -=  Jump;
    }
    private void Start()
    {
        lastGrounded = Time.time;
        playerRb = GetComponent<Rigidbody2D>();
        gravityScale = playerRb.gravityScale;
    }
    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();
    private void Update()
    {
        force = inputActions.player.movement.ReadValue<Vector2>();
        currentDirection = new Vector2(force.x, force.y);
        if (currentDirection != prevDirection)
        {
            counter /= 2;
            modifiedVelocity = 0;
        }
        prevDirection = currentDirection;
              
        if (inputActions.player.SingleThrow.WasPressedThisFrame())
        {
            if (SpawnManager.Instance.ThrowStone(transform.localScale.x))
            {
                player.Mass = 10 + InventoryManager.Instance.inventory.Items.Count * 10;
                UIManager.Instance.OnPlayerWeightUpdated(player.Mass);
            }
        }
        isOnPlatform = IsOnPlatform();
        if (IsGrounded())
        {
            momentBeforeGroundHit = Time.time % 100;
        }
    }
    private bool IsOnPlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 10f, LayerMask.GetMask("WeightPlatform"));
        return hit.collider != null;
    }
    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 10f, _groundMask);
        return hit.collider != null;
    }
    private void FixedUpdate()
    {
        Move(force);
    }
    public void Move(Vector3 force)
    {
        if (force != Vector3.zero)
        {
            counter++;
            modifiedVelocity = player.MoveSpeed * (1 + (accelaration * counter / 1000) + (counter / 100));
            modifiedVelocity = Mathf.Clamp(modifiedVelocity, 0, 60);
            bool faceRight = force.x >= 0;
            SetFacingDirection(faceRight);
        }
        _anim.SetFloat(moveKey, force.magnitude);
        Vector3 targetVelocity = new Vector2(force.x * modifiedVelocity, playerRb.velocity.y);
        playerRb.velocity = Vector3.SmoothDamp(playerRb.velocity, targetVelocity, ref velocity, velocityDampning);
        
        if (force.magnitude > 0) _anim.SetFloat(moveKey, force.magnitude);       
        else _anim.SetFloat(moveKey, 0);       
    }
    private void SetFacingDirection(bool faceRight)
    {
        transform.localScale = new Vector3(faceRight ? Mathf.Abs(transform.localScale.x) : -Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    public void Jump()
    {
        momentOfJump = Time.time % 100;
        playerRb.AddForce(new Vector2(0, verticalSpeed + (modifiedVelocity / 1.5f)));
        _anim.SetBool(jumpKey, !IsGrounded());
    }
    public void DoubleJump()
    {
        isDoubleJumpAllowed = false;
        playerRb.velocity = new Vector2(playerRb.velocity.x,0);
        playerRb.AddForce(new Vector2(0, verticalSpeed/1.25f));
        _anim.SetBool(jumpKey, !IsGrounded());
    }
    public void Jump(InputAction.CallbackContext callbackContext)
    {
        transform.SetParent(null);
        if (callbackContext.performed && (IsGrounded() || (Time.time - lastGroundExit <= coyoteTimeValue) ||
            (momentBeforeGroundHit - momentOfJump) >= 1.5f && (momentBeforeGroundHit - momentOfJump) <= 1.55f))
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, 0f);    
            isDoubleJumpAllowed = true;
            Jump();
        }
        else if (callbackContext.performed && isDoubleJumpAllowed)
        {
            DoubleJump();
        }
    }
    public void Move(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.canceled)
        {
            playerRb.velocity = new Vector2(force.x, player.MoveSpeed);
        }
    }
    public void OnPlayerTakeDamage(int value, bool isInWater = false)
    {
        player.Health -= value;
        _anim.SetBool(onDamageKey, true);
        StartCoroutine(RestoreToIdleAnimation());
        if (player.Health <= 0f)
        {
            GameManager.Instance.GameOver();
            var go = Instantiate(bloodSplashAnimation, transform.position, Quaternion.identity);
            Destroy(go, bloodSplashDuration);
            gameObject.SetActive(false);
        }
        UIManager.Instance.OnPlayerHealthUpdated(player.Health + 1, isInWater);
    }
    IEnumerator RestoreToIdleAnimation()
    {
        yield return new WaitForSeconds(0.2f);
        _anim.SetBool(onDamageKey, false);
    }
    public void ActivatePlayerRecoveryFromEnemy(bool value)
    {
        player.MoveSpeed = 0f;
        player.JumpSpeed = 0f;
        if (value) playerRb.AddForce(Vector2.right * pushForce, ForceMode2D.Impulse);
        else playerRb.AddForce(Vector2.left * pushForce, ForceMode2D.Impulse);
        StartCoroutine(RestoreSpeed());
    }
    public void ActivatePlayerRecoveryFromObstacles()
    {
        player.MoveSpeed = 0f;
        player.JumpSpeed = 0f;
        playerRb.AddForce(Vector2.up * pushForce * 5, ForceMode2D.Impulse);
        StartCoroutine(RestoreSpeed());
    }
    IEnumerator RestoreSpeed()
    {
        yield return new WaitForSeconds(0.1f);
        player.MoveSpeed = 40f;
        player.JumpSpeed = 50f;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //isDoubleJumpAllowed = true;
        if (collision.gameObject.CompareTag("WeightPlatform") || collision.gameObject.CompareTag("HangingPlatform"))        
            transform.SetParent(collision.transform);        
        if (collision.gameObject.CompareTag("Item"))
        {
            IPickable iPickable = collision.gameObject.GetComponent<IPickable>();
            var isPickUpSuccess = iPickable?.Pick();
            if (isPickUpSuccess != null && isPickUpSuccess == true)
            {
                if (InventoryManager.Instance.inventory.Items.Count <= InventoryManager.Instance.inventoryCapacity)
                {
                    player.Mass = 10 + InventoryManager.Instance.inventory.Items.Count * 10;
                    UIManager.Instance.OnPlayerWeightUpdated(player.Mass);
                }
            }
        }
        if (collision.gameObject.CompareTag("Door")) GameManager.Instance.GameOver();       
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground")) lastGroundExit = Time.time;       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Enemy") || collision.transform.CompareTag("Thorne"))
        {
            var enemy = collision.gameObject.GetComponent<EnemyController>();
            var thorne = collision.gameObject.GetComponent<ThorneController>();
            if (enemy != null)
            {
                ActivatePlayerRecoveryFromEnemy(enemy.movingRight);
                enemy.ActivateEnemyRecovery();
            }
            if (thorne != null) ActivatePlayerRecoveryFromObstacles();
            
            OnPlayerTakeDamage(1);
        }
        if (collision.transform.CompareTag("WaterRemovalButton")) GameManager.Instance.OnWaterRemove.Invoke();
    }
}