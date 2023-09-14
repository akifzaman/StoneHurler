using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private bool isCollided = false;
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isCollided)
        {
            GameManager.Instance.LoadNextScene();
            isCollided = true;
        }
        Destroy(gameObject);
    }
}
