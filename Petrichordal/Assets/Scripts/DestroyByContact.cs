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
        // enemy projectile hits player
        if (this.CompareTag("EnemyShot") && other.CompareTag("Player"))
        {
            other.GetComponent<Animation>().Play("Player_Hurt");
            FMODUnity.RuntimeManager.PlayOneShot("event:/Game/playerdamaged");
            playerHealth.health -= this.GetComponent<Damage>().damage;
            Destroy(this.gameObject);
            if (playerHealth.health <= 0)
            {
                LifeLost();
            }
        }
        // enemy projectile hits player shield
        if (this.CompareTag("EnemyShot") && other.CompareTag("Shield"))
        {
            Destroy(this.gameObject);
        }
        // enemy hit by player projectile
        if (this.CompareTag("Enemy") && other.CompareTag("PlayerShot"))
        {
            this.GetComponent<Animation>().Play("Enemy_Hurt");
            FMODUnity.RuntimeManager.PlayOneShot("event:/Game/enemydamaged");
            this.GetComponent<Health>().health -= other.GetComponent<Damage>().damage;
            Destroy(other.gameObject);
            if (this.GetComponent<Health>().health <= 0)
            {
                scoreupdate();
                Instantiate(explosion, this.transform.position, this.transform.rotation);
                FMODUnity.RuntimeManager.PlayOneShot("event:/Game/enemydeath");
                Destroy(this.gameObject);
               
            }
        }
        // player projectile hits land
        if (this.CompareTag("Land") && other.CompareTag("PlayerShot"))
        {
            Destroy(other.gameObject);
        }
        // enemy hit by player tri attack
        if (this.CompareTag("Enemy") && other.CompareTag("TriShot"))
        {
            this.GetComponent<Animation>().Play("Enemy_Hurt");
            FMODUnity.RuntimeManager.PlayOneShot("event:/Game/enemydamaged");
            this.GetComponent<Health>().health -= other.GetComponent<Damage>().damage;
            if (this.GetComponent<Health>().health <= 0)
            {
                scoreupdate();
                Instantiate(explosion, this.transform.position, this.transform.rotation);
                FMODUnity.RuntimeManager.PlayOneShot("event:/Game/enemydeath");
                Destroy(this.gameObject);
            }
        }
        // enemy collides with player
        if (this.CompareTag("Enemy") && other.CompareTag("Player"))
        {
            other.GetComponent<Animation>().Play("Player_Hurt");
            FMODUnity.RuntimeManager.PlayOneShot("event:/Game/playerdamaged");
            playerHealth.health -= this.GetComponent<Damage>().damage;
            if (playerHealth.health <= 0)
            {
                LifeLost();
            }
            Instantiate(explosion, this.transform.position, this.transform.rotation);
            FMODUnity.RuntimeManager.PlayOneShot("event:/Game/enemydeath");
            Destroy(this.gameObject);
        }
        // stalagmite hit by player projectile or tri attack
        if (this.CompareTag("Stalag") && other.CompareTag("PlayerShot") || this.CompareTag("Stalag") && other.CompareTag("TriShot"))
        {
            this.GetComponent<Health>().health -= other.GetComponent<Damage>().damage;
            this.GetComponent<Animation>().Play();
            FMODUnity.RuntimeManager.PlayOneShot("event:/Game/enemydamaged");
            Destroy(other.gameObject);
            if (this.GetComponent<Health>().health <= 0)
            {
                Instantiate(explosion, this.transform.position, this.transform.rotation);
                FMODUnity.RuntimeManager.PlayOneShot("event:/Game/enemydeath");
                Destroy(this.gameObject);
            }
        }
        // player makes contact with lava
        if (this.name == "Lava" && other.CompareTag("Player"))
        {
            lavaActive = true;
            FMODUnity.RuntimeManager.PlayOneShot("event:/Game/playerdamaged");
            StartCoroutine("LavaHurt");
        }
        // player hit by fireball
        if (this.CompareTag("Fireball") && other.CompareTag("Player"))
        {
            playerHealth.health -= this.gameObject.GetComponent<Damage>().damage;
            other.gameObject.GetComponent<Animation>().Play("Player_Hurt");
            FMODUnity.RuntimeManager.PlayOneShot("event:/Game/playerdamaged");
            Instantiate(explosion, this.transform.position, this.transform.rotation);
            Destroy(gameObject);
            if (playerHealth.health <= 0)
            {
                LifeLost();
            }
        }
        // player hit by smoke
        if (this.name == "Smoke_Trigger" && other.CompareTag("Player"))
        {
            smokeActive = true;
            FMODUnity.RuntimeManager.PlayOneShot("event:/Game/playerdamaged");
            StartCoroutine("SmokeHurt");
        }
    }
    void scoreupdate()
    {
        if (tag == "Boss" || tag == "BossPart") playerstats.score += 25000;
        if (tag == "MIDBOSS") playerstats.score += 10000;
        if (tag == "Enemy") playerstats.score += 1000;
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
        // player collides with stalagmite
        if (this.transform.CompareTag("Stalag") && other.transform.CompareTag("Player"))
        {
            playerHealth.health -= this.gameObject.GetComponent<Damage>().damage;
            other.gameObject.GetComponent<Animation>().Play("Player_Hurt");
            FMODUnity.RuntimeManager.PlayOneShot("event:/Game/playerdamaged");
            if (playerHealth.health <= 0)
            {
                LifeLost();
            }
        }
        // player collides with pipe
        if (this.transform.name == "Pipe" && other.transform.CompareTag("Player"))
        {
            playerHealth.health -= this.gameObject.GetComponent<Damage>().damage;
            other.gameObject.GetComponent<Animation>().Play("Player_Hurt");
            FMODUnity.RuntimeManager.PlayOneShot("event:/Game/playerdamaged");
            if (playerHealth.health <= 0)
            {
                LifeLost();
            }
        }
    }

    void LifeLost()
    {
        GameObject.Find("GameController").GetComponent<GameController>().LifeLost();
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game/playerdeath");
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
