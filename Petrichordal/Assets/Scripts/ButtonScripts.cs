using FMOD;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class ButtonScripts : MonoBehaviour
{

    public GameObject pausePanel;
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public GameObject fade;

    private FMOD.Studio.Bus masterBus;

    public SoundManager soundManager;

    private void Start()
    {
        masterBus = FMODUnity.RuntimeManager.GetBus("bus:/Master");
        soundManager = GameObject.Find("Main Camera").GetComponent<SoundManager>();
        soundManager.Start();
    }

    public void StartGameButton()
    {
        //fade.SetActive(true);
        //StartCoroutine(StartGame());
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Resume()
    {
        GameController.musicBus.setPaused(false);
        GameController.sfxBus.setPaused(false);
        pausePanel.SetActive(false);
        GameObject.Find("GameController").GetComponent<GameController>().paused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

    public void Restart()
    {
        FMOD.Studio.PLAYBACK_STATE playbackState;
        
        SoundManager.songInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        SoundManager.songInstance.release();
        masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //soundManager.Start();
        

        GameController.musicBus.setPaused(false);
        GameController.sfxBus.setPaused(false);
        soundManager.RestartMusic();
        //SoundManager.songInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Music/" + SoundManager.currentSongName + "bgm");
        
        //SoundManager.songInstance.start();
        SoundManager.songInstance.getPlaybackState(out  playbackState);
        
        UnityEngine.Debug.Log(playbackState.ToString());
      
       
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
