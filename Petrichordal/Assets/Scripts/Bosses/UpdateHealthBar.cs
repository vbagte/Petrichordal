using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateHealthBar : MonoBehaviour
{
    public SimpleHealthBar healthBar;
    public GameObject bossPanel;
    private int healthMax;
    private bool active = false;
    // Start is called before the first frame update
    void Start()
    {
        healthMax = gameObject.GetComponent<Enemy>().health;
    }

    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            healthBar.UpdateBar(gameObject.GetComponent<Enemy>().health, healthMax);
        }
        else if (transform.position.x <= 10 && transform.position.x >= -10 && transform.position.y >= -5.5 && transform.position.y < 3.5)
        {
            bossPanel.SetActive(true);
            active = true;
        }
    }
}
