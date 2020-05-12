using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour
{

    public GameObject explosion;
    public GameObject playerExplosion;
    public int scoreValue;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boundary"))
        {
            return;
        }
        if (other.CompareTag("PlayerShot") && this.CompareTag("EnemyShot"))
        {
            return;
        }
        if (other.CompareTag("EnemyShot") && this.CompareTag("Enemy"))
        {
            return;
        }
        if (other.CompareTag("Enemy") && this.CompareTag("EnemyShot"))
        {
            return;
        }
        if (other.tag == "Player")
        {
            Destroy(other.gameObject);
            Instantiate(playerExplosion, other.GetComponent<Transform>().position, other.GetComponent<Transform>().rotation);
        }
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(other.gameObject);
        Destroy(gameObject);
    }

}
