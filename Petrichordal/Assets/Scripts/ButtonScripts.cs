using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScripts : MonoBehaviour
{

    public GameObject pausePanel;
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public GameObject fade;

    private FMOD.Studio.Bus masterBus;

    private void Start()
    {
        masterBus = FMODUnity.RuntimeManager.GetBus("bus:/Master");   
    }

    public void StartGameButton()
    {
        //fade.SetActive(true);
        //StartCoroutine(StartGame());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Resume()
    {
        GameController.musicBus.setPaused(false);
        GameController.sfxBus.setPaused(false);
        pausePanel.SetActive(false);
        GameObject.Find("GameController").GetComponent<GameController>().paused = false;
        Time.timeScale = 1;
    }

    public void Restart()
    {
        masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        GameController.musicBus.setPaused(false);
        GameController.sfxBus.setPaused(false);
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

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
