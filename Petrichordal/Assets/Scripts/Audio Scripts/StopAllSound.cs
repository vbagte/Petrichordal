using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAllSound : MonoBehaviour
{
    FMOD.Studio.Bus masterBus;

    // Start is called before the first frame update
    void Start()
    {
        masterBus = FMODUnity.RuntimeManager.GetBus("bus:/Master");
    }

    private void OnDestroy()
    {
        masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
