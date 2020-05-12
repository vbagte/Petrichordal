using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax;
}

public class PlayerController : MonoBehaviour
{

    public float speed;
    public float fireRate;
    public Boundary boundary;
    public GameObject shot;
    public Transform shotSpawn;

    private float nextFire;
    private Rigidbody2D rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shot.GetComponent<Transform>().rotation);
            //GetComponent<AudioSource>().Play();
        }
    }

    private void FixedUpdate()
    {
        int moveX = 0;
        int moveY = 0;

        if (Input.GetKey(KeyCode.D))
        {
            moveX = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1;
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            moveX = 0;
        }
        if (Input.GetKey(KeyCode.W))
        {
            moveY = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveY = -1;
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            moveY = 0;
        }

        Vector2 movement = new Vector2(moveX, moveY);
        rigidbody.velocity = movement * speed;

        rigidbody.position = new Vector2
        (
            Mathf.Clamp(rigidbody.position.x, boundary.xMin, boundary.xMax),
            Mathf.Clamp(rigidbody.position.y, boundary.yMin, boundary.yMax)
        );
    }
}
