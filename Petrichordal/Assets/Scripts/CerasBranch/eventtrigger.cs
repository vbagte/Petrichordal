using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eventtrigger : MonoBehaviour
{
    private long counter;
    public long seconds=8;
    public float speed_on_exit_multiplier = 1;
    public float new_speed_multiplier = 1;
        public GameObject foreground;
    public GameObject midground;
    public GameObject background;
    private float bgOS, mgOS, fgOS;
    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
        bgOS = background.GetComponent<Scroll>().layerspeed;
        mgOS = midground.GetComponent<Scroll>().layerspeed;
        fgOS = foreground.GetComponent<Scroll>().layerspeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= 10 && transform.position.x >= -10 && transform.position.y >= -5.5 && transform.position.y < 3.5)    
        {
            counter++;
            if (Mathf.Floor(counter / 60) < seconds)
            {
                background.GetComponent<Scroll>().layerspeed = bgOS * new_speed_multiplier;
                midground.GetComponent<Scroll>().layerspeed = mgOS * new_speed_multiplier;
                foreground.GetComponent<Scroll>().layerspeed = fgOS * new_speed_multiplier;
            }
            else
            {
               background.GetComponent<Scroll>().layerspeed = bgOS*speed_on_exit_multiplier;
                midground.GetComponent<Scroll>().layerspeed = mgOS * speed_on_exit_multiplier;
                foreground.GetComponent<Scroll>().layerspeed = fgOS * speed_on_exit_multiplier;
                Destroy(gameObject);
            }
             
          
       
        }
    }
}
