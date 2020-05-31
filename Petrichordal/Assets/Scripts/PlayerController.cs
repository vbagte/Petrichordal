using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax;
}

[System.Serializable]
public class Voltage
{
    public int voltageSawtooth;
    public int voltageTri;
}

[System.Serializable]
public class HealthVoltage
{
    public Text healthText;
    public SimpleHealthBar healthBar;
    public Text voltageText;
    public SimpleHealthBar voltageBar;
}

[System.Serializable]
public class WeaponCharge
{
    public SimpleHealthBar triBar;
}

public class PlayerController : MonoBehaviour
{
    //variables
    #region
    public static bool triEnabled = false;

    public Boundary boundary;
    public HealthVoltage hv;
    public Voltage v;
    public WeaponCharge wc;
    public int voltageMax;
    public int voltageRechargeAmount;
    private int triChargeCurrent;
    public int triChargeMax;
    public int triRechargeSpeed;
    public int triRechargeAmount;
    public int lives;
    public float voltageRechargeSpeed;
    public float speed;
    public float evadeSpeed;
    public float evadeTime;
    public float evadeCooldownTime;
    public float fireRate;
    public GameObject[] livesIcon;
    public GameObject shotSawtooth;
    public GameObject shotTriAOE;
    public GameObject shotTri;
    public GameObject triPanel;
    public Transform shotSpawn;
    public Transform triSpawn;

    private float nextFire;
    public int healthMax;
    private int voltageCurrent;    
    private bool voltageRechargeActive = false;
    public bool evadeActive = false;
    public bool evadeCooldownActive = false;
    private bool stopped = false;
    public Rigidbody2D rb;
    #endregion

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
        triChargeCurrent = triChargeMax;
        DestroyByBoundary.playerLeft = false;
        BossHurt.bossActive = false;
        if (triEnabled)
        {
            triPanel.SetActive(true);
        }
    }

    private void Update()
    {
        GameController.playerEnable = true;
        hv.healthText.text = GetComponent<Health>().health + "/" + healthMax;
        hv.voltageText.text = voltageCurrent + "/" + voltageMax;
        hv.healthBar.UpdateBar(GetComponent<Health>().health, healthMax);
        hv.voltageBar.UpdateBar(voltageCurrent, voltageMax);
        wc.triBar.UpdateBar(triChargeCurrent, triChargeMax);

        //Debug.Log(BeatSystem.bar);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("MusicBarGlobal", BeatSystem.bar);

        //sawtooth attack
        if (Input.GetKey(KeyCode.UpArrow) && Time.time > nextFire && v.voltageSawtooth <= voltageCurrent && GameController.playerEnable == true)
        {

            //FMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMOD
            sawInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Game/lv01/waveform abilities/main/saw");
            sawInstance.start();
            sawInstance.release();
            //FMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMOD

            nextFire = Time.time + fireRate;
            Instantiate(shotSawtooth, shotSpawn.position, shotSawtooth.GetComponent<Transform>().rotation);
            voltageCurrent -= v.voltageSawtooth;
            StopCoroutine("VoltageRecharge");
            StartCoroutine("VoltageRecharge");
        }
        //tri attack
        if (Input.GetKeyDown(KeyCode.DownArrow) && triEnabled == true && v.voltageTri <= voltageCurrent && triChargeCurrent == triChargeMax && GameController.playerEnable == true)
        {

            //FMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMOD
            //sawInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Game/lv01/waveform abilities/main/saw");
            //sawInstance.start();
            //sawInstance.release();
            //FMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMODFMOD

            Instantiate(shotTriAOE, triSpawn.position, shotTriAOE.GetComponent<Transform>().rotation);
            Vector3 start = new Vector3(0, 0, 0);
            GameObject.Find("TriAOE(Clone)").transform.localScale = start;
            StartCoroutine(TriAOE(2, 2));
            voltageCurrent -= v.voltageTri;
            voltageRechargeActive = true;
            triChargeCurrent = 0;
            StopCoroutine("VoltageRecharge");
            StartCoroutine("VoltageRecharge");
            StopCoroutine("TriRecharge");
            StartCoroutine("TriRecharge");
        }

        //health/voltage logic
        #region
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
        }
        if (GetComponent<Health>().health > healthMax)
        {
            GetComponent<Health>().health = healthMax;
        }
        #endregion

        float moveX = 0;
        float moveY = 0;

        // movement
        if (evadeActive == false && GameController.playerEnable == true)
        {
            if (Input.GetKey(KeyCode.D))
            {
                moveX = 1;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                moveX = -1;
            }
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            {
                moveX = 0;
                stopped = false;
            }
            if (Input.GetKey(KeyCode.W))
            {
                moveY = 1;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                moveY = -1;
            }
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
            {
                moveY = 0;
                stopped = false;
            }
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
            {
                moveY = 0;
                stopped = true;
            }
            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
            {
                moveX = 0;
                stopped = true;
            }
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
            Vector2 movement = new Vector2(moveX, moveY);
            rb.velocity = movement * speed;
        }

        rb.position = new Vector2
        (
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            Mathf.Clamp(rb.position.y, boundary.yMin, boundary.yMax)
        );

    }

    IEnumerator TriAOE(float size, float time)
    {
        float startTime = 0;
        while(startTime < time)
        {
            Vector3 sizeChange = new Vector3(size, size, 0);
            GameObject.Find("TriAOE(Clone)").transform.position = transform.position;
            GameObject.Find("TriAOE(Clone)").transform.localScale += sizeChange * Time.deltaTime;
            startTime += 1 * Time.deltaTime;
            yield return null;
        }
        Destroy(GameObject.Find("TriAOE(Clone)"));
        shotTri.GetComponent<Mover>().speed = -15;
        Instantiate(shotTri, transform.position, shotTri.GetComponent<Transform>().rotation);
        shotTri.GetComponent<Mover>().speed = 15;
        Instantiate(shotTri, transform.position, shotTri.GetComponent<Transform>().rotation);
    }

    IEnumerator TriRecharge()
    {
        while (triChargeCurrent < triChargeMax)
        {
            yield return new WaitForSeconds(triRechargeSpeed);
            triChargeCurrent += triRechargeAmount;
        }
    }

    //health/movement logic
    #region
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
    #endregion


}
