using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    public GameObject shot;
    public Transform shotSpawn;
    public float fireDelay;
    public float fireRate;

    void Start()
    {
        InvokeRepeating("Fire", fireDelay, fireRate);
    }

    void Fire()
    {
        Instantiate(shot, shotSpawn.position, shot.GetComponent<Transform>().rotation);
    }

}
