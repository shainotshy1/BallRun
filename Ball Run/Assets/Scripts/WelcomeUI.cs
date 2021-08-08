using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class WelcomeUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI totalScore;
    [SerializeField] TextMeshProUGUI highScore;
    [SerializeField] TextMeshProUGUI gemTotal;
    [SerializeField] Slider slider;
    private void Start()
    {
        DisplayScores();
        slider.value = PlayerPrefs.GetFloat("Volume");
    }
    public void UpdateVolumeValue()
    {
        PlayerPrefs.SetFloat("Volume",slider.value);
        PlayerPrefs.Save();
    }
    private void DisplayScores()
    {
        totalScore.text = $"{PlayerPrefs.GetInt("TotalScore")}";
        highScore.text = $"High Score: {PlayerPrefs.GetInt("HighScore")}";
        gemTotal.text = $"{PlayerPrefs.GetInt("TotalGems")}";
    }
}
