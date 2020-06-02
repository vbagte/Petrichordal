using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax;
}

[System.Serializable]
public class Voltage
{
    public int voltageSawtooth;
}

[System.Serializable]
public class HealthVoltage
{
    public Text healthText;
    public SimpleHealthBar healthBar;
    public Text voltageText;
    public SimpleHealthBar voltageBar;
}
public class PlayerController : MonoBehaviour
{

    public Boundary boundary;
    public HealthVoltage hv;
    public Voltage v;
    public int voltageMax;
    public int voltageRechargeAmount;
    public int lives;
    public float voltageRechargeSpeed;
    public float speed;
    public float evadeSpeed;
    public float evadeTime;
    public float evadeCooldownTime;
    public float fireRate;
    public GameObject[] livesIcon;
    public GameObject shotSawtooth;
    public Transform shotSpawn;

    private int weaponType = 0;
    private float nextFire;
    public int healthMax;
    private int voltageCurrent;
    private bool voltageRechargeActive = false;
    public bool evadeActive = false;
    public bool evadeCooldownActive = false;
    private bool stopped = false;
    public Rigidbody2D rb;

    //FMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMOD
    private FMOD.Studio.EventInstance sawInstance;
    //FMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMOD

    private void Start()
    {
    
        livesIcon = GameObject.FindGameObjectsWithTag("Life");
        lives = livesIcon.Length;
        rb = GetComponent<Rigidbody2D>();
        healthMax = GetComponent<Health>().health;
        voltageCurrent = voltageMax;
        DestroyByBoundary.playerLeft = false;
        BossHurt.bossActive = false;
    }

    private void Update()
    {
        GameController.playerEnable = true;
        hv.healthText.text = GetComponent<Health>().health + "/" + healthMax;
        hv.voltageText.text = voltageCurrent + "/" + voltageMax;
        hv.healthBar.UpdateBar(GetComponent<Health>().health, healthMax);
        hv.voltageBar.UpdateBar(voltageCurrent, voltageMax);

        //Debug.Log(BeatSystem.bar);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("MusicBarGlobal", BeatSystem.bar);

        // if (Input.GetKey(KeyCode.UpArrow) && Time.time > nextFire && weaponType == 0 && v.voltageSawtooth <= voltageCurrent && GameController.playerEnable == true)
        if (Input.GetButton("Fire1") && Time.time > nextFire && weaponType == 0 && v.voltageSawtooth <= voltageCurrent && GameController.playerEnable == true)

        {

            //FMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMOD
            sawInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Game/lv01/waveform abilities/main/saw");
            sawInstance.start();
            sawInstance.release();
            //FMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMOD

            nextFire = Time.time + fireRate;
            // Instantiate(shotSawtooth, shotSpawn.position, shotSawtooth.GetComponent<Transform>().rotation);
            Instantiate(shotSawtooth, shotSpawn.position, transform.rotation);
            voltageCurrent -= v.voltageSawtooth;
            StopCoroutine("VoltageRecharge");
            StartCoroutine("VoltageRecharge");
        }
        if (voltageCurrent < voltageMax)
        {
            voltageRechargeActive = true;
        }
        else if (voltageCurrent >= voltageMax)
        {
            voltageRechargeActive = false;
        }
        if (voltageCurrent > voltageMax)
        {
            voltageCurrent = voltageMax;
        }
        if (voltageCurrent < 0)
        {
            voltageCurrent = 0;
        }
        if (GetComponent<Health>().health <= 0)
        {
            LifeLost();
          //  GetComponent<Health>().health = 0;
        }
        if (GetComponent<Health>().health > healthMax)
        {
            GetComponent<Health>().health = healthMax;
        }

        float moveX = 0;
        float moveY = 0;

        if (evadeActive == false)
        {

            float translationY = Input.GetAxis("Vertical") * speed;
            float translationX = Input.GetAxis("Horizontal") * speed;
            translationY *= Time.deltaTime;
            translationX *= Time.deltaTime;
            if (transform.eulerAngles.z == 90)
            {
                 transform.Translate(-translationY, -translationX, 0);
            } else  transform.Translate(translationX, -translationY, 0);
            //if (Input.GetKey(KeyCode.D))
            //{
            //    moveX = 1;
            //}
            //else if (Input.GetKey(KeyCode.A))
            //{
            //    moveX = -1;
            //}
            //if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            //{
            //    moveX = 0;
            //    stopped = false;
            //}
            //if (Input.GetKey(KeyCode.W))
            //{
            //    moveY = 1;
            //}
            //else if (Input.GetKey(KeyCode.S))
            //{
            //    moveY = -1;
            //}
            //if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
            //{
            //    moveY = 0;
            //    stopped = false;
            //}
            //if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
            //{
            //    moveY = 0;
            //    stopped = true;
            //}
            //if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
            //{
            //    moveX = 0;
            //    stopped = true;
            //}
            if (Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.D) && Input.GetKey(KeyCode.Space))
            {
                if (evadeCooldownActive == false && stopped == false)
                {
                    moveX = evadeSpeed;
                    StartCoroutine(Evade());
                    StartCoroutine(EvadeCooldown());
                }
            }
            if (Input.GetKey(KeyCode.A) && Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.A) && Input.GetKey(KeyCode.Space))
            {
                if (evadeCooldownActive == false && stopped == false)
                {
                    moveX = -evadeSpeed;
                    StartCoroutine(Evade());
                    StartCoroutine(EvadeCooldown());
                }
            }
            if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) && Input.GetKey(KeyCode.Space))
            {
                if (evadeCooldownActive == false && stopped == false)
                {
                    moveY = evadeSpeed;
                    StartCoroutine(Evade());
                    StartCoroutine(EvadeCooldown());
                }
            }
            if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.S) && Input.GetKey(KeyCode.Space))
            {
                if (evadeCooldownActive == false && stopped == false)
                {
                    moveY = -evadeSpeed;
                    StartCoroutine(Evade());
                    StartCoroutine(EvadeCooldown());
                }
            }
            rb.velocity = new Vector2(moveX * speed, moveY * speed);
        }
        if (transform.position.x < boundary.xMin)
            transform.position = new Vector2(boundary.xMin, transform.position.y);
        if (transform.position.x > boundary.xMax)
            transform.position = new Vector2(boundary.xMax, transform.position.y);
        if (transform.position.y > boundary.yMax)
            transform.position = new Vector2(transform.position.x, boundary.yMax);
        if (transform.position.y < boundary.yMin)
            transform.position = new Vector2(transform.position.x, boundary.yMin);
    }

    public void LifeLost()
    {
        livesIcon[lives - 1].SetActive(false);
        lives -= 1;
        if (lives <= 0)
        {
            GameObject.Find("GameController").GetComponent<GameController>().PlayerDeath();
        }
    }

    public void HealthUpdate()
    {
        hv.healthText.text = GetComponent<Health>().health + "/" + healthMax;
        hv.healthBar.UpdateBar(GetComponent<Health>().health, healthMax);
    }

    IEnumerator Evade()
    {
        evadeActive = true;
        yield return new WaitForSeconds(evadeTime);
        evadeActive = false;
    }

    IEnumerator EvadeCooldown()
    {
        evadeCooldownActive = true;
        yield return new WaitForSeconds(evadeCooldownTime);
        evadeCooldownActive = false;
    }

    IEnumerator VoltageRecharge()
    {
        while (voltageRechargeActive)
        {
            yield return new WaitForSeconds(voltageRechargeSpeed);
            voltageCurrent += voltageRechargeAmount;
        }
    }

}
