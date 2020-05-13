using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour
{

    public GameObject explosion;
    public GameObject playerExplosion;

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
        if (other.CompareTag("EnemyShot") && this.CompareTag("EnemyShot"))
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
        if (other.CompareTag("Player") && this.CompareTag("EnemyShot"))
        {
            if (other.GetComponent<PlayerController>().health > this.GetComponent<EnemyShotLogic>().damage)
            {
                other.GetComponent<Animation>().Play("Player_Hurt");
                other.GetComponent<PlayerController>().health -= this.GetComponent<EnemyShotLogic>().damage;
                other.GetComponent<PlayerController>().StartCoroutine("PlayerITimer");
                Destroy(this.gameObject);
            }
            else
            {
                other.GetComponent<PlayerController>().health = 0;
                other.GetComponent<PlayerController>().StartCoroutine("PlayerITimer");
                Destroy(other.gameObject);
                Destroy(this.gameObject);
                Instantiate(playerExplosion, other.GetComponent<Transform>().position, other.GetComponent<Transform>().rotation);
            }
        }
        if (other.CompareTag("PlayerShot") && this.CompareTag("Enemy"))
        {
            if (this.GetComponent<EnemyLogic>().health > 1)
            {
                this.GetComponent<Animation>().Play("Enemy_Hurt");
                this.GetComponent<EnemyLogic>().health -= other.GetComponent<PlayerShotLogic>().damage;
                Destroy(other.gameObject);
            }
            else
            {
                Destroy(other.gameObject);
                Destroy(this.gameObject);
                Instantiate(explosion, this.GetComponent<Transform>().position, this.GetComponent<Transform>().rotation);
            }
        }
        if (other.CompareTag("Player") && this.CompareTag("Enemy"))
        {
            if (other.GetComponent<PlayerController>().health > 1)
            {
                other.GetComponent<Animation>().Play("Player_Hurt");
                other.GetComponent<PlayerController>().health -= 1;
                other.GetComponent<PlayerController>().StartCoroutine("PlayerITimer");
                if (this.GetComponent<EnemyLogic>().health > 1)
                {
                    this.GetComponent<Animation>().Play("Enemy_Hurt");
                    this.GetComponent<EnemyLogic>().health -= 1;
                }
                else
                {
                    Destroy(this.gameObject);
                    Instantiate(explosion, this.GetComponent<Transform>().position, this.GetComponent<Transform>().rotation);
                }
            }
            else
            {
                other.GetComponent<PlayerController>().health -= 1;
                other.GetComponent<PlayerController>().StartCoroutine("PlayerITimer");
                Destroy(other.gameObject);
                Destroy(this.gameObject);
                Instantiate(explosion, this.GetComponent<Transform>().position, this.GetComponent<Transform>().rotation);
                Instantiate(playerExplosion, other.GetComponent<Transform>().position, other.GetComponent<Transform>().rotation);
            }
        }
        //Instantiate(explosion, transform.position, transform.rotation);
        //Destroy(other.gameObject);
        //Destroy(gameObject);
    }

}
