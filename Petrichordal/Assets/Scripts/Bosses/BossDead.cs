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
    public float exitSpeed;

    private bool deadActive = false;
    private bool exitActive = false;

    private void Update()
    {
        if (bossMain == null && deadActive == false)
        {
            StartCoroutine(Death());
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

}
