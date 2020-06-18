using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMOD.Studio;
using UnityEngine.Tilemaps;

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
    public GameObject nextLevelPanel;
    public GameObject fadePanel;
    public GameObject foreground;
    public GameObject midground;
    public GameObject background;
    public GameObject lvl2Boss;
    public bool paused = false;

    public GameObject player;
    public GameObject playerExit;
    private GameObject[] explosionSpots;
    public Health playerHealth;
    private bool bossDead;
    public bool exitActive = false;
    private bool continueEnable = false;
    private Vector3 exitSpot;

    //private string currentScene;
    //public static string currentSongName;
    //private BeatSystem bS;
    //public static FMOD.Studio.EventInstance songInstance;
    public static FMOD.Studio.Bus masterBus;
    public static FMOD.Studio.Bus musicBus;
    public static FMOD.Studio.Bus sfxBus;
    public static FMOD.Studio.Bus uiBus;

    public SoundManager soundManager;

    private void Start()
    {
        masterBus = FMODUnity.RuntimeManager.GetBus("bus:/Master");
        musicBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
        sfxBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
        uiBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/UI");
        soundManager = GameObject.Find("Main Camera").GetComponent<SoundManager>();
        
        if (gameStart == false)
        {
            gameStart = true;
        }

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        }
        playerExit = GameObject.Find("PlayerExitSpot");
        exitSpot = (playerExit.transform.position - player.transform.position);
        playerEnable = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        //keep track of playtime
        if (player != null && player.GetComponent<PlayerController>().lives > 0 && paused == false && player.GetComponent<PlayerController>().enabled == true)
        {
            playerstats.playtime += Time.deltaTime;
        }
        //pause menu toggle
        if (Input.GetKeyDown(KeyCode.Escape) && paused == false)
        {
            pauseMenu.SetActive(true);
            settingsMenu.SetActive(false);
            pausePanel.SetActive(true);
            paused = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            
            //pause music
            musicBus.setPaused(true);
            //sfxBus.setPaused(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && paused == true)
        {
            pausePanel.SetActive(false);
            paused = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;

            //unpause music
            musicBus.setPaused(false);
            //sfxBus.setPaused(false);
        }
        if (Input.GetKeyDown(KeyCode.E) && continueEnable == true)
        {
            StartCoroutine(LevelChange(0));
        }
        //stop objects after player dies
        if (player == null || player.GetComponent<PlayerController>().lives <= 0)
        {
            for (int i = 0; i < bg.Length; i++)
            {
                bg[i].GetComponent<BGScrollerX>().enabled = false;
            }
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
            if (SceneManager.GetActiveScene().name == "Level_01")
            {
                for (int i = 0; i < GameObject.FindGameObjectsWithTag("Enemy").Length; i++)
                {
                    GameObject.FindGameObjectsWithTag("Enemy")[i].GetComponent<WeaponController>().enabled = false;
                }
                levelManager.GetComponent<Lvl1_Manager>().stalag.stalagSpeed = 0;
                levelManager.GetComponent<Lvl1_Manager>().lava.fireballActive = false;
                boss.GetComponent<WardenBoss>().enabled = false;
            }
        }
        if (exitActive)
        {
            player.transform.position += exitSpot * 0.25f * Time.deltaTime;
        }
        //if player leaves screen after defeating boss
        if (DestroyByBoundary.playerLeft == true)
        {
            bossDead = true;
            //exitActive = true;            
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
            player.GetComponent<Collider2D>().enabled = false;
            player.GetComponent<Animation>().Stop();
            player.GetComponent<SpriteRenderer>().color = Color.white;
            if (SceneManager.GetActiveScene().name == "Level_03")
            {
                Vector2 respawn = new Vector2(0, -3);
                player.transform.position = respawn;
         
            }
            else if (SceneManager.GetActiveScene().name == "Level_02")
            {
                foreground.GetComponent<Scroll>().enabled = false;
                midground.GetComponent<Scroll>().enabled = false;
                background.GetComponent<Scroll>().enabled = false;
                bool badspot = true;
                Vector2 respawn = new Vector2(0, 3);
                while (badspot == true)
                {
                    badspot = false;
                    foreach (GameObject land in GameObject.FindGameObjectsWithTag("Land"))
                    {
                        if (land.GetComponent<TilemapCollider2D>().OverlapPoint(respawn))
                        {
                            Debug.Log(land.name);
                            badspot = true;
                        }
                    }
                    if (badspot == true) respawn += new Vector2(0, -.5f);
                }
                player.transform.position = respawn;
            }
            else
            {
                Vector2 respawn = new Vector2(-6, 0);
                player.transform.position = respawn;
            }
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
        player.GetComponent<Collider2D>().enabled = false;
        if (SceneManager.GetActiveScene().name == "Level_02")
        {
            foreground.GetComponent<Scroll>().speed = 0;
            foreach (Enemy script in foreground.GetComponentsInChildren<Enemy>())
            {
                script.enabled = false;
            }
            foreach (Enemy script in lvl2Boss.GetComponentsInChildren<Enemy>())
            {
                script.enabled = false;
            }
            midground.GetComponent<Scroll>().speed = 0;
            background.GetComponent<Scroll>().speed = 0;
        }
        if (SceneManager.GetActiveScene().name == "Level_03")
        {
            foreground.GetComponent<Scroll>().speed = 0;
            foreach (Enemy script in foreground.GetComponentsInChildren<Enemy>())
            {
                script.enabled = false;
            }
            background.GetComponent<Scroll>().speed = 0;
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        StartCoroutine(GameOver());
    }

    public void NextLevelPanel()
    {
        continueEnable = true;
        nextLevelPanel.GetComponent<Animation>().Play();
    }

    public void NextLevelButton()
    {
        StartCoroutine(LevelChange(0));
    }

    public IEnumerator LevelChange(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
        if (SceneManager.GetActiveScene().name == "Level_01")
        {
            GameObject lava = GameObject.Find("Lava");
            lava.GetComponent<Collider2D>().enabled = false;
        }
        player.GetComponent<PlayerController>().enabled = false;
        yield return new WaitForSeconds(1);
        exitActive = true;
        print(exitSpot);
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
        musicBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
        sfxBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/gameover"); // play sound
        yield return new WaitForSeconds(2);
        gameOverPanel.SetActive(true);
        yield return new WaitForSeconds(1);
        blockPanel.SetActive(false);
    }

    IEnumerator PlayerRespawn()
    {
        yield return new WaitForSeconds(2);
        if (SceneManager.GetActiveScene().name == "Level_02")
        {
            foreground.GetComponent<Scroll>().enabled = true;
            midground.GetComponent<Scroll>().enabled = true;
            background.GetComponent<Scroll>().enabled = true;
        }
        player.GetComponent<PlayerController>().enabled = true;
        player.GetComponent<Collider2D>().enabled = true;
    }

}
