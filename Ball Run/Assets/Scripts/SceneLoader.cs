using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    Canvas pauseCanvas;
    Canvas gameOverCanvas;

    private void Start()
    {
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
    }
    public void LoadGame()
    {
        SceneManager.LoadScene(1);
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
