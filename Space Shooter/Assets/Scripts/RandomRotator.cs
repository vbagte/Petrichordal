using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotator : MonoBehaviour
{

    private float tumble;

    private void Start()
    {
        tumble = Random.Range(50, 150);
    }

    private void Update()
    {
        transform.Rotate(0, 0, tumble * Time.deltaTime);
    }

}
