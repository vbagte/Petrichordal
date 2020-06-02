using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage;
    public float speed;
    private Health playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        //  Debug.Log(transform.rotation);
        Destroy(gameObject, 10f);
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        }
    }

    // Update is called once per frame
    void Update()
    {
      //  transform.up *= speed;
       // transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z));
        transform.Translate(new Vector3(0, Time.deltaTime*speed, 0));
        if (transform.position.y >= 5 || transform.position.x <=-9.5 || transform.position.y<=-5) Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag=="Player")
        {
            other.gameObject.GetComponent<Health>().health -= damage;
            Destroy(gameObject);
            if (playerHealth.health <= 0)
            {
                LifeLost();
            }
            other.gameObject.GetComponent<Animation>().Play("Player_Hurt");
        }
    }

    public void LifeLost()
    {
        GameObject.Find("GameController").GetComponent<GameController>().LifeLost();
    }
}
