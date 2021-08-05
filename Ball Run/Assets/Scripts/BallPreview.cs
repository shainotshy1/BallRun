using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class BallPreview : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI totalScore;
    [SerializeField] TextMeshProUGUI highScore;
    [SerializeField] Slider slider;
    private void Start()
    {
        DisplayScores();
        slider.value = PlayerPrefs.GetFloat("Volume");
    }
    public void UpdateVolumeValue()
    {
        PlayerPrefs.SetFloat("Volume",slider.value);
    }
    private void DisplayScores()
    {
        totalScore.text = $"Total: {PlayerPrefs.GetInt("TotalScore")}";
        highScore.text = $"High Score: {PlayerPrefs.GetInt("HighScore")}";
    }
}