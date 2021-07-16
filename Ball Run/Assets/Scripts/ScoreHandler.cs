using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreHandler
{
    static int gameScore;

    TextMeshProUGUI scoreBoard = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();
    public ScoreHandler(int score)
    {
        gameScore = score;
    }
    public ScoreHandler()
    {

    }
    public static void AddScoreToTotal()
    {
        PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore") + gameScore);
        PlayerPrefs.Save();
        gameScore = 0;
    }
    public void ChangeScore(int delta)
    {
        gameScore += delta;
    }
    public void DisplayScore()
    {
        if (scoreBoard != null)
        {
            scoreBoard.text = $"Score: {gameScore}";
        }
        else
        {
            Debug.Log("No Score Board");
        }
    }
}
