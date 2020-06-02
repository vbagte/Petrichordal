using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class tacos
{
    public int dogs;
}


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
    public enum shottypes { none = 0, single = 1, dual = 2, triad = 3, spreader = 4, burst = 5,at_player=6 }
    public shottypes shottype;
    public enum Eflypattern { none=0,oneway=1,hover=2,stopngo=3,loop=4,squareexit=5 };
    public Eflypattern flypattern;
    private bool firing = true;
    // Start is called before the first frame update
    void Start()
    {   
        suminterval = fireinterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= 9 && transform.position.x >= -9 && transform.position.y >= -6.5 && transform.position.y <= 4.5)
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
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerShot")
        {
            Destroy(collision.gameObject);
            health -= 1;
            if (health < 1)
            {
                Instantiate(explosion,transform.position, explosion.transform.rotation);
                Destroy(gameObject);
            }
        }
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            collision.gameObject.GetComponent<Health>().health -= collisiondamage;
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
                projectileGO.GetComponent<Transform>().Translate(0, .1f, 0);
                break;
            case 2: //dual
                projectileGO = Instantiate(projectile, new Vector2(transform.position.x, transform.position.y), transform.rotation, transform.parent.transform);
                projectileGO.GetComponent<Transform>().Translate(-.3f, .1f, 0);
                projectileGO.GetComponent<EnemyBullet>().speed = projectilespeed;
                projectileGO = Instantiate(projectile, new Vector2(transform.position.x, transform.position.y), transform.rotation, transform.parent.transform);
                projectileGO.GetComponent<Transform>().Translate(.3f, .1f, 0);
                projectileGO.GetComponent<EnemyBullet>().speed = projectilespeed;
                break;
            case 3: //triad
                int angle3 = -45;
                for (int i = 0; i < 3; i++)
                {
                    projectileGO = Instantiate(projectile, new Vector2(transform.position.x, transform.position.y), transform.rotation, transform.parent.transform);
                    projectileGO.GetComponent<EnemyBullet>().speed = projectilespeed;
                    projectileGO.GetComponent<Transform>().Rotate(0, 0, transform.rotation.z + angle3);
                    angle3 += 45;
                }
                break;
            case 4: //spreader
                int angle = -50;
                for (int i = 0; i < 6; i++)
                {
                    projectileGO = Instantiate(projectile, new Vector2(transform.position.x, transform.position.y), transform.rotation, transform.parent.transform);
                    projectileGO.GetComponent<EnemyBullet>().speed = projectilespeed;
                    projectileGO.GetComponent<Transform>().Rotate(0, 0, transform.rotation.z + angle);
                    angle += 20;
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
            case 6:
                Transform target;
                if (GameObject.FindGameObjectWithTag("Player") != null)
                {
                      
                    target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
                    Vector3 targetDir = (transform.position - target.position) ;
                    float angle5 = 90+Mathf.Atan2(targetDir.y,targetDir.x) * 180 / Mathf.PI;
                    projectileGO = Instantiate(projectile, new Vector2(transform.position.x, transform.position.y), transform.rotation, transform.parent.transform);
                    projectileGO.GetComponent<EnemyBullet>().speed = projectilespeed;
                    projectileGO.GetComponent<Transform>().Rotate(0, 0,  angle5-transform.eulerAngles.z);
                }
                break;

        }

    }
}
