using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl1_EnemyWave01 : MonoBehaviour
{

    public int enemies;
    public int enemySpawnWait;
    public float delay;
    public float fireRate;
    public float waveSpawn;
    public GameObject enemy01;
    public Movement movement;

    private Vector3 startPosition;
    
    [System.Serializable]
    public class Movement
    {
        public float xSpeed, ySpeed, yMax, yMin;
    }

    private void Start()
    {
        startPosition = new Vector3(10, waveSpawn, 0);
        StartCoroutine(SpawnWave());
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
        enemy01.GetComponent<Lvl1_Wave01Mover>().movement.xSpeed = movement.xSpeed;
        enemy01.GetComponent<Lvl1_Wave01Mover>().movement.ySpeed = movement.ySpeed;
        enemy01.GetComponent<Lvl1_Wave01Mover>().movement.yMax = movement.yMax;
        enemy01.GetComponent<Lvl1_Wave01Mover>().movement.yMin = movement.yMin;
        Instantiate(enemy01, startPosition, enemy01.GetComponent<Transform>().rotation);
    }

}
