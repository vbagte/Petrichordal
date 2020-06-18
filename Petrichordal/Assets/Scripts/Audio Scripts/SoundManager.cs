using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    private string currentScene;
    public static string currentSongName;
    private BeatSystem bS;
    public static FMOD.Studio.EventInstance lv01Snapshot;
    public static string sxSaw;
    public static string sxTri;
    public static string sxSqu;
    public static string sxSin;
    //public static FMOD.Studio.EventInstance bossMusic;
    public static FMOD.Studio.EventInstance songInstance;
    private FMOD.Studio.Bus musicBus;

    public void Start()
    {
            // gets current scene/level name, so that the correct level bgm is played
            currentScene = SceneManager.GetActiveScene().name;
            switch (currentScene)
            {
                case "_Main_Menu":
                    currentSongName = "startmenu"; 
                    break;
                case "Level_01":
                    currentSongName = "lv01";
                    break;
                case "Level_02": 
                    currentSongName = "lv02";
                    break;
                case "Level_03":
                    currentSongName = "lv03";
                    break;
                case "Level_04":
                    currentSongName = "lv04";
                    break;
                default:
                    currentSongName = null;
                    break; 
            }

            //Debug.Log(BeatSystem.bar);
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("MusicBarGlobal", BeatSystem.bar);

            
            bS = GameObject.Find("Main Camera").GetComponent<BeatSystem>();
            bS.AssignBeatEvent(songInstance);

        // assigns path of current level's corresponding ability sound events
        sxSaw = "event:/Game/" + currentSongName + "/saw";
        sxTri = "event:/Game/" + currentSongName + "/tri";
        sxSqu = "event:/Game/" + currentSongName + "/squ";
        sxSin = "event:/Game/" + currentSongName + "/sin";

        // debug
        //Debug.Log(sxSaw);
        //Debug.Log(sxTri);
        //Debug.Log(sxSqu);
        //Debug.Log(sxSin);
        //Debug.Log("CURRENT SONG = " + currentSongName);

        //masterBus = FMODUnity.RuntimeManager.GetBus("bus:/Master");
        musicBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");

        // PLAY LEVEL BACKGROUND MUSIC
        PlayMusic();
        
        
        
    }
    
    public void PlayMusic()
    {
        FMOD.Studio.PLAYBACK_STATE playbackState;
        Debug.Log("Current Song = " + currentSongName);
        
        songInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Music/" + currentSongName + "bgm");
        songInstance.start();
        songInstance.release();

        // debug
        songInstance.getPlaybackState(out playbackState);
        Debug.Log(playbackState);

    }
    public void RestartMusic()
    {
        Debug.Log("Current Song (after restart) = " + currentSongName);

        //songInstance.release();
        songInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Music/" + currentSongName + "bgm");
        songInstance.start();
        songInstance.release();

    }

    public void PlayBossMusic()
    {
        // start beat system again
        // stop level music and start boss music
        musicBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        songInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Music/bossbgm");
        bS = GameObject.Find("Main Camera").GetComponent<BeatSystem>();
        bS.AssignBeatEvent(songInstance);
        songInstance.start();
        songInstance.release();
    }
  
    
}
