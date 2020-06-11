using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMOD.Studio;

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
    public int voltageSqu;
    public int voltageSin;
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
    public SimpleHealthBar squBar;
    public SimpleHealthBar sinBar;
}
public class PlayerController : MonoBehaviour
{

    public Boundary boundary;
    public HealthVoltage hv;
    public Voltage v;
    public WeaponCharge wc;
    public int voltageMax;
    public int voltageRechargeAmount;
    private float triChargeCurrent;
    public float triChargeMax;
    public float triRechargeSpeed;
    public float triRechargeAmount;
    private float squChargeCurrent;
    public float squChargeMax;
    public float squRechargeSpeed;
    public float squRechargeAmount;
    private float sinChargeCurrent;
    public float sinChargeMax;
    public float sinRechargeSpeed;
    public float sinRechargeAmount;
    public float healthRecoverPercent;
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
    public GameObject shield;
    public GameObject heal;
    public Transform shotSpawn;
    public Transform shieldSpawn;

    private float nextFire;
    public float healthMax;
    private int voltageCurrent;
    private bool voltageRechargeActive = false;
    public bool evadeActive = false;
    public bool evadeCooldownActive = false;
    private bool stopped = false;
    private bool canShield = true;
    public Rigidbody2D rb;

    // waveform ability sound instances
    private string sxSaw;
    private string sxTri;
    private string sxSqu;
    private string sxSin;
    private EventInstance squInst;
    private void Start()
    {
    
        livesIcon = GameObject.FindGameObjectsWithTag("Life");
        lives = livesIcon.Length;
        rb = GetComponent<Rigidbody2D>();
        healthMax = GetComponent<Health>().health;
        voltageCurrent = voltageMax;
        triChargeCurrent = triChargeMax;
        squChargeCurrent = squChargeMax;
        sinChargeCurrent = sinChargeMax;
        DestroyByBoundary.playerLeft = false;
        BossHurt.bossActive = false;

        sxSaw = "event:/Game/" + GameController.currentSongName + "/saw";
        sxTri = "event:/Game/" + GameController.currentSongName + "/tri";
        sxSqu = "event:/Game/" + GameController.currentSongName + "/squ";
        sxSin = "event:/Game/" + GameController.currentSongName + "/sin";

    }

