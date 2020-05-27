using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballMover : MonoBehaviour
{

    public float force;
    public float flipTime;

    private float currentYPosition;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentYPosition = transform.position.y;
        Vector2 speed = new Vector2(0, force);
        rb.AddForce(speed);
    }

    private void Update()
    {
        if (transform.position.y > currentYPosition)
        {
            currentYPosition = transform.position.y;
        }

        if (transform.position.y < currentYPosition)
        {
            GetComponent<SpriteRenderer>().flipY = true;
        }
    }

}
