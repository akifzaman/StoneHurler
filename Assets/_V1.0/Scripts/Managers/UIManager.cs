using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI StoneCountText;
    public List<GameObject> HealthIcons;
    public GameObject GameOverPanel;
    public Slider WeightSlider;
    #region Singleton
    public void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance);
    }
    #endregion
    public void OnPlayerHealthUpdated(int value, bool isInWater = false)
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
    public void OnScoreUpdated(float value) => ScoreText.text = $"Score: {value}";
    public void OnStoneCountUpdated(int value) => StoneCountText.text = $"x{value}";
    public void OnPlayerWeightUpdated(float value)
    {
        WeightSlider.value = value;
    }


}