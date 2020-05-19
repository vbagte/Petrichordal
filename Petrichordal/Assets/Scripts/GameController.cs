using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public float timeDelay;
    public GameObject player;
    public GameObject[] bg;
    public GameObject[] stalags;
    public GameObject levelManager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Level_01");
            timeDelay = Time.time;
        }
        if (player.GetComponent<Health>().health <= 0 || player == null)
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
            levelManager.GetComponent<Lvl1_Manager>().lava.fireballActive = false;
        }
    }

    public void PlayerDeath()
    {
        player.SetActive(false);
    }

}
