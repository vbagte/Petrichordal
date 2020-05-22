using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Shot02 : MonoBehaviour
{

    public float speed = 5f;  

    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }

}