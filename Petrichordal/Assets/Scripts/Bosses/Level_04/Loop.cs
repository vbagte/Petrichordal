using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loop : MonoBehaviour
{
    public GameObject foreground;
    public GameObject midground;
    public GameObject background;
    public float foregroundX;
    public float midgroundX;
    public float backgroundX;
    private float y;
    private float z;

    // Start is called before the first frame update
    void Start()
    {
        y = foreground.transform.position.y;
        z = foreground.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= 10 && transform.position.x >= -10 && transform.position.y >= -5.5 && transform.position.y < 3.5)
        {
            foreground.transform.position = new Vector3(foregroundX,y,z);
            midground.transform.position = new Vector3(midgroundX,y,z);
            background.transform.position = new Vector3(backgroundX,y,z);
        }
    }
}
