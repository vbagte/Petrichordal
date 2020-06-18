using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossDead3 : MonoBehaviour
{

    public GameObject player;
    public GameObject bossMain;
    public GameObject exitSpot;
    public GameObject eventTrigger;
    public GameObject explosion;
    public GameObject[] explosionSpots;
    public float exitSpeed;
    public float bossExitSpeed;

    private bool deadActive = false;
    private bool explosionActive = false;
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
            explosionActive = true;
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
        player.tag = "Untagged";
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<Collider2D>().isTrigger = true;
        bossMain.SetActive(false);
            //bossMain.GetComponent<Enemy>().enabled = false;
        Enemy[] scripts = GameObject.FindObjectsOfType<Enemy>();
        foreach (Enemy script in scripts)
        {
            script.enabled = false;
        }
        eventtrigger[] events = GameObject.FindObjectsOfType<eventtrigger>();
        foreach (eventtrigger Event in events)
        {
            Event.enabled = false;
        }
        Vector2 stop = new Vector2(0, 0);
        player.GetComponent<Rigidbody2D>().velocity = stop;
        yield return new WaitForSeconds(1);
        exitActive = true;
        yield return new WaitForSeconds(2);
        explosionActive = false;
        GameObject.Find("GameController").GetComponent<GameController>().NextLevelPanel();
    }

    public IEnumerator BossExplode()
    {
        do
        {
            Instantiate(explosion, explosionSpots[Random.Range(0, 8)].transform.position, explosion.transform.rotation);
            yield return new WaitForSeconds(0.2f);
        } while (explosionActive);
    }

}