    private void Update()
    {
        GameController.playerEnable = true;
        hv.healthText.text = GetComponent<Health>().health + "/" + healthMax;
        hv.voltageText.text = voltageCurrent + "/" + voltageMax;
        hv.healthBar.UpdateBar(GetComponent<Health>().health, healthMax);
        hv.voltageBar.UpdateBar(voltageCurrent, voltageMax);
        wc.triBar.UpdateBar(triChargeCurrent, triChargeMax);
        wc.squBar.UpdateBar(squChargeCurrent, squChargeMax);
        wc.sinBar.UpdateBar(sinChargeCurrent, sinChargeMax);
        //Debug.Log(BeatSystem.bar);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("MusicBarGlobal", BeatSystem.bar);

        //primary attack (saw)
        if (Input.GetButton("Fire1") && Time.time > nextFire && v.voltageSawtooth <= voltageCurrent && GameController.playerEnable == true)
        {
            FMODUnity.RuntimeManager.PlayOneShot(sxSaw);
            nextFire = Time.time + fireRate;
            // Instantiate(shotSawtooth, shotSpawn.position, shotSawtooth.GetComponent<Transform>().rotation);
            Instantiate(shotSawtooth, shotSpawn.position, transform.rotation);
            voltageCurrent -= v.voltageSawtooth;
            StopCoroutine("VoltageRecharge");
            StartCoroutine("VoltageRecharge");
        }
        //secondary attack (tri)
        if (Input.GetButtonDown("Fire2") && v.voltageTri <= voltageCurrent && triChargeCurrent >= triChargeMax && GameController.playerEnable == true)
        {
            FMODUnity.RuntimeManager.PlayOneShot(sxTri);
            Instantiate(shotTriAOE, transform.position, shotTriAOE.GetComponent<Transform>().rotation);
            Vector3 start = new Vector3(0, 0, 0);
            GameObject.Find("TriAOE(Clone)").transform.localScale = start;
            StartCoroutine(TriAOE(2, 2));
            voltageCurrent -= v.voltageTri;
            voltageRechargeActive = true;
            triChargeCurrent = 0;
            StopCoroutine("TriRecharge");
            StartCoroutine("TriRecharge");
            StopCoroutine("VoltageRecharge");
            StartCoroutine("VoltageRecharge");
        }
        //shield (squ)
        if (Input.GetButtonDown("Fire3") && v.voltageSqu <= voltageCurrent && squChargeCurrent >= squChargeMax && canShield == true && GameController.playerEnable == true)
        {
            squInst = FMODUnity.RuntimeManager.CreateInstance(sxSqu);
            squInst.start();
            squInst.release();
            canShield = false;
            Instantiate(shield, shieldSpawn.transform.position, shieldSpawn.GetComponent<Transform>().rotation);
            Vector3 start = new Vector3(2, 0, 0);
            GameObject.Find("Shield(Clone)").transform.localScale = start;
            StartCoroutine(ShieldSpawn(2, 2, 6));
            voltageCurrent -= v.voltageSqu;
            voltageRechargeActive = true;
            StopCoroutine("VoltageRecharge");
            StartCoroutine("VoltageRecharge");
        }
        //heal (sin)
        if (Input.GetButtonDown("Fire4") && v.voltageSin <= voltageCurrent && sinChargeCurrent >= sinChargeMax && GetComponent<Health>().health < healthMax &&  GameController.playerEnable == true)
        {
            FMODUnity.RuntimeManager.PlayOneShot(sxSin);
            heal.SetActive(true);
            GetComponent<Health>().health += (healthMax * (0.01f * healthRecoverPercent));
            StartCoroutine(Heal());
            voltageCurrent -= v.voltageSin;
            voltageRechargeActive = true;
            sinChargeCurrent = 0;
            StopCoroutine("SinRecharge");
            StartCoroutine("SinRecharge");
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
          GetComponent<Health>().health = 0;
        }
        if (GetComponent<Health>().health > healthMax)
        {
            GetComponent<Health>().health = healthMax;
        }
        if (triChargeCurrent > triChargeMax)
        {
            triChargeCurrent = triChargeMax;
        }
        if (triChargeCurrent < 0)
        {
            triChargeCurrent = 0;
        }
        if (squChargeCurrent > squChargeMax)
        {
            squChargeCurrent = squChargeMax;
        }
        if (squChargeCurrent < 0)
        {
            squChargeCurrent = 0;
        }
        if (sinChargeCurrent > sinChargeMax)
        {
            sinChargeCurrent = sinChargeMax;
        }
        if (sinChargeCurrent < 0)
        {
            sinChargeCurrent = 0;
        }
        //movement
        #region

        float moveX = 0;
        float moveY = 0;

        if (evadeActive == false)
        {

            //float translationY = Input.GetAxis("Vertical") * speed;
            //float translationX = Input.GetAxis("Horizontal") * speed;
            //translationY *= Time.deltaTime;
            //translationX *= Time.deltaTime;
            //if (transform.eulerAngles.z == 90)
            //{
            //     transform.Translate(-translationY, -translationX, 0);
            //} else  transform.Translate(translationX, -translationY, 0);
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

        #endregion
    }

    public void LifeLost()
    {
        if (lives > 0)
        {
            livesIcon[lives - 1].SetActive(false);
            lives -= 1;
        }
        if (lives <= 0)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Game/playerdeath");
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
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game/playerboost");
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

    IEnumerator TriAOE(float size, float time)
    {
        float startTime = 0;
        while (startTime < time)
        {
            Vector3 sizeChange = new Vector3(size, size, 0);
            GameObject.Find("TriAOE(Clone)").transform.position = transform.position;
            GameObject.Find("TriAOE(Clone)").transform.localScale += sizeChange * Time.deltaTime;
            startTime += 1 * Time.deltaTime;
            yield return null;
        }
        Destroy(GameObject.Find("TriAOE(Clone)"));
        if (SceneManager.GetActiveScene().name == "Level_03")
        {
            shotTri.transform.localRotation = Quaternion.Euler(shotTri.transform.localRotation.x, shotTri.transform.localRotation.y, 90);
            shotTri.GetComponent<Mover>().direction = Mover.Edirection.vertical;
        }
        else
        {
            shotTri.transform.localRotation = Quaternion.Euler(shotTri.transform.localRotation.x, shotTri.transform.localRotation.y, 0);
            shotTri.GetComponent<Mover>().direction = Mover.Edirection.horizontal;
        }
        shotTri.GetComponent<Mover>().speed = -15;
        Instantiate(shotTri, transform.position, transform.rotation);
        shotTri.GetComponent<Mover>().speed = 15;
        Instantiate(shotTri, transform.position, transform.rotation);
    }

    IEnumerator ShieldSpawn(float sizeChange, float sizeMax, float duration)
    {
        while (GameObject.Find("Shield(Clone)").transform.localScale.y < sizeMax)
        {
            Vector3 size = new Vector3(0, sizeChange, 0);
            GameObject.Find("Shield(Clone)").transform.position = shieldSpawn.transform.position;
            GameObject.Find("Shield(Clone)").transform.localScale += size * Time.deltaTime;
            yield return null;
        }
        float startTime = 0;
        while (startTime < duration)
        {
            GameObject.Find("Shield(Clone)").transform.position = shieldSpawn.transform.position;
            startTime += 1 * Time.deltaTime;
            yield return null;
        }
        while (GameObject.Find("Shield(Clone)").transform.localScale.y > 0)
        {
            Vector3 size = new Vector3(0, sizeChange, 0);
            GameObject.Find("Shield(Clone)").transform.position = shieldSpawn.transform.position;
            GameObject.Find("Shield(Clone)").transform.localScale -= size * Time.deltaTime;
            yield return null;
        }
        Destroy(GameObject.Find("Shield(Clone)"));
        squInst.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        squChargeCurrent = 0;
        StopCoroutine("SquRecharge");
        StartCoroutine("SquRecharge");
        canShield = true;
    }

    IEnumerator Heal()
    {
        yield return new WaitForSeconds(2);
        heal.SetActive(false);
    }

    IEnumerator TriRecharge()
    {
        while (triChargeCurrent < triChargeMax)
        {
            yield return new WaitForSeconds(triRechargeSpeed);
            triChargeCurrent += triRechargeAmount;
        }
    }

    IEnumerator SquRecharge()
    {
        while (squChargeCurrent < squChargeMax)
        {
            yield return new WaitForSeconds(squRechargeSpeed);
            squChargeCurrent += squRechargeAmount;
        }
    }

    IEnumerator SinRecharge()
    {
        while (sinChargeCurrent < sinChargeMax)
        {
            yield return new WaitForSeconds(sinRechargeSpeed);
            sinChargeCurrent += sinRechargeAmount;
        }
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
