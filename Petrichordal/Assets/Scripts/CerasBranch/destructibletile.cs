//==============================================================================
//Project:       Petrichordal
//File Name:     destructibletile.cs
//Author:        Cera Baltzley
//Class:         CS 185 2020 Spring Quarter
//Description:   This script is not currently enabled in game(commented out), but may be used to
//                    allow for the player to destroy individual tiles with its weapon

//==============================================================================
//Known Bugs:   
//==============================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class destructibletile : MonoBehaviour
{
  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D collision)
    {
              Tilemap tilemap = GetComponent<Tilemap>();
        if (collision.gameObject.tag == "PlayerShot")
        {
        //Vector3 hitPosition = new Vector3();
        //    foreach (ContactPoint2D hit in collision.contacts)
        //    {
        //    hitPosition.x = hit.point.x;
        //    hitPosition.y = hit.point.y;
        //        tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
        //    }
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Player")
        {
       
            collision.gameObject.GetComponent<Health>().health -= 20;
        }

    }
}
