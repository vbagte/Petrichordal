using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class LeftFire
{
    public GameObject shot, shotSpawn;
    public int shots;
    public float fireRate;
}

[System.Serializable]
public class RightFire
{
    public GameObject shot, shotSpawn;
    public int shots;
    public float fireRate;
}

[System.Serializable]
public class Signal
{
    public GameObject enemy01, enemy02;
    public float xMove, fireTime, fireRate;
}

[System.Serializable]
public class LavaBoss
{
    public float lavaGlowDelay;
    public GameObject lavaObject, fireball;
    public bool fireballActive;
}

public class WardenBoss : MonoBehaviour
{
    public LeftFire lf;
    public RightFire rf;
    public Signal s;
    public LavaBoss lb;
    public GameObject wardenPanel;
    public GameObject explosion;
    public float bossAttackDelay;
    public float attackStart;
    public float attackEnd;
    public float explodeDelay;

    private float maxHealth;
    private bool attackActive = false;
    private bool lavaActive = false;
    private bool deadActive = false;

    

    private void Start()
    {
        wardenPanel.SetActive(true);
        GetComponent<Animator>().SetBool("WalkEnd", true);
        maxHealth = GetComponent<Health>().health;
        s.enemy02.GetComponent<Enemy_Movement_02>().xMove = s.xMove;
        s.enemy02.GetComponent<Enemy_Movement_02>().fireTime = s.fireTime;
        s.enemy02.GetComponent<WeaponController>().fireRate = s.fireRate;
        BossHurt.bossActive = true;

        

    }

    private void Update()
    {
        if (attackActive == false)
        {
            int choice = Random.Range(1, 4);
            switch (choice)
            {
                case 1:
                    StartCoroutine(FireRight());
                    break;
                case 2:
                    StartCoroutine(FireLeft());
                    break;
                case 3:
                    StartCoroutine(Signal());
                    break;
            }
            attackActive = true;
        }
        if (GetComponent<Health>().health <= maxHealth / 2 && lavaActive == false)
        {
            lb.fireballActive = true;
            StartCoroutine(FireballSpawn());
            lb.lavaObject.GetComponent<Animation>().Play("Lava_Enter");
            lavaActive = true;
        }
        if (GetComponent<Health>().health <= 0 && deadActive == false)
        {
            Death();
            deadActive = true;
        }
    }

    IEnumerator FireRight()
    {
        yield return new WaitForSeconds(bossAttackDelay);
        GetComponent<Animator>().SetBool("FireRight", true);
        yield return new WaitForSeconds(attackStart);
        for (int i = 0; i < rf.shots; i++)
        {
            Instantiate(rf.shot, rf.shotSpawn.transform.position, rf.shotSpawn.transform.rotation);
            yield return new WaitForSeconds(rf.fireRate);
        }
        yield return new WaitForSeconds(attackEnd);
        GetComponent<Animator>().SetBool("FireRight", false);
        attackActive = false;
    }

    IEnumerator FireLeft()
    {
        yield return new WaitForSeconds(bossAttackDelay);
        GetComponent<Animator>().SetBool("FireLeft", true);
        yield return new WaitForSeconds(attackStart);
        for (int i = 0; i < lf.shots; i++)
        {
            Instantiate(lf.shot, lf.shotSpawn.transform.position, lf.shot.transform.rotation);
            yield return new WaitForSeconds(lf.fireRate);
        }
        yield return new WaitForSeconds(attackEnd);
        GetComponent<Animator>().SetBool("FireLeft", false);
        attackActive = false;
    }

    IEnumerator Signal()
    {
        yield return new WaitForSeconds(bossAttackDelay);
        GetComponent<Animator>().SetBool("Signal", true);
        yield return new WaitForSeconds(1.5f);
        int choice = Random.Range(1, 3);
        switch(choice)
        {
            case 1:
                Signal01();
                break;
            case 2:
                StartCoroutine(Signal02());
                break;
        }
        yield return new WaitForSeconds(1.9f);
        GetComponent<Animator>().SetBool("Signal", false);
        attackActive = false;
    }

    void Signal01()
    {
        Vector3 spawn = new Vector3(10, 2.5f, 0);
        Instantiate(s.enemy01, spawn, s.enemy01.transform.rotation);
        Vector3 spawn2 = new Vector3(10, 0, 0);
        Instantiate(s.enemy01, spawn2, s.enemy01.transform.rotation);
        Vector3 spawn3 = new Vector3(10, -2.5f, 0);
        Instantiate(s.enemy01, spawn3, s.enemy01.transform.rotation);
    }

    IEnumerator Signal02()
    {
        Vector3 spawn = new Vector3(10, 2.5f, 0);
        Instantiate(s.enemy02, spawn, s.enemy01.transform.rotation);
        yield return new WaitForSeconds(1);
        Vector3 spawn2 = new Vector3(10, 0, 0);
        Instantiate(s.enemy02, spawn2, s.enemy01.transform.rotation);
        yield return new WaitForSeconds(1);
        Vector3 spawn3 = new Vector3(10, -2.5f, 0);
        Instantiate(s.enemy02, spawn3, s.enemy01.transform.rotation);
    }

    IEnumerator FireballSpawn()
    {
        do
        {
            lb.lavaObject.GetComponent<Animation>().Play("Lava_Glow");
            yield return new WaitForSeconds(2);
            Vector2 fireballSpawn = new Vector2(Random.Range(-8, 3), -3.7f);
            Instantiate(lb.fireball, fireballSpawn, lb.fireball.transform.rotation);
            yield return new WaitForSeconds(lb.lavaGlowDelay);
        } while (lb.fireballActive);
    }

    void Death()
    {
        SoundManager.songInstance.setParameterByName("BossWin", 1); // ends boss music with outro stinger
        
        StopAllCoroutines();
        GameObject.Find("GameController").GetComponent<GameController>().StartCoroutine("BossDefeat");
        GameObject.Find("GameController").GetComponent<GameController>().StartCoroutine("BossExplode"); 
    }

}
