using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByBoundary : MonoBehaviour
{
    public static bool playerLeft = false;

    private void OnCollisionExit2D(Collision2D collision)
    {
      
        Destroy(collision.gameObject);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        //Destroy(other.gameObject);
      
        if (other.CompareTag("Player"))
        {
           // playerLeft = true;
        }
        else
        {
            Destroy(other.gameObject);
        }
    }
}
