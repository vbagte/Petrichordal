using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    private string currentScene;
    public static string currentSongName;
    private BeatSystem bS;
    public static string sxSaw;
    public static string sxTri;
    public static string sxSqu;
    public static string sxSin;
    public static FMOD.Studio.EventInstance bossMusic;
    public static FMOD.Studio.EventInstance songInstance;

    public void Start()
    {
            // gets current scene/level name, so that the correct level bgm is played
            currentScene = SceneManager.GetActiveScene().name;
            switch (currentScene)
            {
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
                    currentSongName = "lv01";
                    break;
            }

            //Debug.Log(BeatSystem.bar);
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("MusicBarGlobal", BeatSystem.bar);

            
            bS = GetComponent<BeatSystem>();
            bS.AssignBeatEvent(songInstance);

        // assigns path of current level's corresponding ability sound events
        sxSaw = "event:/Game/" + currentSongName + "/saw";
        sxTri = "event:/Game/" + currentSongName + "/tri";
        sxSqu = "event:/Game/" + currentSongName + "/squ";
        sxSin = "event:/Game/" + currentSongName + "/sin";

        Debug.Log(sxSaw);
        Debug.Log(sxTri);
        Debug.Log(sxSqu);
        Debug.Log(sxSin);

    }

    public void PlayMusic()
    {
        Debug.Log("Current Song = " + currentSongName);
        
        songInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Music/" + currentSongName + "bgm");
        songInstance.start();
        songInstance.release();
    }

    public void PlayBossMusic()
    {
        // stop level music and start boss music
        songInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        bossMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Music/bossbgm");
        bossMusic.start();
        bossMusic.release();

        //bossMusic.setParameterByName("BossWin", 1);
    }
  
}
