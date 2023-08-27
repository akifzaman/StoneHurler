using UnityEngine;

public class GroundController : MonoBehaviour
{
    public PlayerController PlayerController;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //PlayerController.isOnPlatform = false;
        }
    }
}
