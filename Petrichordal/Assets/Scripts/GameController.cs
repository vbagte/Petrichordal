using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public static bool gameStart = false;

    public float timeDelay;
    public GameObject explosion;
    public GameObject[] bg;
    public GameObject[] stalags;
    public GameObject[] pipes;
    public GameObject levelManager;

    private GameObject player;
    private Health playerHealth;

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
    }

    private void Update()
    {
        //restart level
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Level_01");
            timeDelay = Time.time;
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
    }

    IEnumerator PlayerRespawn()
    {
        yield return new WaitForSeconds(2);
        player.GetComponent<PlayerController>().enabled = true;
        player.GetComponent<BoxCollider2D>().enabled = true;
    }

}
