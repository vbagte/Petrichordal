using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wiggler : MonoBehaviour
{

    public int frames_per_wiggle;
    private int counter= 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (counter < frames_per_wiggle-1) 
        {
            transform.Translate(.5f * Time.deltaTime,0, 0);
        }
        else if (counter >= frames_per_wiggle*2-3 ) counter = 0;
        else transform.Translate(-.5f * Time.deltaTime, 0, 0);
        counter++;

    }
}
