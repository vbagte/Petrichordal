using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl2_Manager : MonoBehaviour
{
    public PlayerStart playerStart;
    public GameObject foreground;
    
    //public float foregroundSpeed;
    //public GameObject player;


    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(Killme());
        GameController.playerEnable = true;
        

        
    }

    IEnumerator Killme()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game/playerignition"); // play sound
        yield return new WaitForSeconds(playerStart.playerTakeOffDelay);
    playerStart.readyText.GetComponent<Animation>().Play();
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game/ready");
        
        //Vector2 movement = new Vector2(0, playerStart.takeOffSpeed);
        yield return new WaitForSeconds(playerStart.playerEnable);
    playerStart.goText.GetComponent<Animation>().Play();
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game/go");
        //foreground.GetComponent<Mover>().speed = foregroundSpeed;
        //   // Vector2 movement2 = new Vector2(0, 0);
        //   // player.GetComponent<Rigidbody2D>().velocity = movement2;
        //   // player.GetComponent<PlayerController>().enabled = true;
        //   //GameController.playerEnable = true;
    }


    // Update is called once per frame
    void Update()
    {

    }
    //private void Start()
    //{

    //}
    //private void Update()
    //{

    //}


}
