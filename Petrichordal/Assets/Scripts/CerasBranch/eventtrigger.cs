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
  //  public GameObject enemylayer;
    private float bgOS, mgOS, fgOS;
    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
        bgOS = background.GetComponent<Scroll>().speed;
        mgOS = midground.GetComponent<Scroll>().speed;
        fgOS = foreground.GetComponent<Scroll>().speed;
        //elOS = enemylayer.GetComponent<Scroll>().layerspeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= 10 && transform.position.x >= -10 && transform.position.y >= -5.5 && transform.position.y < 3.5)    
        {
            counter++;
            if (Mathf.Floor(counter / 60) < seconds)
            {
                background.GetComponent<Scroll>().speed = bgOS * new_speed_multiplier;
                midground.GetComponent<Scroll>().speed = mgOS * new_speed_multiplier;
                foreground.GetComponent<Scroll>().speed = fgOS * new_speed_multiplier;
                //enemylayer.GetComponent<Scroll>().layerspeed = elOS * new_speed_multiplier;
            }
            else
            {
               background.GetComponent<Scroll>().speed = bgOS*speed_on_exit_multiplier;
                midground.GetComponent<Scroll>().speed = mgOS * speed_on_exit_multiplier;
                foreground.GetComponent<Scroll>().speed = fgOS * speed_on_exit_multiplier;
                //enemylayer.GetComponent<Scroll>().layerspeed = elOS * speed_on_exit_multiplier;
                Destroy(gameObject);
            }
             
          
       
        }
    }
}
