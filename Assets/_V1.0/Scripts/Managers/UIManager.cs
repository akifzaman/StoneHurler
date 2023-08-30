using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public TextMeshProUGUI PlayerHealthText;
    public TextMeshProUGUI ScoreText;
    public GameObject GameOverPanel;

    #region Singleton
    public void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance);
    }
    #endregion
    public void OnPlayerHealthUpdate(float value) => PlayerHealthText.text = $"Health: {value}";
    public void OnScoreUpdate(float value) => ScoreText.text = $"Score: {value}";


}