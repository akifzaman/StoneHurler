using UnityEngine;

public class HangingRopeController : MonoBehaviour
{
    public float oscillationSpeed = 2f;
    public float oscillationAmplitude = 0.5f;
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        float yOffset = Mathf.Sin(Time.time * oscillationSpeed) * oscillationAmplitude;
        Vector3 newPosition = initialPosition + new Vector3(0f, yOffset, 0f);
        transform.position = newPosition;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Item"))
        { 
            GetComponentInChildren<Rigidbody2D>().gravityScale = 15f;
        }
    }
}
