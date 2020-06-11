using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStationWhole : MonoBehaviour
{
    public int hitpoints;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(hitpoints < 1){
            Destroy(gameObject);
        }
    }
}
