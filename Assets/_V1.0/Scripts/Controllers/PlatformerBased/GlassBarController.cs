using UnityEngine;

public class GlassBarController : MonoBehaviour
{
    public GameObject ShatterAnimationPrefab;
    private void OnTriggerEnter2D(Collider2D other)
    {
        var stone = other.gameObject.GetComponent<ItemController>();
        if (stone != null)
        {
            var go = Instantiate(ShatterAnimationPrefab, other.transform.position, Quaternion.identity);
            Destroy(go, 0.2f);
            stone.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Destroy(gameObject);
        }
    }
}
