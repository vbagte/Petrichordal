using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHurt : MonoBehaviour
{
    public static bool bossActive = false;
    public GameObject boss;
    public SimpleHealthBar healthBar;

    private float healthMax;

    private void Start()
    {
        healthMax = boss.GetComponent<Health>().health;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerShot") || other.CompareTag("TriShot"))
        {
            Destroy(other.gameObject);
            if (bossActive)
            {
                StartCoroutine("Hurt");
                boss.GetComponent<Health>().health -= other.GetComponent<Damage>().damage;
            }
        }
        if (other.CompareTag("Player") && this.CompareTag("BossPart"))
        {
            other.GetComponent<Health>().health -= 1000;
            if (other.GetComponent<Health>().health <= 0)
            {
                LifeLost();
            }
            StartCoroutine("Hurt");
        }
    }

    private void Update()
    {
        healthBar.UpdateBar(boss.GetComponent<Health>().health, healthMax);
    }

    IEnumerator Hurt()
    {
        GetComponent<SpriteRenderer>().color = new Color32(255, 79, 79, 255);
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    void LifeLost()
    {
        GameObject.Find("GameController").GetComponent<GameController>().LifeLost();
    }
}
