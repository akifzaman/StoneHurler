using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI StoneCountText;
    public List<GameObject> HealthIcons;
    public Slider WeightSlider;
    public float tweenDuration;
    public Image imageToTween;
    #region Singleton
    public void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance);
    }
    private void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex < 5) TransitionToTransparent();
        else TransitionToBlack();
    }
    #endregion
    public void TransitionToTransparent()
    {
        imageToTween.DOColor(new Color(0, 0, 0, 0), tweenDuration);
    }
    public void TransitionToBlack()
    {
        imageToTween.DOColor(new Color(0, 0, 0, 1), tweenDuration);
    }
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