using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossDead4 : MonoBehaviour
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
    private bool bossDown = false;
    private bool exitActive = false;

    private void Start()
    {
        explosionSpots = GameObject.FindGameObjectsWithTag("ExplosionSpot");
    }

    private void Update()
    {
        if (bossMain.GetComponent<SpaceStationWhole>().hitpoints < 1 && deadActive == false)
        {
            StartCoroutine(Death());
            StartCoroutine(BossExplode());
            deadActive = true;
            bossDown = true;
        }
        if (bossDown)
        {
            bossMain.transform.Translate(0, -bossExitSpeed * Time.deltaTime, 0);
            if (bossMain.transform.position.y <= -6)
            {
                bossExitSpeed = 0;
                bossDown = false;
            }
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
        bossMain.GetComponent<Enemy>().enabled = false;
        Enemy[] scripts = GameObject.FindObjectsOfType<Enemy>();
        foreach (Enemy script in scripts)
        {
            script.enabled = false;
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
            yield return new WaitForSeconds(0.2f);
        } while (bossDown);
    }

}
