using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI StoneCountText;
    public List<GameObject> HealthIcons;
    public GameObject GameOverPanel;

    #region Singleton
    public void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance);
    }
    #endregion
    public void OnPlayerHealthUpdate(int value, bool isInWater = false)
    {
        if (isInWater)
        {
            foreach (var icon in HealthIcons)
            {
                icon.SetActive(false);
            }
            return;
        }
        HealthIcons[value - 1].SetActive(false);
    }
    public void OnScoreUpdate(float value) => ScoreText.text = $"Score: {value}";
    public void OnStoneCountUpdate(int value) => StoneCountText.text = $"x{value}";


}