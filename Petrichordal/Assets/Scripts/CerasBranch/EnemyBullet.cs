//==============================================================================
//Project:       Petrichordal
//File Name:     EnemyBullet.cs
//Author:        Cera Baltzley
//Class:         CS 185 2020 Spring Quarter
//Description:   This script defines and manages the operation of the Bullet style projectiles
//
//==============================================================================
//Known Bugs: 
//==============================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage;
    public float speed;

    private Health playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        //  Debug.Log(transform.rotation);
        //Destroy(gameObject, 10f);
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, Time.deltaTime*speed, 0));
        if (transform.position.y >= 5 || transform.position.x <=-9.5 || transform.position.y<=-5 || transform.position.x >=9.5) Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag=="Player")
        {
            collision.gameObject.GetComponent<Health>().health -= damage;
            if (playerHealth.health <= 0)
            {
                LifeLost();
            }
            collision.gameObject.GetComponent<Animation>().Play("Player_Hurt");
            FMODUnity.RuntimeManager.PlayOneShot("event:/Game/playerdamaged");
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Shield")
        {
            Destroy(gameObject);
        }
    }

    public void LifeLost()
    {
        GameObject.Find("GameController").GetComponent<GameController>().LifeLost();
    }

}
