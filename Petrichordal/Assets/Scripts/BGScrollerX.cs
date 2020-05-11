using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScrollerX : MonoBehaviour
{

    public float scrollSpeed;
    public float tileSizeX;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeX);
        transform.position = startPosition + Vector3.left * newPosition;
    }

}
