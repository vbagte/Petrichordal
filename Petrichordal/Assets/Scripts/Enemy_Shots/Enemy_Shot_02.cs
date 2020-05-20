using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shot_02 : MonoBehaviour
{

    public float speed = 5f;
    
    private Transform target;
    private Vector3 normalizeDirection;

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
    }

    void Start()
    {
        normalizeDirection = (target.position - transform.position).normalized;
    }

    void Update()
    {
        transform.position += normalizeDirection * speed * Time.deltaTime;
    }

}

//Seeking Missle Code
//
//public float speed;

//private float step;
//public Transform playerTransform;
//public Vector2 target;

//private void Awake()
//{
//    playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
//}

//private void Start()
//{
//    target = new Vector2(playerTransform.position.x, playerTransform.position.y);
//}

//private void Update()
//{
//    step = speed * Time.deltaTime;
//}

//private void FixedUpdate()
//{
//    transform.position = Vector2.MoveTowards(transform.position, target, step);
//}
