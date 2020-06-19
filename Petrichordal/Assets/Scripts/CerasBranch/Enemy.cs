//==============================================================================
//Project:       Petrichordal
//File Name:     Enemy.cs
//Author:        Cera Baltzley, w/edits by Reed,Bryan
//Class:         CS 185 2020 Spring Quarter
//Description:   This script defines and manages enemy behaviour, including
//               movement patterns, firing patterns, and collision behaviours.
//               Also provides custom unity editor, and customizable options 
//               allowing for creation of wide variety of custom enemy types
//
//==============================================================================
//Known Bugs: 
//==============================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//----------------------------------------------------------------------
//Type:        Class
//Description: Custom Unity Editor providing user friendly descriptions and
//             layout for managing enemy options.
//
//Written by:  Cera Baltzley
//----------------------------------------------------------------------
#if UNITY_EDITOR
using UnityEditor;
[CanEditMultipleObjects]
[CustomEditor(typeof(Enemy))]
public class EnemyEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Enemy enemy = (Enemy)target;
   
        if (enemy.shottype != Enemy.shottypes.none)
        {
            enemy.projectile = (GameObject)EditorGUILayout.ObjectField("Projectile Object" ,enemy.projectile, typeof(GameObject), true);
            if (enemy.shottype != Enemy.shottypes.lazar) enemy.projectilespeed = EditorGUILayout.FloatField("Projectile Speed", enemy.projectilespeed);
            enemy.fireinterval_sec = EditorGUILayout.FloatField("Firing Time(seconds)", enemy.fireinterval_sec);
            enemy.burstcooldown_sec = EditorGUILayout.FloatField("Cooldown Time(seconds", enemy.burstcooldown_sec);
            enemy.projectiles_per_burst = EditorGUILayout.IntField("Projectiles per Firing", enemy.projectiles_per_burst);
            if (enemy.shottype != Enemy.shottypes.lazar)  enemy.projectiledirection = (Enemy.projectiledirections)EditorGUILayout.EnumPopup("Projectile Direction", enemy.projectiledirection);
        }
         if (enemy.shottype == Enemy.shottypes.lazar)
        {
            EditorGUILayout.LabelField("[Lazar Options]", EditorStyles.boldLabel);
            enemy.lazarglow = (GameObject)EditorGUILayout.ObjectField("Firing Glow Object",enemy.lazarglow, typeof(GameObject),true);
            enemy.lazar_damage = EditorGUILayout.IntField("Lazar Damage", enemy.lazar_damage);
            enemy.lazar_grow_rate = EditorGUILayout.FloatField("Lazar Growth Rate", enemy.lazar_grow_rate);
            enemy.lazar_max_width = EditorGUILayout.FloatField("Lazar Max Width", enemy.lazar_max_width);
        }
        //allow changes to instances
        if (PrefabUtility.GetPrefabInstanceStatus(target) != PrefabInstanceStatus.NotAPrefab) EditorUtility.SetDirty(target);
    }
}
#endif


//----------------------------------------------------------------------
//Type:        Custom Class
//Description: This class controls behaviour of enemys
//
//Written by:  Cera Baltzley
//----------------------------------------------------------------------
public class Enemy : MonoBehaviour
{
    public enum projectiledirections { forward = 1, toward_player = 2 }
    public enum Eflypattern { none = 0, oneway = 1, hover = 2, stopngo = 3, loop = 4, squareexit = 5, rotating = 6, faceplayer = 7 };
    public enum shottypes { none = 0, single = 1, dual = 2, triad = 3, spreader = 4, burst = 5, lazar = 6 }

    [Header("[Enemy Health Options]")]
    public GameObject explosion;
    public float initial_cooldown = 0;
    public int health = 1;
    public int collisiondamage = 50;

    [Header("[Enemy Movement Options]")]
    public Eflypattern flypattern;
    public float speed;

    [Header("[Enemy Weapon Options]")]
    public shottypes shottype;
    [HideInInspector]
    public float projectilespeed;
    [HideInInspector]
    public float fireinterval_sec;
    [HideInInspector]
    public float burstcooldown_sec;
    [HideInInspector]
    public int projectiles_per_burst;
    [HideInInspector]
    public projectiledirections projectiledirection;
    [HideInInspector]
    public GameObject projectile;
    [HideInInspector]
    public GameObject lazarglow;
    [HideInInspector]
    public float lazar_max_width=8f;
    [HideInInspector]
    public float lazar_grow_rate = .2f;
    [HideInInspector]
    public int lazar_damage = 500;

