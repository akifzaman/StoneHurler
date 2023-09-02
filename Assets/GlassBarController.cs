using UnityEngine;

public class GlassBarController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var stone = other.gameObject.GetComponent<ItemController>();
        if (stone != null)
        {
            stone.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Destroy(gameObject);
        }
    }
}
