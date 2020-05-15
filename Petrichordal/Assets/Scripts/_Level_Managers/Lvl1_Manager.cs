using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyMovement
{
    public float xSpeed, ySpeed, yMax, yMin;
}

[System.Serializable]
public class PlayerStart
{
    public float playerTakeOffDelay, playerEnable, takeOffSpeed;
}

[System.Serializable]
public class Stalags
{
    public float stalagtiteSpawn, stalagmiteSpawn, stalagSpeed, stalagtiteDelay, stalagmiteDelay;
    public GameObject stalagtite, stalagmite;
    public bool stalagSpawn = false;
}

public class Lvl1_Manager : MonoBehaviour
{

    public int enemies;
    public int enemySpawnWait;
    public float delay;
    public float fireRate;
    public float waveSpawn;
    public GameObject player;
    public GameObject enemy01;
    public GameObject readyText;
    public GameObject goText;
    public GameObject[] backgrounds;
    public PlayerStart playerStart;
    public Stalags stalag;
    public EnemyMovement movement;

    private Vector3 startPosition;
    private Vector3 stalagtiteSpawnStart;
    private Vector3 stalagmiteSpawnStart;

    private void Start()
    {
        startPosition = new Vector3(10, waveSpawn, 0);
        stalagtiteSpawnStart = new Vector3(10, stalag.stalagtiteSpawn, 0);
        stalagmiteSpawnStart = new Vector3(10, stalag.stalagmiteSpawn, 0);
        stalag.stalagtite.GetComponent<Mover>().speed = stalag.stalagSpeed;
        stalag.stalagmite.GetComponent<Mover>().speed = stalag.stalagSpeed;
        StartCoroutine(PlayerStart());
    }

    IEnumerator PlayerStart()
    {
        yield return new WaitForSeconds(playerStart.playerTakeOffDelay);
        readyText.GetComponent<Animation>().Play();
        Vector2 movement = new Vector2(0, playerStart.takeOffSpeed);
        player.GetComponent<Rigidbody2D>().velocity = movement;
        yield return new WaitForSeconds(playerStart.playerEnable);
        goText.GetComponent<Animation>().Play();
        Vector2 movement2 = new Vector2(0, 0);
        player.GetComponent<Rigidbody2D>().velocity = movement2;
        player.GetComponent<PlayerController>().enabled = true;
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].GetComponent<BGScrollerX>().enabled = true;
        }
        stalag.stalagSpawn = true;
        StartCoroutine(StalagSpawn());
    }

    IEnumerator StalagSpawn()
    {
        while(stalag.stalagSpawn)
        {
            SpawnStalagtite();
            yield return new WaitForSeconds(stalag.stalagmiteDelay);
            SpawnStalagmite();
            yield return new WaitForSeconds(stalag.stalagtiteDelay);
        }
    }

    void SpawnStalagtite()
    {
        Instantiate(stalag.stalagtite, stalagtiteSpawnStart, stalag.stalagtite.GetComponent<Transform>().rotation);
    }

    void SpawnStalagmite()
    {
        Instantiate(stalag.stalagmite, stalagmiteSpawnStart, stalag.stalagmite.GetComponent<Transform>().rotation);
    }

    IEnumerator SpawnWave()
    {
        for (int i = 0; i < enemies; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(enemySpawnWait);
        }
    }

    void SpawnEnemy()
    {
        enemy01.GetComponent<WeaponController>().delay = delay;
        enemy01.GetComponent<WeaponController>().fireRate = fireRate;
        enemy01.GetComponent<Lvl1_Manager>().movement.xSpeed = movement.xSpeed;
        enemy01.GetComponent<Lvl1_Manager>().movement.ySpeed = movement.ySpeed;
        enemy01.GetComponent<Lvl1_Manager>().movement.yMax = movement.yMax;
        enemy01.GetComponent<Lvl1_Manager>().movement.yMin = movement.yMin;
        Instantiate(enemy01, startPosition, enemy01.GetComponent<Transform>().rotation);
    }

}
