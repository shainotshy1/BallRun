using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    GameObject pauseCanvas;
    GameObject gameOverCanvas;

    private void Start()
    {
        pauseCanvas = GameObject.FindGameObjectWithTag("Pause");
        gameOverCanvas = GameObject.FindGameObjectWithTag("GameOver");

        if(pauseCanvas != null) pauseCanvas.SetActive(false);
        if(gameOverCanvas!=null) gameOverCanvas.SetActive(false);
    }
    public void Pause()
    {
        if (gameOverCanvas != null)
        {
            if (!gameOverCanvas.activeInHierarchy)
            {
                pauseCanvas.SetActive(true);
                PathHandler.pathRunning = false;
            }
        }
    }
    public void SetGameOverActive(bool active)
    {
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(active);
        }
    }
    public void Resume()
    {
        pauseCanvas.SetActive(false);
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
