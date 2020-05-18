using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour
{

    public GameObject explosion;
    public GameObject otherExplosion;   
    public float playerIFramesTime;

    private bool playerIFramesActive;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (this.CompareTag("EnemyShot") && other.CompareTag("Player"))
        {
            other.GetComponent<Animation>().Play("Player_Hurt");
            other.GetComponent<Health>().health -= this.GetComponent<Damage>().damage;
            Destroy(this.gameObject);
            if (other.GetComponent<Health>().health <= 0)
            {
                Instantiate(explosion, other.transform.position, other.transform.rotation);
                PlayerDeath();
            }
        }
        if (this.CompareTag("Enemy") && other.CompareTag("PlayerShot"))
        {
            this.GetComponent<Animation>().Play("Enemy_Hurt");
            this.GetComponent<Health>().health -= other.GetComponent<Damage>().damage;
            Destroy(other.gameObject);
            if (this.GetComponent<Health>().health <= 0)
            {
                Instantiate(explosion, this.transform.position, this.transform.rotation);               
                Destroy(this.gameObject);
            }
        }
        if (this.CompareTag("Enemy") && other.CompareTag("Player"))
        {
            other.GetComponent<Animation>().Play("Player_Hurt");
            other.GetComponent<Health>().health -= this.GetComponent<Damage>().damage;
            if (other.GetComponent<Health>().health <= 0)
            {
                Instantiate(explosion, other.transform.position, other.transform.rotation);
                PlayerDeath();
            }
            Instantiate(explosion, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }
        if (this.CompareTag("Stalag") && other.CompareTag("PlayerShot"))
        {
            this.GetComponent<Health>().health -= other.GetComponent<Damage>().damage;
            this.GetComponent<Animation>().Play();
            Destroy(other.gameObject);
            if (this.GetComponent<Health>().health <= 0)
            {
                Instantiate(explosion, this.transform.position, this.transform.rotation);
                Destroy(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (this.transform.CompareTag("Stalag") && other.transform.CompareTag("Player"))
        {
            if (playerIFramesActive == false)
            {
                other.gameObject.GetComponent<Health>().health -= this.gameObject.GetComponent<Damage>().damage;
                other.gameObject.GetComponent<Animation>().Play("Player_Hurt");
                playerIFramesActive = true;
                StartCoroutine(PlayerIFrames());
                if (other.gameObject.GetComponent<Health>().health <= 0)
                {
                    Instantiate(otherExplosion, other.gameObject.GetComponent<Transform>().transform.position, other.gameObject.GetComponent<Transform>().transform.rotation);
                    PlayerDeath();
                }
            }
        }
    }

    void PlayerDeath()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().health = 0;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().HealthUpdate();
        GameObject.Find("GameController").GetComponent<GameController>().PlayerDeath();
    }

    IEnumerator PlayerIFrames()
    {
        yield return new WaitForSeconds(playerIFramesTime);
        playerIFramesActive = false;
    }
}
