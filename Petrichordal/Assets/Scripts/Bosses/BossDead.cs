using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossDead : MonoBehaviour
{

    public GameObject player;
    public GameObject bossMain;
    public GameObject exitSpot;
    public GameObject eventTrigger;
    public GameObject explosion;
    public GameObject[] explosionSpots;
    public float exitSpeed;

    private bool deadActive = false;
    private bool exitActive = false;

    private void Start()
    {
        explosionSpots = GameObject.FindGameObjectsWithTag("ExplosionSpot");
    }

    private void Update()
    {
        if (bossMain.GetComponent<Enemy>().health < 1 && deadActive == false)
        {
            StartCoroutine(Death());
            StartCoroutine(BossExplode());
            deadActive = true;
        }
        if (exitActive)
        {
            float step = exitSpeed * Time.deltaTime; // calculate distance to move
            player.transform.position = Vector3.MoveTowards(player.transform.position, exitSpot.transform.position, step);
        }
    }

    IEnumerator Death()
    {
        //Lvl1_Manager.bossMusic.setParameterByName("BossWin", 1);
        player.GetComponent<PlayerController>().enabled = false;
        bossMain.GetComponent<Enemy>().enabled = false;
        if (SceneManager.GetActiveScene().name == "Level_02")
        {
            player.tag = "Untagged";
        }
        foreach(Enemy script in GetComponentsInChildren<Enemy>())
        {
            script.enabled = false;
        }
        foreach (Collider2D collider in GetComponentsInChildren<Collider2D>())
        {
            collider.enabled = false;
        }
        Vector2 stop = new Vector2(0, 0);
        player.GetComponent<Rigidbody2D>().velocity = stop;
        yield return new WaitForSeconds(1);
        exitActive = true;
        yield return new WaitForSeconds(2);
        GameObject.Find("GameController").GetComponent<GameController>().NextLevelPanel();
    }

    public IEnumerator BossExplode()
    {
        do
        {
            Instantiate(explosion, explosionSpots[Random.Range(0, 8)].transform.position, explosion.transform.rotation);
            yield return new WaitForSeconds(0.5f);
        } while (deadActive);
    }

}
