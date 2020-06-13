using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnExit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
          if (transform.position.y >= 3.5 || transform.position.x <=-9.5 || transform.position.y<=-4.5 || transform.position.x >=9.5) Destroy(gameObject);
    }
}
