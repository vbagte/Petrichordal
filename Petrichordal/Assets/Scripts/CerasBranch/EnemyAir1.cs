using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAir1 : MonoBehaviour
{

    public int health = 1;
    public int collisiondamage = 50;
    private int counter2 = 0;
    public float speed;
    private float counter;
    private float elapsedseconds;
    public float fireinterval;
    float suminterval;
    public GameObject projectile;
    public GameObject explosion;
    public float projectilespeed;
    public enum shottypes { none = 0, single = 1, dual = 2, triad = 3, spreader = 4, burst = 5 }
    public shottypes shottype;
    public enum Eflypattern { none=0,oneway=1,hover=2,stopngo=3,loop=4,squareexit=5 };
    public Eflypattern flypattern;
    private bool firing = true;
    private bool inBoundary = false;
    private Health playerHealth;
    // Start is called before the first frame update
    void Start()
    {   
        suminterval = fireinterval;
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= 10.5 && transform.position.x >= -10.5 && transform.position.y >= -5 && transform.position.y <= 5)
        {
            counter++;
            elapsedseconds = counter / 60;
            if (elapsedseconds >= suminterval && firing ==true)
            {
                firelaser();
                suminterval += fireinterval;
            }
           movement();
            //  transform.localPosition += new Vector3((int)movedir * Time.deltaTime * speed / 10f, 0);
            inBoundary = true;
        }
        if (transform.position.x < -10.5) Destroy(gameObject);
    }
    public void movement() //    public enum Eflypattern { none=0,oneway=1,hover=2,stopngo=3,loop=4,squareexit=5 };
    {
        switch ((int)flypattern)
        {
            case 0://none
                break;
            case 1://straight line
                transform.Translate(0, -Time.deltaTime * speed / 10f,0);
             
                break;
            case 2://hover
                if (elapsedseconds<=1)
                {
                    transform.localPosition += new Vector3(Time.deltaTime * speed / 10f, 0);
                }
                break;
            case 3://stopngo
                if (elapsedseconds <= 1)
                {
                    transform.localPosition += new Vector3(Time.deltaTime * speed / 10f, 0);
                }
                else if (elapsedseconds <= 4.5) { }
                else
                {
                    transform.localPosition += new Vector3(Time.deltaTime * speed / 10f, 0);
                    firing = false;
                }

       
                break;
            case 4://loop
                if (elapsedseconds <= 1)
                {
                    transform.localPosition += new Vector3(Time.deltaTime * speed / 10f, 0);
                }
                else if (counter2 <= 120)
                {
                    counter2++;
                    transform.localPosition += new Vector3(0, Time.deltaTime * speed / 10f);
                }
                else if (counter2 >= 240)
                {
                    counter2 = 0;
                }
                else
                {
                    counter2++;
                    transform.localPosition += new Vector3(0, -Time.deltaTime * speed / 10f);
                }

                break;
            case 5://squareexist
                if (elapsedseconds <= 1)
                {
                    transform.localPosition += new Vector3(Time.deltaTime * speed / 10f, 0);
                }
                else if (elapsedseconds <= 2)
                {
                    transform.localPosition -= new Vector3(0, Time.deltaTime * speed / 10f);
                }
                else if (transform.position.x>=10)
                    {
                    Destroy(gameObject);
                }
                else
                {
                    transform.localPosition -= new Vector3(Time.deltaTime * speed / 10f, 0);
                }
                break;
        }

    
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (inBoundary)
        {
            if (other.gameObject.tag == "PlayerShot")
            {
                Destroy(other.gameObject);
                health -= 1;
                if (health < 1)
                {
                    Instantiate(explosion, transform.position, explosion.transform.rotation);
                    Destroy(gameObject);
                }
                this.GetComponent<Animation>().Play("Enemy_Hurt");
            }
            if (other.gameObject.tag == "TriShot")
            {
                health -= 5;
                if (health < 1)
                {
                    Instantiate(explosion, transform.position, explosion.transform.rotation);
                    Destroy(gameObject);
                }
                this.GetComponent<Animation>().Play("Enemy_Hurt");
            }
            if (other.gameObject.tag == "Player")
            {
                Destroy(gameObject);
                Instantiate(explosion, transform.position, explosion.transform.rotation);
                other.gameObject.GetComponent<Health>().health -= collisiondamage;
                if (playerHealth.health <= 0)
                {
                    LifeLost();
                }
                other.gameObject.GetComponent<Animation>().Play("Player_Hurt");
            }
        }
    }

    public void firelaser()
    {
        GameObject projectileGO;
        switch ((int)shottype)
        {
            //    public enum shottypes { none=0,single=1,dual=2,triad=3,spreader=4,burst=5,shootingstar=6,}
            case 0: //none
                break;
            case 1: //single
                projectileGO = Instantiate(projectile, new Vector2(transform.position.x, transform.position.y), transform.rotation, transform.parent.transform);
                projectileGO.GetComponent<EnemyBullet>().speed = projectilespeed;
                break;
            case 2: //dual
                projectileGO = Instantiate(projectile, new Vector2(transform.position.x + .3f, transform.position.y + .5f), transform.rotation, transform.parent.transform);
                projectileGO.GetComponent<EnemyBullet>().speed = projectilespeed;
                projectileGO = Instantiate(projectile, new Vector2(transform.position.x - .3f, transform.position.y + .5f), transform.rotation, transform.parent.transform);
                projectileGO.GetComponent<EnemyBullet>().speed = projectilespeed;
                break;
            case 3: //triad
                int angle3 = 135;
                for (int i = 0; i < 3; i++)
                {
                    GameObject temp = new GameObject();
                    temp.transform.Rotate(transform.rotation.x, transform.rotation.y, transform.rotation.z + angle3);
                    projectileGO = Instantiate(projectile, new Vector2(transform.position.x, transform.position.y), temp.transform.rotation, transform.parent.transform);
                    projectileGO.GetComponent<EnemyBullet>().speed = projectilespeed;
                    angle3 -= 45;
                    Destroy(temp);
                }
                break;
            case 4: //spreader
                int angle = 140;
                for (int i = 0; i < 6; i++)
                {
                    GameObject temp = new GameObject();
                    temp.transform.Rotate(transform.rotation.x, transform.rotation.y, transform.rotation.z + angle);
                    projectileGO = Instantiate(projectile, new Vector2(transform.position.x, transform.position.y ), temp.transform.rotation, transform.parent.transform);
                    projectileGO.GetComponent<EnemyBullet>().speed = projectilespeed;
                    angle -= 20;
                    Destroy(temp);
                }
                break;
            case 5: //burst        
                int angle2 = 0;
                for (int i = 0; i < 12; i++)
                {
                    GameObject a = new GameObject();
                    a.transform.Rotate(transform.rotation.x, transform.rotation.y, transform.rotation.z + angle2);
                    projectileGO = Instantiate(projectile, new Vector2(transform.position.x, transform.position.y), a.transform.rotation, transform.parent.transform);
                    projectileGO.GetComponent<EnemyBullet>().speed = projectilespeed;
                    angle2 += 30;
                    Destroy(a);

                }
                break;
        }

    }
    public void LifeLost()
    {
        GameObject.Find("GameController").GetComponent<GameController>().LifeLost();
    }
}
