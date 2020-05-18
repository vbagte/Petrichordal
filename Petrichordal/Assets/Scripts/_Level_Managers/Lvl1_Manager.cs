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
    public GameObject readyText;
    public GameObject goText;
    public GameObject[] backgrounds;
}

[System.Serializable]
public class Stalags
{
    public float stalagtiteYSpawn, stalagmiteYSpawn, stalagSpeed, stalagStart, stalagtiteDelay, stalagmiteDelay;
    public GameObject stalagtite, stalagmite;
    public bool stalagSpawn = false;
}

[System.Serializable]
public class Wave01
{
    public GameObject enemy;
}

public class Lvl1_Manager : MonoBehaviour
{

    public GameObject player;
    public GameObject enemy01;
    public int enemies;
    public int enemySpawnDelay;
    public float enemySpawnStartY;
    public float enemyFireDelay;
    public float fireRate;
    public PlayerStart playerStart;
    public Stalags stalag;
    public EnemyMovement eMovement;

    private Vector3 startPosition;
    private Vector3 stalagtiteSpawnStart;
    private Vector3 stalagmiteSpawnStart;

    private void Start()
    {
        player.GetComponent<PlayerController>().enabled = false;
        stalagtiteSpawnStart = new Vector3(10, stalag.stalagtiteYSpawn, 0);
        stalagmiteSpawnStart = new Vector3(10, stalag.stalagmiteYSpawn, 0);
        stalag.stalagtite.GetComponent<Mover>().speed = stalag.stalagSpeed;
        stalag.stalagmite.GetComponent<Mover>().speed = stalag.stalagSpeed;
        StartCoroutine(PlayerStart());
    }

    IEnumerator PlayerStart()
    {
        yield return new WaitForSeconds(playerStart.playerTakeOffDelay);
        playerStart.readyText.GetComponent<Animation>().Play();
        Vector2 movement = new Vector2(0, playerStart.takeOffSpeed);
        player.GetComponent<Rigidbody2D>().velocity = movement;
        yield return new WaitForSeconds(playerStart.playerEnable);
        playerStart.goText.GetComponent<Animation>().Play();
        Vector2 movement2 = new Vector2(0, 0);
        player.GetComponent<Rigidbody2D>().velocity = movement2;
        player.GetComponent<PlayerController>().enabled = true;
        for (int i = 0; i < playerStart.backgrounds.Length; i++)
        {
            playerStart.backgrounds[i].GetComponent<BGScrollerX>().enabled = true;
        }
        stalag.stalagSpawn = true;
        StartCoroutine(StalagSpawn());
        StartCoroutine(Wave01());
    }

    IEnumerator StalagSpawn()
    {
        yield return new WaitForSeconds(stalag.stalagStart);
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

    IEnumerator Wave01()
    {
        yield return new WaitForSeconds(enemySpawnDelay);
        enemySpawnStartY = 0;
        SpawnEnemy();
        yield return new WaitForSeconds(enemySpawnDelay);
        enemySpawnStartY = 2.5f;
        SpawnEnemy();
        yield return new WaitForSeconds(enemySpawnDelay);
        enemySpawnStartY = -2.5f;
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        startPosition = new Vector3(10, enemySpawnStartY, 0);
        enemy01.GetComponent<Enemy_Movement_01>().movement.xSpeed = eMovement.xSpeed;
        enemy01.GetComponent<Enemy_Movement_01>().movement.ySpeed = eMovement.ySpeed;
        enemy01.GetComponent<Enemy_Movement_01>().movement.yMax = eMovement.yMax;
        enemy01.GetComponent<Enemy_Movement_01>().movement.yMin = eMovement.yMin;
        Instantiate(enemy01, startPosition, enemy01.GetComponent<Transform>().rotation);
    }

}
