using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyByContact : MonoBehaviour
{

    public GameObject explosion;
    public GameObject otherExplosion;   

    private GameObject player;
    private Health playerHealth;
    private bool lavaActive;
    private bool smokeActive;

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
                LifeLost();
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
        if (this.CompareTag("Enemy") && other.CompareTag("TriShot"))
        {
            this.GetComponent<Animation>().Play("Enemy_Hurt");
            this.GetComponent<Health>().health -= other.GetComponent<Damage>().damage;
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
                LifeLost();
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
                LifeLost();
            }
        }
        if (this.name == "Smoke_Trigger" && other.CompareTag("Player"))
        {
            smokeActive = true;
            StartCoroutine("SmokeHurt");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (this.name == "Lava" && other.CompareTag("Player"))
        {
            lavaActive = false;
            StopCoroutine("LavaHurt");
        }
        if (this.name == "Smoke_Trigger" && other.CompareTag("Player"))
        {
            smokeActive = false;
            StopCoroutine("SmokeHurt");
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (this.transform.CompareTag("Stalag") && other.transform.CompareTag("Player"))
        {
            playerHealth.health -= this.gameObject.GetComponent<Damage>().damage;
            other.gameObject.GetComponent<Animation>().Play("Player_Hurt");
            if (playerHealth.health <= 0)
            {
                LifeLost();
            }
        }
        if (this.transform.name == "Pipe" && other.transform.CompareTag("Player"))
        {
            playerHealth.health -= this.gameObject.GetComponent<Damage>().damage;
            other.gameObject.GetComponent<Animation>().Play("Player_Hurt");
            if (playerHealth.health <= 0)
            {
                LifeLost();
            }
        }
    }

    void LifeLost()
    {
        GameObject.Find("GameController").GetComponent<GameController>().LifeLost();
    }

    IEnumerator LavaHurt()
    {
        while(lavaActive)
        {
            player.GetComponent<Animation>().Play("Player_Hurt");
            playerHealth.health -= GameObject.Find("Lava").GetComponent<Damage>().damage;
            if (playerHealth.health <= 0)
            {
                LifeLost();
                lavaActive = false;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator SmokeHurt()
    {
        while (smokeActive)
        {
            player.GetComponent<Animation>().Play("Player_Hurt");
            playerHealth.health -= GameObject.Find("Smoke_Trigger").GetComponent<Damage>().damage;
            if (playerHealth.health <= 0)
            {
                LifeLost();
                smokeActive = false;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

}
