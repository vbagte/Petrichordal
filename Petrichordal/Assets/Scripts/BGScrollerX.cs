using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScrollerX : MonoBehaviour
{

    public float scrollSpeed;
    public float tileSizeX;
    public GameObject lvl1Manager;

    private float timeDelay, restartDelay;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
        timeDelay = lvl1Manager.GetComponent<Lvl1_Manager>().playerStart.playerTakeOffDelay + lvl1Manager.GetComponent<Lvl1_Manager>().playerStart.playerEnable;
    }

    private void Update()
    {
        float newPosition = Mathf.Repeat((Time.time - timeDelay) * scrollSpeed, tileSizeX);
        transform.position = startPosition + Vector3.left * newPosition;
    }

}
