using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isGameOver;
    public float Score;
    #region Singleton
    public void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance);
    }
    #endregion
    public void Start()
    {
        isGameOver = false;
    }
    public void UpdateScore()
    {
        UIManager.Instance.OnScoreUpdate(Score);
    }
    public void GameOver()
    {
        isGameOver = true;
        UIManager.Instance.GameOverPanel.SetActive(true);
    }
    public void OnPlayAgain() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

}