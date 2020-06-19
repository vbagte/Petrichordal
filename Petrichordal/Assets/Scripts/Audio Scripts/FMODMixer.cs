using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODMixer : MonoBehaviour
{
    FMOD.Studio.EventInstance sfxTestEvent;
    FMOD.Studio.Bus musicBus;
    FMOD.Studio.Bus sfxBus;
    private float musicVol = 0.5f;
    private float sfxVol = 0.5f;

    void Awake()
    {
        musicBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
        sfxBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
        sfxTestEvent = FMODUnity.RuntimeManager.CreateInstance("event:/UI/sfxvolumetest");
    }

    void Update()
    {
        musicBus.setVolume(musicVol);
        sfxBus.setVolume(sfxVol);
    }

     public void MusicVolumeLevel(float newMusicVol)
    {
        musicVol = newMusicVol;

    }

    public void SFXVolumeLevel(float newSFXVol)
    {
        sfxVol = newSFXVol;

        FMOD.Studio.PLAYBACK_STATE playbackState;
        sfxTestEvent.getPlaybackState(out playbackState);
        if (playbackState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            sfxTestEvent.start();
        }
    }
}
