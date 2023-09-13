using UnityEngine;

public class AntiGravityController : MonoBehaviour
{
    [SerializeField] private bool isUpwardAllowed;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Item"))
        {
            var rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null && isUpwardAllowed)
            {
                rb.gravityScale = -25f;
            }
            else if (rb != null && !isUpwardAllowed)
            {
                rb.gravityScale = 15f;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Item"))
        {
            var rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 9.8f;
            }
        }
    }
}
