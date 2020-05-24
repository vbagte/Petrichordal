using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStart
{
    public float playerTakeOffDelay = 2, playerEnable = 2, takeOffSpeed;
    public GameObject readyText;
    public GameObject goText;
    public GameObject[] backgrounds;
}

[System.Serializable]
public class Enemy01
{
    public float xSpeed, ySpeed, yMax, yMin;
}

[System.Serializable]
public class Enemy02
{
    public float xSpeed, moveInTime, fireTime, fireRate, moveOutTime;
}

[System.Serializable]
public class Enemy03
{
    public float xSpeed, fireDelay, fireRate;
}

[System.Serializable]
public class Stalags
{
    public float stalagtiteYSpawn, stalagmiteYSpawn, stalagSpeed, stalagStart, stalagtiteDelay, stalagmiteDelay;
    public GameObject stalagtite, stalagmite;
    public bool stalagSpawn = false;
}

[System.Serializable]
public class Lava
{
    public float lavaGlowDelay;
    public GameObject lavaObject, fireball;
    public bool fireballActive;
}

[System.Serializable]
public class Smoke
{
    public float smokeSpawnDelay;
    public GameObject pipe;
    public bool pipeActive;
}

public class Lvl1_Manager : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy01;
    public GameObject enemy02;
    public GameObject enemy03;
    public GameObject stoneFloor;
    public GameObject boss;
    public ParticleSystem ps;
    public int enemySpawnDelay = 3;
    public float enemySpawnStartY;
    public float enemyFireDelay;
    public float fireRate;
    public PlayerStart playerStart;
    public Stalags stalag;
    public Lava lava;
    public Smoke smoke;
    public Enemy01 e01;
    public Enemy02 e02;
    public Enemy03 e03;

    private bool enemy03Active;
    public bool bossActive = false;
    public bool bgStop = false;
    private Vector3 startPosition;
    private Vector3 stalagtiteSpawnStart;
    private Vector3 stalagmiteSpawnStart;

    private void Start()
    {
        stalagtiteSpawnStart = new Vector3(10, stalag.stalagtiteYSpawn, 0);
        stalagmiteSpawnStart = new Vector3(10, stalag.stalagmiteYSpawn, 0);
        stalag.stalagtite.GetComponent<Mover>().speed = stalag.stalagSpeed;
        stalag.stalagmite.GetComponent<Mover>().speed = stalag.stalagSpeed;
        StartCoroutine(PlayerStart());
    }

    private void Update()
    {
        if (bossActive)
        {
            if (playerStart.backgrounds[0].transform.position.x < -17.95f || playerStart.backgrounds[0].transform.position.x > 0.05f)
            {
                bgStop = true;
            }
            if (bgStop)
            {
                for (int i = 0; i < playerStart.backgrounds.Length; i++)
                {
                    playerStart.backgrounds[i].GetComponent<BGScrollerX>().enabled = false;
                }
            }
        }
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
        GameController.playerEnable = true;
    }

    IEnumerator Wave01()
    {
        //part 1
        yield return new WaitForSeconds(enemySpawnDelay);
        enemySpawnStartY = 0;
        SpawnEnemy01();
        yield return new WaitForSeconds(enemySpawnDelay);
        enemySpawnStartY = 2.5f;
        SpawnEnemy01();
        yield return new WaitForSeconds(enemySpawnDelay);
        enemySpawnStartY = -2.5f;
        SpawnEnemy01();
        //part 2
        yield return new WaitForSeconds(enemySpawnDelay + 3);
        enemySpawnStartY = 3.5f;
        e01.ySpeed = 3;
        e01.yMax = 3.5f;
        e01.yMin = -3.5f;
        SpawnEnemy01();
        yield return new WaitForSeconds(enemySpawnDelay);
        enemySpawnStartY = -3.5f;
        SpawnEnemy01();
        yield return new WaitForSeconds(enemySpawnDelay);
        enemySpawnStartY = -3.5f;
        SpawnEnemy01();
        enemySpawnStartY = 3.5f;
        SpawnEnemy01();
        StartCoroutine(Wave02());
    }

    IEnumerator Wave02()
    {
        stalag.stalagmiteDelay += 3;
        stalag.stalagtiteDelay += 3;
        yield return new WaitForSeconds(enemySpawnDelay + 6);
        lava.lavaObject.GetComponent<Animation>().Play("Lava_Enter");
        yield return new WaitForSeconds(2);
        lava.fireballActive = true;
        StartCoroutine("FireballSpawn");
        yield return new WaitForSeconds(2);
        enemySpawnStartY = 0;
        SpawnEnemy02();
        yield return new WaitForSeconds(enemySpawnDelay);
        enemySpawnStartY = 0;
        SpawnEnemy01();
        enemySpawnStartY = -2.3f;
        SpawnEnemy02();
        enemySpawnStartY = 2.3f;
        SpawnEnemy02();
        yield return new WaitForSeconds(enemySpawnDelay);
        enemySpawnStartY = -2.3f;
        SpawnEnemy02();
        yield return new WaitForSeconds(1);
        enemySpawnStartY = 0;
        SpawnEnemy02();
        yield return new WaitForSeconds(1);
        enemySpawnStartY = 2.3f;
        SpawnEnemy02();
        yield return new WaitForSeconds(enemySpawnDelay);
        enemySpawnStartY = -2.5f;
        e01.ySpeed = 3;
        e01.yMax = 2.5f;
        e01.yMin = -2.5f;
        SpawnEnemy01();
        enemySpawnStartY = 2.5f;
        e01.ySpeed = 3;
        SpawnEnemy01();
        enemySpawnStartY = 2.3f;
        SpawnEnemy02();
        e02.fireTime = 2;
        enemySpawnStartY = 0;
        SpawnEnemy02();
        e02.fireTime = 3;
        enemySpawnStartY = -2.3f;
        SpawnEnemy02();
        yield return new WaitForSeconds(enemySpawnDelay + 5);
        StartCoroutine(Wave03());
    }

    IEnumerator Wave03()
    {
        lava.fireballActive = false;
        stalag.stalagSpawn = false;
        smoke.pipeActive = true;
        StopCoroutine("FireballSpawn");
        StopCoroutine("StalagSpawn");
        StartCoroutine("PipeSpawn");
        lava.lavaObject.GetComponent<Animation>().Play("Lava_Exit");
        yield return new WaitForSeconds(enemySpawnDelay);
        stoneFloor.GetComponent<Animation>().Play("Stone_Floor_Enter");
        yield return new WaitForSeconds(enemySpawnDelay);
        enemy03Active = true;
        StartCoroutine("Enemy03Spawn");
        yield return new WaitForSeconds(2);
        e01.ySpeed = 0;
        enemySpawnStartY = 1.5f;
        SpawnEnemy01();
        enemySpawnStartY = -1.5f;
        SpawnEnemy01();
        yield return new WaitForSeconds(1);
        enemySpawnStartY = 0;
        e02.fireTime = 1;
        SpawnEnemy02();
        yield return new WaitForSeconds(3);
        enemySpawnStartY = -3.5f;
        e01.ySpeed = 3;
        SpawnEnemy01();
        yield return new WaitForSeconds(3);
        enemySpawnStartY = 0;
        e02.fireRate = 2;
        e02.moveOutTime = 6;
        SpawnEnemy02();
        yield return new WaitForSeconds(1);
        enemySpawnStartY = 3.5f;
        e01.ySpeed = 3;
        SpawnEnemy01();
        yield return new WaitForSeconds(5);
        enemySpawnStartY = 0;
        e01.ySpeed = 0;
        SpawnEnemy01();
        yield return new WaitForSeconds(1);
        e01.ySpeed = 0;
        enemySpawnStartY = -2.5f;
        e01.ySpeed = 3;
        SpawnEnemy01();
        enemySpawnStartY = 2.5f;
        e01.ySpeed = 3;
        SpawnEnemy01();
        yield return new WaitForSeconds(2);
        enemySpawnStartY = 0;
        SpawnEnemy02();
        enemySpawnStartY = 2.5f;
        SpawnEnemy02();
        enemySpawnStartY = -2.5f;
        SpawnEnemy02();
        yield return new WaitForSeconds(1);
        smoke.pipeActive = false;
        enemy03Active = false;
        StopCoroutine("PipeSpawn");
        StopCoroutine("Enemy03Spawn");
        yield return new WaitForSeconds(10);
        bossActive = true;
        stoneFloor.GetComponent<Animation>().Play("Stone_Floor_Exit");
        StartCoroutine("Boss");
    }

    IEnumerator Boss()
    {
        yield return new WaitForSeconds(3);
        boss.SetActive(true);
        Vector2 move = new Vector2(-1.5f, 0);
        boss.GetComponent<Rigidbody2D>().velocity = move;
        yield return new WaitForSeconds(3);
        Vector2 stop = new Vector2(0, 0);
        boss.GetComponent<Rigidbody2D>().velocity = stop;
        boss.GetComponent<WardenBoss>().enabled = true;
    }

    IEnumerator StalagSpawn()
    {
        yield return new WaitForSeconds(stalag.stalagStart);
        while (stalag.stalagSpawn)
        {
            SpawnStalagtite();
            yield return new WaitForSeconds(stalag.stalagmiteDelay);
            SpawnStalagmite();
            yield return new WaitForSeconds(stalag.stalagtiteDelay);
        }
    }

    IEnumerator FireballSpawn()
    {
        do
        {
            lava.lavaObject.GetComponent<Animation>().Play("Lava_Glow");
            yield return new WaitForSeconds(2);
            Vector2 fireballSpawn = new Vector2(Random.Range(-8, 8), -3.7f);
            Instantiate(lava.fireball, fireballSpawn, lava.fireball.transform.rotation);
            yield return new WaitForSeconds(lava.lavaGlowDelay);
        } while (lava.fireballActive);
    }

    IEnumerator PipeSpawn()
    {
        while (smoke.pipeActive == true)
        {
            yield return new WaitForSeconds(smoke.smokeSpawnDelay);
            Vector2 spawn = new Vector2(9.3f, 3.3f);
            GameObject pipe = Instantiate(smoke.pipe, spawn, smoke.pipe.transform.rotation);
            ps = pipe.GetComponentInChildren<ParticleSystem>();
            GameObject smokeTrigger = GameObject.Find("Smoke_Trigger");
            yield return new WaitForSeconds(Random.Range(2,5));
            ps.Play();
            yield return new WaitForSeconds(1);
            smokeTrigger.GetComponent<BoxCollider2D>().enabled = true;
            yield return new WaitForSeconds(5);
            smokeTrigger.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    IEnumerator Enemy03Spawn()
    {
        while (enemy03Active)
        {
            enemySpawnStartY = -3;
            SpawnEnemy03();
            yield return new WaitForSeconds(5);
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

    void SpawnEnemy01()
    {
        startPosition = new Vector3(10, enemySpawnStartY, 0);
        enemy01.GetComponent<Enemy_Movement_01>().movement.xSpeed = e01.xSpeed;
        enemy01.GetComponent<Enemy_Movement_01>().movement.ySpeed = e01.ySpeed;
        enemy01.GetComponent<Enemy_Movement_01>().movement.yMax = e01.yMax;
        enemy01.GetComponent<Enemy_Movement_01>().movement.yMin = e01.yMin;
        Instantiate(enemy01, startPosition, enemy01.GetComponent<Transform>().rotation);
    }

    void SpawnEnemy02()
    {
        startPosition = new Vector3(10, enemySpawnStartY, 0);
        enemy02.GetComponent<Enemy_Movement_02>().xMove = e02.xSpeed;
        enemy02.GetComponent<Enemy_Movement_02>().moveInTime = e02.moveInTime;
        enemy02.GetComponent<Enemy_Movement_02>().fireTime = e02.fireTime;
        enemy02.GetComponent<Enemy_Movement_02>().moveOutTime = e02.moveOutTime;
        enemy02.GetComponent<WeaponController>().fireRate = e02.fireRate;
        Instantiate(enemy02, startPosition, enemy02.GetComponent<Transform>().rotation);
    }

    void SpawnEnemy03()
    {
        startPosition = new Vector3(10, enemySpawnStartY, 0);
        enemy03.GetComponent<Mover>().speed = e03.xSpeed;
        enemy03.GetComponent<WeaponController>().fireDelay = e03.fireDelay;
        enemy03.GetComponent<WeaponController>().fireRate = e03.fireRate;
        Instantiate(enemy03, startPosition, enemy03.GetComponent<Transform>().rotation);
    }

}
