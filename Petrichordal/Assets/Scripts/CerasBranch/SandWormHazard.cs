﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandWormHazard : MonoBehaviour
{
    private long initial_entry_frames=0;
    private long counter;
    private int swapnegative = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (initial_entry_frames < 180)
        {
            transform.Translate(2f * Time.deltaTime, 0, 0);
            initial_entry_frames++;
        }
        else if (initial_entry_frames > 1700)
        {
            transform.Translate(-2f * Time.deltaTime, 0, 0);
            initial_entry_frames++;
        }
        else if (counter < 90)
        {
            transform.Translate(1f *swapnegative*Time.deltaTime, 0, 0);
            counter++;
            initial_entry_frames++;
        }
        if (counter ==90)
        {
            if (swapnegative==1)
            {
                gameObject.GetComponentInChildren<Enemy>().speed *= -.25f;
            } else if (swapnegative == -1)
            {
                gameObject.GetComponentInChildren<Enemy>().speed *= -4f;
            }
           
            swapnegative *= -1;
            counter = 0;
        }
            
 

    }
}