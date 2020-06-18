//==============================================================================
//Project:       Petrichordal
//File Name:     Scroll.cs
//Author:        Cera Baltzley
//Class:         CS 185 2020 Spring Quarter
//Description:   This script is used for scrolling backgrounds and enemies, with 
//                  option for horizontal or vertial, and speed (negative speed
//                  values may be used to reverse direction)
//==============================================================================
//Known Bugs: 
//==============================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{

    public float speed;
    public enum Edirection { horizontal=1,vertical=2 };
    public Edirection direction;
    //public float speed;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        //transform.Translate(-layerspeed / 1000, 0, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.Translate(-layerspeed / 1000, 0, 0);


    }
    void Update()
    {
        //rb = GetComponent<Rigidbody2D>();
        //switch ((int)direction)
        //{
        //    case 1:
        //        rb.velocity = new Vector3(-speed, 0, 0);

        //        break;
        //    case 2:
        //        rb.velocity = new Vector3(0, -speed, 0);

        //        break;
        //}


        switch ((int)direction)
        {
            case 1:
                transform.Translate(-speed * Time.deltaTime, 0, 0);
                // transform.Rotate(0, 0, transform.eulerAngles.z);
                break;
            case 2:
                transform.Translate(0, -speed * Time.deltaTime, 0);
                //transform.Rotate(0, 0, transform.eulerAngles.z);
                break;
        }


    }
}
