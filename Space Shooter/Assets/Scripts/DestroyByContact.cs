using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour
{

    public GameObject explosion;
    public Transform explosionPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            Instantiate(explosion, explosionPoint.position, transform.rotation);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

}
