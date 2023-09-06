using DG.Tweening;
using UnityEngine;

public class WaterTrapController : MonoBehaviour
{
    [SerializeField] private bool isMoveRight = false;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float distance;
    [SerializeField] private float TargetY;
    [SerializeField] private float removeDuration;
    [SerializeField] private Vector2 initialPosition;
    [SerializeField] private Vector2 targetPosition;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = rb.position;
        targetPosition = initialPosition - new Vector2(distance, 0);
        GameManager.Instance.OnWaterRemove.AddListener(RemoveWater);
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
    private void RemoveWater()
    {
        transform.DOMoveY(TargetY, removeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null) player.OnPlayerTakeDamage(3, true);
        var stone = collision.gameObject.GetComponent<ItemController>();
        if (stone != null) stone.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        var stonePick = collision.gameObject.GetComponent<PickItem>();
        if (stonePick != null) stonePick.isPickable = false;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        //var stone = collision.gameObject.GetComponent<ItemController>();
        //if (stone != null) stone.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        var stonePick = collision.gameObject.GetComponent<PickItem>();
        if (stonePick != null) stonePick.isPickable = true;
    }
}
