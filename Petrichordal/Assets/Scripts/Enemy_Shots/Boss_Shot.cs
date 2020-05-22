using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Shot : MonoBehaviour
{

    public float speed;
    public float trackTime;

    private float step;
    private bool trackActive;
    private Transform playerTransform;
    private Vector2 target;
    private Vector3 normalizeDirection;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        trackActive = true;
        StartCoroutine(TrackTime());
    }

    private void Update()
    {
        if (trackActive)
        {
            target = new Vector2(playerTransform.position.x, playerTransform.position.y);
            step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target, step);
            normalizeDirection = (playerTransform.position - transform.position).normalized;
        }
        else
        {
            transform.position += normalizeDirection * speed * Time.deltaTime;
        }
    }

    IEnumerator TrackTime()
    {
        yield return new WaitForSeconds(trackTime);
        trackActive = false;
    }

}
