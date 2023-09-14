using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ItemController : MonoBehaviour
{
    public bool isOnWater;
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
        if (scalingFactor >= 0) rb.AddForce(Vector2.right * stoneSpeed, ForceMode2D.Impulse);
        else rb.AddForce(Vector2.left * stoneSpeed, ForceMode2D.Impulse);
    }
    public float GetMass()
    {
         return mass;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("WeightPlatform") || collision.gameObject.CompareTag("WaterTrap")) transform.SetParent(collision.transform);
        else transform.SetParent(null);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.gameObject.GetComponent<EnemyController>();
        if (enemy != null && gameObject.GetComponent<Rigidbody2D>().velocity.x != 0)
        {
            enemy.OnEnemyTakeDamage(5f); 
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var enemy = collision.gameObject.GetComponent<EnemyController>();
        if (enemy != null && gameObject.GetComponent<Rigidbody2D>().velocity.x != 0)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, gameObject.GetComponent<Rigidbody2D>().velocity.y);
        }
    }
}
