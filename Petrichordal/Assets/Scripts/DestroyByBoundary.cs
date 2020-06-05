using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByBoundary : MonoBehaviour
{
    public static bool playerLeft = false;

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerLeft = true;
        }
        else
        {
            Destroy(other.gameObject);
        }
    }
}
