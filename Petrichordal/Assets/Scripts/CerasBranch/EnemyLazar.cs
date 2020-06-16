//==============================================================================
//Project:       Petrichordal
//File Name:     EnemyLazar.cs
//Author:        Cera Baltzley
//Class:         CS 185 2020 Spring Quarter
//Description:   This script defines and manages the operation of the Lazar type beam weapon
//
//==============================================================================
//Known Bugs: 
//==============================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyLazar : MonoBehaviour
{
    public int damage;
    public GameObject glow;
    public float maxXscale;
    public float Xscaler;
    private long counter=0;
    private Health playerHealth;
    private bool growing=true;
    // Start is called before the first frame update
    void Start()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game/bosslaser");
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        counter++; //we use the counter to keep track of the growing and shrinking
        if (transform.localScale.x <= 0 && growing == false) Destroy(gameObject); //if we are done animating, destroy object
        if (counter >= 30) // we allow a half second \ here to allow the player to get out of the way
        {
            if (growing)
            {
                transform.localScale += new Vector3(Xscaler, 0, 0); //make laser wider in the X direciton
                if (transform.localScale.x >= maxXscale) growing = false; //if we reached the desired X scale
            }
            else transform.localScale += new Vector3(-Xscaler, 0, 0); //make laser skinnier in the X direction
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Health>().health -= damage;
            if (playerHealth.health <= 0)
            {
                LifeLost();
            }
            collision.gameObject.GetComponent<Animation>().Play("Player_Hurt");
            FMODUnity.RuntimeManager.PlayOneShot("event:/Game/playerdamaged");
            // Destroy(gameObject);
        }
        //if (collision.gameObject.tag == "Shield")
        //{
        //    Destroy(gameObject);
        //}
    }

    public void LifeLost()
    {
        GameObject.Find("GameController").GetComponent<GameController>().LifeLost();
    }

}


