using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScripts : MonoBehaviour
{

    public GameObject pausePanel;
    public GameObject pauseMenu;
    public GameObject settingsMenu;

    public void Resume()
    {
        pausePanel.SetActive(false);
        GameObject.Find("GameController").GetComponent<GameController>().paused = false;
        Time.timeScale = 1;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Settings()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void Back()
    {
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
