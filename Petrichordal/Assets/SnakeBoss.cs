using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Cannons
{
    public GameObject shot, shotSpawn1, shotSpawn2, shotSpawn3, shotSpawn4, shotSpawn5, shotSpawn6;
    public int shots;
    public float fireRate;
}

[System.Serializable]
public class Reposition
{
    public GameObject SnakeBoss;
    public float yMove;
}

[System.Serializable]
public class HomingMissle
{
    public GameObject shot, shotSpawn;
    public int shots;
    public float fireRate;
}

[System.Serializable]
public class Laser
{
    public GameObject shot, shotSpawn;
    public int shots;
    public float fireRate;
}

[System.Serializable]
public class Barrage
{
    public GameObject shot, shotSpawn;
    public int shots;
    public float fireRate;
}
[System.Serializable]
public class moveToTop
{
    public GameObject SnakeBoss;

}



public class SnakeBoss : MonoBehaviour
{

    public Cannons ca;
    public Laser la;
    public Reposition rep;
    public HomingMissle hm;
    public Barrage bar;
    public moveToTop mtop;

    public float attackStart;
    public float attackEnd;
    public float explodeDelay;
    public float bossAttackDelay;
    public bool bossIsTopOfScreen = false;

    public GameObject snakePanel;
    private float maxHealth;
    private bool attackActive = false;
    private bool deadActive = false;
    public int transitonCount = 0;
    public int bossAttackCount = 0;


    // Start is called before the first frame update
    void Start()
    {
        snakePanel.SetActive(true);
        GetComponent<Animator>().SetBool("entryIsDone", true);
        maxHealth = GetComponent<Health>().health;
        BossHurt.bossActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        // if (bossIsTopOfScreen == false && GetComponent<Health>().health >= ((maxHealth * 3) / 4))
        //beginning moves force to phase 2 if attack count > 100?
        //turn to switch
        if (bossAttackCount > 100)
        {
            if (bossIsTopOfScreen)
            {
                transitonCount++;
                bossAttackCount = 0;
                StartCoroutine(Reposition());
            }
            else
            {
                transitonCount++;
                bossAttackCount = 0;
                StartCoroutine(moveToTop());
            }


        }
        switch (transitonCount)
        {
            //case 1
            case 0:
                if (bossIsTopOfScreen == false)
                {
                    if (attackActive == false)
                    {
                        int choice = Random.Range(1, 3);
                        switch (choice)
                        {
                            case 1:
                 
                                    StartCoroutine(Cannons());
                                    break;
                                
                            case 2:
                                StartCoroutine(HomingMissle());
                                break;
                        }
                        attackActive = true;
                    }
                    break;
                }
                else
                {
                    if (attackActive == false)
                    {
                        int choice = Random.Range(1, 3);
                        switch (choice)
                        {
                           
                            case 1:

                                StartCoroutine(Cannons());
                                break;

                            case 2:
                                StartCoroutine(HomingMissle());
                                break;
                        }
                        attackActive = true;
                    }
                    break;
                }


            //case 2

            case 1:
                {
                    if (bossIsTopOfScreen == false)
                    {
                        if (attackActive == false)
                        {
                            int choice = Random.Range(1, 3);
                            switch (choice)
                            {
                                case 1:

                                    StartCoroutine(Cannons());
                                    break;

                                case 2:
                                    StartCoroutine(HomingMissle());
                                    break;
                                case 3:
                                    StartCoroutine(Reposition());
                                    break;
                            }
                            attackActive = true;

                        }
                        break;
                    }
                    else
                    {
                        if (attackActive == false)
                        {
                            int choice = Random.Range(1, 3);
                            switch (choice)
                            {
                                case 1:

                                    StartCoroutine(Cannons());
                                    break;

                                case 2:
                                    StartCoroutine(HomingMissle());
                                    break;
                            }
                            attackActive = true;
                        }
                        break;
                    }
                }

            // case 3

            case 2:
                {
                    if (bossIsTopOfScreen == false)
                    {
                        if (attackActive == false)
                        {
                            int choice = Random.Range(1, 4);
                            switch (choice)
                            {
                                case 1:

                                    StartCoroutine(Cannons());
                                    break;

                                case 2:
                                    StartCoroutine(HomingMissle());
                                    break;
                                case 3:
                                    StartCoroutine(Reposition());
                                    break;
                            }
                            attackActive = true;
                        }
                        break;
                    }
                    else
                    {
                        if (attackActive == false)
                        {
                            int choice = Random.Range(1, 3);
                            switch (choice)
                            {
                                case 1:
                                   
                                        StartCoroutine(Laser());
                                        break;
                                    
                                case 2:
                                    StartCoroutine(HomingMissle());
                                    break;
                            }
                            attackActive = true;
                        }
                        break;
                    }

                }

            //case 4

            case 3:
                {
                    if (attackActive == false)
                    {
                        int choice = Random.Range(1, 4);
                        switch (choice)
                        {
                           
                            case 1:

                                StartCoroutine(Laser());
                                break;

                            case 2:
                                StartCoroutine(HomingMissle());
                                break;
                            case 3:
                                StartCoroutine(Barrage());
                                break;
                        }
                        attackActive = true;
                    }
                    break;
                }
       
         }
            // boss is on top of the screen
          
            // 3/4 transiton to top screen
            if (GetComponent<Health>().health <= ((maxHealth * 3) / 4) && transitonCount == 0)
            {

                transitonCount++;
                StartCoroutine(moveToTop());

            }
            // 1/2 transiton to bottom screen
            if (GetComponent<Health>().health <= maxHealth / 2 && transitonCount == 1)
            {

                transitonCount++;
                StartCoroutine(Reposition());

            }
            if (GetComponent<Health>().health <= maxHealth / 4 && transitonCount == 2)
            {

                transitonCount++;
                StartCoroutine(moveToTop());

            }
            //death of boss
            if (GetComponent<Health>().health <= 0 && deadActive == false)
            {
                Death();
                deadActive = true;
            }
        }

