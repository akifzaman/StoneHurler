using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float Score;
    public UnityEvent OnWaterRemove;
    
    #region Singleton
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    #endregion
    public void LoadNextScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex + 1;
        Debug.Log(index);
        SceneManager.LoadSceneAsync(index);
    }
    public void ReloadCurrentScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
    public void UpdateScore()
    {
        Score += 10f;
        UIManager.Instance.OnScoreUpdated(Score);
    }
    public void GameOver()
    {
        ReloadCurrentScene();
    }
}