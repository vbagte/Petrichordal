using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{

    public float layerspeed;
    public enum Edirection { horizontal=1,vertical=2 };
    public Edirection direction;

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


        switch ((int)direction)
        {
            case 1:
                transform.Translate(-layerspeed / 1000, 0, 0);
               // transform.Rotate(0, 0, transform.eulerAngles.z);
                break;
            case 2:
                transform.Translate(0, -layerspeed / 1000, 0);
                //transform.Rotate(0, 0, transform.eulerAngles.z);
                break;
        }

    }
}
