using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour
{

    public GameObject explosion;
    public Transform explosionPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boundary")
        {
            return;
        }
        Instantiate(explosion, explosionPoint.position, transform.rotation);
        Destroy(other.gameObject);
        Destroy(gameObject);
        if (other.tag == "Player")
        {
            Destroy(other.gameObject);
            Instantiate(explosion, other.GetComponent<Transform>().position, other.GetComponent<Transform>().rotation);
        }
    }

}
