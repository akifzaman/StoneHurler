using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Configurations
    [Header("Configuration")]
	private bool variableJumpHeight = true;
	private bool appexModifier = true;
	private bool jumpBuffer = true;
	private bool coyoteTime = true;
	private bool clampedFallSpeed = true;
	private bool juice = true;
    #endregion

    #region Animation Keys
    private static readonly int jumpKey = Animator.StringToHash("isPressable");
	private static readonly int moveKey = Animator.StringToHash("playerSpeed");
	#endregion

	#region Variables
	[SerializeField] private Animator _anim;
	//[SerializeField] private AudioSource _source;
	[SerializeField] private float speedFactor = .1f;
	[SerializeField] private float verticalSpeed = 30f;
	private float verticalSpeedTemp = 30f;
	private Movement inputActions;
	private Rigidbody2D playerRb;
	private Vector2 force;
	[SerializeField] private float _maxTilt = .1f;
	[SerializeField] private float _tiltSpeed = 1;
	[SerializeField] private float pushForce = 1.5f;
	[SerializeField] private float bloodSplashDuration = 0.2f;

	[SerializeField] private UnityEngine.Transform targetSprite;
	[SerializeField] private LayerMask _groundMask;
	
	private float jumpCount;
	[SerializeField] private float maxJump;
	[SerializeField] private float variableJumpHeightValue;
	[SerializeField] private float jumpBufferTime;
	[SerializeField] private float apexThresold;
	private float applyApexTime;
	private float jumpBufferTimeTemp;
	private float lastGrounded = 0;
	private float lastGroundExit = 0;
	[SerializeField] private float coyoteTimeValue = 1;
	private float maxJumpTemp;
	[SerializeField] private bool isGrounded = true;
	[SerializeField] private bool pressable = true;
	private float gravityScale;
    #endregion
    public Player player = new Player();
	public bool isOnPlatform;
	public GameObject bloodSplashAnimation;

    private void Awake()
	{
		inputActions = new Movement();
	}
	private void Start()
	{
		maxJumpTemp = maxJump;
		verticalSpeedTemp = verticalSpeed;
		jumpBufferTimeTemp = jumpBufferTime;
		lastGrounded = Time.time;
		OnVariableJumpHeightChange(true);
		OnJumpBufferChange(true);
		OnAppexModifierChange(true);
		playerRb = GetComponent<Rigidbody2D>();
		gravityScale = playerRb.gravityScale;
        UIManager.Instance.OnPlayerHealthUpdate(player.Health);
    }
    private void OnEnable() => inputActions.Enable();
	private void OnDisable() => inputActions.Disable();
	private void Update()
	{
		force = inputActions.player.movement.ReadValue<Vector2>();
		if (inputActions.player.Jump.IsPressed() && pressable)
		{
			_anim.SetBool(jumpKey, !pressable);
            Jump();
		}
        if (inputActions.player.SingleThrow.WasPressedThisFrame()) //for single stone throw
        {
            if (SpawnManager.Instance.ThrowStone(transform.localScale.x)) player.Mass -= 10f;
        }
        if (inputActions.player.MultipleThrow.IsPressed()) //for multiple stone throw
        {
			if (SpawnManager.Instance.ThrowStone(transform.localScale.x))
			{
				player.Mass -= 10f;
			}
        }
        var targetRotVector = new Vector3(0, 0, Mathf.Lerp(-_maxTilt, _maxTilt, Mathf.InverseLerp(-1, 1, force.x)));
		targetSprite.rotation = Quaternion.RotateTowards(targetSprite.rotation, Quaternion.Euler(targetRotVector), _tiltSpeed * Time.deltaTime);
		var groundHit = Physics2D.Raycast(transform.position, Vector3.down, 2, _groundMask);
        isOnPlatform = IsOnPlatform();
    }
    private bool IsOnPlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 10f, LayerMask.GetMask("WeightPlatform"));
        return hit.collider != null;
    }
	private void FixedUpdate()
	{
		Move(force);
	}
	public void Move(Vector3 force)
	{
		if (force.magnitude > 0)
		{
			_anim.SetFloat(moveKey, force.magnitude);
			transform.position = Vector2.Lerp(transform.position, transform.position + force * speedFactor, Time.deltaTime);
			transform.localScale = new Vector3(force.x >= 0 ? Mathf.Abs(transform.localScale.x) : Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
			if (appexModifier && !isGrounded)
			{
				float apexPoint = Mathf.InverseLerp(.5f, 0, playerRb.velocity.y);
				if (apexPoint > 0 && applyApexTime <= apexThresold) applyApexTime += Time.deltaTime;	
			}
		}
        else _anim.SetFloat(moveKey, 0);
    }
    public void Jump()
	{
		jumpCount++;
		if (jumpCount <= maxJump)
		{
			if (variableJumpHeight) playerRb.AddForce(new Vector2(0, verticalSpeed));
			else
			{
				playerRb.AddForce(new Vector2(0, variableJumpHeightValue));
				pressable = false;
				jumpCount = 0;
			}
		}
		else
		{
			pressable = false;
			_anim.SetBool(jumpKey, !pressable);
			jumpCount = 0;
		}
	}
	public void Jump(InputAction.CallbackContext callbackContext)
	{
        transform.SetParent(null);
        if (callbackContext.performed && (isGrounded || (coyoteTime && (Time.time - lastGroundExit) <= coyoteTimeValue)) && (Time.time - lastGrounded) >= jumpBufferTime)
		{
			pressable = true;
		}
		else if (callbackContext.canceled) pressable = false;	
	}
	public void Move(InputAction.CallbackContext callbackContext)
	{
		if (callbackContext.canceled)
		{
			playerRb.velocity = new Vector2(force.x, player.MoveSpeed);
			applyApexTime = 0;
		}
	}
	public void OnVariableJumpHeightChange(bool value)
	{
		variableJumpHeight = value;
	}
	public void OnAppexModifierChange(bool value)
	{
		appexModifier = value;
	}
	public void OnJumpBufferChange(bool value)
	{
		jumpBuffer = value;
		if (jumpBuffer)
		{
			jumpBufferTime = 0;
		}
		else
		{
			jumpBufferTime = jumpBufferTimeTemp;
		}
	}
	public void OnPlayerTakeDamage(float value)
	{
        player.Health -= value;
        UIManager.Instance.OnPlayerHealthUpdate(player.Health);
        if (player.Health <= 0f)
        {
            GameManager.Instance.GameOver();
			var go = Instantiate(bloodSplashAnimation, transform.position, Quaternion.identity);
			Destroy(go, bloodSplashDuration);
            gameObject.SetActive(false);
        }
    }
    public void ActivatePlayerRecoveryFromEnemy(bool value)
    {
        player.MoveSpeed = 0f;
        player.JumpSpeed = 0f;
		if(value) playerRb.AddForce(Vector2.right * pushForce, ForceMode2D.Impulse);	
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
        yield return new WaitForSeconds(1f);
        player.MoveSpeed = 40f;
        player.JumpSpeed = 50f;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            lastGrounded = Time.time;
            jumpCount = 0;
            pressable = true;
            isGrounded = true;
        }
        if (collision.gameObject.CompareTag("WeightPlatform") || collision.gameObject.CompareTag("HangingPlatform"))
        {
            pressable = true;
            isGrounded = true;
            transform.SetParent(collision.transform);
        }
        if (collision.gameObject.CompareTag("Item"))
        {
            pressable = true;
            isGrounded = true;
            if (InventoryManager.Instance.inventory.Items.Count < InventoryManager.Instance.inventoryCapacity)
            {
                player.Mass += 10f;
            }
            IPickable iPickable = collision.gameObject.GetComponent<IPickable>();
            iPickable?.Pick();
        }
        if (collision.gameObject.CompareTag("Door"))
        {
            GameManager.Instance.GameOver();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            isGrounded = false;
            lastGroundExit = Time.time;
        }
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
			if (thorne != null)
			{
                ActivatePlayerRecoveryFromObstacles();
            }
            OnPlayerTakeDamage(5f);
        }
    }
}