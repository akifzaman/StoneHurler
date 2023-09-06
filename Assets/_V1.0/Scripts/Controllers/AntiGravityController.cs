using UnityEngine;

public class AntiGravityController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Item"))
        {
            var rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = -19.6f;
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