    private bool cooldown = false;
    private bool firing = true;
    private int counter_burst;
    private int counter2 = 0;
    private int counter_cooldown = 0;
    private float elapsedseconds;
    private int counter_projectile = 0;
    private float start = 0;
    private float frames_per_projectile;
    private bool inBoundary = false;
    private Health playerHealth;
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
            //operate movement behaviour
            movement();
            inBoundary = true;
            if (elapsedseconds > initial_cooldown)
            { 
            //operate firing behaviour
            if (firing == true) //if the gun is turned on (sometimes the movement patterns turn it off)
            {
                // if the weapon is on cooling down
                if (cooldown == true)
                {
                    counter_cooldown++; //counts frames we are in cooldown mode
                    //and the cooldown timer is finished, flip the cooldown flag reset the counter
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
                    //regulate the frequency of shots with counter
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

        }
        else { start = 0; cooldown = false; counter_burst = 0; counter_cooldown = 0; counter_projectile = 0; }
    }
    //----------------------------------------------------------------------
    //Type:        Function
    //Description: This function processes the movement and provides 8 options
    //             
    //             0. None: Enemy does not move.
    //             1. Straight Line: Enemy moves forward at a constant custom speed indefinitely
    //             2. Hover: Enemy moves forward for 2 seconds without firing
    //                  at custom constant speed then stops and fires indefinitely
    //             3. Stop n Go: Enemy moves forward for 2 seconds stops and fires for 4
    //                  seconds and then stops firing and continues forward indefinitely 
    //             4. Loop: Enemy moves onto screen without firing, then begins firing while
    //                  moving from side to side indefinitely
    //             5. SquareExit: Enemy moves onto screen without firing, moves sideways 
    //                  while firing , then retreats off screen the way it came in
    //             6. Rotating: Enemy stays in place but rotates at a custom speed,
    //                  reversible direction by using negative numbers (useful for turrets)
    //             7. Face Player: Enemy stays in place, but constantly rotates to face player
    //
    //Written by:  Cera Baltzley
    //----------------------------------------------------------------------
    public void movement()
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
                    Destroy(gameObject, 2);
                    transform.Translate(0, -Time.deltaTime * speed / 10f, 0);
                    hover();  
                }
                break;
            case 6://rotating turret
                transform.Rotate(0, 0, speed * Time.deltaTime);
                break;
            case 7: //faceplayer
                bool lockdirection = false;
                if (shottype == shottypes.lazar)
                {

                    if (transform.childCount > 0) { lockdirection = true; } else lockdirection=false;
                }
                if (lockdirection==false) 
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
                break;
        }
    }
    //----------------------------------------------------------------------
    //Type:        Function
    //Description: This function updates the score based on tag (could be more detailed)
    //
    //Written by:  Cera
    //----------------------------------------------------------------------
    void scoreupdate()
    {
        if (tag == "Boss" || tag == "BossPart") {
            playerstats.levelscore += 25000;
        }
        if (tag == "MIDBOSS") playerstats.levelscore += 10000;
        if (tag == "Enemy") playerstats.levelscore += 1000;
    }
    //----------------------------------------------------------------------
    //Type:        Function
    //Description: This function defines behaviour upon collisions with various
    //                 objects
    //
    //Written by:  Reed?
    //----------------------------------------------------------------------
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
                    scoreupdate();
                    if (name != "BOSSCONTAINER" && name != "LAZARBOSSTURRET")
                    {
                        Instantiate(explosion, transform.position, explosion.transform.rotation);
                        Destroy(gameObject);
                        FMODUnity.RuntimeManager.PlayOneShot("event:/Game/enemydeath"); // play sound
                       
                    }
                  
                }
                this.GetComponent<Animation>().Play("Enemy_Hurt");
                FMODUnity.RuntimeManager.PlayOneShot("event:/Game/enemydamaged"); // play sound
            }
            if (other.gameObject.tag == "TriShot")
            {
                health -= 5;
                if (health < 1)
                {
                    scoreupdate();
                    if (name != "BOSSCONTAINER" && name != "LAZARBOSSTURRET")
                    {
                        Instantiate(explosion, transform.position, explosion.transform.rotation);
                        Destroy(gameObject);
                        FMODUnity.RuntimeManager.PlayOneShot("event:/Game/enemydeath"); // play sound
                        
                    }
                 
                }
                this.GetComponent<Animation>().Play("Enemy_Hurt");
                FMODUnity.RuntimeManager.PlayOneShot("event:/Game/enemydamaged"); // play sound
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
                FMODUnity.RuntimeManager.PlayOneShot("event:/Game/playerdamaged"); // play sound
            }
        }
    }
    //----------------------------------------------------------------------
    //Type:        Function
    //Description: This function is used by the movement function to account for level scrolling
    //                so that it stays in the same place
    //
    //Written by:  Cera Baltzley
    //----------------------------------------------------------------------
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
    //----------------------------------------------------------------------
    //Type:        Function
    //Description: This function processes the weapon firing and provides 2 options for
    //                fire direction combined withh 7 options for weapon type.
    //                the weapon firing may also be customized with burst fire and cooldown
    //                periods, and number of projectiles fired per period
    //             
    //             Projectile Direction: 
    //                  1: Forward: Fires forward
    //                  2: Toward Player: Fires at angle corresponding to player location
    //             
    //             Shot Type:
    //             0. None: Enemy does not fire
    //             1. Single: Enemy fires a single stream
    //             2. Dual: Enemy fires two parallel streams
    //             3. Triad: Enemy fires 3 streams, forward, left and right at a 45 degree angle
    //             4. Spreader: Enemy fires 6 streams forward at regularly spaced angles
    //             5. Burst: Enemy fires 12 streams outward in all directions
    //             6: Lazar: Enemy fires a beam weapon of customizable width and duration
    //
    //Written by:  Cera Baltzley
    //----------------------------------------------------------------------
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
                Instantiate(lazarglow, new Vector2(transform.position.x, transform.position.y), transform.rotation, GameObject.FindWithTag("Foreground").transform);
                projectileGO = Instantiate(projectile, new Vector2(transform.position.x, transform.position.y), transform.rotation, transform);
                projectileGO.transform.localScale = new Vector3(0, 20f, 1);
                projectileGO.transform.Translate(0, 10f, 0);
                projectileGO.GetComponent<EnemyLazar>().maxXscale = lazar_max_width;
                projectileGO.GetComponent<EnemyLazar>().damage = lazar_damage;
                projectileGO.GetComponent<EnemyLazar>().Xscaler = lazar_grow_rate;
                break;
        }
    }

    public void LifeLost()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game/playerdeath"); // play sound
        GameObject.Find("GameController").GetComponent<GameController>().LifeLost();
    }

}