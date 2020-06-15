//==============================================================================
//Project:       Petrichordal
//File Name:     EventSandWorm.cs
//Author:        Cera Baltzley
//Class:         CS 185 2020 Spring Quarter
//Description:   This script is used by an event trigger to spawn the sandworm
//
//==============================================================================
//Known Bugs: 
//==============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSandWorm : MonoBehaviour
{
    public GameObject sandwormcontainer;
    private GameObject sandGO;

    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= 10 && transform.position.x >= -10 && transform.position.y >= -5.5 && transform.position.y < 3.5)
        {
            sandGO = Instantiate(sandwormcontainer);
            sandGO.GetComponent<Transform>().Translate(-1, 0, 0);
            Destroy(gameObject);
   
        }
    }
}
