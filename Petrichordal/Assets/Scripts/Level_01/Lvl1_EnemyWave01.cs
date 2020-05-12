using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl1_EnemyWave01 : MonoBehaviour
{

    public int enemies;
    public int enemySpawnWait;
    public GameObject enemy01;

    private bool waveActive = true;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = new Vector3(10, 4.5f, 0);
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
        Instantiate(enemy01, startPosition, enemy01.GetComponent<Transform>().rotation);
    }

}
