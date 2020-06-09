using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lazarglow : MonoBehaviour
{
        private long counter=0;
    private bool fading = false;
    // Start is called before the first frame update
    void Start()
    {
        //make image transparent
        GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        counter++; //counter is used to keep track of fading in and out
        if (counter >= 15) fading = true; //first 1/4 second we fade in, second 1/4 we fade out
        if  (fading==false)
        {
            GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, .0666f); //fade in
        }
        else GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, .0666f); //fade out
        if (counter >= 30) Destroy(gameObject); //destroy object
    }
}
