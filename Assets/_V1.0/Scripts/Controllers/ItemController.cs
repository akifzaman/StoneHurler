using UnityEngine;

public class ItemController : MonoBehaviour
{
    private float mass = 5f;
    private float stoneSpeed = 0f;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void IncreaseMoveSpeed(float value, float scaleX)
    {
        rb = GetComponent<Rigidbody2D>();
        stoneSpeed += value;
        MoveStone(scaleX);
    }
    public void MoveStone(float scalingFactor)
    {  
        if(scalingFactor >= 0) rb.AddForce(Vector2.right * stoneSpeed, ForceMode2D.Impulse);
        else rb.AddForce(Vector2.left * stoneSpeed, ForceMode2D.Impulse);
    }
    public float GetMass()
    {
         return mass;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("WeightPlatform")) transform.SetParent(collision.transform);
        else transform.SetParent(null);
    }
}
