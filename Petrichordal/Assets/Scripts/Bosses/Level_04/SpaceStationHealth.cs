using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStationHealth : MonoBehaviour
{
    public SimpleHealthBar healthBar;
    public GameObject bossPanel;
    public GameObject shipFront;
    private int healthMax;
    private bool active = false;
    // Start is called before the first frame update
    void Start()
    {
        healthMax = gameObject.GetComponent<SpaceStationWhole>().hitpoints;
    }

    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            healthBar.UpdateBar(gameObject.GetComponent<SpaceStationWhole>().hitpoints, healthMax);
        }
        else if (shipFront.transform.position.x <= 10 && shipFront.transform.position.x >= -10 && transform.position.y >= -5.5 && transform.position.y < 3.5)
        {
            bossPanel.SetActive(true);
            active = true;
        }
    }
}
