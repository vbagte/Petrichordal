using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{

    public float layerspeed;
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
        
        transform.Translate(-layerspeed / 1000, 0, 0);

    }
}
