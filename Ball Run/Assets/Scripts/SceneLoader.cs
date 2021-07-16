using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    Canvas pauseCanvas;
    Canvas gameOverCanvas;

    [SerializeField] Transform ball;
    bool loadingGame;
    private void Start()
    {
        loadingGame = false;
        GameObject pause = GameObject.FindGameObjectWithTag("Pause");
        GameObject gameOver = GameObject.FindGameObjectWithTag("GameOver");
        if (pause != null)
        {
            pauseCanvas = pause.GetComponent<Canvas>();
            pauseCanvas.enabled = false;
        }
        if (gameOver != null)
        {
            gameOverCanvas = gameOver.GetComponent<Canvas>();
            gameOverCanvas.enabled = false;
        }
    }
    public void Pause()
    {
        if (!gameOverCanvas.enabled)
        {
            pauseCanvas.enabled = true;
            PathHandler.pathRunning = false;
        }
    }
    public void Resume()
    {
        pauseCanvas.enabled = false;
        PathHandler.pathRunning = true;
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        ScoreHandler.AddScoreToTotal();
    }
    public void LoadGame()
    {
        if (ball != null&&!loadingGame)
        {
            StartCoroutine(LoadGameCoroutine());
        }
    }
    IEnumerator LoadGameCoroutine()
    {
        while (ball.localPosition.y > 0)
        {
            yield return new WaitForEndOfFrame();
            loadingGame = true;
        }
        SceneManager.LoadScene(1);
        loadingGame = false;
    }
    public void Replay()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
