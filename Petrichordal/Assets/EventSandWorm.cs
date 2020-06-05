using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSandWorm : MonoBehaviour
{
    public GameObject sandwormcontainer;
    private GameObject sandGO;

    // Start is called before the first frame update
    void Start()
    {
        //sandGO=Instantiate(sandwormcontainer);
        //sandGO.GetComponent<Transform>().Translate(-3, 0, 0);
        //Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= 10 && transform.position.x >= -10 && transform.position.y >= -5.5 && transform.position.y < 3.5)
        {
            sandGO = Instantiate(sandwormcontainer);
            sandGO.GetComponent<Transform>().Translate(-1, 0, 0);
            Destroy(gameObject);
   
        }
    }
}
