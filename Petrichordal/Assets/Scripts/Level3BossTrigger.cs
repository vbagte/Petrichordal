using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3BossTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Boundary")
        {
            GetComponentInParent<Mover>().speed = 0;
        }
    }
}
