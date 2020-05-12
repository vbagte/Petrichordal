using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Movement
{
    public float xSpeed, ySpeed, yMax, yMin;
}

public class Lvl1_Wave01Mover : MonoBehaviour
{

    public Movement movement;

    private float xMove;
    private float yMove;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        xMove = movement.xSpeed;
    }

    private void Update()
    {
        if (rb.position.y >= movement.yMax)
        {
            yMove = -movement.ySpeed;
        }
        else if (rb.position.y <= movement.yMin)
        {
            yMove = movement.ySpeed;
        }
    }

    void FixedUpdate()
    {
        Vector2 movement = new Vector2(xMove, yMove);
        rb.velocity = movement;
    }

}