        IEnumerator Cannons()
        {
            yield return new WaitForSeconds(bossAttackDelay);
            yield return new WaitForSeconds(attackStart);
        for (int i = 0; i < ca.shots; i++)
        {
            Instantiate(ca.shot, ca.shotSpawn1.transform.position, ca.shotSpawn1.transform.rotation);
            Instantiate(ca.shot, ca.shotSpawn2.transform.position, ca.shotSpawn2.transform.rotation);
            Instantiate(ca.shot, ca.shotSpawn3.transform.position, ca.shotSpawn3.transform.rotation);
            Instantiate(ca.shot, ca.shotSpawn4.transform.position, ca.shotSpawn4.transform.rotation);
            Instantiate(ca.shot, ca.shotSpawn5.transform.position, ca.shotSpawn5.transform.rotation);
            Instantiate(ca.shot, ca.shotSpawn6.transform.position, ca.shotSpawn6.transform.rotation);
            yield return new WaitForSeconds(ca.fireRate);
        }
        yield return new WaitForSeconds(attackEnd);
            bossAttackCount++;
            attackActive = false;
        }
        IEnumerator HomingMissle()
        {
            yield return new WaitForSeconds(bossAttackDelay);
            yield return new WaitForSeconds(attackStart);

            // This is spawinging mulitple bullets from the head that spread out then track player

            yield return new WaitForSeconds(attackEnd);
            bossAttackCount++;
            attackActive = false;
        }

        IEnumerator Reposition()
        {
            yield return new WaitForSeconds(bossAttackDelay);
            GetComponent<Animator>().SetTrigger("Reposition");
            yield return new WaitForSeconds(attackStart);

            // Leave the screen set an indicator at player location and re-enter the 
            //level trying collide with the player

            yield return new WaitForSeconds(attackEnd);
            bossAttackCount++;
            attackActive = false;
        }

        IEnumerator Laser()
        {
            yield return new WaitForSeconds(bossAttackDelay);
            GetComponent<Animator>().SetBool("FireLaser", true);
            yield return new WaitForSeconds(attackStart);
            for (int i = 0; i < la.shots; i++)
            {
                Instantiate(la.shot, la.shotSpawn.transform.position, la.shotSpawn.transform.rotation);
                yield return new WaitForSeconds(la.fireRate);
            }
        yield return new WaitForSeconds(attackEnd);
            GetComponent<Animator>().SetBool("FireLaser", false);
            bossAttackCount++;
            attackActive = false;
        }
        IEnumerator Barrage()
        {
            yield return new WaitForSeconds(bossAttackDelay);
            GetComponent<Animator>().SetBool("FireBarrage", true);
            yield return new WaitForSeconds(attackStart);
            for (int i = 0; i < bar.shots; i++)
            {
                Instantiate(bar.shot, bar.shotSpawn.transform.position, bar.shotSpawn.transform.rotation);
                yield return new WaitForSeconds(bar.fireRate);
            }
        yield return new WaitForSeconds(attackEnd);
            GetComponent<Animator>().SetBool("FireBarrage", false);
            bossAttackCount++;
            attackActive = false;
        }
        IEnumerator moveToTop()
        {
            yield return new WaitForSeconds(bossAttackDelay);
            GetComponent<Animator>().SetTrigger("MoveToTop");
            yield return new WaitForSeconds(attackStart);
            
            // Move the monster to the top of the screen and stay stationary

            yield return new WaitForSeconds(attackEnd);
            GetComponent<Animator>().SetBool("MoveToTop", false);
            bossAttackCount++;
            attackActive = false;
        }
        void Death()
        {
            StopAllCoroutines();
            GameObject.Find("GameController").GetComponent<GameController>().StartCoroutine("BossDefeat");
            GameObject.Find("GameController").GetComponent<GameController>().StartCoroutine("BossExplode");
        }
    }

