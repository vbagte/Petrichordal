using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl2_Manager : MonoBehaviour
{
    public PlayerStart playerStart;
    //public GameObject player;


    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(Killme());
        GameController.playerEnable = true;
       // PlayerController.triEnabled = true;
    }

    IEnumerator Killme()
    {
 
    yield return new WaitForSeconds(playerStart.playerTakeOffDelay);
   playerStart.readyText.GetComponent<Animation>().Play();
    //Vector2 movement = new Vector2(0, playerStart.takeOffSpeed);
      yield return new WaitForSeconds(playerStart.playerEnable);
    playerStart.goText.GetComponent<Animation>().Play();
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
