using UnityEngine;

public class ItemController : MonoBehaviour
{
    private float mass = 5f;
    private float stoneSpeed = 0f;
    private Rigidbody2D rb;
    
    public void IncreaseMoveSpeed(float value)
    {
        rb = GetComponent<Rigidbody2D>();
        stoneSpeed += value;
        MoveStone();
    }
    public void MoveStone()
    {  
        rb.AddForce(Vector2.right * stoneSpeed, ForceMode2D.Impulse);
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
