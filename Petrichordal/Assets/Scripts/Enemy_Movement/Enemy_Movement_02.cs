using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement_02 : MonoBehaviour
{

    public float xMove;
    public float moveInTime;
    public float fireTime;
    public float moveOutTime;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        Vector2 move = new Vector2(xMove, 0);
        rb.velocity = move;
        yield return new WaitForSeconds(moveInTime);
        move = new Vector2(0, 0);
        rb.velocity = move;
        yield return new WaitForSeconds(fireTime);
        GetComponent<WeaponController>().enabled = true;
        yield return new WaitForSeconds(moveOutTime);
        GetComponent<WeaponController>().enabled = false;
        move = new Vector2(-xMove, 0);
        rb.velocity = move;
    }

}
