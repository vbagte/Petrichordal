using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eventtrigger : MonoBehaviour
{
    private long counter;
    public long seconds=8;
    public int new_speed_multiplier = 1;
        public GameObject foreground;
    public GameObject midground;
    public GameObject background;
    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= 10.5)
        {
            counter++;
            if (Mathf.Floor(counter / 60) < seconds)
            {
               background.GetComponent<Scroll>().layerspeed = 0;
                midground.GetComponent<Scroll>().layerspeed = 0;
                foreground.GetComponent<Scroll>().layerspeed = 0;
            }
            else
            {
               background.GetComponent<Scroll>().layerspeed = 1*new_speed_multiplier;
                midground.GetComponent<Scroll>().layerspeed = 2 * new_speed_multiplier;
                foreground.GetComponent<Scroll>().layerspeed = 16 * new_speed_multiplier;
                Destroy(gameObject);
            }
             
          
       
        }
    }
}
