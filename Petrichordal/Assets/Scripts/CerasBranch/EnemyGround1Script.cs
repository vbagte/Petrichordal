using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyGround1Script : MonoBehaviour
{
    public int collisiondamage;
    public float projectilespeed;
    public enum shottypes { none=0,single=1,dual=2,triad=3,spreader=4,burst=5}
    public shottypes shottype;
    public GameObject projectile;
    public enum updown { normal = -1, none = 0, reverse = 1 };
    public enum leftright { left = -1, none = 0, right = 1 };
    public updown gravdir;
    public leftright movedir;
    public float speed;
    private float counter;
    private float elapsedseconds;
    public float fireinterval;
    float suminterval;
    // Start is called before the first frame update
    void Start()
    {

        suminterval = fireinterval;
        // transform.localPosition = gridLayout.CellToLocal(cellPosition);

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= 10 && transform.position.x >= -10 && transform.position.y >= -5 && transform.position.y <= 5)
        {
            float offset = 0;
            if (movedir == leftright.left) offset = 0.5f;
            if (movedir == leftright.right) offset = -0.5f;
            GridLayout gridLayout = transform.parent.GetComponent<GridLayout>();
            Vector3Int nexttile = gridLayout.LocalToCell(new Vector3(transform.localPosition.x + (int)movedir + offset, transform.localPosition.y, 0));
            Vector3Int nextfloortile = gridLayout.LocalToCell(new Vector3(transform.localPosition.x + (int)movedir + offset, transform.localPosition.y + (int)gravdir, 0));
            Tilemap tileLayer = transform.parent.GetComponent<Tilemap>();
            //Debug.Log(tileLayer.GetTile(nextfloortile).name);
            //if (
            //    tileLayer.GetTile(nexttile) != null ||
            //    (((tileLayer.GetTile(nextfloortile) == null) || tileLayer.GetTile(nextfloortile).name == "def" || tileLayer.GetTile(nextfloortile).name == "deh") &&
            //    tileLayer.GetTile(nexttile) == null)
            //    )

            //if (movedir == leftright.left)
            //{
            //    movedir = leftright.right;
            //}
            //else if (movedir == leftright.right)
            //    movedir = leftright.left;

            //framecounter
            counter++;
            elapsedseconds = counter / 60;
            if (elapsedseconds >= suminterval)
            {
                firelaser();
                suminterval += fireinterval;
            }
            //move
            transform.localPosition += new Vector3((int)movedir * Time.deltaTime * speed / 10f, 0);
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerShot")
        {
            Destroy(collision.gameObject);

        }
        if (collision.gameObject.tag == "Player")
        {
 
            collision.gameObject.GetComponent<Health>().health -= collisiondamage;
        }
    }


    public void firelaser()
    {
        GameObject projectileGO;
        switch ((int)shottype)
        {
            //    public enum shottypes { none=0,single=1,dual=2,triad=3,spreader=4,burst=5,shootingstar=6,}
            

            case 0: //none
                break;
            case 1: //single
                projectileGO = Instantiate(projectile, new Vector2(transform.position.x, transform.position.y), transform.rotation, transform.parent.transform);
                projectileGO.GetComponent<EnemyBullet>().speed = projectilespeed;
                projectileGO.GetComponent<Transform>().Translate(0, .1f, 0);
                break;
            case 2: //dual
                projectileGO = Instantiate(projectile, new Vector2(transform.position.x , transform.position.y ), transform.rotation, transform.parent.transform);
                projectileGO.GetComponent<Transform>().Translate(-.3f, .1f, 0);
                projectileGO = Instantiate(projectile, new Vector2(transform.position.x , transform.position.y), transform.rotation, transform.parent.transform);
                projectileGO.GetComponent<Transform>().Translate(.3f, .1f, 0);
                break;
            case 3: //triad
                int angle3 = -45;
                for (int i = 0; i < 3; i++)
                {  
                    projectileGO = Instantiate(projectile, new Vector2(transform.position.x, transform.position.y),transform.rotation, transform.parent.transform);
                    projectileGO.GetComponent<EnemyBullet>().speed = projectilespeed;
                    projectileGO.GetComponent<Transform>().Rotate(0, 0, transform.rotation.z+angle3);
                    angle3 += 45;
                }
                break;
            case 4: //spreader
                int angle = -50;
                for (int i = 0; i < 6; i++)
                {     
                    projectileGO = Instantiate(projectile, new Vector2(transform.position.x, transform.position.y), transform.rotation, transform.parent.transform);
                    projectileGO.GetComponent<Transform>().Rotate(0, 0, transform.rotation.z + angle);
                    angle += 20;
                }   
                break;
            case 5: //burst        
                int angle2 = 0;
                for (int i = 0; i < 12; i++)
                {
                    GameObject a = new GameObject();
                    a.transform.Rotate(transform.rotation.x, transform.rotation.y, transform.rotation.z + angle2);
                    Instantiate(projectile, new Vector2(transform.position.x, transform.position.y), a.transform.rotation, transform.parent.transform);
                    angle2 += 30;
                    Destroy(a);

                }
                break;
        }

    }
}


      

