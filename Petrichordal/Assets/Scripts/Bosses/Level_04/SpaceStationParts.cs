using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStationParts : MonoBehaviour
{
    public GameObject boss;
    private int hitpoints;
    private bool shot;
    private int bossHP;
    // Start is called before the first frame update
    void Start()
    {
        bossHP = boss.GetComponent<SpaceStationWhole>().hitpoints;
        hitpoints = gameObject.GetComponent<Enemy>().health;
    }

    // Update is called once per frame
    void Update()
    {
        boss.GetComponent<SpaceStationWhole>().hitpoints -= (hitpoints - gameObject.GetComponent<Enemy>().health);
        hitpoints = gameObject.GetComponent<Enemy>().health;
    }

}
