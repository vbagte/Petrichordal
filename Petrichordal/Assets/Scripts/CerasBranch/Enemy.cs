using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int health = 1;
    public int collisiondamage = 50;
    public float speed;
    public enum Eflypattern { none = 0, oneway = 1, hover = 2, stopngo = 3, loop = 4, squareexit = 5, rotating = 6, faceplayer=7 };
    public Eflypattern flypattern;
    public GameObject projectile;
    public GameObject explosion;
    public float projectilespeed;
    public float fireinterval_sec;
    public float burstcooldown_sec;
    public int projectiles_per_burst;
    public enum shottypes { none = 0, single = 1, dual = 2, triad = 3, spreader = 4, burst = 5,lazar=6 }
    public shottypes shottype;
    public enum projectiledirections { forward = 1, toward_player = 2 }
    public projectiledirections projectiledirection;
    public GameObject lazarglow;

    private bool cooldown = false;
    private bool firing = true;
    private int counter_burst;
    private int counter2 = 0;
    private int counter_cooldown = 0;
    private float elapsedseconds;
    private int counter_projectile = 0;
    private float start = 0;
    private bool clone = true;
    private float frames_per_projectile;
    private bool inBoundary = false;
    private Health playerHealth;
    private bool firinmahlazar = false;
    private long lazarcounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //elapsedseconds = framecounter / 60;

        //Only update the object if it is within 1 tile of the camera field
        if (transform.position.x <= 10 && transform.position.x >= -10 && transform.position.y > -4.5 && transform.position.y <= 5)
        {
            if (start == 0) start = Time.time;
            elapsedseconds = Time.time - start;
            movement();
            inBoundary = true;
            if (firing == true) //if the gun is turned on (sometimes the movement patterns turn it off)
            {

                // if the weapon is cooling down,
                if (cooldown == true)
                {
              
                    counter_cooldown++; //counts frames we are in cooldown mode
                    //and the cooldown timer is up, flip the cooldown flag reset the counter
                    if ((float)counter_cooldown / 60.0 >= burstcooldown_sec)
                    {
                        cooldown = false;
                        counter_cooldown -= (int)(burstcooldown_sec * 60);
                    }

                }
                // if the weapon is in its fire phase
                else if (cooldown == false)
                {

                    counter_burst++; //counts frames we are in fire mode
                    //we have to divide the number of projectiles into the amount of time we are firing
                    frames_per_projectile = fireinterval_sec / projectiles_per_burst * 60f;
                    if (counter_burst >= (int)frames_per_projectile)
                    {
                        counter_burst -= (int)frames_per_projectile;
                        firelaser(); //fire
                        counter_projectile++; //we fired so add one to the counter
                        if (counter_projectile == projectiles_per_burst) //if this was the final in the burst
                        {
                            counter_projectile = 0;
                            cooldown = true;
                        }
   
                    }
                }
            }

        }
     //   if (transform.position.x < -10.5) Destroy(gameObject);
    }
    public void movement() //    public enum Eflypattern { none=0,oneway=1,hover=2,stopngo=3,loop=4,squareexit=5 };
    {
        switch ((int)flypattern)
        {
            case 0://none
                break;
            case 1://straight line
                transform.Translate(0, Time.deltaTime * speed / 10f, 0);
                break;
            case 2://hover
                if (elapsedseconds <= 2)
                {
                    firing = false;
                    transform.Translate(0, Time.deltaTime * speed / 10f, 0);
         
                }
                else {
                    firing = true;
                    hover();
                }
                break;
            case 3://stopngo
                if (elapsedseconds <= 2)
                {
                    transform.Translate(0, Time.deltaTime * speed / 10f, 0);
                    firing = false;
         
                }
                else if (elapsedseconds <= 6)
                {
                    firing = true;
                    hover();
                }
                else
                {
                    transform.Translate(0, Time.deltaTime * speed / 10f, 0);
             
                    firing = false;
                }
                break;
            case 4://loop

                if (elapsedseconds <= 1.5)
                {
                    firing = false;
                    transform.Translate(0, Time.deltaTime * speed / 10f, 0);
             
                }
                else if (counter2 <= 90)
                {
                    firing = true;
                    counter2++;
                    transform.Translate(Time.deltaTime * speed / 10f, 0, 0);
                    hover();
             
                }
                else if (counter2 >=180)
                {
                    counter2 = 0;
                }
                else
                {
                    firing = true;
                    counter2++;
                    transform.Translate(-Time.deltaTime * speed / 10f, 0, 0);
                    hover();
   
                }

                break;
            case 5://squareexit
                if (elapsedseconds <= 1.5)
                {
                    firing = false;
              
                    transform.Translate(0, Time.deltaTime * speed / 10f, 0);
                }
                else if (elapsedseconds <= 2.5)
                {
                    firing = true;

                    transform.Translate(Time.deltaTime * speed / 10f, 0, 0);
                    hover();
                }
                else
                {
                    firing = false;
                    Destroy(gameObject, 4);
                    transform.Translate(0, -Time.deltaTime * speed / 10f, 0);
                    hover();
  
                }
                break;
            case 6://rotating turret
                transform.Rotate(0, 0, speed * Time.deltaTime);
                break;
            case 7: //faceplayer

                if (firinmahlazar == false) //if we are firin mah lazar then stop updating facing rotation
                {
                    float angle = 0;

                    if (GameObject.FindGameObjectWithTag("Player") != null)
                    {
                        Transform target;
                        Vector3 lead;
                        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
                        if ((int)GetComponentInParent<Scroll>().direction == 2) //vertical
                        {
                            lead = new Vector3(0f, -GetComponentInParent<Scroll>().speed, 0f);
                        }
                        else lead = new Vector3(-GetComponentInParent<Scroll>().speed, 0f, 0f);
                        Vector3 targetDir = (transform.position - target.position + lead);
                        angle = 90 + Mathf.Atan2(targetDir.y, targetDir.x) * 180 / Mathf.PI - transform.eulerAngles.z;
                    }
                    transform.Rotate(0, 0, angle);
                }
                else
                {
                    lazarcounter++;
                    if (lazarcounter >= 60)
                    {
                        lazarcounter = 0;
                        firinmahlazar = false;
                    }
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
                if(gameObject.tag != "Boss"){
                Destroy(gameObject);
                Instantiate(explosion, transform.position, explosion.transform.rotation);
                }
                other.gameObject.GetComponent<Health>().health -= collisiondamage;
                if (playerHealth.health <= 0)
                {
                    LifeLost();
                }
                other.gameObject.GetComponent<Animation>().Play("Player_Hurt");
            }
        }
    }

    public void hover()
    {
        float temp = transform.eulerAngles.z;
        transform.Rotate(0, 0, 180 - transform.eulerAngles.z);
        if ((int)transform.parent.GetComponent<Scroll>().direction == 2)
        {
            transform.Translate(0,- transform.parent.GetComponent<Scroll>().speed * Time.deltaTime, 0);
        }
        else if ((int)transform.parent.GetComponent<Scroll>().direction == 1)
        {
            transform.Translate(-transform.parent.GetComponent<Scroll>().speed*Time.deltaTime, 0, 0);
        }

        transform.Rotate(0, 0, -(180 - temp));
    }
    public void firelaser()
    {
        GameObject projectileGO;
        float angle= 0;
        if (projectiledirection == projectiledirections.toward_player)
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                Transform target;
                Vector3 lead;
                target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
                if ((int)GetComponentInParent<Scroll>().direction==2) //vertical
                {
                    lead = new Vector3(0f, -GetComponentInParent<Scroll>().speed, 0f);
                } else lead = new Vector3( -GetComponentInParent<Scroll>().speed,0f, 0f);
                Vector3 targetDir = (transform.position - target.position+lead);
                angle = 90 + Mathf.Atan2(targetDir.y, targetDir.x) * 180 / Mathf.PI- transform.eulerAngles.z;
            }
        }

        switch ((int)shottype)
        {
    
            case 0: //none
                break;
            case 1: //single
                projectileGO = Instantiate(projectile, new Vector2(transform.position.x, transform.position.y), transform.rotation, GameObject.FindWithTag("Foreground").transform);
                projectileGO.GetComponent<EnemyBullet>().speed = projectilespeed;
                projectileGO.GetComponent<Transform>().Rotate(0, 0, angle);
                projectileGO.GetComponent<Transform>().Translate(0, .3f, 0);
                break;
            case 2: //dual
                projectileGO = Instantiate(projectile, new Vector2(transform.position.x, transform.position.y), transform.rotation, GameObject.FindWithTag("Foreground").transform);

                projectileGO.GetComponent<EnemyBullet>().speed = projectilespeed;
                projectileGO.GetComponent<Transform>().Rotate(0, 0, angle);
                projectileGO.GetComponent<Transform>().Translate(-.3f, .3f, 0);

                projectileGO = Instantiate(projectile, new Vector2(transform.position.x, transform.position.y), transform.rotation, GameObject.FindWithTag("Foreground").transform);
                projectileGO.GetComponent<EnemyBullet>().speed = projectilespeed;
                projectileGO.GetComponent<Transform>().Rotate(0, 0, angle);
                projectileGO.GetComponent<Transform>().Translate(.3f, .3f, 0);
                break;
            case 3: //triad
                angle -= 45;
                for (int i = 0; i < 3; i++)
                {
                    projectileGO = Instantiate(projectile, new Vector2(transform.position.x, transform.position.y), transform.rotation, GameObject.FindWithTag("Foreground").transform);
                    projectileGO.GetComponent<EnemyBullet>().speed = projectilespeed;
                    projectileGO.GetComponent<Transform>().Rotate(0, 0, angle);
                    angle += 45;
                }
                break;
            case 4: //spreader
                 angle-= 50;
                for (int i = 0; i < 6; i++)
                {
                    projectileGO = Instantiate(projectile, new Vector2(transform.position.x, transform.position.y), transform.rotation, GameObject.FindWithTag("Foreground").transform);
                    projectileGO.GetComponent<EnemyBullet>().speed = projectilespeed;
                    projectileGO.GetComponent<Transform>().Rotate(0, 0, angle);
                    angle += 20;
                }
                break;
            case 5: //burst        
                for (int i = 0; i < 12; i++)
                {
                     projectileGO = Instantiate(projectile, new Vector2(transform.position.x, transform.position.y), transform.rotation, GameObject.FindWithTag("Foreground").transform);
                    projectileGO.GetComponent<EnemyBullet>().speed = projectilespeed;
                    projectileGO.GetComponent<Transform>().Rotate(0, 0, angle);
                    angle += 30;
                }
                break;
            case 6: //lazar     
                    //if (firinmahlazar==false)
                    //{

                //GameObject glow = GameObject.Find("lazarglow");
                   Instantiate(lazarglow, new Vector2(transform.position.x, transform.position.y), transform.rotation, GameObject.FindWithTag("Foreground").transform);

                projectileGO = Instantiate(projectile, new Vector2(transform.position.x, transform.position.y), transform.rotation, GameObject.FindWithTag("Foreground").transform);
                    projectileGO.transform.localScale = new Vector3(0, 12, 1);
                projectileGO.transform.Translate(0, 6.2f,0);
                projectileGO.GetComponent<EnemyLazar>().maxXscale = 8;
                    firinmahlazar = true;
                //}
                //else lazarcounter++;
                //if (lazarcounter >= 5)
                //{
                //    lazarcounter = 0;
                //    firinmahlazar = false;
                //}


                break;
        }

    }
    public void LifeLost()
    {
        GameObject.Find("GameController").GetComponent<GameController>().LifeLost();
    }

}
