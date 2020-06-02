using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public enum Edirection { vertical=1,horizontal=2};
    public Edirection direction;
    public float speed;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        switch ((int)direction)
        {
            case 1:
                rb.velocity = new Vector3(0,speed, 0);
                break;
            case 2:
                rb.velocity = new Vector3(speed, 0, 0);
                break;
        }
     
    }

}
