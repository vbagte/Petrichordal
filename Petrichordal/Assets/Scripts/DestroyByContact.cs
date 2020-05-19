using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour
{

    public GameObject explosion;
    public GameObject otherExplosion;   
    public float playerIFramesTime;

    private GameObject player;
    private Health playerHealth;
    private bool playerIFramesActive;
    private bool lavaActive;

    private void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (this.CompareTag("EnemyShot") && other.CompareTag("Player"))
        {
            other.GetComponent<Animation>().Play("Player_Hurt");
            playerHealth.health -= this.GetComponent<Damage>().damage;
            Destroy(this.gameObject);
            if (playerHealth.health <= 0)
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
            playerHealth.health -= this.GetComponent<Damage>().damage;
            if (playerHealth.health <= 0)
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
        if (this.name == "Lava" && other.CompareTag("Player"))
        {
            lavaActive = true;
            StartCoroutine("LavaHurt");
        }
        if (this.CompareTag("Fireball") && other.CompareTag("Player"))
        {
            playerHealth.health -= this.gameObject.GetComponent<Damage>().damage;
            other.gameObject.GetComponent<Animation>().Play("Player_Hurt");
            Instantiate(explosion, this.transform.position, this.transform.rotation);
            Destroy(gameObject);
            if (playerHealth.health <= 0)
            {
                Instantiate(otherExplosion, other.gameObject.GetComponent<Transform>().transform.position, other.gameObject.GetComponent<Transform>().transform.rotation);
                PlayerDeath();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (this.name == "Lava" && other.CompareTag("Player"))
        {
            lavaActive = false;
            StopCoroutine("LavaHurt");
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (this.transform.CompareTag("Stalag") && other.transform.CompareTag("Player"))
        {
            if (playerIFramesActive == false)
            {
                playerHealth.health -= this.gameObject.GetComponent<Damage>().damage;
                other.gameObject.GetComponent<Animation>().Play("Player_Hurt");
                playerIFramesActive = true;
                StartCoroutine(PlayerIFrames());
                if (playerHealth.health <= 0)
                {
                    Instantiate(otherExplosion, other.gameObject.GetComponent<Transform>().transform.position, other.gameObject.GetComponent<Transform>().transform.rotation);
                    PlayerDeath();
                }
            }
        }
    }

    void PlayerDeath()
    {
        playerHealth.health = 0;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().HealthUpdate();
        GameObject.Find("GameController").GetComponent<GameController>().PlayerDeath();
    }

    IEnumerator PlayerIFrames()
    {
        yield return new WaitForSeconds(playerIFramesTime);
        playerIFramesActive = false;
    }

    IEnumerator LavaHurt()
    {
        while(lavaActive)
        {
            player.GetComponent<Animation>().Play("Player_Hurt");
            playerHealth.health -= GameObject.Find("Lava").GetComponent<Damage>().damage;
            if (playerHealth.health <= 0)
            {
                Instantiate(explosion, player.GetComponent<Transform>().transform.position, player.GetComponent<Transform>().transform.rotation);
                PlayerDeath();
                lavaActive = false;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

}
