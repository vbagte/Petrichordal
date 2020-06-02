using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public static bool gameStart = false;
    public static bool playerEnable = false;

    public GameObject explosion;
    public GameObject[] bg;
    public GameObject[] stalags;
    public GameObject[] pipes;
    public GameObject levelManager;
    public GameObject boss;
    public GameObject pausePanel;
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public GameObject gameOverPanel;
    public GameObject blockPanel;
    public bool paused = false;

    private GameObject player;
    private GameObject playerExit;
    private GameObject[] explosionSpots;
    private Health playerHealth;
    private bool bossDead;
    private bool exitActive = false;
    private Vector3 exitSpot;

    //FMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMOD
    private BeatSystem bS;
    private FMOD.Studio.EventInstance songInstance;
    //FMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMOD

    private void Start()
    {
        //Debug.Log(BeatSystem.bar);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("MusicBarGlobal", BeatSystem.bar);
        //FMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMOD
        songInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Music/lvl1/main");
        bS = GetComponent<BeatSystem>();
        bS.AssignBeatEvent(songInstance);
        if (gameStart == false)
        {
            songInstance.start();
            songInstance.release();
            gameStart = true;
        }
        //FMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMOD

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        }
        playerExit = GameObject.Find("PlayerExitSpot");
        exitSpot = (playerExit.transform.position - player.transform.position);
        playerEnable = false;
    }

    private void Update()
    {
        //restart level
        if (Input.GetKeyDown(KeyCode.Escape) && paused == false)
        {
            pauseMenu.SetActive(true);
            settingsMenu.SetActive(false);
            pausePanel.SetActive(true);
            paused = true;
            Time.timeScale = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && paused == true)
        {
            pausePanel.SetActive(false);
            paused = false;
            Time.timeScale = 1;
        }
        //stop objects after player dies
        if (player.GetComponent<PlayerController>().lives <= 0 || player == null)
        {
            for (int i = 0; i < bg.Length; i++)
            {
                bg[i].GetComponent<BGScrollerX>().enabled = false;
            }
            levelManager.GetComponent<Lvl1_Manager>().stalag.stalagSpeed = 0;
            levelManager.SetActive(false);
            stalags = GameObject.FindGameObjectsWithTag("Stalag");
            foreach (GameObject stalag in stalags)
            {
                Vector2 movement = new Vector2(0, 0);
                stalag.GetComponent<Rigidbody2D>().velocity = movement;
                stalag.GetComponent<Mover>().speed = 0;
            }
            pipes = GameObject.FindGameObjectsWithTag("Pipe");
            foreach (GameObject pipe in pipes)
            {
                Vector2 movement = new Vector2(0, 0);
                pipe.GetComponent<Rigidbody2D>().velocity = movement;
                pipe.GetComponent<Mover>().speed = 0;
            }
            for (int i = 0; i < GameObject.FindGameObjectsWithTag("Enemy").Length; i++)
            {
                GameObject.FindGameObjectsWithTag("Enemy")[i].GetComponent<WeaponController>().enabled = false;
            }
            levelManager.GetComponent<Lvl1_Manager>().lava.fireballActive = false;
            boss.GetComponent<WardenBoss>().enabled = false;
        }
        if (exitActive)
        {
            player.transform.position += exitSpot * 0.25f * Time.deltaTime;
        }
        //if player leaves screen after defeating boss
        if (DestroyByBoundary.playerLeft == true)
        {
            bossDead = false;
            exitActive = false;            
        }
    }

    public void LifeLost()
    {
        player.GetComponent<PlayerController>().LifeLost();
        playerHealth.health = player.GetComponent<PlayerController>().healthMax;
        if (player != null && player.GetComponent<PlayerController>().lives > 0)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().HealthUpdate();
            player.GetComponent<PlayerController>().enabled = false;
            player.GetComponent<BoxCollider2D>().enabled = false;
            player.GetComponent<Animation>().Stop();
            player.GetComponent<SpriteRenderer>().color = Color.white;
            Vector2 respawn = new Vector2(-6, 0);
            player.transform.position = respawn;
            Vector2 stay = new Vector2(0, 0);
            player.GetComponent<Rigidbody2D>().velocity = stay;
            player.GetComponent<Animation>().Play("Player_Respawning");
            StartCoroutine(PlayerRespawn());
        }
    }

    public void PlayerDeath()
    {
        Instantiate(explosion, player.transform.position, explosion.transform.rotation);
        player.GetComponent<Health>().health = 0;
        player.GetComponent<PlayerController>().HealthUpdate();
        player.GetComponent<SpriteRenderer>().enabled = false;
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(GameOver());
    }

    public IEnumerator BossDefeat()
    {
        Vector2 stop = new Vector2(0, 0);
        player.GetComponent<Rigidbody2D>().velocity = stop;
        Vector2 bossDown = new Vector2(0, -1.5f);
        boss.GetComponent<Rigidbody2D>().velocity = bossDown;
        //disable objects that hurt player
        GameObject[] shots = GameObject.FindGameObjectsWithTag("EnemyShot");
        for (int i = 0; i < shots.Length; i++)
        {
            shots[i].tag = "Untagged";
        }
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].tag = "Untagged";
        }
        GameObject[] bossParts = GameObject.FindGameObjectsWithTag("BossPart");
        for (int i = 0; i < bossParts.Length; i++)
        {
            bossParts[i].tag = "Untagged";
        }
        GameObject lava = GameObject.Find("Lava");
        lava.GetComponent<Collider2D>().enabled = false;
        player.GetComponent<PlayerController>().enabled = false;
        yield return new WaitForSeconds(1);
        exitActive = true;
        yield return new WaitForSeconds(8);
        Vector2 stop2 = new Vector2(0, 0);
        boss.GetComponent<Rigidbody2D>().velocity = stop2;
    }

    public IEnumerator BossExplode()
    {
        bossDead = true;
        explosionSpots = GameObject.FindGameObjectsWithTag("ExplosionSpot");
        do
        {
            Instantiate(explosion, explosionSpots[Random.Range(0, 8)].transform.position, explosion.transform.rotation);
            yield return new WaitForSeconds(0.5f);
        } while (bossDead);
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2);
        gameOverPanel.SetActive(true);
        yield return new WaitForSeconds(1);
        blockPanel.SetActive(false);
    }

    IEnumerator PlayerRespawn()
    {
        yield return new WaitForSeconds(2);
        player.GetComponent<PlayerController>().enabled = true;
        player.GetComponent<BoxCollider2D>().enabled = true;
    }

}
