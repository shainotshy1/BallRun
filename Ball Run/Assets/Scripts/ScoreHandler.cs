using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreHandler
{
    static int gameScore;
    static int additionalScore;
    static int previousScore;

    TextMeshProUGUI scoreBoard = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();
    public ScoreHandler(int score)
    {
        gameScore = score;
        additionalScore = score;
        previousScore = score;
    }
    public ScoreHandler()
    {

    }
    public static void ResetScore()
    {
        gameScore = 0;
        additionalScore = 0;
    }
    public static void SetScoreToPrevious()
    {
        gameScore = previousScore;
        PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore") - gameScore);
    }
    public static void AddScoreToTotal()
    {
        if (PlayerPrefs.GetInt("HighScore") < gameScore)
        {
            PlayerPrefs.SetInt("HighScore", gameScore);
        }
        PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore") + additionalScore);
        PlayerPrefs.Save();
        ResetScore();
    }
    public void ChangeScore(int delta)
    {
        gameScore += delta;
        additionalScore += delta;
        previousScore = gameScore;
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
