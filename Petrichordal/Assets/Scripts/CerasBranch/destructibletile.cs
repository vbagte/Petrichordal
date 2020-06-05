using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class destructibletile : MonoBehaviour
{
  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D collision)
    {
              Tilemap tilemap = GetComponent<Tilemap>();
        if (collision.gameObject.tag == "PlayerShot")
        {
        //Vector3 hitPosition = new Vector3();
        //    foreach (ContactPoint2D hit in collision.contacts)
        //    {
        //    hitPosition.x = hit.point.x;
        //    hitPosition.y = hit.point.y;
        //        tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
        //    }
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Player")
        {
       
            collision.gameObject.GetComponent<Health>().health -= 20;
        }

    }
}
